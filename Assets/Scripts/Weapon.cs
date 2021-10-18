using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    public int[] damagePoint = {1, 2, 3, 4, 5, 6, 7};
    public float[] pushForce = {2.0f, 2.2f, 2.4f, 2.6f, 2.8f, 3.0f, 3.2f};

    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    private Animator anim;
    private float cooldown = 0.5f;
    private float lastSwing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();   
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (lastSwing + cooldown < Time.time)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollide(Collider2D collided)
    {
        if (collided.CompareTag("Fighter"))
        {
            if(collided.name == "Player") return;
            
            // Send Damage
            var dmg = new Damage
            {
                pushForce=pushForce[weaponLevel],
                damageAmount=damagePoint[weaponLevel],
                origin=transform.position
            };

            collided.SendMessage("ReceiveDamage", dmg);
        }
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
        
    }

    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }

    public Sprite GetWeaponSprite()
    {
        return spriteRenderer.sprite;
    }
    
    private void Swing()
    {
        anim.SetTrigger("Swing");
    }

}
