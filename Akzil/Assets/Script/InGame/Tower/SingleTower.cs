using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTower : TowerBase
{
    protected override void Attack()
    {
        if (targetObject != null)
        {
            if (targetObject.GetComponent<MonsterBase>().CurrentType == MonsterType.Boss) SpawnBullet(20, Damage * 4);
            else SpawnBullet(20, Damage);
        }
    }
}
