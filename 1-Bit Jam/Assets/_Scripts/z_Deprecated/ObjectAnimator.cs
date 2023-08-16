using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public abstract class ObjectAnimator : MonoBehaviour
{
    protected AnimatorController controller;


    protected abstract void SetAnimator();

    protected List<int> animationQueue = new();

    //This code should pass in objects instead; grabbing the values listed in the objects instead of them being manually put in
    /* protected IEnumerator AddToQueue(int animationStatePlay, float animationLength, float animationSpeed = 1, int animationStateResume = -1, float animationResumeSpeed = 1)
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
    */
}
