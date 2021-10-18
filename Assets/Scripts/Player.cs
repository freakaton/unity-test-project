using UnityEngine;


public class Player : Mover
{
    private bool isAlive = true;
    private void FixedUpdate()
    {
        if (!isAlive) return;

        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        UpdateMotor(new Vector3(x, y, 0));
    }

    public override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive) return;
        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitPointChange();
    }

    public void SwapSprite(int skinId)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
    }

    public void OnLevelUp()
    {
        maxHp++;
        healthPoint = maxHp;
        GameManager.instance.OnHitPointChange();
    }

    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
        {
            OnLevelUp();
        }
    }

    public void Heal(int healAmount)
    {
        if (healthPoint + healAmount > maxHp)
            healAmount = maxHp - healthPoint;

        if (healAmount == 0)
            return;

        healthPoint += healAmount;
        GameManager.instance.OnHitPointChange();
        GameManager.instance.ShowText(
            "+ " + healAmount + " hp",
            transform.position,
            color: Color.red
        );
    }

    protected override void Death()
    {
        GameManager.instance.deathMenuAnim.SetTrigger("Show");
        isAlive = false;
    }

    public void Respawn()
    {
        Heal(maxHp);
        isAlive = true;
        pushDirection = Vector3.zero;
    }
}
