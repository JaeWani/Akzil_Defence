using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun.Demo.Asteroids;

public enum TowerType
{

}

public class TowerBase : MonoBehaviour
{
    #region Variable

    [SerializeField] private MonsterBase targetObject;
    [SerializeField] private GameObject BulletPrefab; // 임시

    [Header("Stat")]
    public float attackDelay;
    public float attackRange;
    public float damage;

    #endregion

    #region Unity_Function

    protected void Update()
    {
        SetTarget();
    }
    
    private void FixedUpdate()
    {
        Attack();
    }
    private void OnDrawGizmosSelected()
    {
        Vector2 origin = transform.position;
        Handles.color = Color.red;
        Handles.DrawWireDisc(origin, new Vector3(0, 0, 1), attackRange);
    }
    #endregion

    #region Function
    private void Attack()
    {
        if (targetObject != null) Instantiate(BulletPrefab, transform.position, Quaternion.identity).GetComponent<BulletBase>().target = targetObject.gameObject;
    }

    private void SetTarget()
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
    #endregion
}
