using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackCombo", menuName = "ScriptableObjects/AttackCombo")]
public class AttackCombo : ScriptableObject
{
    public List<string> AttackAnimationNames;
}
