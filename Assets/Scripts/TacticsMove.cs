using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticsMove : MonoBehaviour 
{
    public bool turn = false;
    public TurnManager TurnM;
    GameObject Game;

    List<Node> selectableNodes = new List<Node>();
    GameObject[] Nodes;

    Stack<Node> path = new Stack<Node>();
    Node currentNode;

    public bool moving = false;
    public bool idle = false;
    public bool NotSelected = false;
    public int move = 3;
    public float moveSpeed = 2;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    private Collider2D Self;

    bool fallingDown = false;
    bool movingEdge = false;

    public Node actualTargetNode;

    protected void Init()
    {
        Nodes = GameObject.FindGameObjectsWithTag("Floor");
        TurnM = Camera.main.GetComponent<TurnManager>();
        Game = Camera.main.GetComponent<Loader>().GetGameManager();
        TurnManager.AddUnits(this);
    }

    public void GetCurrentNode()
    {
        currentNode = GetTargetNode(gameObject);
        currentNode.Current = true;
    }

    public Node GetTargetNode(GameObject target)
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(target.transform.position, Vector3.back, 10f);
        Node Node = null;

        foreach(RaycastHit2D obj in hit)
        {
            if(obj.transform.tag == "Floor" || obj.transform.tag == "GoalL")
            {
                Node = obj.collider.GetComponent<Node>();
            }
        }    
        return Node;
    }

    public void ComputeAdjacencyLists( Node target)
    {
        //Nodes = GameObject.FindGameObjectsWithTag("Floor");

        foreach (GameObject Node in Nodes)
        {
            Node N = Node.GetComponent<Node>();
            N.FindNeighbors(target);
        }
    }

    public void FindSelectableNodes()
    {
        ComputeAdjacencyLists(null);
        GetCurrentNode();

        Queue<Node> Queue = new Queue<Node>();

        Queue.Enqueue(currentNode);
        currentNode.visited = true;

        while (Queue.Count > 0)
        {
            Node N = Queue.Dequeue();

            selectableNodes.Add(N);
            N.MovementTrigger = true;

            if (N.distance < move)
            {
                foreach (Node Node in N.adjacencyList)
                {
                    if (!Node.visited)
                    {
                        Node.parent = N;
                        Node.visited = true;
                        Node.distance = 1 + N.distance;
                        Queue.Enqueue(Node);
                    }
                }
            }
        }
    }

    public void MoveToNode(Node Node)
    {
        path.Clear();
        Node.target = true;
        moving = true;

        Node next = Node;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    public void Move()
    {
        if (path.Count > 0)
        {
            Node N = path.Peek();
            Vector3 target = N.transform.position;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                    SetHeading(target);
                    SetVelocity();


                //Locomotion
                transform.right = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //Node center reached
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            RemoveSelectableNodes();
            moving = false;
            if(transform.tag == "Player")
            {
                transform.GetComponent<PlayerInfo>().Moved = true;
            }
            TurnManager.Ready = true;
            TurnManager.ResetSelctablaty();
            if (transform.tag == "Enemy")
            {
                transform.GetComponent<Enamy>().CheckInRange(Vector3.left);
                transform.GetComponent<Enamy>().CheckInRange(Vector3.right);
                idle = true;
                transform.GetComponent<Enamy>().NotSelected = false;
                TurnManager.EndTurn();
            }
        }
    }

    protected void RemoveSelectableNodes()
    {
        if (currentNode != null)
        {
            currentNode.Current = false;
            currentNode = null;
        }

        foreach (Node Node in selectableNodes)
        {
            Node.Reset();
        }

        selectableNodes.Clear();
    }

    void SetHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetVelocity()
    {
        velocity = heading * moveSpeed;
    }

    protected Node FindLowestF(List<Node> list)
    {
        Node lowest = list[0];

        foreach (Node N in list)
        {
            if (N.F < lowest.F)
            {
                lowest = N;
            }
        }

        list.Remove(lowest);

        return lowest;
    }

    protected Node FindEndNode(Node N)
    {
        Stack<Node> tempPath = new Stack<Node>();

        Node next = N.parent;
        while (next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= move)
        {
            return N.parent;
        }

        Node endNode = null;
        for (int i = 0; i <= move; i++)
        {
            endNode = tempPath.Pop();
        }

        return endNode;
    }

    protected void FindPath(Node target)
    {
        ComputeAdjacencyLists(target);
        GetCurrentNode();

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(currentNode);
        currentNode.H = Vector3.Distance(currentNode.transform.position, target.transform.position);
        currentNode.F = currentNode.H;

        while (openList.Count > 0)
        {
            Node N = FindLowestF(openList);

            closedList.Add(N);

            if (N == target)
            {
                actualTargetNode = FindEndNode(N);
                MoveToNode(actualTargetNode);

                RaycastHit2D hit = Physics2D.Raycast(actualTargetNode.transform.position, Vector3.back, 100f);

                if(hit.transform.tag == "Enemy" || hit.transform.tag == "Player")
                {
                    actualTargetNode = FindEndNode(actualTargetNode);
                    MoveToNode(actualTargetNode);
                }
                return;
            }

            foreach (Node Node in N.adjacencyList)
            {
                if (closedList.Contains(Node))
                {
                    //Do nothing
                }
                else if (openList.Contains(Node))
                {
                    float tempG = N.G + Vector3.Distance(Node.transform.position, N.transform.position);

                    if (tempG < Node.G)
                    {
                        Node.parent = N;

                        Node.G = tempG;
                        Node.F = Node.G + Node.H;
                    }
                }
                else
                {
                    Node.parent = N;

                    Node.G = N.G + Vector3.Distance(Node.transform.position, N.transform.position);
                    Node.H = Vector3.Distance(Node.transform.position, target.transform.position);
                    Node.F = Node.G + Node.H;

                    openList.Add(Node);
                }
            }
        }
    }

    public void BeginTurn()
    {
        turn = true;
    }

    public void EndTurn()
    {
        turn = false;
    }

    //public void DestroySelf()
    //{
    //    FootballInfo football = GameObject.FindGameObjectWithTag("Football").GetComponent<FootballInfo>();

    //    Destroy(gameObject);
    //    football.NumDestroyed++;
    //}
}
