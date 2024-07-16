using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum TowerType
{
    Single, Range, Debuff, Buff
}

public class TowerBase : MonoBehaviour
{
    #region Variable
    public TowerType CurrentType;
    public TowerSlot CurrentSlot;

    [SerializeField] protected MonsterBase targetObject;
    [SerializeField] private GameObject BulletPrefab; // 임시

    [Header("Stat")]
    [SerializeField] protected float attackDelay;
    [SerializeField] protected int delayLevel;
    [SerializeField] protected int maxDelayLevel;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float damage;
    [SerializeField] protected float damageLevel;


    public float CurrentAttackDelay { get; private set; } = 0;

    public float AttackDelay { get { return attackDelay; } set { attackDelay = value; } }
    public int DelayLevel { get { return delayLevel; } set { delayLevel = value; } }
    public int MaxDelayLevel { get { return maxDelayLevel; } private set { maxDelayLevel = value; } }
    public float AttackRange { get { return attackRange; } private set { attackRange = value; } }
    public float Damage { get { return damage; } set { damage = value; } }
    public float DamageLevel { get { return damageLevel; }  set { damageLevel = value; } }

    [Header("Upgrade Increase/Decrease Value")]
    [SerializeField] private float damageIncreaseValue;
    [SerializeField] private List<float> delayDecreaseValueList;

    public float DamageIncreaseValue { get { return damageIncreaseValue; } private set { damageIncreaseValue = value; } }
    public List<float> DelayDecreaseValue { get { return delayDecreaseValueList; } private set { delayDecreaseValueList = value; } }
    #endregion

    #region Unity_Function

    protected void Start()
    {
        CurrentSlot = transform.parent.GetComponent<TowerSlot>();
    }

    protected void Update()
    {
        SetTarget();
        AttackCoolTime();
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 origin = transform.position;
        Handles.color = Color.red;
        Handles.DrawWireDisc(origin, new Vector3(0, 0, 1), attackRange);
    }
    #endregion

    #region Function
    protected virtual void Attack()
    {
        if (targetObject != null) SpawnBullet(20, Damage);
    }

    protected BulletBase SpawnBullet(float moveSpeed, float damage)
    {
        BulletBase bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity).GetComponent<BulletBase>();
        bullet.Init(moveSpeed, damage, targetObject.gameObject);

        return bullet;
    }

    protected void AttackCoolTime()
    {
        CurrentAttackDelay += Time.deltaTime;
        if (CurrentAttackDelay <= AttackDelay) return;
        else
        {
            if (CurrentSlot.CanUse == true)
            {
                CurrentAttackDelay = 0;
                Attack();
            }
        }
    }

    protected void SetTarget()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, attackRange);
        List<MonsterBase> mobs = new List<MonsterBase>();

        bool inCircle = false;
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].CompareTag("Monster")) mobs.Add(colls[i].GetComponent<MonsterBase>());

            if (colls[i].GetComponent<MonsterBase>() == targetObject) inCircle = true;
        }
        if (!inCircle) targetObject = null;
        foreach (var item in mobs)
        {
            if (targetObject == null) targetObject = item;
            else
            {
                if (item.moveDistance > targetObject.moveDistance) targetObject = item;
            }
        }
    }

    public void SetAttackDelay(float value) => AttackDelay = value;
    #endregion
}
