using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : Collidable
{
    protected bool isCollected;


    protected override void OnCollide(Collider2D collided)
    {
        if (collided.name == "Player") {
            OnCollect(collided);
        }
    }

    protected virtual void OnCollect(Collider2D player)
    {
        Debug.Log("Give coin to " + player.name);
    }
}
