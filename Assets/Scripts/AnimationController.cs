using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Platformer/Controller", fileName = "Animation Controller")]
public class AnimationControllerSO : ScriptableObject
{
    public Animator Animator { get; set; }
    [SerializeField] List<string> animStates;
    //[SerializeField] string baseAnimation;
    private Dictionary<string, int> states = new Dictionary<string, int>();
    const float crossFadeDuration = 0.1f;
    private void OnEnable()
    {
        foreach (var state in animStates)
            states[state] = Animator.StringToHash(state);
    }
    public void PlayAnimation(string state)
    {
        Debug.Log(Animator);
        Animator.CrossFade(states[state], crossFadeDuration);
    }

    public void UpdateAnimator(string param, float value)
    {
        Animator.SetFloat(param, value);
    }
}
