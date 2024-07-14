using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterBase : MonoBehaviour
{
    #region Variable

    [Header("Component")]
    private Rigidbody2D rb;
    [SerializeField] private TextMeshPro hpText;
    [Header("Info")]
    [SerializeField] private MonsterType currentType;
    public MonsterType CurrentType { get { return currentType; } private set { currentType = value; } }

    [SerializeField] private Vector2 direction = Vector2.up;

    public float moveDistance = 0;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float health;
    public float MoveSpeed { get { return moveSpeed; } private set { moveSpeed = value; } }
    public float Health { get { return health; } private set { health = value; } }
    #endregion

    #region Unity_Function
    private void Start()
    {
        ComponentInit();
        SetHpText();
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

        rb.velocity = direction * MoveSpeed;
    }

    private void SetMoveDistance() // 움직인 거리 계산해주는 함수
    {
        moveDistance += rb.velocity.x * Time.deltaTime;
        moveDistance += rb.velocity.y * Time.deltaTime;
    }

    private void SetHpText() => hpText.text = Health.ToString();

    public void TakeDamage(float Damage)
    {
        Health -= Damage;
        if (Health <= 0)
        {
            GameManager.SortMonsterList();
            Destroy(gameObject);
        }
        SetHpText();
    }
    #endregion
}
