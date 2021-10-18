using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int maxHp = 10;
    public int healthPoint = 10;
    public float immuneAmount = 1.0f;
    public float pushRecoverySpeed = 0.05f;
    private float lastReceivedDamage;
    protected Vector3 pushDirection;

    public virtual void ReceiveDamage(Damage dmg)
    {
        if (lastReceivedDamage + immuneAmount < Time.time)
        {
            healthPoint -= dmg.damageAmount;
            lastReceivedDamage = Time.time;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
            GameManager.instance.ShowText(
                "- " + dmg.damageAmount, gameObject.transform.position,
                color: Color.red, duration: 0.5f
            );
            if (healthPoint <= 0)
            {
                healthPoint = 0;
                Death();
            }
        }
    }

    protected virtual void Death()
    {
        Debug.Log(this.name + " DEAD.");
    }
}
