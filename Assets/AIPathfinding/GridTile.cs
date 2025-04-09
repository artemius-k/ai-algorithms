using UnityEngine;

public class GridTile : MonoBehaviour
{
    public int x;
    public int y;

    public bool isWalkable = false;
    
    public GridTile parent;
    private Renderer externalRenderer;

    public int gCost;
    public int hCost;

    public int FCost => gCost + hCost;

    public void UpdateColor()
    {
        if (!isWalkable)
        {
            externalRenderer.material.color = Color.black; 
        }
        else
        {
            externalRenderer.material.color = Color.white; 
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
