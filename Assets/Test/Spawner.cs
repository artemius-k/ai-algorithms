using NUnit.Framework;
using UnityEngine;

[RequireComponent(typeof(GridManager))]
public class Spawner : MonoBehaviour
{
    public GameObject unitPrefab;
    public int unitsAmountToSpawn;
    private GridManager gridManager;

    public GameObject[] units;

    private void Start()
    {
        gridManager = GetComponent<GridManager>();
        units = new GameObject[unitsAmountToSpawn];
        SpawnUnits();
    }

    private void SpawnUnits()
    {
        for (int i = 0; i < unitsAmountToSpawn; ++i)
        {
            GridTile tileToSpawnOn = gridManager.GetRandomWalkableNode();
            Vector3 positionToSpawnOn = new Vector3(tileToSpawnOn.x, 0, tileToSpawnOn.y);
            GameObject unit = Instantiate(unitPrefab, positionToSpawnOn, Quaternion.identity);
            units[i] = unit;
        }
    }
}
