using System.Collections.Generic;
using System.Collections;   
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnProps();//10분 39초
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnProps()
    {
        foreach(GameObject spawnPoint in propSpawnPoints)
        {
            int randomIndex = Random.Range(0, propPrefabs.Count);
            GameObject prop = Instantiate(propPrefabs[randomIndex], spawnPoint.transform.position, Quaternion.identity);
            prop.transform.parent = spawnPoint.transform;
        }
    }
}
