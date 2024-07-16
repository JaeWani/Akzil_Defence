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

    public float StopDuration { get { return stopDuration; } private set { stopDuration = value; } }
    public float StopCount { get { return stopCount; } private set { stopCount = value; } }

    [Header("Speed Up Attack")]
    [SerializeField] private float speedUpIncreaseRate;

    public float SpeedUpIncreaseRate { get { return speedUpIncreaseRate; } private set { speedUpIncreaseRate = value; } }

    [Header("Delay Attack")]
    [SerializeField] private bool isDelay;
    [SerializeField] private float delayRate;
    [SerializeField] private List<float> originalAttackDelayList;

    public bool IsDelay { get { return isDelay; } private set { isDelay = value; } }
    public float DelayRate { get { return delayRate; } private set { delayRate = value; } }
    public List<float> OriginalAttackDelayList { get { return originalAttackDelayList; } private set { originalAttackDelayList = value; } }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public static void StopAttack()
    {
        for (int i = 0; i < Instance.StopCount; i++)
        {
            TowerSlot towerSlot = GameManager.TowerSlots[Random.Range(0, GameManager.TowerSlots.Count - 1)];
            towerSlot.SlotLock(Instance.StopDuration);
        }
    }

    public static void SpeedUpAttack()
    {
        for (int i = 0; i < GameManager.Instance.CurrentMonsterList.Count; i++)
        {
            MonsterBase monster = GameManager.Instance.CurrentMonsterList[i];
            if (monster != null) monster.MoveSpeed += monster.MoveSpeed * (Instance.speedUpIncreaseRate / 100);
        }
    }

    public static void DelayAttack()
    {
        Instance.IsDelay = true;
        List<float> delayList = new List<float>();
        foreach (TowerSlot towerSlot in GameManager.TowerSlots)
        {
            if (towerSlot.currentTower != null)
            {
                delayList.Add(towerSlot.currentTower.AttackDelay);
                towerSlot.currentTower.SetAttackDelay(towerSlot.currentTower.AttackDelay - (towerSlot.currentTower.AttackDelay * (Instance.DelayRate / 100)));
            }
            else delayList.Add(0);
        }

        Instance.OriginalAttackDelayList = delayList;
    }

    public static void EndDelayAttack()
    {
        Instance.IsDelay = false;
        for (int i = 0; i < Instance.OriginalAttackDelayList.Count; i++)
        {
            if (Instance.OriginalAttackDelayList[i] != 0) GameManager.TowerSlots[i].currentTower.SetAttackDelay(Instance.OriginalAttackDelayList[i]);
        }
    }

    public static void LevelDownAttack()
    {
        foreach(TowerSlot towerSlot in GameManager.TowerSlots)
        {
            if(towerSlot.currentTower != null)
            {
                if(towerSlot.currentTower.DamageLevel > 1)
                {
                    towerSlot.currentTower.DamageLevel--;
                    towerSlot.currentTower.Damage -= towerSlot.currentTower.DamageIncreaseValue;
                }
            }
        }
    }
}
