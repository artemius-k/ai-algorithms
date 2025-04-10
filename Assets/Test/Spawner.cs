using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject unitPrefab;
    public int unitsAmountToSpawn;

    private GridManager gridManager;
    private Pathfinder pathfinder;
    public GameObject player;

    private void Start()
    {
        gridManager = GetComponent<GridManager>();
        pathfinder = GetComponent<Pathfinder>();
    }

    public void SpawnUnits()
    {
        for (int i = 0; i < unitsAmountToSpawn; ++i)
        {
            GridTile tileToSpawnOn = gridManager.GetRandomWalkableNode();
            Vector3 positionToSpawnOn = new Vector3(tileToSpawnOn.x, 0, tileToSpawnOn.y);
            GameObject npc = Instantiate(unitPrefab, positionToSpawnOn, Quaternion.identity);
            
            var npcController = npc.GetComponent<NPCController>();
            npcController.player = player;
            npcController.pathfinder = pathfinder;
            npcController.gridManager = gridManager;
        }
    }
}
