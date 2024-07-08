using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    public float speed;
    [SerializeField] private Vector2 direction = Vector2.up;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }
    
    private void Move()
    {
        if (transform.position == new Vector3(-2.5f, -0.5f)) direction = Vector2.right;
        else if (transform.position == new Vector3(2.5f, -0.5f)) direction = Vector2.down;

        rb.velocity = direction * speed;
    }
}
