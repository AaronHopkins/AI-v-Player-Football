using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGameManager : MonoBehaviour {

    public CreateBoard BoardScript;

    void Awake()
    {
        BoardScript = GetComponent<CreateBoard>();
        InitGame();

    }

    void InitGame()
    {
        BoardScript.SetUpMainMenu();
    }

}
