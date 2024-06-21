using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "attack", menuName = "ScriptableObjects/Attacks/AttackData")]
public class AttackData : ScriptableObject
{
    public Vector2 Movement;
    public string AnimationString;
    public float MoveDuration;
}
