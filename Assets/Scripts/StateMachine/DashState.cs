using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Platformer
{
    public class DashState : BaseState
    {
        public DashState(PlayerController player) : base(player) {
           
        }
        public override void OnEnter()
        {
            player.animController.PlayAnimation("Dash");
        }
        public override void FixedUpdate()
        {

            player.HandleMovement();
        }
    }
}

