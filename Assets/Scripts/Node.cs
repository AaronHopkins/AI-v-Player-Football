using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public bool walkable = true;
    public bool MovementTrigger = false;
    public bool target = false;
    public bool Current = false;


    public List<Node> adjacencyList = new List<Node>();

    //Needed BFS (breadth first search)
    public bool visited = false;
    public Node parent = null;
    public int distance = 0;
    public Vector2 position;

    public float F;
    public float G;
    public float H;
    public int Number;
    public int x;
    public int y;
    public string[] Conected;
    public int[] IDArray;
    public int TargetX;
    public int TargetY;

    private SpriteRenderer SpriteR;
    private string SpriteName;
    private string OrSpriteName;

    void Awake()
    {
        SpriteR = gameObject.GetComponent<SpriteRenderer>();
        position = transform.position;
        F = 0;
        G = 1;
        H = 0;
    }

    private void Update()
    {
        if (Current)
        {
            if(this.name == "Floor 1")
            {
                OrSpriteName = "Grass";
            }
            else if (this.name == "Floor 2")
            {
                OrSpriteName = "Grass 2";
            }

            transform.GetComponent<SpriteRenderer>().color = Color.cyan;
        }        
        else if (target)
        {
            transform.GetComponent<SpriteRenderer>().color = Color.gray;            
        }
        else if (MovementTrigger)
        {
            transform.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    //private void Start()
    //{
        
    //    Conected = new string[8];
    //    IDArray = new int[8];
    //    Vector3 Node = transform.position;
    //    int array = 0;
    //    Collider2D[] collidersHit = Physics2D.OverlapCircleAll(Node, 1.3f);

    //    for (int i = 0; i < collidersHit.Length; i++)
    //    {
    //        if (Number == collidersHit[i].GetComponent<Node>().Number)
    //        {
    //            array--;
    //        }
    //        else
    //        {
    //            Conected[array] = collidersHit[i].name;
    //            IDArray[array] = collidersHit[i].GetComponent<Node>().Number;
    //        }
    //        array++;
    //    }

    //}

    public void Reset()
    {
        adjacencyList.Clear();

        MovementTrigger = false;
        target = false;
        Current = false;

        visited = false;
        parent = null;
        distance = 0;

        F = G = H = 0;
    }

    public void FindNeighbors(Node target)
    {
        Reset();

        CheckTile(Vector3.up, target);
        CheckTile(Vector3.down, target);
        CheckTile(Vector3.right, target);
        CheckTile(Vector3.left, target);
    }

    public void CheckTile(Vector3 direction, Node target)
    {        
        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(transform.position + direction, 0.2f);

        foreach (Collider2D item in collidersHit)
        {
            
            Node N = item.GetComponent<Node>();
            if (N != null && N.walkable)
            {
                RaycastHit hit;

                if (!Physics.Raycast(N.transform.position, Vector3.back, out hit, 1) || (N == target))
                {

                    adjacencyList.Add(N);
                }
            }
        }
    }

    public void SetF()
    {
        F = G + H;
    }

    public void SetTarget(int x, int y)
    {
        TargetX = x;
        TargetY = y;
        SetH();
        SetF();
    }

    public void SetH()
    {
        H = Mathf.Sqrt(Mathf.Pow((x - TargetX), 2) + Mathf.Pow((y - TargetY), 2));
    }
}
