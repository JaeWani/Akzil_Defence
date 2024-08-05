using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBullet : BulletBase
{
    public RangeTower ParentTower;

    [SerializeField] private GameObject effectObject;

    protected override void Hit(MonsterBase monsterBase)
    {
        List<MonsterBase> hits = ParentTower.GetInRangeMonster(monsterBase.transform.position);
        foreach (MonsterBase monster in hits) monster.TakeDamage(Damage);
        Instantiate(effectObject, monsterBase.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
