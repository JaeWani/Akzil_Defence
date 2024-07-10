using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{

    #region Variable

    private Rigidbody2D rb;
    [SerializeField] private Vector2 direction = Vector2.up;

    public float moveDistance = 0;

    [Header ("Stat")]
    public float moveSpeed;
    public float health;

    #endregion
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        moveDistance += rb.velocity.x * Time.deltaTime;
        moveDistance += rb.velocity.y * Time.deltaTime;
    }
    
    private void Move()
    {
        if (transform.position == new Vector3(-2.5f, -0.5f)) direction = Vector2.right;
        else if (transform.position == new Vector3(2.5f, -0.5f)) direction = Vector2.down;

        rb.velocity = direction * moveSpeed;
    }
}
