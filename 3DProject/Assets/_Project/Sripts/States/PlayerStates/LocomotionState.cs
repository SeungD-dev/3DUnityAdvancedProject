using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class LocomotionState : BaseState
    {
        public LocomotionState(PlayerController player, Animator animator) : base(player, animator)
        {

        }

        public override void OnEnter()
        {
            animator.CrossFade(LocomotionHash,crossFadeDuration);
        }

        public override void FixedUpdate()
        {
            //call Players's move logic
            player.HandleMovement();
        }
    }
}
