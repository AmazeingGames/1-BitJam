using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    [Header("Ground Check")]
    [SerializeField] GameObject groundRaycastStart;
    [SerializeField] float groundRaycastLength;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool showGroundCheckDebug;

    public CircleCollider2D Collider { get; private set; }
    Rigidbody2D rigidbody;

    public bool IsGrounded { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponent<CircleCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        StartCoroutine(GroundCheck());
    }


    void OnEnable()
    {
        GameManager.GameStart += HandleGameStart;
        GameManager.GameResume += HandleGameStart;

        GameManager.GameStop += HandleGameStop;
    }

    void OnDisable()
    {
        GameManager.GameStart -= HandleGameStart;
        GameManager.GameResume -= HandleGameStart;

        GameManager.GameStop -= HandleGameStop;
    }

    void HandleGameStart()
    {
        Debug.Log($"Player handled game start");
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void HandleGameStop()
    {
        Debug.Log($"Player handled game stop");
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    

    IEnumerator GroundCheck()
    {
        while (true)
        {
            RaycastHit2D raycast = Physics2D.Raycast(groundRaycastStart.transform.position, Vector3.down, groundRaycastLength, groundLayer);

            DebugHelper.ShouldLog($"Is grounded : {(bool)raycast}", showGroundCheckDebug);

            IsGrounded = raycast;

            yield return null;
        }
        
    }

    void OnDrawGizmos()
    {
        Vector3 rayDirection = new(0, -groundRaycastLength, 0);
        Gizmos.DrawRay(groundRaycastStart.transform.position, rayDirection);    
    }
}
