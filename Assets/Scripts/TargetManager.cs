using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
namespace Platformer
{
    public class TargetManager : MonoBehaviour
    {
        private HashSet<GameObject> potentialTargets = new HashSet<GameObject>();
        public GameObject LastTarget { get; private set; }
        public UnityEvent GameOver;
        public UnityEvent ResetBall;

        private void Start()
        {
            ChooseTarget();
        }
        public void AddTarget(GameObject target)
        {
            if (!potentialTargets.Contains(target))
            {
                potentialTargets.Add(target);
                Debug.Log("Added target: " + target.name);
            }
        }

        public void RemoveTarget(GameObject target)
        {
            if (potentialTargets.Contains(target))
            {
                potentialTargets.Remove(target);
                Debug.Log("Removed target: " + target.name);
            }
            if (LastTarget == target)
            {
                ResetBall.Invoke();
            }
        }

        public GameObject ChooseTarget()
        {
            if (potentialTargets.Count < 2)
            {
                GameOver.Invoke();
                return null;
            }
            List<GameObject> filteredTargets = new List<GameObject>();
            foreach (GameObject target in potentialTargets)
            {
                if (target != LastTarget)
                {
                    filteredTargets.Add(target);
                }
            }

            if (filteredTargets.Count > 0)
            {
                LastTarget = filteredTargets[Random.Range(0, filteredTargets.Count)];
                return LastTarget;
            }

            return null;
        }
    }
}

