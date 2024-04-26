using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class DashState : BaseState
    {
        public DashState(PlayerController player, Animator animator) : base(player, animator) {        
        }

        private int DashHash = Animator.StringToHash("Dash");
        protected const float crossFadeDuration = 0.1f;
        public override void OnEnter()
        {
            animator.CrossFade(DashHash, crossFadeDuration);
        }
        public override void FixedUpdate()
        {

            player.HandleMovement();
        }
    }
}

