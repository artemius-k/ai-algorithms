using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCController : MonoBehaviour
{
    public enum State { Idle, Patrol, Flee }
    public State currentState = State.Idle;
    
    public GridManager gridManager;
    public Pathfinder pathfinder;
    public GameObject player;

    private List<GridTile> path;
    private int currentIndex;

    private float detectionRadius = 3f;
    private float idleTime = 2f;

    public static List<NPCController> allNPCs = new List<NPCController>();

    private void Start()
    {
        path = null;
        allNPCs.Add(this);
    }

    private void OnDestroy()
    {
        allNPCs.Remove(this);
        if (allNPCs.Count == 0)
        {
            GameManager.instance.OnWin();
        }
    }

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist < detectionRadius && currentState != State.Flee)
        {
            ChangeState(State.Flee);
            AlertOthers();
            return;
        }

        switch (currentState)
        {
            case State.Idle:
                idleTime -= Time.deltaTime;
                if (idleTime <= 0)
                {
                    idleTime = 2f;
                    ChangeState(State.Patrol);
                }
                break;

            case State.Patrol:
                MoveAlongPath();
                break;

            case State.Flee:
                MoveAlongPath();
                break;
        }
    }

    void MoveAlongPath()
    {
        if (path == null || currentIndex >= path.Count-1)
        {
            ChangeState(State.Patrol);
            return;
        }

        Vector3 targetPos = new Vector3(path[currentIndex].x, 0, path[currentIndex].y);
        if (currentState == State.Patrol)
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 3f * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 4.3f * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            currentIndex++;
    }

    void ChangeState(State newState)
    {
        currentState = newState;

        switch (newState)
        {
            case State.Idle:
                idleTime = Random.Range(1f, 3f);
                path = null;
                break;

            case State.Patrol:
                Vector3 randomPos = new Vector3(Random.Range(1, gridManager.width-1), 0, Random.Range(1, gridManager.height-1));
                path = pathfinder.FindPath(transform.position, randomPos);
                if (path == null)
                { 
                    return;
                }
                currentIndex = 0;
                break;

            case State.Flee:
                Vector3 fleeDir = (transform.position - player.transform.position).normalized;
                Vector3 fleeTarget = transform.position + fleeDir * 5f;
                path = pathfinder.FindPath(transform.position, fleeTarget);
                if (path == null)
                {
                    return;
                }
                currentIndex = 0;
                break;
        }
    }

    void AlertOthers()
    {
        foreach (var npc in allNPCs)
        {
            if (npc != this)
                npc.ChangeState(State.Flee);
        }
    }

    void CalmOthers()
    {
        foreach (var npc in allNPCs)
        {
            if (npc != this)
                npc.ChangeState(State.Patrol);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            Destroy(this.gameObject);
    }
}
