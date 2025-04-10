using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GridManager : MonoBehaviour
{
    public int width = 10, height = 10;  
    public float cellSize = 1f;         
    public GameObject tilePrefab;      

    private GridTile[,] grid;

    public Material tileWalkable;
    public Material tileWall;

    public Spawner spawner;

    void Start()
    {
        grid = new GridTile[width, height];
        GenerateGrid();
        GenerateMaze();
        spawner.SpawnUnits(this);
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * cellSize, 0, y * cellSize);
                GameObject nodeObj = Instantiate(tilePrefab, pos, Quaternion.identity);
                GridTile node = nodeObj.AddComponent<GridTile>();
                node.SetPosition(x, y);
                node.tileWalkable = tileWalkable;
                node.tileWall = tileWall;
                grid[x, y] = node;

                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    node.isWalkable = false;
                }
            }
        }
    }

    void GenerateMaze()
    {
        Stack<GridTile> stack = new Stack<GridTile>();
        GridTile startNode = grid[1, 1];
        startNode.isWalkable = true;
        stack.Push(startNode);

        int[][] directions = {new int[] { 0, 2 }, new int[] { 2, 0 }, new int[] { 0, -2 }, new int[] { -2, 0 } };

        while (stack.Count > 0)
        {
            GridTile current = stack.Pop();
            ShuffleArray(directions);  

            foreach (int[] dir in directions)
            {
                int nx = current.x + dir[0];
                int ny = current.y + dir[1];

                if (IsInBounds(nx, ny) && !grid[nx, ny].isWalkable)
                {
                    grid[nx, ny].isWalkable = true;
                    grid[current.x + dir[0] / 2, current.y + dir[1] / 2].isWalkable = true;
                    stack.Push(grid[nx, ny]);
                }
            }
        }

    }

    bool IsInBounds(int x, int y) => x > 0 && y > 0 && x < width - 1 && y < height - 1;

    void ShuffleArray(int[][] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]); 
        }
    }


    public GridTile GetNode(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / cellSize);
        int y = Mathf.RoundToInt(worldPosition.z / cellSize);
        return (x >= 0 && x < width && y >= 0 && y < height) ? grid[x, y] : null;
    }

    public List<GridTile> GetNeighbours(GridTile node)
    {
        List<GridTile> neighbours = new List<GridTile>();

        int[][] directions = { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { -1, 0 } };

        foreach (int[] dir in directions)
        {
            int checkX = node.x + dir[0];
            int checkY = node.y + dir[1];
            if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
            {
                neighbours.Add(grid[checkX, checkY]);
            }
        }
        return neighbours;
    }

    public GridTile GetRandomFurthestNode()
    {
        List<GridTile> walkableNodes = new List<GridTile>();
  
        foreach (GridTile node in grid)
        {
            if (node.isWalkable)  
            {
                walkableNodes.Add(node);
            }
        }

        if (walkableNodes.Count == 0) return null;
      
        Vector3 center = new Vector3(width / 2, 0, height / 2);
        GridTile furthestNode = walkableNodes.OrderByDescending(n => Vector3.Distance(n.worldPosition, center)).FirstOrDefault();

        return furthestNode;
    }


    public GridTile GetRandomWalkableNode()
    {
        List<GridTile> walkableNodes = new List<GridTile>();
        Debug.Log(grid);
        foreach (var node in grid)
        {
            if (node.isWalkable)  
            {
                walkableNodes.Add(node);
            }
        }

        if (walkableNodes.Count == 0) return null;

        return walkableNodes[Random.Range(0, walkableNodes.Count)];
    }

}