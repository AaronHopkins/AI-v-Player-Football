using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Moving : MonoBehaviour {

    public float MoveTime = 1f;
    public LayerMask blockinglayer;

    private BoxCollider2D BoxCollider;
    private Rigidbody2D rd2D;
    private float inversMoveTime;

    protected virtual void Start()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        rd2D = GetComponent<Rigidbody2D>();
        inversMoveTime = 1f / MoveTime;

    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        BoxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockinglayer);
        BoxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovemenet(end));
            return true;
        }
        return false;
    }

    protected IEnumerator SmoothMovemenet(Vector3 end)
    {

        float sqrRemaniningDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemaniningDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rd2D.position, end, inversMoveTime * Time.deltaTime);
            rd2D.MovePosition(newPosition);
            sqrRemaniningDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }

    }

    protected virtual void AttemptMoving<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool CanMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
        {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();

        if (!CanMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;

}
