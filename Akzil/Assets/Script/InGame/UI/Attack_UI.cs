using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Attack_UI : MonoBehaviour
{
    #region Variable
    public static Attack_UI Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI AttackerGoldText;
    [SerializeField] private TextMeshProUGUI AttackRoundMonsterCount;

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
    private void Update()
    {
        AttackerGoldText.text = GameManager.Instance.AttackerGold + "G";
        AttackRoundMonsterCount.text = "소환 가능한 몹 : " + (GameManager.Instance.maxMonsterNumber - GameManager.Instance.CurrentMonsterNumber);
    }
    #endregion

    #region Function
    private void ButtonInit()
    {
        BasicMonster.onClick.AddListener(() => { if (GameManager.CurrentTurn == TurnState.AttackTurn) { GameManager.MonsterEnqueue(MonsterType.Basic); GameManager.Instance.CurrentMonsterNumber++; } });
        SpeedMonster.onClick.AddListener(() => { if (GameManager.CurrentTurn == TurnState.AttackTurn) { GameManager.MonsterEnqueue(MonsterType.Speed); GameManager.Instance.CurrentMonsterNumber++; } });
        BossMonster.onClick.AddListener(() => { if (GameManager.CurrentTurn == TurnState.AttackTurn) { GameManager.MonsterEnqueue(MonsterType.Boss); GameManager.Instance.CurrentMonsterNumber++; } });

        StopAttack.onClick.AddListener(() => AttackSkillManager.StopAttack());
        SpeedUpAttack.onClick.AddListener(() => AttackSkillManager.SpeedUpAttack());
        DelayAttack.onClick.AddListener(() => AttackSkillManager.DelayAttack());
        LevelDownAttack.onClick.AddListener(() => AttackSkillManager.LevelDownAttack());
    }
    #endregion
}
