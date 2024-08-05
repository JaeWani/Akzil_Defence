using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTower : TowerBase
{
    [SerializeField] private float range;

    private RangeBullet SpawnRangeBullet()
    {
        BulletBase bullet = SpawnBullet(20, damage);
        RangeBullet rangeBullet = bullet.GetComponent<RangeBullet>();

        rangeBullet.ParentTower = this;

        return rangeBullet;
    }

    protected override void Attack()
    {
        if (targetObject != null) SpawnRangeBullet();
    }

    public List<MonsterBase> GetInRangeMonster(Vector3 pos)
    {
        List<MonsterBase> results = new List<MonsterBase>();
        RaycastHit2D[] hitObjects = Physics2D.CircleCastAll(pos, range, Vector2.zero);

        foreach (RaycastHit2D hit in hitObjects)
        {
            GameObject obj = hit.collider.gameObject;
            if (obj.CompareTag("Monster")) results.Add(obj.GetComponent<MonsterBase>());
            else continue;
        }

        return results;
    }
}
