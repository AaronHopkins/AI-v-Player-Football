using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour 
{
    static Dictionary<string, List<TacticsMove>> units = new Dictionary<string, List<TacticsMove>>();
    public static Queue<string> Turns = new Queue<string>();
    public static List<TacticsMove> Team = new List<TacticsMove>();

    public Text Player_Score;
    public Text Enemy_Score;
    public Text Message_Output;
    public bool chosen = false;
    public static int EnemyScore = 0;
    public static int PlayerScore = 0;
    public static bool Ready = true;
    public static bool Found = false;
    public static bool tackleFail = false;
    public static bool tackleSucc = false;
    public static bool EnTackleFail = false;
    public static bool EnTackleSucc = false;
    public static bool Finished = false;
    public static int Roll;
    GameObject target;
    GameObject Game;

    // Use this for initialization
    void Start () 
	{
        Game = Camera.main.GetComponent<Loader>().GetGameManager();
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (Team.Count == 0)
        {
            TeamTurnQueue();
        }

        foreach (TacticsMove player in Team)
        {
            if (player.tag == "Player")
            {

                if (player.GetComponent<PlayerInfo>().Selected)
                {
                    foreach (TacticsMove P in Team)
                    {
                        if (P == player)
                        {
                            //Do Nothing
                        }
                        else
                        {
                            P.NotSelected = true;
                        }
                    }
                }
            }
            else
            {
                if (player.GetComponent<Enamy>().Selected)
                {
                    foreach (TacticsMove P in Team)
                    {
                        if (P == player)
                        {
                            //Do Nothing
                        }
                        else
                        {
                            P.NotSelected = true;
                        }
                    }
                }
            }
        }

        if (Turns.Peek() == "Enemy")
        {
            if (Ready)
            {
                Enamy EnWithBall = null;
                
                foreach(TacticsMove en in Team)
                {
                    if (en.GetComponent<Enamy>().IHaveIt)
                    {
                        EnWithBall = en.GetComponent<Enamy>();
                        chosen = true;
                    }
                }

                if(!chosen)
                {
                    int rand = Random.Range(0, Team.Count);
                    Ready = false;
                    Team[rand].GetComponent<Enamy>().Selected = true;
                    Team[rand].GetComponent<Enamy>().NumTackles = 0;
                }
                else
                {
                    EnWithBall.Selected = true;
                    chosen = false;
                }
                
            }
            
        }
	}


    static void TeamTurnQueue()
    {
        List<TacticsMove> teamList = units[Turns.Peek()];

        foreach (TacticsMove unit in teamList)
        {
            Team.Add(unit);
        }

        StartTurn();
    }

    public static void StartTurn()
    {
        if (Team.Count > 0)
        {
            foreach (TacticsMove Player in Team)
            {
                Player.idle = false;
                Player.transform.GetComponent<SpriteRenderer>().color = Color.white;
                Player.BeginTurn();
            }
            //Team.Peek().BeginTurn();
        }
    }

    public static void EndTurn()
    {
        foreach(TacticsMove player in Team.ToArray())
        {
            if(player.idle)
            {
                player.EndTurn();
                Team.Remove(player);
            }
        }

        //TacticsMove unit = Team.Dequeue();
        //unit.EndTurn();

        if (Team.Count > 0)
        {
            StartTurn();
        }
        else
        {
            string team = Turns.Dequeue();
            Turns.Enqueue(team);
            TeamTurnQueue();
        }
    }

    // Adds all the units on the bored to the Dictenery depending on their tag
    public static void AddUnits(TacticsMove unit)
    {
        List<TacticsMove> list;

        if (!units.ContainsKey(unit.tag))
        {
            list = new List<TacticsMove>();
            units[unit.tag] = list;

            if (!Turns.Contains(unit.tag))
            {
                Turns.Enqueue(unit.tag);
            }
        }
        else
        {
            list = units[unit.tag];
        }

        list.Add(unit);
    }

    public static void ResetSelctablaty()
    {
        foreach(TacticsMove player in Team)
        {
            player.NotSelected = false;
        }
    }

    public void NextTurn()
    {
       foreach(TacticsMove player in Team.ToArray())
       {
            player.GetComponent<PlayerInfo>().idle = true;
            player.GetComponent<PlayerInfo>().CanPass = false;
            player.GetComponent<PlayerInfo>().CanShoot = false;
            player.GetComponent<PlayerInfo>().CanTackle = false;
            Team.Remove(player);
       }

        string team = Turns.Dequeue();
        Turns.Enqueue(team);
        TeamTurnQueue();

    }

    public void EndPlayersTurn()
    {
        GameObject SelectedPlayer = FindSelctedPlayer();
        TacticsMove pl = SelectedPlayer.GetComponent<TacticsMove>();
        pl.idle = true;
        
        SelectedPlayer.GetComponent<PlayerInfo>().Moved = false;

        if (SelectedPlayer.transform.tag == "Player")
        {
            SelectedPlayer.transform.GetComponent<PlayerInfo>().Selected = false;
        }

        Team.Remove(pl);

        if (Team.Count > 0)
        {
            StartTurn();
        }
        else
        {
            string team = Turns.Dequeue();
            Turns.Enqueue(team);
            TeamTurnQueue();
        }

    }

    public GameObject FindSelctedPlayer()
    {

        foreach(TacticsMove player in Team)
        {
            if (player.transform.tag == "Player")
            {
                if (player.GetComponent<PlayerInfo>().Selected == true)
                {
                    target = player.gameObject;
                    Found = true;
                }
                else if(!Found)
                {
                    target = null;
                }
            }
            else if(player.transform.tag == "Enemy")
            {
                if (player.GetComponent<Enamy>().Selected == true)
                {
                    target = player.gameObject;
                    Found = true;
                }
                else if(!Found)
                {
                    target = null;
                }
            }
            
        }
        Found = false;

        return target;
    }

    public Text GetMessgae()
    {
        Text mes = Message_Output.GetComponent<Text>();

        return mes;
    }

    public void DestroyAll()
    {

        //if (Turns.Peek() == "Player" || Turns.Peek() == "Enemy")
        //{
        //    GameObject SelectedPlayer = FindSelctedPlayer();

        //    if (SelectedPlayer.transform.tag == "Player")
        //    {
        //        SelectedPlayer.GetComponent<TacticsMove>().EndTurn();
        //        SelectedPlayer.transform.position = SelectedPlayer.GetComponent<PlayerInfo>().StartPos;
        //    }
        //    else
        //    {
        //        SelectedPlayer.GetComponent<TacticsMove>().EndTurn();
        //        SelectedPlayer.transform.position = SelectedPlayer.GetComponent<Enamy>().StartPos;
        //    }
        //}

        foreach (TacticsMove obj in Team.ToArray())
        {
            obj.EndTurn();

            if (obj.transform.tag == "Player")
            {
                Team.Remove(obj);
                obj.transform.position = obj.GetComponent<PlayerInfo>().StartPos;
            }
            else
            {
                Team.Remove(obj);
                obj.transform.position = obj.GetComponent<Enamy>().StartPos;
            }
        }

        string team = Turns.Dequeue();
        Turns.Enqueue(team);
        TeamTurnQueue();

        foreach (TacticsMove obj in Team)
        {

            if (obj.transform.tag == "Player")
            {
                obj.transform.position = obj.GetComponent<PlayerInfo>().StartPos;
            }
            else
            {
                obj.transform.position = obj.GetComponent<Enamy>().StartPos;
            }
        }
    }
}
