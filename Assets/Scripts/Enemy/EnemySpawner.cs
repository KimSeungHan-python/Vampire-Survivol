using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int waveQuota;//적 스폰 총 개수
        public float spawnInterval;//적 스폰 간격
        public int spawnCount;//현재 스폰된 적 개수
    }
    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public int spawnCount;
        public GameObject enemyPrefab;
    }

    public List<Wave> waves;

    public int currentWaveCount;

    [Header("Spawner Attributes")]
    float spawnTimer;
    public int enemiesAlive;
    public int maxEnemiesAllowed; // The maximum number of enemies allowed to be alive at once
    public bool maxEnemiesReached = false; // Flag to indicate if the max enemies limit has been reached
    public float waveInterval;
    bool isWaveActive = false;

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints;
    Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();

    }

    // Update is called once per frame
    void Update()
    {
        if(currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 && !isWaveActive)
        {
            StartCoroutine(BeginNextWave());// Check if the wave has ended and the next wave should begin
        }
        spawnTimer += Time.deltaTime;
        if(spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            SpawnEnemies();
            spawnTimer = 0f;
        }
    }

    IEnumerator BeginNextWave()
    {
        //Wave for 'waveInterval' seconds before starting the next wave
        yield return new WaitForSeconds(waveInterval);

        //if there are more waves to start after the current wave, move on to the next wave
        if(currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQuota();
        }
        
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (EnemyGroup group in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += group.enemyCount;
        }
        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.Log("Wave Quota Calculated: " + currentWaveQuota);
    }


    //<summary>
    // This method will stop spawning enemies if the amount of enemies on the map is maximum
    // the meethod will only spawn enmeis in a particular wave until it is time for the next wave's enemies to spawn
    //</summary>
    void SpawnEnemies()
    {
        //check if the minimum number of enemies in the wave have been spawned
        if(waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            //Spawn each type of enemy until the quota is filled
            foreach(var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                //check if the minimum number of this type have been spawned
                if(enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    if(enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return; // Exit the method if the max enemies limit has been reached
                    }

                    Instantiate(enemyGroup.enemyPrefab, player.position + (Vector3)relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    
                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }
        if(enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }
}
