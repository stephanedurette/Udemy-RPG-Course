using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "attack", menuName = "ScriptableObjects/AttackData")]
public class AttackData : ScriptableObject
{
    [Header("AttackAnimationSettings")]
    public string AnimationString;

    [Header("Attack Movement Settings")]
    public Vector2 Movement;
    public float MoveDuration;
}
    