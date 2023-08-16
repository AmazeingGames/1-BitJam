using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Walk")]
public class Walk : State<CharacterController>
{
    [Header("Walk")]
    [SerializeField] float walkSpeed;
    [SerializeField] float maxVelocity;
    [SerializeField] bool showDebug;

    [Header("Jump")]
    [SerializeField] float coyoteTimeLength;
    [SerializeField] float jumpBufferLength;

    Rigidbody2D rigidbody;
    Player player;

    float jumpTimer;
    float groundedTimer;

    float horizontalInput;
    float delayedVelocity;

    public override void Enter(CharacterController parent)
    {
        base.Enter(parent);

        if (player == null)
            player = parent.GetComponent<Player>();
        if (rigidbody == null)
            rigidbody = parent.GetComponent<Rigidbody2D>();
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

        rigidbody.velocity = new Vector2(walkSpeed * horizontalInput, rigidbody.velocity.y);


    }

    public override void FixedUpdate()
    {
    }

    public override void ChangeState()
    {
        if (jumpTimer > 0 && groundedTimer > 0)
        {
            jumpTimer = 0;
            groundedTimer = 0;

            Debug.Log("Changed state to Jump");
            runner.SetState(typeof(Jump));
        }
    }

    public override void Exit()
    {
    }

    

    
}
