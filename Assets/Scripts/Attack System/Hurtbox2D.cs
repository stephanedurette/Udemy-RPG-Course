using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hurtbox2D : MonoBehaviour
{
    private Collider2D col;

    public Action<Vector2, Hitbox2D> OnHurtBoxHit;

    private void Awake()
    {
        col = GetComponent<Collider2D>();

        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Hitbox2D hitbox))
        {
            OnHurtBoxHit?.Invoke(collision.ClosestPoint(transform.position), hitbox);
        }
    }
}
