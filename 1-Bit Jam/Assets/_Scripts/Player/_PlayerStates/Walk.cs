using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Walk")]
public class Walk : State<CharacterController>
{
    [Header("Walk")]
    [SerializeField] float walkSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float velocityPower;
    [SerializeField] float frictionAmount;

    [SerializeField] bool showDebug;

    [Header("Jump")]
    [SerializeField] float coyoteTimeLength;
    [SerializeField] float jumpBufferLength;

    [Header("Fall")]
    [SerializeField] float maxFallVelocity;

    Rigidbody2D rigidbody;
    Player player;

    float jumpTimer;
    float groundedTimer;

    float horizontalInput;
    float delayedVelocity;

    float maxVerticalVelocity;

    public override void Enter(CharacterController parent)
    {
        base.Enter(parent);

        if (player == null)
            player = parent.GetComponent<Player>();
        if (rigidbody == null)
            rigidbody = parent.GetComponent<Rigidbody2D>();

        maxVerticalVelocity = rigidbody.velocity.y;

        Debug.Log($"Walk Vertical Velocity Max: {maxVerticalVelocity}");
    }

    public override void CaptureInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
            jumpTimer = jumpBufferLength;
    }

    public override void Update()
    {
        groundedTimer -= Time.deltaTime;
        jumpTimer -= Time.deltaTime;

        if (player.IsGrounded)
            groundedTimer = coyoteTimeLength;

        KeepConstantVelocity();
    }
        
    //Makes sure the player can't gain more vertical velocity than they already have.
    //Prevents bouncing.
    //Note: Performance is poor, optimize using Clamp
    void KeepConstantVelocity()
    {
        if (rigidbody.velocity.y > maxVerticalVelocity)
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, maxVerticalVelocity);
        else
            maxVerticalVelocity = rigidbody.velocity.y;

        if (maxVerticalVelocity < 0)
            maxVerticalVelocity = 0;
    }

    public override void FixedUpdate()
    {
        MovePlayer();
        ApplyFriction();
    }

    //Uses forces and math to move the player
    void MovePlayer()
    {
        //Calculates the direction we wish to move in; this is our desired velocity
        float targetSpeed = horizontalInput * walkSpeed;

        //Difference between the current and desired velocity
        float speedDifference = targetSpeed - rigidbody.velocity.x;

        //Changes our acceleration rate to suit the situation
        float acceleartionRate = (Mathf.Abs(targetSpeed > .01f ? acceleration : deceleration));

        //Applies acceleration to the speed difference, then raises it to a power, meaning acceleration increases with higher speeds
        //Multiplies it to reapply direction
        float movement = Mathf.Pow(Mathf.Abs(speedDifference) * acceleartionRate, velocityPower) * Mathf.Sign(speedDifference);

        rigidbody.AddForce(movement * Vector2.right);
    }

    //Applies force opposite to the player
    void ApplyFriction()
    {
        float amount = Mathf.Min(Mathf.Abs(rigidbody.velocity.x), Mathf.Abs(frictionAmount));

        amount *= Mathf.Sign(rigidbody.velocity.x);

        //Applies force against the player's movement direction
        rigidbody.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
    }

    public override void ChangeState()
    {
        if (jumpTimer > 0 && groundedTimer > 0)
        {
            jumpTimer = 0;
            groundedTimer = 0;

            runner.SetState(typeof(Jump));
        }
    }

    public override void Exit()
    {
    }

    

    
}
