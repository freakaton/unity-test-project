using UnityEngine;

public class Chest : Collectible
{
    public Sprite emptyChest;
    public int coinAmount = 5;

    protected override void OnCollect(Collider2D player)
    {
        if (!isCollected) {
            isCollected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.coins += coinAmount;
            GameManager.instance.ShowText("Player got " + coinAmount + " coins!", transform.position);
        }
    }
}
