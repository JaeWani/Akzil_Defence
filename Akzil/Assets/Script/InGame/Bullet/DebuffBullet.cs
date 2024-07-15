using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffBullet : BulletBase
{
    [SerializeField] private float decreaseRate;
    [SerializeField] private float debuffTime;

    protected override void Hit(MonsterBase monsterBase)
    {
        monsterBase.Debuff(decreaseRate, debuffTime);
        base.Hit(monsterBase);
    }

    public void Debuff_Init(float rate, float time)
    {
        decreaseRate = rate;
        debuffTime = time;
    }
}
