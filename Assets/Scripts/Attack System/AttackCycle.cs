using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "attackcycle", menuName = "ScriptableObjects/AttackCycle")]
public class AttackCycle : ScriptableObject
{
    public List<Attack> Attacks;
}
