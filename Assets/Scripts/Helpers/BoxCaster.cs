using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCaster : MonoBehaviour
{
    [SerializeField] private float halfExtents;
    [SerializeField] private LayerMask detectLayer;

    public bool IsColliding => Physics2D.BoxCast(
            transform.position,
            Vector2.one * halfExtents,
            0,
            Vector2.up,
            0,
            detectLayer
        );

    private void OnDrawGizmos()
    {
        Vector3 topRightPos = transform.position + new Vector3(1,1) * halfExtents;
        Vector3 topLeftPos = transform.position + new Vector3(-1, 1) * halfExtents;
        Vector3 bottomLeftPos = transform.position + new Vector3(-1, -1) * halfExtents;
        Vector3 bottomRightPos = transform.position + new Vector3(1, -1) * halfExtents;

        Gizmos.DrawLine(topRightPos, topLeftPos);
        Gizmos.DrawLine(topRightPos, bottomRightPos);
        Gizmos.DrawLine(bottomRightPos, bottomLeftPos);
        Gizmos.DrawLine(topLeftPos, bottomLeftPos);
    }
}
