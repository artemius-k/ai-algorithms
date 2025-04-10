using NUnit.Framework;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject unitPrefab;
    public int unitsAmountToSpawn;

    public GameObject[] units;

    private void Start()
    {
        units = new GameObject[unitsAmountToSpawn];
    }

    public void SpawnUnits(GridManager gridManager)
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
