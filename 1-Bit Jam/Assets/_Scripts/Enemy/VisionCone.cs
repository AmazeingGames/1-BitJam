using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [SerializeField] LayerMask targetLayerMask;
    [SerializeField] LayerMask obstructionLayerMask;

    [SerializeField] [Range(1, 360)] float visionAngle = 45;
    [SerializeField] float visionRadius = 5;

    [SerializeField] float timeBetweenCalls = .2f;
    [SerializeField] bool showDebugLogs;
    [SerializeField] bool showGizmos;
    public List<Collider2D> VisibleTargets { get; } = new();

    Enemy enemy;

    void Awake()
    {
        enemy = transform.parent.GetComponent<Enemy>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FOVCheck());
    }

    IEnumerator FOVCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenCalls);
            FOV();
        }
    }

    void FOV()
    {
        if (!enemy.IsActiveProperty)
        {
            Log("Not active");
            return;
        }

        if (showDebugLogs)
            Log("Checked colliders");

        VisibleTargets.Clear();

        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, visionRadius, targetLayerMask);

        foreach (var collider in targetsInRange)
        {
            Transform target = collider.transform;

            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, directionToTarget) < visionAngle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayerMask))
                {
                    if (showDebugLogs)
                        Debug.Log($"Found target {collider.gameObject.name}");
                    VisibleTargets.Add(collider);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = Color.blue;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, visionRadius);

        Vector3 angle1 = DirectionFromAngle(-transform.eulerAngles.z, -visionAngle / 2);
        Vector3 angle2 = DirectionFromAngle(-transform.eulerAngles.z, visionAngle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + angle1 * visionRadius);
        Gizmos.DrawLine(transform.position, transform.position + angle2 * visionRadius);

        foreach (var collider in VisibleTargets)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, collider.transform.position);
        }
    }

    Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


    //Probably should turn this into some sort of extension method
    void Log(string message)
    {
        if (showDebugLogs)
            Debug.Log(message);
    }
}
