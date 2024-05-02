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
        public UnityEvent TargetChanged;
        EventBinding<AddTarget> addTargetEventBinding;
        EventBinding<RemoveTarget> removeTargetEventBinding;
        EventBinding<ChooseTarget> chooseTargetEventBinding;
        private void OnEnable()
        {
            addTargetEventBinding = new EventBinding<AddTarget>(AddTarget);
            EventBus<AddTarget>.Register(addTargetEventBinding);

            removeTargetEventBinding = new EventBinding<RemoveTarget>(RemoveTarget);
            EventBus<RemoveTarget>.Register(removeTargetEventBinding);

            chooseTargetEventBinding = new EventBinding<ChooseTarget>(ChooseTarget);
            EventBus<ChooseTarget>.Register(chooseTargetEventBinding);
        }
        private void OnDisable()
        {
            EventBus<AddTarget>.Deregister(addTargetEventBinding);
            EventBus<RemoveTarget>.Deregister(removeTargetEventBinding);
            EventBus<ChooseTarget>.Deregister(chooseTargetEventBinding);
        }


        private void Start()
        {
            ChooseTarget();
        }
        void AddTarget(AddTarget addTarget)
        {
            var target = addTarget.target;
            if (!potentialTargets.Contains(target))
            {
                potentialTargets.Add(target);
                Debug.Log("Added target: " + target.name);
            }
        }

        void RemoveTarget(RemoveTarget removeTarget)
        {
            var target = removeTarget.target;
            if (potentialTargets.Contains(target))
            {
                potentialTargets.Remove(target);
                //Debug.Log("Removed target: " + target.name);
            }
            if (LastTarget == target)
            {
                ResetBall?.Invoke();
            }
        }

        void ChooseTarget()
        {
            if (potentialTargets.Count < 2)
            {
                GameOver?.Invoke();
                GameManager.Instance.LoadScene("Lobby");
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
                TargetChanged?.Invoke();
            }


        }
    }
}

