using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private LayerMask detectLayer;

    public bool IsColliding {  get; private set; }

    public Vector3 Direction { get { return direction; } set {  direction = value; } }
    
    // Update is called once per frame
    void Update()
    {
        IsColliding = Physics2D.Raycast(transform.position, direction.normalized, direction.magnitude, detectLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + direction);
    }
}
