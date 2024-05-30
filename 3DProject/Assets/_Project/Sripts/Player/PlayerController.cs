using UnityEngine;
using KBCore.Refs;
using Cinemachine;
using System;
using System.Collections.Generic;
using Utilities;

namespace Platformer
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")]
        [SerializeField, Self] Rigidbody rb;
        [SerializeField, Self] GroundChecker groundChecker;
        [SerializeField, Self] Animator animator;
        [SerializeField, Anywhere] CinemachineFreeLook freeLookVCam;
        [SerializeField,Anywhere] InputReader input;

        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f;

        [Header("Jump Settings")]
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float jumpDuration = 0.5f;
        [SerializeField] float jumpCooldown = 0f;
        [SerializeField] float gravityMultiplier = 3f;

        [Header("Dash Settings")]
        [SerializeField] float dashForce = 10f;
        [SerializeField] float dashDuration = 1f;
        [SerializeField] float dashCooldown = 2f;

        [Header("Attack Settings")]
        [SerializeField] float attackCooldown = 0.5f;
        [SerializeField] float attackDistance = 1f;
        [SerializeField] int attackDamage = 10;

        Transform mainCam;

        const float ZeroF = 0f;
        float currentSpeed;
        float velocity;
        float jumpVelocity;
        float dashVelocity =1f;

        Vector3 movement;

        List<Timer> timers;
        CountdownTimer jumpTimer;
        CountdownTimer jumpCooldownTimer;
        CountdownTimer dashTimer;
        CountdownTimer dashCooldownTimer;
        CountdownTimer attackTimer;

        //Animator parameters
        static readonly int Speed = Animator.StringToHash("Speed");
        static readonly int JumpTime = Animator.StringToHash("JumpTime");

        StateMachine stateMachine;
        Stamina stamina;
        UI_Stamina UIstamina;

        private void Awake()
        {
            stamina = GetComponent<Stamina>();
            mainCam = Camera.main.transform;
            freeLookVCam.Follow = transform;
            freeLookVCam.LookAt = transform;
            freeLookVCam.OnTargetObjectWarped(transform, transform.position - freeLookVCam.transform.position - Vector3.forward);

            rb.freezeRotation = true;

            SetupTimers();
            
            SetUpStates();
        }

        private void SetUpStates()
        {
            //State Machine
            stateMachine = new StateMachine();

            //Declare states
            var locomotionState = new LocomotionState(this, animator);
            var jumpState = new JumpState(this, animator);
            var dashState = new DashState(this, animator);
            var attackState = new AttackState(this, animator);

            //Declare transitions
            At(locomotionState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));
            At(locomotionState, dashState, new FuncPredicate(() => dashTimer.IsRunning));
            At(locomotionState, attackState, new FuncPredicate(() => attackTimer.IsRunning));
            At(attackState, locomotionState, new FuncPredicate(() => !attackTimer.IsRunning));

            Any(locomotionState, new FuncPredicate(ReturnToLocomotionState));


            //Set initial state
            stateMachine.SetState(locomotionState);
        }

        bool ReturnToLocomotionState()
        {
            return groundChecker.IsGrounded 
                && !jumpTimer.IsRunning
                && !attackTimer.IsRunning
                && !dashCooldownTimer.IsRunning;

        }

        private void SetupTimers()
        {
            //Setup timers
            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);


            jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
            jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();

            dashTimer = new CountdownTimer(dashDuration);
            dashCooldownTimer = new CountdownTimer(dashCooldown);
            dashTimer.OnTimerStart += () => dashVelocity = dashForce;
            dashTimer.OnTimerStop += () =>
            {
                dashVelocity = 1f;
                dashCooldownTimer.Start();
            };

            attackTimer = new CountdownTimer(attackCooldown);

            timers = new List<Timer>(5) { jumpTimer, jumpCooldownTimer, dashTimer, dashCooldownTimer, attackTimer };
        }

        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

        private void Start() => input.EnablePlayerActions();

        private void OnEnable()
        {
            input.Jump += OnJump;
            input.Dash += OnDash;
            input.Attack += OnAttack;
            
        }

        private void OnDisable()
        {
            input.Jump -= OnJump;
            input.Dash -= OnDash;
            input.Attack -= OnAttack;
            
        }

        void OnAttack()
        {
            if (!attackTimer.IsRunning)
            {
                attackTimer.Start();
            }
        }

        public void Attack()
        {
            Vector3 attackPos = transform.position + transform.forward;
            Collider[] hitEnemies = Physics.OverlapSphere(attackPos, attackDistance);

            foreach (var enemy in hitEnemies)
            {
                Debug.Log(enemy.name);
            }
        }

        void OnJump(bool performed)
        {
            if(performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpTimer.Start();
                jumpVelocity = jumpForce;
                
            }
            else if(!performed && jumpTimer.IsRunning)
            {
                jumpTimer.Stop();
                
            }
        }

        void OnDash(bool performed)
        {
            if (performed && !dashTimer.IsRunning && !dashCooldownTimer.IsRunning)
            {
               dashTimer.Start();


            }
            else if (!performed && dashTimer.IsRunning)
            {
                dashTimer.Stop();

            }
        }

        private void Update()
        {
            movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
            stateMachine.Update();
            
            HandleTimers();
            UpdateAnimator();

        }

        private void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        private void UpdateAnimator()
        {
            animator.SetFloat(Speed, currentSpeed);
            animator.SetFloat(JumpTime, jumpCooldown);
        }

        void HandleTimers()
        {
            foreach(var timer in timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }

        public void HandleJump()
        {
            
            //점프 상태가 아니고 착지 상태가 아니라면, 점프 속도 0
            if(!jumpTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpVelocity = ZeroF;
                jumpTimer.Stop() ;
                return;
            }

            
            //점프 중이거나 떨어지는 상태일 때 속도 계산
            if(!jumpTimer.IsRunning)
            {
                
                jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }

            
            //속도 적용
            rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
        }

        public void HandleMovement()
        {
            
            
            
            //움직임 방향과 카메라 회전 일치
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;

            if(adjustedDirection.magnitude > ZeroF)
            {
                HandleRotation(adjustedDirection);

                HandleHorizontalMovement(adjustedDirection);

               SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
               SmoothSpeed(ZeroF);

                
                //좀 더 깔끔한 정지를 위해 수평 속도 리셋 
                rb.velocity = new Vector3(ZeroF,rb.velocity.y,ZeroF);
            }
        }

        private void HandleHorizontalMovement(Vector3 adjustedDirection)
        {
            //플레이어 움직임
           Vector3 velocity = adjustedDirection * moveSpeed *dashVelocity* Time.fixedDeltaTime;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            
            //움직이는 방향과 회전 방향 동일시
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.LookAt(transform.position + adjustedDirection);
        }

        private void SmoothSpeed(float value)
        {
            currentSpeed= Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
        }


    }
}
