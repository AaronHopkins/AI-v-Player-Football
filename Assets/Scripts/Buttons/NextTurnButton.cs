using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextTurnButton : MonoBehaviour {

    TurnManager TurnM;

    private void Awake()
    {
         TurnM = Camera.main.GetComponent<TurnManager>();
    }

    public void SetNextTurn ()
    {
        TurnM.NextTurn(); 
    }

}
