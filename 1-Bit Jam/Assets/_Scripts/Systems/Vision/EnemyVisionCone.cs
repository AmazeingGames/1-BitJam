using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyVisionCone : Vision
{
    [SerializeField] AlertConsequence alertConsequence;
    [SerializeField] GameObject alertIcon;
    [SerializeField] [Range(1, 360)] protected float visionAngle = 45;


    public enum AlertConsequence { Warning, Lose }

    public bool CanSeePlayer { get; private set; }

    [SerializeField] Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        alertIcon.SetActive(false);

        StartCoroutine(FOVCheck());
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameRunning)
            return;

        CheckForPlayer();
    }

    void CheckForPlayer()
    {
        CanSeePlayer = VisibleTargets.Contains(Player.Instance.Collider);

        switch (alertConsequence)
        {
            case AlertConsequence.Warning:
                alertIcon.SetActive(CanSeePlayer);
                break;
            case AlertConsequence.Lose:
                if (CanSeePlayer)
                    GameManager.Instance.UpdateGameState(GameManager.GameState.Lose);
                break;
        }
    }


    protected override List<Collider2D> FOV()
    {
        if (!enemy.IsActiveProperty)
        {
            DebugHelper.ShouldLog("Not active", showDebugLogs);
            VisibleTargets.Clear();
            return null;
        }

        DebugHelper.ShouldLog("Checked FOV", showDebugLogs);

        List<Collider2D> targetsInRange = new();

        targetsInRange.AddRange(base.FOV());

        VisibleTargets.Clear();

        foreach (Collider2D collider in targetsInRange)
        {
            Vector2 directionToTarget = (collider.transform.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, directionToTarget) < visionAngle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, collider.transform.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayerMask))
                {
                    DebugHelper.ShouldLog($"Found target {collider.gameObject.name}", showDebugLogs);
                    VisibleTargets.Add(collider);
                }
            }
        }

        return VisibleTargets;
    }

    private new void OnDrawGizmos()
    {
    #if UNITY_EDITOR

        if (!showGizmos)
            return;

        base.OnDrawGizmos();

        Vector3 angle1 = DirectionFromAngle(-transform.eulerAngles.z, -visionAngle / 2);
        Vector3 angle2 = DirectionFromAngle(-transform.eulerAngles.z, visionAngle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + angle1 * visionRadius);
        Gizmos.DrawLine(transform.position, transform.position + angle2 * visionRadius);
    #endif
    }
}
