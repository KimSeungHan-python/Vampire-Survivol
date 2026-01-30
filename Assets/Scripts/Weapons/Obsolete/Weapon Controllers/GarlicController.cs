using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class GarlicController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Attack()
    {
        base.Attack();
        GameObject spawnGarlic = Instantiate(weaponData.Prefab);
        spawnGarlic.transform.position = transform.position;
        spawnGarlic.transform.parent = transform;
        // 플레이어가 마지막으로 바라본 방향으로 마늘 발사
    }
}
