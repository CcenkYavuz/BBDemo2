using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer
{
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}

