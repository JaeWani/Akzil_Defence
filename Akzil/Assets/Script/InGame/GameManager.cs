using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UnityEngine;


public enum MonsterType { Basic, Speed, Boss }

public enum TurnState { Start, AttackTurn, DefenceTurn, End }

public enum PlayerState { Master, User }
public enum TypeState { Attacker, Defencer }

public class GameManager : MonoBehaviour
{
    #region Variable
    public static GameManager Instance { get; private set; }

    [Header("동기화")]
    public int masterGold = 0;
    public int userGold = 0;

    public int AttackerGold
    {
        get
        {
            if (currentTypeState == TypeState.Attacker) return masterGold;
            else return userGold;
        }
        set
        {
            if (currentTypeState == TypeState.Attacker) masterGold = value;
            else userGold = value;
        }
    }

    public int DefencerGold
    {
        get
        {
            if (currentTypeState == TypeState.Attacker) return userGold;
            else return masterGold;
        }
        set
        {
            if (currentTypeState == TypeState.Attacker) userGold = value;
            else masterGold = value;
        }
    }
    public int MasterRound = 0;
    public int UserRound = 0;

    public PlayerState currentPlayerState = PlayerState.Master;
    public TypeState currentTypeState = TypeState.Defencer;

    public bool IsChange = false;

    [SerializeField] private List<Transform> wayPoints = new List<Transform>();
    public static List<Transform> WayPoints => Instance.wayPoints;

    [SerializeField] private List<TowerSlot> towerSlots = new List<TowerSlot>();
    public static List<TowerSlot> TowerSlots => Instance.towerSlots;

    [SerializeField] private Transform monsterSpawnPos;
    [SerializeField] private Transform homePos;

    [SerializeField] private int waveCount;
    [SerializeField] private float roundTime;
    [SerializeField] private float currentRoundTime;
    public float CurrentRoundTime { get { return currentRoundTime; } private set { currentRoundTime = value; } }
    public float RoundTime { get { return roundTime; } private set { roundTime = value; } }
    public int WaveCount { get { return waveCount; } private set { waveCount = value; } }

    private Coroutine CURRENT_ROUND_COROUTINE = null;
    private Coroutine ATTACK_ROUND_COROUTINE = null;

    [SerializeField] private TurnState currentTurn = TurnState.Start;
    public static TurnState CurrentTurn { get { return Instance.currentTurn; } set { Instance.currentTurn = value; } }



    [SerializeField] private List<MonsterBase> currentMonsterList = new List<MonsterBase>();

    public Queue<MonsterType> MonsterQueue { get; private set; } = new Queue<MonsterType>();
    public List<MonsterBase> CurrentMonsterList { get { return currentMonsterList; } private set { currentMonsterList = value; } }

    [Header("Defence Turn")]
    public int MaxRoundInstallTowerCount = 0;
    public int CurrentRoundInstallTowerCount = 0;

