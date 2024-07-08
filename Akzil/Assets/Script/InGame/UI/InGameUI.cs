using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    #region Variable
    public static InGameUI Instance { get; private set; }

    private Camera mainCamera;

    [SerializeField] private TowerSlot currentSlot = null;

    #region UI Variable
    [Header("UI")]
    [SerializeField] private Image TowerSeletPanel;

    [SerializeField] private Button SingleAttack;
    [SerializeField] private Button DebuffAttack;
    [SerializeField] private Button BuffAttack;
    [SerializeField] private Button RangeAttack;
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
        if (Input.GetMouseButtonDown(0))
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
                    Debug.Log("활성화");
                }
                else
                {
                    currentSlot = null;
                    TowerSeletPanel.gameObject.SetActive(false);
                    Debug.Log("비활성화");
                }
            }
        }
    }
    #endregion

    #region Function
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
    }

    #endregion 
}