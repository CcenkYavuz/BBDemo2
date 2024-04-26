using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer
{
    public abstract class BaseState : IState
    {
        protected readonly PlayerController player;
        protected readonly Animator animator;
        protected BaseState(PlayerController player, Animator animator)
        {
            this.player = player;
            this.animator = animator;
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
            Debug.Log("BaseStateExit");
            //throw new System.NotImplementedException();
        }


    }

}
