using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour
{

    //*********************************
    // Unity methods
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 1.6(Level Manager) Cuando se toque el collider se quitara el primer bloque y luego se añadira uno.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LevelManager.sharedInstanceLevelManager.RemoveLevelBlock();
            LevelManager.sharedInstanceLevelManager.AddLevelBlock();
        }
    }
    //*********************************
}
