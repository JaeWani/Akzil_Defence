using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack_UI : MonoBehaviour
{
    #region Variable
    public static Attack_UI Instance { get; private set; }

    [Header("Monster UI")]
    [SerializeField] private Button BasicMonster;
    [SerializeField] private Button SpeedMonster;
    [SerializeField] private Button BossMonster;

    [Header("Attack UI")]
    [SerializeField] private Button StopAttack;
    [SerializeField] private Button SpeedUpAttack;
    [SerializeField] private Button RandomAttack;
    [SerializeField] private Button DelayAttack;
    [SerializeField] private Button LevelDownAttack;
    #endregion

    #region Unity_Function
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        ButtonInit();
    }

    #endregion

    #region Function
    private void ButtonInit()
    {
        BasicMonster.onClick.AddListener(() => { if (GameManager.CurrentTurn == TurnState.AttackTurn) GameManager.MonsterEnqueue(MonsterType.Basic); });
        SpeedMonster.onClick.AddListener(() => { if (GameManager.CurrentTurn == TurnState.AttackTurn) GameManager.MonsterEnqueue(MonsterType.Speed); });
        BossMonster.onClick.AddListener(() => { if (GameManager.CurrentTurn == TurnState.AttackTurn) GameManager.MonsterEnqueue(MonsterType.Boss); });

        StopAttack.onClick.AddListener(() => AttackSkillManager.StopAttack());
        SpeedUpAttack.onClick.AddListener(() => AttackSkillManager .SpeedUpAttack());
    }
    #endregion
}
