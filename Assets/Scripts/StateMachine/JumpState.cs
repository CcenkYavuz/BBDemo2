using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
namespace Platformer
{
    public class JumpState : BaseState
    {
        public JumpState(PlayerController player) : base(player) { }
        public override void OnEnter()
        {
            player.animController.PlayAnimation("Jump");
        }
        public override void FixedUpdate()
        {
            player.HandleJump();
            player.HandleMovement();
        }
   
    }
}

