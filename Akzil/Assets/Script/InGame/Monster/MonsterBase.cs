using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterBase : MonoBehaviour
{
    #region Variable

    private int wayPointIndex = 0;

    [Header("Component")]
    private Rigidbody2D rb;
    [SerializeField] private TextMeshPro hpText;
    [Header("Info")]
    [SerializeField] private MonsterType currentType;
    public MonsterType CurrentType { get { return currentType; } private set { currentType = value; } }
    public float Step = 0;

    [SerializeField] private Vector2 direction = Vector2.up;

    public float moveDistance = 0;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float health;
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public float Health { get { return health; } private set { health = value; } }

    [SerializeField] private bool isDebuff = false;
    public bool IsDebuff { get { return isDebuff; } set { isDebuff = value; } }
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
        Arrival();
    }
    #endregion

    #region Function
    private void ComponentInit()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Move() // 라인 따라 움직이는 함수
    {
        if (wayPointIndex < GameManager.WayPoints.Count)
        {
            Step = MoveSpeed * Time.deltaTime;

            Vector2 pos = Vector2.MoveTowards(transform.position, GameManager.WayPoints[wayPointIndex].position, Step);

            float frameDistance = Vector2.Distance(transform.position, pos);
            moveDistance += frameDistance;

            transform.position = pos;
            if (Vector2.Distance(GameManager.WayPoints[wayPointIndex].position, transform.position) == 0f) wayPointIndex++;
        }
    }
    
    private void SetHpText() => hpText.text = Health.ToString();

    private void Arrival()
    {
        if(transform.position == GameManager.WayPoints[GameManager.WayPoints.Count - 1].position)
        {
            Debug.Log("게임 끝남");
        }
    }

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

    public void Debuff(float rate, float time)
    {
        if (!IsDebuff) StartCoroutine(StartDebuff());

        IEnumerator StartDebuff()
        {
            float originalSpeed = MoveSpeed;
            IsDebuff = true;
            MoveSpeed -= MoveSpeed * (rate / 100);

            yield return new WaitForSeconds(time);

            IsDebuff = false;
            MoveSpeed = originalSpeed;
        }
    }
    #endregion
}
