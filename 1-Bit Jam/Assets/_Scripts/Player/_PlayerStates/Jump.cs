using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Jump")]
public class Jump : State<CharacterController>
{
    [SerializeField] float jumpHeight;

    Rigidbody2D rigidBody;

    public override void Enter(CharacterController parent)
    {
        base.Enter(parent);
        
        if (rigidBody == null)
            rigidBody = parent.GetComponent<Rigidbody2D>();

        Debug.Log("Jumped");
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight);

    }

    public override void CaptureInput()
    {
    }
    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void ChangeState()
    {
        runner.SetState(typeof(Walk));
    }

    public override void Exit()
    {
    }

   
}
