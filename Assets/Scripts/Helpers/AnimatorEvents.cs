using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class AnimatorEvents : MonoBehaviour
{
    public Action OnAnimationFinished;
   
    public void FinishAnimation()
    {
        OnAnimationFinished?.Invoke();
    }
}
