using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public CreateBoard BoardScript;
    public List<PlayerInfo> PlayerList;
    public List<Enamy> EnemyList;

    private bool enemiesMoving;
    [HideInInspector] public bool PlayersTurn = true;

	void Awake ()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        //Assign enemies to a new List of Enemy objects.
        EnemyList = new List<Enamy>();

        BoardScript = GetComponent<CreateBoard>();
        InitGame();

	}

    public void AddPlayerToList(PlayerInfo other)
    {
        PlayerList.Add(other);
    }

    public void AddEnemyToList(Enamy other)
    {
        EnemyList.Add(other);
    }

    void InitGame()
    {
        EnemyList.Clear();
        BoardScript.SetUpScene();
    }

}
