using UnityEngine;

public class EnemyHitbox : Collidable
{
    public int damagePoint = 1;
    public float pushForce = 2.5f;

    protected override void OnCollide(Collider2D collided)
    {
        if (collided.CompareTag("Fighter") && collided.name == "Player")
        {
            // Send Damage
            var dmg = new Damage
            {
                pushForce = pushForce,
                damageAmount = damagePoint,
                origin = transform.position
            };
            collided.SendMessage("ReceiveDamage", dmg);
        }
    }
}
