using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    public Text TimerText;
    private float time = 480;

	void Start ()
    {
        TimerText = GetComponent<Text>();
        CountdownTimer();
	}

    private void Update()
    {
        if(TimerText.text == "00:00")
        {
            TurnManager.Finished = true;
        }

        if(TimerText.text == "-01:-10")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void CountdownTimer()
    {
        if(TimerText != null)
        {
            time = 480;
            TimerText.text = "8:00";
            InvokeRepeating("UpdateTimer", 0f, 0.01667f);
        }
    }

    void UpdateTimer()
    {
        if(TimerText != null)
        {
            time -= Time.deltaTime;
            string minutes = Mathf.Floor(time / 60).ToString("00");
            string seconds = (time % 60).ToString("00");
            TimerText.text = minutes + ":" + seconds;
        }
    }
}
