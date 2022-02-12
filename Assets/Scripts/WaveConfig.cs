using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy WaveConfig")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawns = 7f;
    [SerializeField] float spawnRandomFactor = 15f;
    [SerializeField] int numberOfEnemies = 2;
    [SerializeField] float moveSpeed = 3f;

    public GameObject GetEnemyPrefab() { return enemyPrefab; }
    public List<Transform> GetWaypoints() 
    {
        var waveWaypoints = new List<Transform>();

        foreach(Transform child in pathPrefab.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints;
    }
    public float GetTimeBetweenSpanws() { return timeBetweenSpawns; }
    public float GetSpawnRandomFactor() { return spawnRandomFactor; }
    public int GetNumberOfEnemies() { return numberOfEnemies; }
    public float GetMoveSpeed() { return moveSpeed; }
    

}
