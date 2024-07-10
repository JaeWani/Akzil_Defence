using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{

    #region Variable

    [Header ("Component")]
    private Rigidbody2D rb;

    [Header ("Info")]
    [SerializeField] private Vector2 direction = Vector2.up;

    public float moveDistance = 0;

    [Header("Stat")]
    public float moveSpeed;
    public float health;
    #endregion

    #region Unity_Function
    private void Start()
    {
        ComponentInit();
    }

    private void Update()
    {
        Move();
        SetMoveDistance();
    }
    #endregion

    #region Function
    private void ComponentInit()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Move() // 라인 따라 움직이는 함수
    {
        if (transform.position == new Vector3(-2.5f, -0.5f)) direction = Vector2.right;
        else if (transform.position == new Vector3(2.5f, -0.5f)) direction = Vector2.down;

        rb.velocity = direction * moveSpeed;
    }

    private void SetMoveDistance() // 움직인 거리 계산해주는 함수
    {
        moveDistance += rb.velocity.x * Time.deltaTime;
        moveDistance += rb.velocity.y * Time.deltaTime;
    }
    #endregion
}
