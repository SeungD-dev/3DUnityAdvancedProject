using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class JumpState : BaseState
    {
        public JumpState(PlayerController player, Animator animator) : base(player, animator)
        {

        }

        public override void OnEnter()
        {
            Debug.Log("JumpState.OnEnter");
           animator.CrossFade(JumpHash, crossFadeDuration);
        }

        public override void FixedUpdate()
        {
            
            //플레이어 점프와 움직임 로직 호출
            player.HandleJump();
            player.HandleMovement();
        }
    }
}
