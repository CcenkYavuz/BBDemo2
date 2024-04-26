using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer
{
    public class LocomotionState : BaseState
    {
        public LocomotionState(PlayerController player, Animator animator) : base(player, animator) { }

        private int LocomotionHash = Animator.StringToHash("Locomotion");
        protected const float crossFadeDuration = 0.1f;
        public override void OnEnter()
        {
            Debug.Log("Locomotion Entered");
            animator.CrossFade(LocomotionHash, crossFadeDuration);
        }
        public override void FixedUpdate()
        {
            player.HandleMovement();
        }
    }
}

