using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridManager))]
public class Pathfinder : MonoBehaviour
{
    private GridManager gridManager;

    private void Start()
    {
        gridManager = GetComponent<GridManager>();
    }

    public List<GridTile> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        GridTile startNode = gridManager.GetNode(startPos);
        GridTile targetNode = gridManager.GetNode(targetPos);

        if (startNode == null || targetNode == null)
            return null;

        while (!targetNode.isWalkable)
        {
            var neighbours = gridManager.GetNeighbours(targetNode);

            GridTile lastNeighbour = targetNode;
            foreach(var neighbour in neighbours)
            {
                if (neighbour.isWalkable)
                {
                    targetNode = neighbour;
                    break;
                }
                lastNeighbour = neighbour;
            }

            if (!targetNode.isWalkable) targetNode = lastNeighbour;
        }

        List<GridTile> openSet = new List<GridTile>();
        HashSet<GridTile> closedSet = new HashSet<GridTile>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            GridTile currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost ||
                   (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (GridTile neighbour in gridManager.GetNeighbours(currentNode))
            {
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return null;
    }

    List<GridTile> RetracePath(GridTile startNode, GridTile endNode)
    {
        List<GridTile> path = new List<GridTile>();
        GridTile currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    int GetDistance(GridTile a, GridTile b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
