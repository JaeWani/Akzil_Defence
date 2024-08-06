using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class AttackSkillManager : MonoBehaviour
{

    public static AttackSkillManager Instance { get; private set; }

    private enum AttackSkillType { Stop, SpeedUp, Delay, LevelDown }



    [Header("Use UI")]
    [SerializeField] private Queue<AttackSkillType> skillTypeQueue = new Queue<AttackSkillType>();
    private Coroutine SkillEmotionCoroutine;

    [SerializeField] private RectTransform skillUseImage;
    [SerializeField] private Image skillImage;
    [SerializeField] private RectTransform lightImage;

    [SerializeField] private TextMeshProUGUI skillText;

    [Header("Stop Attack")]
    [SerializeField] private float stopDuration;
    [SerializeField] private float stopCount;
    [SerializeField] private int stopAttackGold;

    [SerializeField] private Sprite stopAttackSprite;
    [SerializeField] private string stopAttackName;

    public float StopDuration { get { return stopDuration; } private set { stopDuration = value; } }
    public float StopCount { get { return stopCount; } private set { stopCount = value; } }

    [Header("Speed Up Attack")]
    [SerializeField] private float speedUpIncreaseRate;
    [SerializeField] private int speedUpAttackGold;

    [SerializeField] private Sprite speedUpAttackSprite;
    [SerializeField] private string speedUpAttackName;

    public float SpeedUpIncreaseRate { get { return speedUpIncreaseRate; } private set { speedUpIncreaseRate = value; } }

    [Header("Delay Attack")]
    [SerializeField] private bool isDelay;
    [SerializeField] private float delayRate;
    [SerializeField] private List<float> originalAttackDelayList;
    [SerializeField] private int delayAttackGold;

    [SerializeField] private Sprite delayAttackSprite;
    [SerializeField] private string delayAttackName;

    public bool IsDelay { get { return isDelay; } private set { isDelay = value; } }
    public float DelayRate { get { return delayRate; } private set { delayRate = value; } }
    public List<float> OriginalAttackDelayList { get { return originalAttackDelayList; } private set { originalAttackDelayList = value; } }
    [Header("Level Down")]
    [SerializeField] private int levelDownAttackGold;

    [SerializeField] private Sprite levelDownAttackSprite;
    [SerializeField] private string levelDownAttackName;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        Vector3 rotation = lightImage.localEulerAngles;
        rotation.z += 45f * Time.deltaTime; // Z 축으로 45도 회전
        lightImage.localEulerAngles = rotation;
    }

    public static void StopAttack()
    {
        int gold = GameManager.Instance.currentTypeState == TypeState.Attacker ? GameManager.Instance.masterGold : GameManager.Instance.userGold;
        if (gold >= Instance.stopAttackGold)
        {
            if (GameManager.Instance.currentTypeState == TypeState.Attacker) GameManager.Instance.masterGold -= Instance.stopAttackGold;
            else GameManager.Instance.userGold -= Instance.stopAttackGold;
            Instance.Enqueue(AttackSkillType.Stop);
            for (int i = 0; i < Instance.StopCount; i++)
            {
                TowerSlot towerSlot = GameManager.TowerSlots[UnityEngine.Random.Range(0, GameManager.TowerSlots.Count - 1)];
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
            Instance.Enqueue(AttackSkillType.SpeedUp);
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
            Instance.Enqueue(AttackSkillType.Delay);
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

            Instance.Enqueue(AttackSkillType.LevelDown);
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

    private void Enqueue(AttackSkillType attackSkillType)
    {
        skillTypeQueue.Enqueue(attackSkillType);
        if (SkillEmotionCoroutine == null) SkillEmotion();
    }

    private Coroutine SkillEmotion()
    {
        Coroutine coroutine = StartCoroutine(start());
        SkillEmotionCoroutine = coroutine;
        return coroutine;
        IEnumerator start()
        {
            Sprite sprite = null;
            string text = null;
            if (skillTypeQueue.Count > 0)
            {
                AttackSkillType attackSkillType = skillTypeQueue.Dequeue();

                switch (attackSkillType)
                {
                    case AttackSkillType.Stop: sprite = stopAttackSprite; text = stopAttackName; break;
                    case AttackSkillType.SpeedUp: sprite = speedUpAttackSprite; text = speedUpAttackName; break;
                    case AttackSkillType.Delay: sprite = delayAttackSprite; text = delayAttackName; break;
                    case AttackSkillType.LevelDown: sprite = levelDownAttackSprite; text = levelDownAttackName; break;
                }
                yield return Emotion(sprite, text + " 발동 !");
                SkillEmotion();
            }
            else yield return null;
        }
    }

    private Coroutine Emotion(Sprite sprite, string text)
    {
        return StartCoroutine(Emotion());
        IEnumerator Emotion()
        {
            skillText.text = text;
            skillImage.sprite = sprite;
            skillUseImage.DOAnchorPosX(0, 0.5f);
            yield return new WaitForSeconds(0.5f + 2);
            skillUseImage.DOAnchorPosX(+1080, 0.5f);
            yield return new WaitForSeconds(0.5f + 1);
            skillUseImage.DOAnchorPosX(-1090, 0);
            yield return new WaitForSeconds(1f);
        }
    }
}