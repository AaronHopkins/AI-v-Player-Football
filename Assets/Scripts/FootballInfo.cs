using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FootballInfo : MonoBehaviour{

    public Vector3 StartPos;
    public bool PickedUp;
    public bool scored;
    public bool EnScore = false;
    public bool PlScore = false;
    public bool Reset = false;
    CreateBoard Board;
    GameObject Player;
    TurnManager TurnM;
    GameObject Game;

    private void Start()
    {
        TurnM = Camera.main.GetComponent<TurnManager>();
        Game = Camera.main.GetComponent<Loader>().GetGameManager();
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update() {

        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector3.back, 10f);

        foreach(RaycastHit2D obj in hit)
        {
            if(obj.transform.tag == "GoalL")
            {
                EnScore = true;
                Reset = true;
                TurnM.DestroyAll();

            }
            else if(obj.transform.tag == "GoalR")
            {
                PlScore = true;
                Reset = true;
                TurnM.DestroyAll();

            }            
        }

        if(Reset)
        {
            transform.position = StartPos;
            Reset = false;
        }

        if (PickedUp)
        {
            gameObject.transform.position = Player.transform.position;
        }
    }
    
    public void StickToPlayer(GameObject obj)
    {
        Player = obj;
    }
}
