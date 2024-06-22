using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "attackcycle", menuName = "ScriptableObjects/Attacks/Attack Cycle")]
public class AttackCycle : ScriptableObject
{
    public List<AttackData> Attacks;
}
