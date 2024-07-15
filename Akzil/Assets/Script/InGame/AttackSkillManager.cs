using System.Collections;
using System.Collections.Generic;
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
}
