using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "attack", menuName = "Scriptable Objects / Attack Data")]
public class AttackData : ScriptableObject
{
    public Vector2 AttackMovement;
    public string AttackAnimationString;
    public float AttackDuration;
    public float MoveDuration;
}
