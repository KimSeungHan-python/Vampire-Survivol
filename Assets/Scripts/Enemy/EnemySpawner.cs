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

    Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    void SpawnEnemies()
    {
        //check if the minimum number of enemies in the wave have been spawned
        if(waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota)
        {
            //Spawn each type of enemy until the quota is filled
            foreach(var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                //check if the minimum number of this type have been spawned
                if(enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    Vector2 spawnPosition = new Vector2(
                        player.transform.position.x + Random.Range(-10f, 10f),
                        player.transform.position.y + Random.Range(-10f, 10f)
                    );
                    Instantiate(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);
                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                }
            }
        }
    }
}
