using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Jump")]
public class Jump : State<CharacterController>
{
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpEndLength = .2f;

    Rigidbody2D rigidBody;
    float switchTimer;

    public override void Enter(CharacterController parent)
    {
        base.Enter(parent);
        
        if (rigidBody == null)
            rigidBody = parent.GetComponent<Rigidbody2D>();

        switchTimer = jumpEndLength;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight);

    }

    public override void CaptureInput()
    {
    }
    public override void Update()
    {
        switchTimer -= Time.deltaTime;
    }

    public override void FixedUpdate()
    {
    }

    public override void ChangeState()
    {
        if (switchTimer <= 0)
            runner.SetState(typeof(Walk));
    }

    public override void Exit()
    {
    }

   
}
