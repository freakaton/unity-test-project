using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public FloatingTextManager floatingTextManager;
    public RectTransform healthBar;
    public GameObject hud;
    public GameObject menu;
    public GameObject cam;
    public Player player;
    public Weapon weapon;
    public Animator deathMenuAnim;

    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public int coins;
    public int experience = 0;
    public List<int> xpTable;
    public List<int> weaponPrices;

    // Experience System
    public int GetCurrentLevel()
    {
        var ret = 0;
        var add = 0;

        while (experience >= add)
        {
            add += xpTable[ret];
            ret++;
            if (ret == xpTable.Count)
            {
                return ret;  // Max Level reached
            }
        }

        return ret;
    }

    public int GetXpToLevel(int level)
    {
        return xpTable.Take(level).Aggregate(0, (acc, x) => acc + x);
    }

    public void OnHitPointChange()
    {
        var ratio = (float)player.healthPoint / (float)player.maxHp;
        healthBar.localScale = new Vector3(1, ratio, 1);
    }

    public void GrantXp(int xp)
    {
        var curLevel = GetCurrentLevel();
        experience += xp;
        if (curLevel < GetCurrentLevel())
        {
            OnLevelUp();
        }
    }

    private void OnLevelUp()
    {
        ShowText("You got level up!", player.transform.position);
        player.OnLevelUp();
    }

    private void Awake() {
        if (GameManager.instance != null) {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            Destroy(cam);

            return;
        }
        instance = this;
        SceneManager.sceneLoaded += LoadState;
    }
    
    public void SaveState()
    {
        // In State we save pesos, experience, player sprite and weapon level
        var dumpedState = "";
        dumpedState += coins.ToString() + "|";
        dumpedState += experience.ToString() + "|";
        dumpedState += player.GetComponent<SpriteRenderer>().sprite.name + "|";
        dumpedState += weapon.weaponLevel;
        PlayerPrefs.SetString("savedState", dumpedState);
        PlayerPrefs.Save();
    }
    private void LoadState(Scene scene, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("savedState")) return;

        string[] p = PlayerPrefs.GetString("savedState").Split('|');
        coins = int.Parse(p[0]);
        experience = int.Parse(p[1]);
        if (GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());

        weapon.SetWeaponLevel(int.Parse(p[3]));
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            var menuAnimator = GameObject.Find("Menu").GetComponent<Animator>();
            if (menuAnimator.GetCurrentAnimatorStateInfo(0).IsName("menu_hidden"))
            {
                menu.GetComponent<CharacterMenu>().UpdateMenu();
                menuAnimator.SetTrigger("show");
            }
            else if (menuAnimator.GetCurrentAnimatorStateInfo(0).IsName("menu_showing"))
            {
                menuAnimator.SetTrigger("hide");
            }
        }
    }

    // , 25, Color.green, transform.position, Vector3.up * 50, 1.5f
    public void ShowText(string msg, Vector3 position, int fontSize=15,
     Color? color=null, Vector3? motion=null, float duration=1.5f)
    {
        var sColor = color ?? Color.green;
        var sMotion = motion ?? Vector3.up * 50;

        floatingTextManager.Show(msg, fontSize, sColor, position, sMotion, duration);
    }

    public bool TryUpgradeWeapon()
    {
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;
        
        if (coins >= weaponPrices[weapon.weaponLevel])
        {
            coins -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    public void Respawn()
    {
        SceneManager.LoadScene("Main");
        coins = 1;
        weapon.SetWeaponLevel(0);
        SaveState();
        deathMenuAnim.SetTrigger("Hide");
        player.Respawn();
    }
}
