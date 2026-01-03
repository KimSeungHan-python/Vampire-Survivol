
using UnityEngine;
using System.Collections.Generic;
using System.Collections;  
public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    public GameObject currentChunk;
    PlayerMovement pm;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOpDist;
    float opDist;

    float optimizerCooldown;
    public float optimizerCooldowndur;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pm = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();   
    }

    void ChunkChecker()
    {
        if(!currentChunk){
            return;
        }

        // 이동 방향에 따라 해당 방향들을 체크 (else if 제거하고 독립적으로 체크)
        if(pm.MoveDir.x > 0) // 오른쪽으로 이동 중
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right").position;
                SpawnChunk();
            }
        }
        if(pm.MoveDir.x < 0) // 왼쪽으로 이동 중
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left").position;
                SpawnChunk();
            }
        }
        if(pm.MoveDir.y > 0) // 위로 이동 중
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Up").position;
                SpawnChunk();
            }
        }
        if(pm.MoveDir.y < 0) // 아래로 이동 중
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Down").position;
                SpawnChunk();
            }
        }
        if(pm.MoveDir.x > 0 && pm.MoveDir.y > 0) // 오른쪽 위 대각선
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right Up").position;
                SpawnChunk();
            }
        }
        if(pm.MoveDir.x > 0 && pm.MoveDir.y < 0) // 오른쪽 아래 대각선
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right Down").position;
                SpawnChunk();
            }
        }
        if(pm.MoveDir.x < 0 && pm.MoveDir.y > 0) // 왼쪽 위 대각선
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left Up").position;
                SpawnChunk();
            }
        }
        if(pm.MoveDir.x < 0 && pm.MoveDir.y < 0) // 왼쪽 아래 대각선
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left Down").position;
                SpawnChunk();
            }
        }
    }

    void SpawnChunk()
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }

    void ChunkOptimizer(){

        optimizerCooldown -= Time.deltaTime;
        if(optimizerCooldown <= 0f){
            optimizerCooldown = optimizerCooldowndur;
        }
        else{
            return;
        }
        foreach(GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if(opDist > maxOpDist)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
