using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer
{
    public class LocomotionState : BaseState
    {
        public LocomotionState(PlayerController player) : base(player) { }

        public override void OnEnter()
        {
            player.animController.PlayAnimation("Locomotion");
        }
        public override void FixedUpdate()
        {
            player.HandleMovement();
        }
    }
}

