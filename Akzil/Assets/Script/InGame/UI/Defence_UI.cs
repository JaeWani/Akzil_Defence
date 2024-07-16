using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Defence_UI : MonoBehaviour
{
    #region Variable
    public static Defence_UI Instance { get; private set; }

    private Camera mainCamera;

    [SerializeField] private TowerSlot currentSlot = null;

    #region UI Variable
    [Header("UI")]
    [SerializeField] private Button Skip;

    [Header("Tower Select Panel")]
    [SerializeField] private Image TowerSelectPanel;

    [SerializeField] private Button SingleAttack;
    [SerializeField] private Button DebuffAttack;
    [SerializeField] private Button BuffAttack;
    [SerializeField] private Button RangeAttack;

    [Header("Tower Info Panel")]
    [SerializeField] private TowerBase currentTower;
    public TowerBase CurrentTower { get { return currentTower; } private set { currentTower = value; } }

    [SerializeField] private Image TowerInfoPanel;

    [SerializeField] private Button TowerInfoExit;
    [SerializeField] private Button DelayUpgrade;
    [SerializeField] private Button DamageUpgrade;

    [SerializeField] private TextMeshProUGUI TowerName;
    [SerializeField] private TextMeshProUGUI TowerDelay;
    [SerializeField] private TextMeshProUGUI TowerDamage;

    #endregion
    #region Prefab Variable
    [Header("Prefab")]
    [SerializeField] private GameObject SinglePrefab;
    [SerializeField] private GameObject DebuffPrefab;
    [SerializeField] private GameObject BuffPrefab;
    [SerializeField] private GameObject RangePrefab;
    #endregion
    #endregion

    #region  Unity_Function
    private void Start()
    {
        mainCamera = Camera.main;
        ButtonInit();
    }

    private void Update()
    {
        EnableTowerPanel();
        EnableTowerInfoPanel();
    }
    #endregion

    #region Function
    private void EnableTowerPanel()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.CurrentTurn == TurnState.DefenceTurn)
        {
            Vector2 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if (hit.collider != null)
            {
                GameObject clickObj = hit.transform.gameObject;

                if (clickObj.TryGetComponent(out TowerSlot towerSlot))
                {
                    currentSlot = towerSlot;
                    TowerSelectPanel.gameObject.SetActive(true);
                }
                else
                {
                    currentSlot = null;
                    TowerSelectPanel.gameObject.SetActive(false);
                }
            }
        }
    }

    private void EnableTowerInfoPanel()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if(hit.collider != null)
            {
                GameObject clickObj = hit.transform.gameObject;

                if(clickObj.TryGetComponent(out TowerBase towerBase))
                {
                    Debug.Log(clickObj.name);
                    CurrentTower = towerBase;
                    TowerInfoPanel.gameObject.SetActive(true);

                    TowerName.text = CurrentTower.name;
                    TowerDelay.text = "Delay : " + CurrentTower.AttackDelay;
                    TowerDamage.text = "Damage : " + CurrentTower.Damage;
                }
            }
        }
    }

    private void ButtonInit()
    {
        SingleAttack.onClick.AddListener(() =>
        {
            TowerBase tower = Instantiate(SinglePrefab, currentSlot.transform).GetComponent<TowerBase>();
            currentSlot.currentTower = tower;
            currentSlot = null;
            TowerSelectPanel.gameObject.SetActive(false);
        });
        DebuffAttack.onClick.AddListener(() =>
        {
            TowerBase tower = Instantiate(DebuffPrefab, currentSlot.transform).GetComponent<TowerBase>();
            currentSlot.currentTower = tower;
            currentSlot = null;
            TowerSelectPanel.gameObject.SetActive(false);
        });
        BuffAttack.onClick.AddListener(() =>
        {
            TowerBase tower = Instantiate(BuffPrefab, currentSlot.transform).GetComponent<TowerBase>();
            currentSlot.currentTower = tower;
            currentSlot = null;
            TowerSelectPanel.gameObject.SetActive(false);
        });
        RangeAttack.onClick.AddListener(() =>
        {
            TowerBase tower = Instantiate(RangePrefab, currentSlot.transform).GetComponent<TowerBase>();
            currentSlot.currentTower = tower;
            currentSlot = null;
            TowerSelectPanel.gameObject.SetActive(false);
        });

        Skip.onClick.AddListener(() => GameManager.DefenceTurnSkip());

        TowerInfoExit.onClick.AddListener(() => 
        { 
            TowerInfoPanel.gameObject.SetActive(false);
            CurrentTower = null;
        });

        DelayUpgrade.onClick.AddListener(() => 
        {
            if(CurrentTower != null)
            {
                if(CurrentTower.DelayLevel - 1 < CurrentTower.MaxDelayLevel - 1) 
                {
                    CurrentTower.DelayLevel++;
                    CurrentTower.AttackDelay = CurrentTower.DelayDecreaseValue[CurrentTower.DelayLevel -1];
                    TowerDelay.text = "Delay : " + CurrentTower.AttackDelay;
                }
            }
        });

        DamageUpgrade.onClick.AddListener(() => 
        {
            if(CurrentTower != null)
            {
                CurrentTower.DamageLevel++;
                CurrentTower.Damage += CurrentTower.DamageIncreaseValue;
                TowerDamage.text = "Damage : " + CurrentTower.Damage;
            }
        });
    }

    #endregion 
}
