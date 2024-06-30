using UnityEngine;

[CreateAssetMenu(fileName = "attack", menuName = "ScriptableObjects/AttackData")]
public class AttackData : ScriptableObject
{
    [Header("Attack Movement Settings")]
    public Vector2 Movement;
    public float MoveDuration;

    public string AnimationString;
}
