using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer
{
    public abstract class BaseState : IState
    {
        protected readonly PlayerController player;
        protected BaseState(PlayerController player)
        {
            this.player = player;
        }

        public virtual void OnEnter()
        {
            //throw new System.NotImplementedException();
        }
        public virtual void Update()
        {
            //throw new System.NotImplementedException();
        }
        public virtual void FixedUpdate()
        {
            //throw new System.NotImplementedException();
        }
        public virtual void OnExit()
        {
            //throw new System.NotImplementedException();
        }


    }

}
