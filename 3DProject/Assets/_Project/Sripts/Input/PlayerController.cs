﻿using UnityEngine;
using KBCore.Refs;
using Cinemachine;
using System;

namespace Platformer
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")]
        [SerializeField,Self] CharacterController characterController;
        [SerializeField, Self] Animator animator;
        [SerializeField, Anywhere] CinemachineFreeLook freeLookVCam;
        [SerializeField,Anywhere] InputReader input;

        [Header("Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f;

        Transform mainCam;

        const float ZeroF = 0f;
        float currentSpeed;
        float velocity;

        //Animator parameters
        static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake()
        {
            mainCam = Camera.main.transform;
            freeLookVCam.Follow = transform;
            freeLookVCam.LookAt = transform;
            freeLookVCam.OnTargetObjectWarped(transform,transform.position - freeLookVCam.transform.position - Vector3.forward);
        }

        private void Start() => input.EnablePlayerActions();


        private void Update()
        {
            HandleMovement();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            animator.SetFloat(Speed, currentSpeed);
        }

        private void HandleMovement()
        {
            var movementDirection = new Vector3(input.Direction.x, 0f, input.Direction.y).normalized;
            
            //Rotate movement direction to match camera rotation
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movementDirection;

            if(adjustedDirection.magnitude > ZeroF)
            {
                HandleRotation(adjustedDirection);

                HandleCharacterController(adjustedDirection);

               SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
               SmoothSpeed(ZeroF);
            }
        }

        private Vector3 HandleCharacterController(Vector3 adjustedDirection)
        {

            //Move the Player
            var adjustedMovement = adjustedDirection * (moveSpeed * Time.deltaTime);
            characterController.Move(adjustedMovement);
            return adjustedMovement;
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            //Adjust rotation to match movement direction
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