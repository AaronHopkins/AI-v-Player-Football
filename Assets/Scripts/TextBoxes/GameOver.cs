using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    public Text Game_Over;

    void Start ()
    {
        Game_Over = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(TurnManager.Finished)
        {
            if(TurnManager.EnemyScore > TurnManager.PlayerScore)
            {
                Game_Over.text = "Game Over Enemy Team Wins!!";
            }
            else if(TurnManager.PlayerScore > TurnManager.EnemyScore)
            {
                Game_Over.text = "Game Over You Have Won!!";
            }
            else
            {
                Game_Over.text = "Game Over Its A Tie!!";
            }
        }
	}
}
