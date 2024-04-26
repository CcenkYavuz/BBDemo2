using Platformer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer
{
    public class FuncPredicate : IPredicate
    {
        readonly Func<bool> func;
        public FuncPredicate(Func<bool> func)
        {
            this.func = func;
        }
        public bool Evaluate() => func.Invoke();
    }
}

