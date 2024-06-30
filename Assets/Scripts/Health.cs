using System;
using UnityEngine;

public class Health
{
    public Action<int> OnHealthIncreased = delegate { };
    public Action<int> OnHealthDecreased = delegate { };
    public Action OnHealthHitZero = delegate { };

    private int maxHealth;
    private int health;

    public Health(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        ChangeHealth(-amount);
    }

    public void Heal(int amount)
    {
        ChangeHealth(amount);
    }

    private void ChangeHealth(int amount)
    {
        int oldHealth = health;

        health = Math.Clamp(health + amount, 0, maxHealth);

        if (health < oldHealth) OnHealthDecreased?.Invoke(health);
        if (health > oldHealth) OnHealthIncreased?.Invoke(health);

        if (health == 0)
            OnHealthHitZero?.Invoke();

    }
}
