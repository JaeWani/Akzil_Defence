using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    #region Variable
    public GameObject target = null;
    public float speed = 10;
    private Rigidbody2D rb;
    #endregion

    #region Unity_Function
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        rb.velocity = (target.transform.position - transform.position) * speed;
    }
    #endregion

    #region Function

    #endregion
}
