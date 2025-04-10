using System.Linq;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public int x;
    public int y;

    public bool isWalkable = false;
    
    public GridTile parent;
    private Renderer externalRenderer;

    public Material tileWalkable;
    public Material tileWall;

    public int gCost;
    public int hCost;

    public int FCost => gCost + hCost;

    public void UpdateColor()
    {
        Debug.Log(externalRenderer != null);
        if (isWalkable)
        {
            externalRenderer.material = tileWalkable; 
        }
        else
        {
            externalRenderer.material = tileWall; 
        }
        
    }

    void Start()
    {
        externalRenderer = GetComponent<Renderer>();
        UpdateColor();
    }

    public int GCost { get; set; }
    public int HCost { get; set; }
    public Vector3 worldPosition { get; set; }

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
