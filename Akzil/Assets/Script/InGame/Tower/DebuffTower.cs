using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

public class DebuffTower : TowerBase
{
    [SerializeField] private float decreaseRate;
    [SerializeField] private float debuffTime;

    private DebuffBullet SpawnDebuffBullet(float rate, float time)
    {
        BulletBase bullet = SpawnBullet(20, damage);
        DebuffBullet debuffBullet = bullet.GetComponent<DebuffBullet>();

        debuffBullet.Debuff_Init(rate, time);

        return debuffBullet;
    }

    protected override void Attack()
    {
        if (targetObject != null) SpawnDebuffBullet(decreaseRate, debuffTime);
    }
}
