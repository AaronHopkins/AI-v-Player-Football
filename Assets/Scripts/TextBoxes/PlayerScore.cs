using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

    public Text Player_Score;
    public int PlScore = 0;

    // Use this for initialization
    void Start () {
               
        Player_Score = GetComponent<Text>();

        Player_Score.text = "Player Team: " + TurnManager.PlayerScore.ToString();
    }

    // Update is called once per frame
    void Update () {

		FootballInfo football = GameObject.FindGameObjectWithTag("Football").GetComponent<FootballInfo>();

        if (football.PlScore)
        {
            TurnManager.PlayerScore++;
            Player_Score.text = "Player Team: " + TurnManager.PlayerScore.ToString();
            football.PlScore = false;
        }
    }
}
