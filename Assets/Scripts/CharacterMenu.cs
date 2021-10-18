using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public Text levelText, hitPointText, pesosText, upgradeCostText, xpText;
    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;


    public void OnUpgradeMenu()
    {
        if (GameManager.instance.TryUpgradeWeapon())
            UpdateMenu();
    }
    
    
    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
                currentCharacterSelection = 0;
            
        }
        else
        {
            currentCharacterSelection--;
            if (currentCharacterSelection < 0)
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;
        }
        OnSelectionChanged();
    }

    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }

    public void UpdateMenu()
    {
        var curLevel = GameManager.instance.GetCurrentLevel();
        var curXp = GameManager.instance.experience;

        pesosText.text = GameManager.instance.coins.ToString();
        hitPointText.text = GameManager.instance.player.healthPoint.ToString();
        xpText.text = (curXp - GameManager.instance.GetXpToLevel(curLevel - 1)).ToString();
        weaponSprite.sprite = GameManager.instance.weapon.GetWeaponSprite();
        levelText.text = curLevel.ToString();
        upgradeCostText.text = (GameManager.instance.xpTable[curLevel - 1].ToString());
    }
}
