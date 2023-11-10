using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pass : MonoBehaviour {

    TurnManager TurnM;
    GameObject SelectedPlayer;

    void Awake () {
        TurnM = Camera.main.GetComponent<TurnManager>();
    }
	
	public void init () {
        SelectedPlayer = TurnM.FindSelctedPlayer();
        SelectedPlayer.GetComponent<PlayerInfo>().CanPass = true;
    }
}
