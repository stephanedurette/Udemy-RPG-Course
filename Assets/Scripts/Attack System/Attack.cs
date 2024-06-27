using ImprovedTimers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private AttackData attackData;
    
    private Hitbox2D[] hitBoxes;

    private CountdownTimer[] hitboxSpawnTimers;
    private CountdownTimer[] hitboxLifetimeTimers;

    
    private void Awake()
    {
        hitBoxes = GetComponentsInChildren<Hitbox2D>();

        hitboxSpawnTimers = new CountdownTimer[hitBoxes.Length];
        hitboxLifetimeTimers = new CountdownTimer[hitBoxes.Length];

        for (int i = 0; i < hitBoxes.Length; i++) {

            //Lambda functions don't work using the incrementing variable since they all share the last known value when the for loop stops
            int index = i;

            hitBoxes[i].gameObject.SetActive(false);

            hitboxSpawnTimers[i] = new CountdownTimer(hitBoxes[i].Data.SpawnDelay);
            hitboxLifetimeTimers[i] = new CountdownTimer(hitBoxes[i].Data.LifeTime);

            Debug.Log(i);
            Debug.Log(index);

            hitboxSpawnTimers[i].OnTimerStop += () => SpawnHitbox(index);
            hitboxLifetimeTimers[i].OnTimerStop += () => DespawnHitbox(index);

            hitboxSpawnTimers[i].Start();
        }

    }

    private void SpawnHitbox(int index)
    {
        Debug.Log(index);
        hitBoxes[index].gameObject.SetActive(true);
        hitboxLifetimeTimers[index].Start();
    }

    private void DespawnHitbox(int index)
    {
        hitBoxes[index].gameObject.SetActive(false);
        CheckDone();
    }

    private void CheckDone()
    {
        if (hitboxLifetimeTimers.All((x) => !x.IsRunning))
        {
            Destroy(gameObject);
        }
    }

    private void OnHit(Vector2 hitPosition, Hurtbox2D hitTarget)
    {
        
    }

    private void OnEnable()
    {
        foreach (var hitbox in hitBoxes) hitbox.OnHitboxHit += OnHit;
    }
    private void OnDisable()
    {
        foreach (var hitbox in hitBoxes) hitbox.OnHitboxHit -= OnHit;
    }
}
