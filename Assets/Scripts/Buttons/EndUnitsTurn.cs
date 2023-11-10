using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndUnitsTurn : MonoBehaviour {

    TurnManager TurnM;

	// Use this for initialization
	void Awake ()
    {
        TurnM = Camera.main.GetComponent<TurnManager>();
	}

    public void intit()
    {
        TurnM.EndPlayersTurn();
    }
}
