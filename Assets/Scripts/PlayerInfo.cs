using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : TacticsMove {

    public Vector3 StartPos;
    public bool Selected = false;
    public bool IHaveIt = false;
    public bool CanTackle = false;
    public bool HasBeenTackled = false;
    public bool TackleSuccessful = false;
    public bool CanShoot = false;
    public bool CanPass = false;
    public bool Moved = false;
    bool InRange = false;
    public Camera mainCamera;

    // Use this for initialization
    void Start()
    {
        Init();
        StartPos = transform.position;
    }

    // Update is called once per frame
    public void Update()
    {

        if (!turn && TurnManager.Finished)
        {
            //Debug.Log("Return");
            return;
        }

        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D coll = this.GetComponent<BoxCollider2D>();

        if (!NotSelected || !idle)
        {
            if (coll.OverlapPoint(wp) && Input.GetMouseButtonDown(0))
            {
                Selected = true;
            }
        }        

        if (Selected) // if the User clicks on a player it will be selected
        {
            if (!moving)
            {
                FindSelectableNodes();
                CheckMouse();
            }
            else if (!Moved)
            {
                transform.GetComponent<SpriteRenderer>().color = Color.white;
                Move();
            }
        }

        if(idle)
        {
            transform.GetComponent<SpriteRenderer>().color = Color.gray;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if(HasBeenTackled)
        {
            IHaveIt = false;
            HasBeenTackled = false;
        }
    }

    void CheckMouse()
    {
        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.back, 100f);
            if (hit == true)
            {
                if (hit.transform.tag == "Floor" || hit.transform.tag == "Football" && !Moved)
                {
                    Node N = hit.collider.GetComponent<Node>();
                    MoveToNode(N);
                }
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            FootballInfo football = GameObject.FindGameObjectWithTag("Football").GetComponent<FootballInfo>();

            RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.back, 10f);
            foreach(RaycastHit2D obj in hit)
            {
                if (obj.transform.tag == "Player")
                {
                    if(CanPass)
                    {
                        football.transform.position = obj.transform.position;
                        football.StickToPlayer(obj.transform.gameObject);
                    }
                }
                else if (obj.transform.tag == "Enemy")
                {
                    Enamy En = obj.transform.GetComponent<Enamy>();
                    Debug.Log(En.name);
                    if (En.CanBeTackled && En.IHaveIt)
                    {
                        Debug.Log("In");
                        TackleUnit();
                        if (TackleSuccessful)
                        {
                            En.IHaveIt = false;
                            TackleSuccessful = false;
                        }
                    }
                }
                else if (obj.transform.tag == "Floor" || obj.transform.tag == "GoalR" || obj.transform.tag == "GoalL")
                {
                    if(CanShoot)
                    {
                        CheckInRange(Vector3.left, obj.transform.gameObject);
                        CheckInRange(Vector3.right, obj.transform.gameObject);
                        IHaveIt = false;
                        football.PickedUp = false;
                    }
                }
            }
        }
    }
    public void CheckInRange(Vector3 direction, GameObject other)
    {
        FootballInfo football = GameObject.FindGameObjectWithTag("Football").GetComponent<FootballInfo>();

        RaycastHit2D[] Hits = Physics2D.RaycastAll(transform.position, direction, 4f);

        foreach (RaycastHit2D item in Hits)
        {
            if (item.transform.tag == other.tag)
            {
                football.transform.position = item.transform.position;
                football.StickToPlayer(item.transform.gameObject);
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
            else if (obj.transform.tag == "Enemy")
            {
                obj.transform.GetComponent<Enamy>().CanBeTackled = true;
            }
        }       
    }

    private void OnTriggerExit(Collider other)
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(other.transform.position, Vector3.back, 10f);

        foreach (RaycastHit2D obj in hit)
        {
            if (obj.transform.tag == "Enemy")
            {
                obj.transform.GetComponent<Enamy>().CanBeTackled = false;
            }
        }
        
    }

    public void TackleUnit()
    {
        FootballInfo football = GameObject.FindGameObjectWithTag("Football").GetComponent<FootballInfo>();
        int rand = Random.Range(1, 6);
        Debug.Log(rand);
        if (gameObject.GetComponent<PlayerInfo>().CanTackle)
        {
            if (rand == 6 || rand == 5)
            {
                TurnManager.tackleFail = true;
                TurnManager.Roll = rand;
            }
            else
            {
                TurnManager.tackleSucc = true;
                TurnManager.Roll = rand;

                football.StickToPlayer(gameObject);
                football.PickedUp = true;
                TackleSuccessful = true;
                gameObject.GetComponent<PlayerInfo>().IHaveIt = true;

            }
        }

    }
}
