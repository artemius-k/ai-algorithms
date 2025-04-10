using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Pathfinder pathfinder;
    public GridManager gridManager;

    private List<GridTile> path;
    private int targetIndex;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                path = pathfinder.FindPath(transform.position, hit.point);
                targetIndex = 0;
                StopCoroutine(FollowPath());
                StartCoroutine(FollowPath());
            }
        }
    }

    IEnumerator FollowPath()
    {
        while (targetIndex < path.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[targetIndex].transform.position, 5f * Time.deltaTime);
            if (Vector3.Distance(transform.position, path[targetIndex].transform.position) < 0.1f)
                targetIndex++;

            yield return null;
        }
    }
}
