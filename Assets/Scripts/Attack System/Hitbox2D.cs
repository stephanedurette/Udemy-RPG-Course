using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hitbox2D : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayerMask;
    public HitboxData Data;

    private Collider2D col;

    public Action<Vector2, Hurtbox2D> OnHitboxHit = delegate { };


    private void Awake()
    {
        col = GetComponent<Collider2D>();

        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!targetLayerMask.ContainsLayer(collision.gameObject.layer)) return;

        if(collision.gameObject.TryGetComponent(out Hurtbox2D hurtbox))
        {
            OnHitboxHit?.Invoke(collision.ClosestPoint(transform.position), hurtbox);
        }
    }
}
