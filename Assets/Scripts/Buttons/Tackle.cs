using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tackle : MonoBehaviour {

    TurnManager TurnM;
    GameObject SelectedPlayer;

    private void Awake()
    {
        TurnM = Camera.main.GetComponent<TurnManager>();
    }

    public void init()
    {
        SelectedPlayer = TurnM.FindSelctedPlayer();
        SelectedPlayer.GetComponent<PlayerInfo>().CanTackle = true;
    }
}
