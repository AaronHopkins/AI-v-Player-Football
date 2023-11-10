using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateBoard : MonoBehaviour {

    public int Colums = 13;
    public int Rows = 9;
    public GameObject[] FloorTiles;
    public GameObject[] PlayerTiles;
    public GameObject[] EnamyTiles;
    public GameObject[] OutterTiles;
    public GameObject[] GoalsTiles;
    public GameObject FootballTile;
    public Node NodeScript;
    private Transform BoradHolder;
    private Transform Units;
    public List<GameObject> Nodes = new List<GameObject>();
    private List<Vector3> GridPositions = new List<Vector3>();
    private bool Outer = false;

    void ClearBoard()
    {
        BoradHolder = new GameObject("Borad").transform;
        Units = new GameObject("Units").transform;

        GridPositions.Clear();

        for (int x = 0; x < Colums; x++)
        {
            for (int y = -1; y < Rows; y++)
            {
                GridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    public void BoradSetup()
    {


        int FT;
        int count = -1;
        int FTcount = 0;

        for (int x = -1; x < Colums + 1; x++)
        {
            for (int y = -1; y < Rows + 1; y++)
            {
                count++;
                

                if (x % 2 == 0)
                {
                    FT = 0;
                    FTcount = 1;
                }
                else
                {
                    FT = 1;
                    FTcount = 2;
                }

                GameObject Create = FloorTiles[FT];
                Create.name = "Floor" + FTcount;
                FloorTiles[FT].layer = 9;

                if (x == -1 || x == Colums || y == -1 || y == Rows)
                {
                    int rand = Random.Range(0, OutterTiles.Length);
                    Create = OutterTiles[rand];
                    Outer = true;
                }

                if (x == -1)
                {
                    if (y == 3)
                    {
                        Create = GoalsTiles[2];
                        Create.name = "GoalL";
                    }
                    else if (y == 4)
                    {
                        Create = GoalsTiles[1];
                        Create.name = "GoalL";
                    }
                    else if (y == 5)
                    {
                        Create = GoalsTiles[0];
                        Create.name = "GoalL";
                    }
                }

                if (x == 13)
                {
                    if (y == 3)
                    {
                        Create = GoalsTiles[5];
                        Create.name = "GoalR";
                        GoalsTiles[5].layer = 9;
                    }
                    else if (y == 4)
                    {
                        Create = GoalsTiles[4];
                        Create.name = "GoalR";
                        GoalsTiles[4].layer = 9;
                    }
                    else if (y == 5)
                    {
                        Create = GoalsTiles[3];
                        Create.name = "GoalR";
                        GoalsTiles[3].layer = 9;
                    }
                }

                if (Outer)
                {
                    GameObject Instance = Instantiate(Create, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    Instance.transform.SetParent(BoradHolder);
                    Outer = false;
                }
                else
                {
                    GameObject Instance = Instantiate(Create, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    Nodes.Add(Instance);
                    NodeScript = Instance.gameObject.GetComponent<Node>();
                    NodeScript.Number = count;
                    Instance.transform.SetParent(BoradHolder);
                }
                
                //NodeScript.x = x;
                //NodeScript.y = y;
            }
        }
    }

    public void PlaceTeam(int Type)
    {

        if (Type == 1)
        {
            GameObject Instance = Instantiate(PlayerTiles[0], new Vector3(4, 4, -0.2f), Quaternion.identity); //Striker
            Instance.transform.SetParent(Units);

            for (int i = 0; i <= 1; i++)//Def
            {
                if (i == 1)
                {
                    Instance = Instantiate(PlayerTiles[0], new Vector3(1, 2, -0.2f), Quaternion.identity);
                    Instance.transform.SetParent(Units);
                }
                else
                {
                    Instance = Instantiate(PlayerTiles[0], new Vector3(1, 6, -0.2f), Quaternion.identity);
                    Instance.transform.SetParent(Units);
                }
            }

            for (int i = 0; i <= 1; i++)//Mid
            {
                if (i == 1)
                {
                    Instance = Instantiate(PlayerTiles[0], new Vector3(2, 3, -0.2f), Quaternion.identity);
                    Instance.transform.SetParent(Units);
                }
                else
                {
                    Instance = Instantiate(PlayerTiles[0], new Vector3(2, 5, -0.2f), Quaternion.identity);
                    Instance.transform.SetParent(Units);
                }
            }

            
        }
    }

    public void PlaceEnamyTeam()
    {
        int Type = 1;
        //Type = Random.Range(1,3);

        if (Type == 1)
        {
            GameObject Instance = Instantiate(EnamyTiles[0], new Vector3(8, 4, -0.2f), Quaternion.identity); //Striker
            Instance.transform.SetParent(Units);

            for (int i = 0; i <= 1; i++)//Def
            {
                if (i == 1)
                {
                    Instance = Instantiate(EnamyTiles[0], new Vector3(11, 2, -0.2f), Quaternion.identity);
                    Instance.transform.SetParent(Units);
                }
                else
                {
                    Instance = Instantiate(EnamyTiles[0], new Vector3(11, 6, -0.2f), Quaternion.identity);
                    Instance.transform.SetParent(Units);
                }
            }

            for (int i = 0; i <= 1; i++)//Mid
            {
                if (i == 1)
                {
                    Instance = Instantiate(EnamyTiles[0], new Vector3(10, 3, -0.2f), Quaternion.identity);
                    Instance.transform.SetParent(Units);
                }
                else
                {
                    Instance = Instantiate(EnamyTiles[0], new Vector3(10, 5, -0.2f), Quaternion.identity);
                    Instance.transform.SetParent(Units);
                }
            }            
        }
    }

    public void PlaceFootball()
    {
            GameObject Instance = Instantiate(FootballTile, new Vector3(6, 4, -0.2f), Quaternion.identity);
            Instance.transform.SetParent(Units);
    }

    public void Destroy()
    {
        foreach(GameObject obj in Nodes)
        {
            Destroy(obj);
        }
    }

    public void SetUpScene()
    {
        ClearBoard();
        BoradSetup();
        PlaceTeam(1);
        PlaceEnamyTeam();
        PlaceFootball();
    }

    public void SetUpMainMenu()
    {
        BoradSetup();
    }

}
