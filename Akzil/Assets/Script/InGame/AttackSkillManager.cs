using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class AttackSkillManager : MonoBehaviour
{
    public static AttackSkillManager Instance { get; private set; }

    [Header("Stop Attack")]
    [SerializeField] private float stopDuration;
    [SerializeField] private float stopCount;
    [SerializeField] private int stopAttackGold;

    public float StopDuration { get { return stopDuration; } private set { stopDuration = value; } }
    public float StopCount { get { return stopCount; } private set { stopCount = value; } }

    [Header("Speed Up Attack")]
    [SerializeField] private float speedUpIncreaseRate;
    [SerializeField] private int speedUpAttackGold;

    public float SpeedUpIncreaseRate { get { return speedUpIncreaseRate; } private set { speedUpIncreaseRate = value; } }

    [Header("Delay Attack")]
    [SerializeField] private bool isDelay;
    [SerializeField] private float delayRate;
    [SerializeField] private List<float> originalAttackDelayList;
    [SerializeField] private int delayAttackGold;

    public bool IsDelay { get { return isDelay; } private set { isDelay = value; } }
    public float DelayRate { get { return delayRate; } private set { delayRate = value; } }
    public List<float> OriginalAttackDelayList { get { return originalAttackDelayList; } private set { originalAttackDelayList = value; } }
    [Header("Level Down")]
    [SerializeField] private int levelDownAttackGold;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public static void StopAttack()
    {
        int gold = GameManager.Instance.currentTypeState == TypeState.Attacker ? GameManager.Instance.masterGold : GameManager.Instance.userGold;
        if (gold >= Instance.stopAttackGold)
        {
            if (GameManager.Instance.currentTypeState == TypeState.Attacker) GameManager.Instance.masterGold -= Instance.stopAttackGold;
            else GameManager.Instance.userGold -= Instance.stopAttackGold;
            for (int i = 0; i < Instance.StopCount; i++)
            {
                TowerSlot towerSlot = GameManager.TowerSlots[Random.Range(0, GameManager.TowerSlots.Count - 1)];
                towerSlot.SlotLock(Instance.StopDuration);
            }
        }
    }

    public static void SpeedUpAttack()
    {
        int gold = GameManager.Instance.currentTypeState == TypeState.Attacker ? GameManager.Instance.masterGold : GameManager.Instance.userGold;
        if (gold >= Instance.speedUpAttackGold)
        {
            if (GameManager.Instance.currentTypeState == TypeState.Attacker) GameManager.Instance.masterGold -= Instance.speedUpAttackGold;
            else GameManager.Instance.userGold -= Instance.speedUpAttackGold;
            for (int i = 0; i < GameManager.Instance.CurrentMonsterList.Count; i++)
            {
                MonsterBase monster = GameManager.Instance.CurrentMonsterList[i];
                if (monster != null) monster.MoveSpeed += monster.MoveSpeed * (Instance.speedUpIncreaseRate / 100);
            }
        }
    }

    public static void DelayAttack()
    {
        int gold = GameManager.Instance.currentTypeState == TypeState.Attacker ? GameManager.Instance.masterGold : GameManager.Instance.userGold;
        if (gold >= Instance.delayAttackGold)
        {
            if (GameManager.Instance.currentTypeState == TypeState.Attacker) GameManager.Instance.masterGold -= Instance.delayAttackGold;
            else GameManager.Instance.userGold -= Instance.delayAttackGold;
            Instance.IsDelay = true;
            List<float> delayList = new List<float>();
            foreach (TowerSlot towerSlot in GameManager.TowerSlots)
            {
                if (towerSlot.CurrentTower != null)
                {
                    delayList.Add(towerSlot.CurrentTower.AttackDelay);
                    towerSlot.CurrentTower.SetAttackDelay(towerSlot.CurrentTower.AttackDelay - (towerSlot.CurrentTower.AttackDelay * (Instance.DelayRate / 100)));
                }
                else delayList.Add(0);
            }

            Instance.OriginalAttackDelayList = delayList;
        }
    }

    public static void EndDelayAttack()
    {
        Instance.IsDelay = false;
        for (int i = 0; i < Instance.OriginalAttackDelayList.Count; i++)
        {
            if (Instance.OriginalAttackDelayList[i] != 0) GameManager.TowerSlots[i].CurrentTower.SetAttackDelay(Instance.OriginalAttackDelayList[i]);
        }
    }

    public static void LevelDownAttack()
    {
        int gold = GameManager.Instance.currentTypeState == TypeState.Attacker ? GameManager.Instance.masterGold : GameManager.Instance.userGold;
        if (gold >= Instance.levelDownAttackGold)
        {
            if (GameManager.Instance.currentTypeState == TypeState.Attacker) GameManager.Instance.masterGold -= Instance.levelDownAttackGold;
            else GameManager.Instance.userGold -= Instance.levelDownAttackGold;
            foreach (TowerSlot towerSlot in GameManager.TowerSlots)
            {
                if (towerSlot.CurrentTower != null)
                {
                    if (towerSlot.CurrentTower.DamageLevel > 1)
                    {
                        towerSlot.CurrentTower.DamageLevel--;
                        towerSlot.CurrentTower.Damage -= towerSlot.CurrentTower.DamageIncreaseValue;
                    }
                }
            }
        }
    }
}
