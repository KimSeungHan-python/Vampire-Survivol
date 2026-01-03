using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class KnifeBehaviour : ProjectileWeaponBehaviour
{
    KnifeController kc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        kc = FindObjectOfType<KnifeController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * kc.speed * Time.deltaTime;
    }
}
