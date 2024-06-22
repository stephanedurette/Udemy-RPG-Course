using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public static class MyExtensions
{
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            int swapIndex = Random.Range(i + 1, list.Count);
            T temp = list[i];
            list[i] = list[swapIndex];
            list[swapIndex] = temp;
        }
    }

    public static T PickRandom<T>(this List<T> list)
    {
        int randIndex = Random.Range(0, list.Count);
        return list[randIndex];
    }

    public static int RandIndex<T>(this List<T> list)
    {
        int randIndex = Random.Range(0, list.Count);
        return randIndex;
    }

    public static bool ContainsLayer(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static float AddVariance(this float num, float variance)
    {
        float randNum = Random.Range(num - num * variance, num + num * variance);
        return randNum;
    }

    public static Vector2 Rotated(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public static bool IsPlaying(this Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public static bool IsPlaying(this Animator animator, string animationName)
    {
        return animator.IsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }
}
