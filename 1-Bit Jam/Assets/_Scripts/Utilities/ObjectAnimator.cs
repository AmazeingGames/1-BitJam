using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public abstract class ObjectAnimator : MonoBehaviour
{
    protected AnimatorController controller;

    protected Animator animator;

    protected abstract void SetAnimator();

    void CheckAnimator()
    {
        if (animator == null || controller == null)
            SetAnimator();

        if (animator == null || controller == null)
            Debug.LogError("ANIMATOR IS STILL NULL");
    }

    protected List<int> animationQueue = new();

    //This code should pass in objects instead; grabbing the values listed in the objects instead of them being manually put in
    protected IEnumerator AddToQueue(int animationStatePlay, float animationLength, float animationSpeed = 1, int animationStateResume = -1, float animationResumeSpeed = 1)
    {
        CheckAnimator();

        animationQueue.Add(animationStatePlay);

        Debug.Log($"Added {animationStatePlay} to the queue");

        while (true)
        {
            if (animationQueue[0] == animationStatePlay)
            {
                Debug.Log("Playing animation");

                PlayAnimation(animationStatePlay, animationSpeed);
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(animationLength);

        Debug.Log("Finished Animation");

        if (animationStateResume != -1)
            PlayAnimation(animationStateResume, animationResumeSpeed);

        yield break;
    }

    protected void PlayAnimation(int animationToPlay, float animationSpeed)
    {
        CheckAnimator();

        var speed = animator.speed;

        animator.speed = animationSpeed;
        animator.CrossFade(animationToPlay, 0, 0);

        animator.speed = speed;
    }

}
