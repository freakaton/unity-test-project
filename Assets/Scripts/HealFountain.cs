using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealFountain : Collidable
{
    public float healCooldown = 1f;
    public int healAmount = 3;
    private float lastHeal;


    protected override void OnCollide(Collider2D collided)
    {
        if (collided.CompareTag("Fighter"))
        {
            if (Time.time > lastHeal + healCooldown)
            {
                GameManager.instance.player.Heal(healAmount);
                lastHeal = Time.time;
            }
        }
    }
}
