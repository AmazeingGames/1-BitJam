using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] bool showDebugLog = false;
    
    [Header("Jump")]
    [SerializeField] float jumpAnimationLength;

    [Header("Idle")]
    [SerializeField] float maxIdleVelocity;


    Animator animator;

    static readonly string runName  = "Run_Player";
    static readonly string jumpName = "Jump_Player";
    static readonly string idleName = "Idle_Player";

    static readonly int run = Animator.StringToHash(runName);
    static readonly int jump = Animator.StringToHash(jumpName);
    static readonly int idle = Animator.StringToHash(idleName);


    bool isJumpPlaying;

    void Start()
    {
        animator = GetComponent<Animator>();

        Debug.Log($"Is animator null: {animator == null}");
    }

    void Update()
    {
        Debug.Log($"is JumpPlaying : {isJumpPlaying}");
    }

    public void ShouldPlayIdle(float playerMovement)
    {
        if (Mathf.Abs(playerMovement) > maxIdleVelocity)
            return;

        if (isJumpPlaying)
            return;

        PlayAnimation(idle);
    }

    public void ShouldPlayWalk(float playerMovement)
    {
        if (Mathf.Abs(playerMovement) < maxIdleVelocity)
            return;

        if (isJumpPlaying)
            return;

        PlayAnimation(run);
    }

    public void ShouldPlayJump(bool startedJump)
    {
        if (startedJump)
        {
            PlayAnimation(jump);

            StartCoroutine(StartJumpTimer());
        }
    }

    public bool IsAnimationPlaying(string animationName) => animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);

    IEnumerator StartJumpTimer()
    {
        float timer = 0;

        while (true)
        {
            isJumpPlaying = true;
            timer += Time.deltaTime;

            if (timer >= jumpAnimationLength)
            {
                isJumpPlaying = false;
                Debug.Log("Finished jump");
                yield break;
            }
            yield return null;
        }
    }

    protected void PlayAnimation(int animationToPlay)
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        animator.CrossFade(animationToPlay, 0, 0);

        DebugHelper.ShouldLog($"Played animation : {animationToPlay}", false);
    }
}
