using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnamyScore : MonoBehaviour {

    public Text Enemy_Score;
    int One = 1;

    // Use this for initialization
    void Start () {

        Enemy_Score = GetComponent<Text>();

        Enemy_Score.text = "Enamy Team: " + TurnManager.EnemyScore.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        FootballInfo football = GameObject.FindGameObjectWithTag("Football").GetComponent<FootballInfo>();

        if (football.EnScore)
        {
            TurnManager.EnemyScore++;
            Enemy_Score.text = "Enemy Team: " + TurnManager.EnemyScore.ToString();
            football.EnScore = false;
        }
    }
}
