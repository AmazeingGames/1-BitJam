using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vision : MonoBehaviour
{
    [SerializeField] protected LayerMask targetLayerMask;
    [SerializeField] protected LayerMask obstructionLayerMask;

    [SerializeField] protected float visionRadius = 5;

    [SerializeField] protected float timeBetweenCalls = .2f;
    [SerializeField] protected bool showDebugLogs;
    [SerializeField] protected bool showGizmos;

    protected List<Collider2D> VisibleTargets { get; } = new();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FOVCheck());
    }

    protected IEnumerator FOVCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenCalls);

            if (!GameManager.Instance.IsLevelPlaying)
                continue;

            FOV();
        }
    }

    protected virtual List<Collider2D> FOV()
    {
        DebugHelper.ShouldLog("Checked colliders", showDebugLogs);

        VisibleTargets.Clear();

        VisibleTargets.AddRange(Physics2D.OverlapCircleAll(transform.position, visionRadius, targetLayerMask));

        return VisibleTargets;
    }

    protected void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = Color.blue;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, visionRadius);

        foreach (var collider in VisibleTargets)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, collider.transform.position);
        }
    }

    protected Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public bool CanSeeCollider(Collider2D collider) => VisibleTargets.Contains(collider);
}
