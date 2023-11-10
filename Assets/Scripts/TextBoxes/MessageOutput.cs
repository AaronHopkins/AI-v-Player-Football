using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageOutput : MonoBehaviour {

    public Text Message_Output;

    private void Start()
    {
        Message_Output = GetComponent<Text>();
    }

    public void Update()
    {

        if (TurnManager.tackleFail)
        {
            Message_Output.text = "";
            Message_Output.text = "You Rolled: " + TurnManager.Roll.ToString() + " You Failed The Tackle!";
            TurnManager.tackleFail = false;
        }

        if (TurnManager.tackleSucc)
        {
            Message_Output.text = "";
            Message_Output.text = "You Rolled: " + TurnManager.Roll.ToString() + " You Successfully Tackled!";
            TurnManager.tackleSucc = false;
        }

        if (TurnManager.EnTackleFail)
        {
            Message_Output.text = "";
            Message_Output.text = "Enemy Rolled: " + TurnManager.Roll.ToString() + " They Failed The Tackle!";
            TurnManager.tackleFail = false;
        }

        if (TurnManager.EnTackleSucc)
        {
            Message_Output.text = "";
            Message_Output.text = "Enemy Rolled: " + TurnManager.Roll.ToString() + " They Successfully Tackled!";
            TurnManager.tackleSucc = false;
        }
    }
}
