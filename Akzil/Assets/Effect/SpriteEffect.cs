using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class UnityAnimationEvent : UnityEvent<string> { };
[RequireComponent(typeof(Animator))]
public class SpriteEffect : MonoBehaviour
{
    private enum OnEnd { Destroy, Disable }
    private enum OnStart { Enable }

    public float EffectSize = 0;

    [SerializeField] private bool useDebug; // 활성화 시 실행한 함수의 이름을 출력합니다.

    [SerializeField] private OnEnd onEnd = OnEnd.Destroy; // 해당 애니메이션이 끝날 때의 행동을 지정합니다.
    [SerializeField] private OnStart onStart = OnStart.Enable; // 해당 애니메이션이 시작될 때의 행동을 지정합니다.

    private Animator animator;

    private string startFunctionName;
    private string endFunctionName;

    private void Start()
    {
        animator = GetComponent<Animator>();

        transform.localScale = new Vector3(EffectSize, EffectSize, EffectSize);

        if (onStart == OnStart.Enable) startFunctionName = "Enable";

        switch (onEnd)
        {
            case OnEnd.Destroy: endFunctionName = "Destroy"; break;
            case OnEnd.Disable: endFunctionName = "Disable"; break;
        }

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            AnimationEvent animationStartEvent = new AnimationEvent();
            animationStartEvent.time = 0;
            animationStartEvent.functionName = "AnimationStartHandler";
            animationStartEvent.stringParameter = startFunctionName;

            AnimationEvent animationEndEvent = new AnimationEvent();
            animationEndEvent.time = clip.length;
            animationEndEvent.functionName = "AnimationCompleteHandler";
            animationEndEvent.stringParameter = endFunctionName;

            clip.AddEvent(animationStartEvent);
            clip.AddEvent(animationEndEvent);
        }
    }

    private void Enable() => gameObject.SetActive(true);

    private void Destroy() => Destroy(gameObject);
    private void Disable() => gameObject.SetActive(false);

    private void AnimationStartHandler(string name)
    {
        Debug.Log($"{name} animation start.");
        Invoke(name, 0);
    }

    private void AnimationCompleteHandler(string name)
    {
        Debug.Log($"{name} animation complete.");
        Invoke(name, 0);
    }
}
