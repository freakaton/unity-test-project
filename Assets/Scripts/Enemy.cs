using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    public int xpValue = 1;


    // Logic
    public float chaseLenght = 5.0f;
    public float triggerLength = 1.0f;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    // Hit box 
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];
    public ContactFilter2D contactFilter;

    protected override void Start() {
        base.Start();
        // TIP: Find game object in whole project by name
        playerTransform = GameObject.Find("Player").transform;
        startingPosition = transform.position;
        // TIP: Get child game objects
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    protected void FixedUpdate()
    {
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLenght)
        {
            chasing = Vector3.Distance(playerTransform.position, startingPosition) < triggerLength;
            if (chasing)
            {
                if(!collidingWithPlayer)
                {
                    UpdateMotor((playerTransform.position - transform.position).normalized);
                }
            }
            else {
                UpdateMotor((startingPosition - transform.position));
            }
        } else {
            chasing = false;
            UpdateMotor((startingPosition - transform.position));
        }

        collidingWithPlayer = false;
        boxCollider.OverlapCollider(contactFilter, hits);
        for (var i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null) continue;
            if (hits[i].CompareTag("Fighter") && hits[i].name == "Player")
            {
                collidingWithPlayer = true;
            }
            hits[i] = null;
        }
        
    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + " xp", transform.position, color: Color.green);
    }
}
