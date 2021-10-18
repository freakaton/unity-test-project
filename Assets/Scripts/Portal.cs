using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Portal : Collidable
{
    public string sceneName;

    protected override void OnCollide(Collider2D collided)
    {
        if (collided.name == "Player") {
            GameManager.instance.SaveState();
            SceneManager.LoadScene(sceneName);
        }
    }
}
