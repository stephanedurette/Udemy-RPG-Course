using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitboxData", menuName = "ScriptableObjects/HitboxData")]
public class HitboxData : ScriptableObject
{
    [Header("Hit Info")]
    public int Damage;
    public float KnockbackForce;
    public int knockbackAngleDegrees;

    [Header("Spawn Info")]
    public float SpawnDelay;
    public float LifeTime;

}
