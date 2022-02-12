using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;

    bool start = true;
    float delay = 50f;

 
    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
            {
             
            yield return StartCoroutine(SpawnAllWaves());
            }
            while (looping);
        
    }

    private void Update()
    {
      
    }
    private IEnumerator SpawnAllWaves()
    {

      
        for (int waveCount = startingWave; waveCount < waveConfigs.Count; waveCount++)
        {
            var currentWave = waveConfigs[waveCount];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }
    }


    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {

        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            //Instantiate Params: What, Where, and Rotation
            var newEnemy = Instantiate(waveConfig.GetEnemyPrefab(),
                waveConfig.GetWaypoints()[0].transform.position,
                Quaternion.identity);

            //sets the wave configuration to the enemy spawner waves
            newEnemy.GetComponent<SmallEnemyPathing>().SetWaveConfig(waveConfig);

            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpanws());
        }
    }

    
}
