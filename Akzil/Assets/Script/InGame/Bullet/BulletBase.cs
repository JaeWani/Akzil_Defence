using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    #region Variable
    public GameObject Target = null;

    public float MoveSpeed { get; private set; }
    public float Damage { get; private set; }

    private Rigidbody2D rb;
    #endregion

    #region Unity_Function
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Target != null) rb.velocity = (Target.transform.position - transform.position) * MoveSpeed;
        else Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Target) Hit(other.GetComponent<MonsterBase>());
    }
    #endregion

    #region Function
    public virtual void Init(float moveSpeed, float damage, GameObject target)
    {
        MoveSpeed = moveSpeed;
        Damage = damage;
        Target = target;
    }

    protected virtual void Hit(MonsterBase monsterBase)
    {
        monsterBase.TakeDamage(Damage);
        Destroy(gameObject);
    }
    #endregion
}
