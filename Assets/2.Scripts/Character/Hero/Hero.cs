using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
    [SerializeField] MonsterSpawner spawner;
    ShotGun gun;
    private void Awake()
    {
        TryGetComponent(out gun);
        gun.SetData(1);
        SetData(GetHp());
    }

    public void SetData(int _hp)
    {
        SetHp(_hp);
    }

    private void Update()
    {
        var mob = spawner.NearestMob(transform.position);
        if(mob)
            gun.SetTarget(mob.transform);
    }
}
