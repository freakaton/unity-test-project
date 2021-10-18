using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson : Collidable
{
    public string message;
    private float speakCooldown = 5.0f;
    private float lastSpeak;

    protected override void OnCollide(Collider2D collided)
    {
        if (collided.name != "Player") return;
        if (Time.time < lastSpeak + speakCooldown) return;
        GameManager.instance.ShowText(
            message,
            gameObject.transform.position,
            color: Color.cyan,
            duration: 3.0f,
            motion: Vector3.up * 20,
            fontSize: 25
        );
        lastSpeak = Time.time;
    }
}
