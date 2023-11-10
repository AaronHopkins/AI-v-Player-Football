using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {
    public GameManager gameManager;

    void Awake()
    {
        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
    }

    public GameObject GetGameManager()
    {
        GameObject game = gameManager.gameObject;
        return game;
    }
}
