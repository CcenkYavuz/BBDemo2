using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
namespace Platformer
{
    public class JumpState : BaseState
    {
        List<Utilities.Timer> timers;
        CountdownTimer jumpTimer;
        CountdownTimer jumpCooldownTimer;
        float jumpForce = 10f;
        float jumpDuration = .5f;
        float jumpCooldown = 0f;
        float jumpVelocity;
        public JumpState(PlayerController player, Animator animator) : base(player, animator) {
            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);

            jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
            jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();

        }
       
        private int JumpHash = Animator.StringToHash("Jump");
        protected const float crossFadeDuration = 0.1f;

        public override void OnEnter()
        {
            Debug.Log("JumpState Entered");
            animator.CrossFade(JumpHash, crossFadeDuration);

        }
        public override void FixedUpdate()
        {
            player.HandleJump();
            player.HandleMovement();
        }
   
    }
}

