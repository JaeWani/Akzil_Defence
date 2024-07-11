using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MonsterType { Basic, Speed, Boss }

public enum TurnState { Start, AttackTurn, DefenceTurn, End }

public class GameManager : MonoBehaviour
{
    #region Variable
    public static GameManager Instance { get; private set; }

    [SerializeField] private Vector2 monsterSpawnPos = new Vector2(-2.5f, -4.5f);

    [SerializeField] private int roundTime;
    [SerializeField] private int currentRoundTime;
    public int CurrentRoundTime { get { return currentRoundTime; } private set { currentRoundTime = value; } }
    public int RoundTime { get { return roundTime; } private set { roundTime = value; } }

    private WaitForSeconds SECONDS1 = new WaitForSeconds(1);

    private Coroutine CURRENT_ROUND_COROUTINE = null;
    private Coroutine ATTACK_ROUND_COROUTINE = null;

    [SerializeField] private TurnState currentTurn = TurnState.Start;
    public static TurnState CurrentTurn { get { return Instance.currentTurn; } set { Instance.currentTurn = value; } }

    private Queue<MonsterType> MonsterQueue = new Queue<MonsterType>();

    [Header("Attack Turn")]
    private bool selectMonster = false;
    private int maxMonsterNumber = 6;
    [SerializeField] private List<MonsterType> SpawnList;

    [Header("Prefabs")]
    [SerializeField] private GameObject BasicMonsterPrefab;
    [SerializeField] private GameObject SpeedMonsterPrefab;
    [SerializeField] private GameObject BossMonsterPrefab;


    #endregion

    #region Unity_Function
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        DefenceTurn();
    }
    #endregion

    #region Function
    private void Progress(TurnState turn)
    {
        CurrentRoundTime = 0;
        StopCoroutine(CURRENT_ROUND_COROUTINE);
        if (turn == TurnState.DefenceTurn) DefenceTurn();
        else if (turn == TurnState.AttackTurn) AttackTurn();
    }

    private void StartMotion()
    {

    }

    private void DefenceTurn()
    {
        RoundTimeLimit();
    }

    private void AttackTurn()
    {
        selectMonster = false;
        ATTACK_ROUND_COROUTINE = StartCoroutine(StartTurn());
        RoundTimeLimit();
        IEnumerator StartTurn()
        {
            while (!selectMonster) yield return null; // 몬스터 선택하지 않을 시, 선택 할 때 까지 대기
            StopCoroutine(CURRENT_ROUND_COROUTINE);

            yield return StartCoroutine(Spawn(0.3f));

            ATTACK_ROUND_COROUTINE = null;
            CurrentTurn = TurnState.DefenceTurn;
            Progress(CurrentTurn);
        }
    }


    private void RoundTimeLimit()
    {
        CURRENT_ROUND_COROUTINE = StartCoroutine(TimeLimit());
        IEnumerator TimeLimit()
        {
            for (int i = 0; i < RoundTime; i++)
            {
                yield return SECONDS1;
                CurrentRoundTime++;
                Game_Interface_UI.SetRoundTime(CurrentRoundTime);
            }

            if (CurrentTurn == TurnState.AttackTurn)
            {
                StopCoroutine(ATTACK_ROUND_COROUTINE);
                // 임의로 몬스터 채우는 코드 기입
                CurrentTurn = TurnState.DefenceTurn;
                Progress(CurrentTurn);
            }
            else if (CurrentTurn == TurnState.DefenceTurn)
            {
                CurrentTurn = TurnState.AttackTurn;
                Progress(CurrentTurn);
            }
            CurrentRoundTime = 0;
        }
    }

    private void EndMotion()
    {

    }

    public static void MonsterEnqueue(MonsterType monsterType)
    {
        var queue = Instance.MonsterQueue;
        if (queue.Count < Instance.maxMonsterNumber)
        {
            Instance.MonsterQueue.Enqueue(monsterType);
            if (queue.Count == Instance.maxMonsterNumber) Instance.selectMonster = true;
        }

        Instance.SpawnList = Instance.MonsterQueue.ToList();
    }

    private GameObject MonsterDequeue()
    {
        GameObject currentPrefab = null;

        switch (MonsterQueue.Dequeue())
        {
            case MonsterType.Basic: currentPrefab = BasicMonsterPrefab; break;
            case MonsterType.Speed: currentPrefab = SpeedMonsterPrefab; break;
            case MonsterType.Boss: currentPrefab = BossMonsterPrefab; break;
        }
        Instance.SpawnList = Instance.MonsterQueue.ToList();
        return Instantiate(currentPrefab, monsterSpawnPos, Quaternion.identity);
    }

    IEnumerator Spawn(float delay)
    {
        for (int i = 0; i < maxMonsterNumber; i++)
        {
            MonsterDequeue();
            yield return new WaitForSeconds(delay);
        }
    }
    #endregion
}
