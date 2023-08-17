using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Create a base class that this and Player Animator both derive from
public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] bool showDebugLog = false;
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
            Debug.Log("Playing active");
            PlayAnimation(idle);
        }
    }

    public void ShouldPlayPhaseIn(bool startedPhase, bool isPhasingIn)
    {
        if (startedPhase)
        {
            isPhasePlaying = true;

            Debug.Log("Started Phase");

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
            Debug.Log("Playing inactive");
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
                Debug.Log("Finished phase in");
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
