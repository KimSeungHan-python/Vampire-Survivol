using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Security.Cryptography;

public class KnifeController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnKnife = Instantiate(weaponData.Prefab);
        spawnKnife.transform.position = transform.position;
        // 플레이어가 마지막으로 바라본 방향으로 칼 발사
        spawnKnife.GetComponent<KnifeBehaviour>().DirectionsChecker(pm.MoveDir);
    }
    // Update is called once per frame
    
}
