using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "attackcycle", menuName = "Scriptable Objects / Attack Cycle")]
public class AttackCycle : ScriptableObject
{
    public List<AttackData> Attacks;
}
