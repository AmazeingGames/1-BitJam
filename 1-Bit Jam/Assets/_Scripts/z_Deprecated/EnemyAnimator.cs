using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : ObjectAnimator
{
    Enemy enemy;

    [SerializeField] float reappearAnimationLength;
    [SerializeField] float reappearAnimationSpeed = 1;
    [SerializeField] float idleAnimationSpeed = 1;

    private static readonly int activeIdle = Animator.StringToHash("Idle");
    private static readonly int reappear = Animator.StringToHash("Reappear");


    void Start()
    {
        enemy = GetComponent<Enemy>();    
    }

    void OnEnable()
    {
        ColorSwap.Instance.OnColorChange += HandleColorChange;
    }

    void HandleColorChange(ColorSwap.Color backgroundColor)
    {
        Debug.Log($"Started phase | Phasing in : {enemy.IsActiveProperty}");
        Phase(enemy.IsActiveProperty);
    }

    void Phase(bool phasingIn)
    {
        int reverse = 1;
        int resumeAnimation = -1;

        if (phasingIn)
        {
            reverse = -1;
            resumeAnimation = activeIdle;
        }

        StartCoroutine(AddToQueue(reappear, reappearAnimationLength, reappearAnimationSpeed * reverse, resumeAnimation, idleAnimationSpeed));
    }

    protected override void SetAnimator()
    {
        GetComponent<Animator>();

        controller = enemy.Sprites.Controller;
    }
}
