using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Image TowerSeletPanel;

    [SerializeField] private Button SingleAttack;
    [SerializeField] private Button DebuffAttack;
    [SerializeField] private Button BuffAttack;
    [SerializeField] private Button RangeAttack;

    [SerializeField] private Button Skip;


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
                    TowerSeletPanel.gameObject.SetActive(true);
                }
                else
                {
                    currentSlot = null;
                    TowerSeletPanel.gameObject.SetActive(false);
                }
            }
        }
    }

    private void EnableTowerInfoPanel()
    {

    }

    private void ButtonInit()
    {
        SingleAttack.onClick.AddListener(() =>
        {
            TowerBase tower = Instantiate(SinglePrefab, currentSlot.transform).GetComponent<TowerBase>();
            currentSlot.currentTower = tower;
            currentSlot = null;
            TowerSeletPanel.gameObject.SetActive(false);
        });
        DebuffAttack.onClick.AddListener(() =>
        {
            TowerBase tower = Instantiate(DebuffPrefab, currentSlot.transform).GetComponent<TowerBase>();
            currentSlot.currentTower = tower;
            currentSlot = null;
            TowerSeletPanel.gameObject.SetActive(false);
        });
        BuffAttack.onClick.AddListener(() =>
        {
            TowerBase tower = Instantiate(BuffPrefab, currentSlot.transform).GetComponent<TowerBase>();
            currentSlot.currentTower = tower;
            currentSlot = null;
            TowerSeletPanel.gameObject.SetActive(false);
        });
        RangeAttack.onClick.AddListener(() =>
        {
            TowerBase tower = Instantiate(RangePrefab, currentSlot.transform).GetComponent<TowerBase>();
            currentSlot.currentTower = tower;
            currentSlot = null;
            TowerSeletPanel.gameObject.SetActive(false);
        });

        Skip.onClick.AddListener(() => GameManager.DefenceTurnSkip());
    }

    #endregion 
}
