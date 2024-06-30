using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class AnimatorEvents : MonoBehaviour
{
    public Action<GameObject, float> OnSpawnHitbox;

    public void SpawnHitbox(AnimationEvent e)
    {
        OnSpawnHitbox?.Invoke(e.objectReferenceParameter as GameObject, e.floatParameter);
    }
}
