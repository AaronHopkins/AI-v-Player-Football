using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enamy : TacticsMove {

    public Vector3 StartPos;
    public bool Selected = false;
    public bool IHaveIt = false;
    public bool CanTackle = false;
    public bool CanBeTackled = false;
    public bool TackleSuccessful = false;
    public int NumTackles = 0;
    bool InRange = false;
    GameObject target;
    GameObject[] targets;

    // Use this for initialization
    void Start()
    {
        Init();
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!turn && TurnManager.Finished)
        {
            return;
        }

        if (Selected)
        {
            if (!moving)
            {
                FindTarget();
                CalculatePath();
                FindSelectableNodes();
                actualTargetNode.target = true;
            }
            else
            {
                Move();
            }
        }       

        if(idle)
        {
            Selected = false;
        }
    }

    void CalculatePath()
    {
        Node targetNode = GetTargetNode(target);
        FindPath(targetNode);
    }

    void FindTarget()
    {
        target = GameObject.FindGameObjectWithTag("Football");

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        if (IHaveIt)
        {
            targets = GameObject.FindGameObjectsWithTag("GoalL");

            foreach (GameObject obj in targets)
            {
                float d = Vector3.Distance(transform.position, obj.transform.position);

                if (d < distance)
                {
                    distance = d;
                    nearest = obj;
                }
            }
            target = nearest;
        }
    }

    public void CheckInRange(Vector3 direction)
    {
        FootballInfo football = GameObject.FindGameObjectWithTag("Football").GetComponent<FootballInfo>();

        RaycastHit2D[] Hits = Physics2D.RaycastAll(transform.position, direction, 4f);

        foreach (RaycastHit2D item in Hits)
        {
            if (item.transform.tag == "GoalL")
            {
                football.transform.position = item.transform.position;
                football.StickToPlayer(item.transform.gameObject);
                IHaveIt = false;
                football.PickedUp = false;
                transform.position = StartPos;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        FootballInfo football = GameObject.FindGameObjectWithTag("Football").GetComponent<FootballInfo>();

        RaycastHit2D[] hit = Physics2D.RaycastAll(other.transform.position, Vector3.back, 10f);

        foreach (RaycastHit2D obj in hit)
        {
            if (obj.transform.tag == "Football" && football.PickedUp == false)
            {
                football.StickToPlayer(gameObject);
                football.PickedUp = true;
                IHaveIt = true;

            }
            else if (obj.transform.tag == "Player" && turn == true)
            {
                if (obj.transform.GetComponent<PlayerInfo>().IHaveIt == true && turn && NumTackles < 1)
                {
                    TackleUnit();
                    if (TackleSuccessful)
                    {
                        obj.transform.GetComponent<PlayerInfo>().IHaveIt = false;
                        TackleSuccessful = false;
                    }
                }
            }
        }
    }

    public void TackleUnit()
    {
        FootballInfo football = GameObject.FindGameObjectWithTag("Football").GetComponent<FootballInfo>();
        int rand = Random.Range(1, 6);

        NumTackles++;
        if (rand < 5)
        {
            TurnManager.EnTackleFail = true;
            TurnManager.Roll = rand;
        }
        else
        {
            TurnManager.EnTackleSucc = true;
            TurnManager.Roll = rand;

            football.StickToPlayer(gameObject);
            football.PickedUp = true;
            TackleSuccessful = true;
            gameObject.GetComponent<Enamy>().IHaveIt = true;

        }
    }
}

