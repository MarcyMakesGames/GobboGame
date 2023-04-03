using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAnimatorController : MonoBehaviour
{
    [SerializeField] private PawnController controller;
    [SerializeField] private Animator anim;

    private bool performingAnimation = false;

    private Queue<Action> postAnimationAction = null;

    public void SetAnimation(AnimationEnums animationDirection, Action onAnimationComplete = null)
    {
        if(onAnimationComplete != null)
            postAnimationAction.Enqueue(onAnimationComplete);

        Debug.Log("Displaying animation: " + animationDirection.ToString());

        switch (animationDirection)
        {
            case AnimationEnums.Up:
                anim.SetTrigger("MoveUp");
                break;
            case AnimationEnums.Down:
                anim.SetTrigger("MoveDown");
                break;
            case AnimationEnums.Left:
                anim.SetTrigger("MoveLeft");
                break;
            case AnimationEnums.Right:
                anim.SetTrigger("MoveRight");
                break;
            case AnimationEnums.Celebration:
                anim.SetTrigger("Celebration");
                break;
            case AnimationEnums.Negative:
                anim.SetTrigger("NegativeSimon");
                break;
            default:
                Debug.Log("Couldn't find the corresponding animation!");
                return;
        }
    }

    public void ResetTriggers()
    {
        Debug.Log("Animation complete.");
        if (postAnimationAction == null)
            Debug.Log("Post animation action is null!");

        anim.ResetTrigger("MoveUp");
        anim.ResetTrigger("MoveDown");
        anim.ResetTrigger("MoveLeft");
        anim.ResetTrigger("MoveRight");
        anim.ResetTrigger("Celebration");
        anim.ResetTrigger("NegativeSimon");

        if(postAnimationAction.Count > 0)
            postAnimationAction.Dequeue()?.Invoke();
    }
    
    public void PlaySound(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.highBlip:
                AudioManager.instance.PlaySound(SoundType.highBlip);
                break;
            case SoundType.highMidBlip:
                AudioManager.instance.PlaySound(SoundType.highMidBlip);
                break;
            case SoundType.LowMidBlip:
                AudioManager.instance.PlaySound(SoundType.LowMidBlip);
                break;
            case SoundType.LowBlip:
                AudioManager.instance.PlaySound(SoundType.LowBlip);
                break;
            case SoundType.Celebration:
                AudioManager.instance.PlaySound(SoundType.Celebration);
                break;
            case SoundType.Failure:
                AudioManager.instance.PlaySound(SoundType.Failure);
                break;
            case SoundType.Pickup:
                AudioManager.instance.PlaySound(SoundType.Pickup);
                break;
            case SoundType.Drop:
                AudioManager.instance.PlaySound(SoundType.Drop);
                break;
        }
    }

    private void Awake()
    {
        postAnimationAction = new Queue<Action>();
    }
}