    [Header("Attack Turn")]
    private bool selectMonster = false;
    public int CurrentMonsterNumber = 0;
    [SerializeField] public int maxMonsterNumber = 6;
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
        StartMotion();
    }
    private void Update()
    {
        monsterSpawnPos.Rotate(0, 0, 100 * Time.deltaTime);
        homePos.Rotate(0, 0, 100 * Time.deltaTime);
        Cheat();
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
        StartCoroutine(Motion());
        IEnumerator Motion()
        {
            yield return Game_Interface_UI.StartRoundEmotion("당신은 이제 수비자입니다!", 90);
            DefenceTurn();
        }
    }

    private void DefenceTurn()
    {
        if (AttackSkillManager.Instance.IsDelay) AttackSkillManager.EndDelayAttack();
        CurrentRoundInstallTowerCount = MaxRoundInstallTowerCount;
        WaveCount++;
        RoundTimeLimit();
    }
    public static void DefenceTurnSkip()
    {
        if (CurrentTurn == TurnState.DefenceTurn && Instance.CURRENT_ROUND_COROUTINE != null)
        {
            Instance.StopCoroutine(Instance.CURRENT_ROUND_COROUTINE);
            CurrentTurn = TurnState.AttackTurn;
            Instance.Progress(CurrentTurn);
        }
    }

    private void AttackTurn()
    {
        selectMonster = false;
        ATTACK_ROUND_COROUTINE = StartCoroutine(StartTurn());
        IEnumerator StartTurn()
        {
            if (currentTypeState == TypeState.Attacker) masterGold += 30;
            else userGold += 30;
            CurrentMonsterNumber = 0;
            yield return Game_Interface_UI.StartRoundEmotion("라운드" + WaveCount, 150);
            RoundTimeLimit();
            while (!selectMonster) yield return null; // 몬스터 선택하지 않을 시, 선택 할 때 까지 대기
            StopCoroutine(CURRENT_ROUND_COROUTINE);

            yield return StartCoroutine(Spawn(0.3f));

            while (CurrentMonsterList.Count != 0) yield return null;

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
            while (CurrentRoundTime < RoundTime)
            {
                yield return null;
                CurrentRoundTime += Time.deltaTime;
                Game_Interface_UI.SetRoundTime(CurrentRoundTime, RoundTime);
            }

            if (CurrentTurn == TurnState.AttackTurn)
            {
                StopCoroutine(ATTACK_ROUND_COROUTINE);
                for (int i = 0; i < maxMonsterNumber; i++)
                {
                    MonsterQueue.Enqueue(MonsterType.Basic);
                }
                yield return StartCoroutine(Spawn(0.3f));
                while (CurrentMonsterList.Count != 0) yield return null;
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

    public static void Change()
    {
        Instance.StartCoroutine(StartChange());
        IEnumerator StartChange()
        {
            Instance.IsChange = true;
            for (int i = Instance.CurrentMonsterList.Count - 1; i >= 0; i--)
            {
                Destroy(Instance.CurrentMonsterList[i].gameObject);
            }

            Instance.CurrentMonsterList = new List<MonsterBase>();

            foreach (var item in Instance.towerSlots)
            {
                item.Reset();
            }

            yield return Game_Interface_UI.StartRoundEmotion("당신은 이제 공격자 입니다.", 90);
            Instance.StopCoroutine(Instance.CURRENT_ROUND_COROUTINE);
            Instance.currentTypeState = TypeState.Attacker;
            Instance.MasterRound = Instance.WaveCount;
            Instance.waveCount = 0;
            CurrentTurn = TurnState.DefenceTurn;
            Instance.DefenceTurn();
        }
    }

    public static void MonsterEnqueue(MonsterType monsterType)
    {
        var queue = Instance.MonsterQueue;
        if (Instance.CurrentMonsterNumber < Instance.maxMonsterNumber)
        {
            Instance.MonsterQueue.Enqueue(monsterType);
            if (queue.Count == Instance.maxMonsterNumber) Instance.selectMonster = true;
        }

        Instance.SpawnList = Instance.MonsterQueue.ToList();
    }

    private MonsterBase MonsterDequeue()
    {
        GameObject currentPrefab = null;

        switch (MonsterQueue.Dequeue())
        {
            case MonsterType.Basic: currentPrefab = BasicMonsterPrefab; break;
            case MonsterType.Speed: currentPrefab = SpeedMonsterPrefab; break;
            case MonsterType.Boss: currentPrefab = BossMonsterPrefab; break;
        }
        Instance.SpawnList = Instance.MonsterQueue.ToList();
        return Instantiate(currentPrefab, monsterSpawnPos.position, Quaternion.identity).GetComponent<MonsterBase>();
    }

    public static void SortMonsterList()
    {
        for (int i = 0; i < Instance.CurrentMonsterList.Count; i++) if (Instance.CurrentMonsterList[i].Health <= 0) Instance.CurrentMonsterList.RemoveAt(i);
    }

    IEnumerator Spawn(float delay)
    {
        for (int i = 0; i < maxMonsterNumber; i++)
        {
            MonsterBase monster = MonsterDequeue();
            if (monster != null) CurrentMonsterList.Add(monster);
            yield return new WaitForSeconds(delay);
        }
    }

    public void Cheat()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach(var item in towerSlots)
            {
                if(item.CurrentTower == null)
                {
                    Defence_UI.Instance.SpawnTower(item);
                    break;
                }
            }
        }
    }

    #endregion
}
