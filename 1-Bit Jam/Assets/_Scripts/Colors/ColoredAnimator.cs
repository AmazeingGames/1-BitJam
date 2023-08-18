using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Create a base class that this and Player Animator both derive from
public class ColoredAnimator : MonoBehaviour
{
    [SerializeField] float phaseAnimationLength;

    Animator animator;

    static readonly string phaseInSpeedMultiplierName = "PhaseMultiplier";

    static readonly string idleName     = "Idle";
    static readonly string phaseInName  = "PhaseIn";
    static readonly string inactiveName = "Inactive";

    static readonly int idle = Animator.StringToHash(idleName);
    static readonly int phaseIn = Animator.StringToHash(phaseInName);
    static readonly int inactive = Animator.StringToHash(inactiveName);

    bool isPhasePlaying;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ShouldPlayIdle(bool isActive)
    {
        if (isPhasePlaying)
            return;

        if (isActive)
        {
            PlayAnimation(idle);
        }
    }

    public void ShouldPlayPhaseIn(bool startedPhase, bool isPhasingIn)
    {
        if (startedPhase)
        {
            isPhasePlaying = true;

            float phaseMultiplier = 1;
            float normalizedTime = 0;

            if (!isPhasingIn)
            {
                phaseMultiplier = -1;
                normalizedTime = 1;
            }

            animator.SetFloat(phaseInSpeedMultiplierName, phaseMultiplier);

            PlayAnimation(phaseIn, normalizedTime);

            StartCoroutine(StartPhaseTimer());
        }
    }

    public void ShouldPlayInactive(bool isActive)
    {
        if (isPhasePlaying)
            return;

        if (!isActive)
        {
            PlayAnimation(inactive);
        }
    }

    IEnumerator StartPhaseTimer()
    {
        float timer = 0;

        while (true)
        {
            isPhasePlaying = true;
            timer += Time.deltaTime;

            if (timer >= phaseAnimationLength)
            {
                isPhasePlaying = false;
                yield break;
            }
            yield return null;
        }
    }

    protected void PlayAnimation(int animationToPlay, float normalizedTimeOffset = float.NegativeInfinity)
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        animator.CrossFade(animationToPlay, 0, 0, normalizedTimeOffset);

        DebugHelper.ShouldLog($"Played animation : {animationToPlay}", false);
    }
}
