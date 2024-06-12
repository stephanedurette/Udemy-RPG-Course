using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCaster : MonoBehaviour
{
    [SerializeField] private float halfExtents;
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 distance;
    [SerializeField] private LayerMask detectLayer;

    public bool IsColliding {  get; private set; }
    
    // Update is called once per frame
    void Update()
    {
        //(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask)
        IsColliding = Physics2D.BoxCast(
            transform.position + position,
            Vector2.one * halfExtents,
            0,
            distance.normalized,
            distance.magnitude,
            detectLayer
        );
    }

    private void OnDrawGizmos()
    {
        Vector3 topRightPos = (transform.position + position + distance) + new Vector3(1,1) * halfExtents;
        Vector3 topLeftPos = (transform.position + position + distance) + new Vector3(-1, 1) * halfExtents;
        Vector3 bottomLeftPos = (transform.position + position + distance) + new Vector3(-1, -1) * halfExtents;
        Vector3 bottomRightPos = (transform.position + position + distance) + new Vector3(1, -1) * halfExtents;

        Gizmos.DrawLine(topRightPos, topLeftPos);
        Gizmos.DrawLine(topRightPos, bottomRightPos);
        Gizmos.DrawLine(bottomRightPos, bottomLeftPos);
        Gizmos.DrawLine(topLeftPos, bottomLeftPos);
    }
}
