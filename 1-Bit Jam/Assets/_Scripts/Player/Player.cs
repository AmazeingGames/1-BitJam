using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    [Header("Ground Check")]
    [SerializeField] GameObject groundRaycastStart;
    [SerializeField] float timeBetweenChecks = .1f;
    [SerializeField] float groundRaycastLength;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool showGroundCheckDebug;

    public CircleCollider2D Collider { get; private set; }

    public bool IsGrounded { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponent<CircleCollider2D>();

        StartCoroutine(GroundCheck());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator GroundCheck()
    {
        while (true)
        {
            RaycastHit2D raycast = Physics2D.Raycast(groundRaycastStart.transform.position, Vector3.down, groundRaycastLength, groundLayer);

            DebugHelper.ShouldLog($"Is grounded : {(bool)raycast}", showGroundCheckDebug);

            IsGrounded = raycast;

            yield return new WaitForSeconds(timeBetweenChecks);
        }
        
    }

    void OnDrawGizmos()
    {
        Vector3 rayDirection = new(0, -groundRaycastLength, 0);
        Gizmos.DrawRay(groundRaycastStart.transform.position, rayDirection);    
    }
}
