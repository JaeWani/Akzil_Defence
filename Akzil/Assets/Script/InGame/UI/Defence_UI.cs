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
    [SerializeField] private bool isInfoPanel = false;

    #region UI Variable
    [Header("UI")]
    [SerializeField] private Button Skip;
    [SerializeField] private TextMeshProUGUI DefencerGoldText;
    [SerializeField] private TextMeshProUGUI TowerInstallCountText;

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

    [SerializeField] private Image TowerImage;
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
    private void Awake() {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        mainCamera = Camera.main;
        ButtonInit();
    }

    private void Update()
    {
        EnableTowerPanel();
        EnableTowerInfoPanel();
        DefencerGoldText.text = GameManager.Instance.DefencerGold + "G";
        TowerInstallCountText.text = "설치 가능한 타워 : " + GameManager.Instance.CurrentRoundInstallTowerCount + "개";
    }
    #endregion

    #region Function
    private void EnableTowerPanel()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.CurrentTurn == TurnState.DefenceTurn && GameManager.Instance.currentTypeState == TypeState.Defencer)
        {
            if (GameManager.Instance.MaxRoundInstallTowerCount - GameManager.Instance.CurrentRoundInstallTowerCount != GameManager.Instance.MaxRoundInstallTowerCount)
            {
                if (!isInfoPanel)
                {
                    Vector2 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

                    if (hit.collider != null)
                    {
                        GameObject clickObj = hit.transform.gameObject;

                        if (clickObj.TryGetComponent(out TowerSlot towerSlot))
                        {
                            if (towerSlot.CurrentTower == null)
                            {
                                currentSlot = towerSlot;
                                TowerSelectPanel.gameObject.SetActive(true);
                            }
                        }
                        else
                        {
                            currentSlot = null;
                            TowerSelectPanel.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    private void EnableTowerInfoPanel()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if (hit.collider != null)
            {
                GameObject clickObj = hit.transform.gameObject;

                if (clickObj.TryGetComponent(out TowerBase towerBase))
                {
                    Debug.Log(clickObj.name);

                    CurrentTower = towerBase;
                    TowerInfoPanel.gameObject.SetActive(true);
                    isInfoPanel = true;

                    TowerImage.sprite = CurrentTower.GetComponent<SpriteRenderer>().sprite;
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
            tower.name = "싱글 타워";
            currentSlot.CurrentTower = tower;
            currentSlot = null;
            GameManager.Instance.CurrentRoundInstallTowerCount--;
            TowerSelectPanel.gameObject.SetActive(false);
        });
        DebuffAttack.onClick.AddListener(() =>
        {
            TowerBase tower = Instantiate(DebuffPrefab, currentSlot.transform).GetComponent<TowerBase>();
            tower.name = "디버프 타워";
            currentSlot.CurrentTower = tower;
            currentSlot = null;
            GameManager.Instance.CurrentRoundInstallTowerCount--;
            TowerSelectPanel.gameObject.SetActive(false);
        });
        BuffAttack.onClick.AddListener(() =>
        {
            TowerBase tower = Instantiate(BuffPrefab, currentSlot.transform).GetComponent<TowerBase>();
            tower.name = "버프 타워";
            currentSlot.CurrentTower = tower;
            currentSlot = null;
            GameManager.Instance.CurrentRoundInstallTowerCount--;
            TowerSelectPanel.gameObject.SetActive(false);
        });
        RangeAttack.onClick.AddListener(() =>
        {
            TowerBase tower = Instantiate(RangePrefab, currentSlot.transform).GetComponent<TowerBase>();
            tower.name = "광역 타워";
            currentSlot.CurrentTower = tower;
            currentSlot = null;
            GameManager.Instance.CurrentRoundInstallTowerCount--;
            TowerSelectPanel.gameObject.SetActive(false);
        });

        Skip.onClick.AddListener(() => GameManager.DefenceTurnSkip());

        TowerInfoExit.onClick.AddListener(() =>
        {
            TowerInfoPanel.gameObject.SetActive(false);
            CurrentTower = null;
            isInfoPanel = false;
        });

        DelayUpgrade.onClick.AddListener(() =>
        {


            if (CurrentTower != null)
            {
                if (CurrentTower.DelayLevel - 1 < CurrentTower.MaxDelayLevel - 1)
                {
                    int gold = GameManager.Instance.currentTypeState == TypeState.Defencer ? GameManager.Instance.masterGold : GameManager.Instance.userGold;
                    if (gold >= 10)
                    {
                        if (GameManager.Instance.currentTypeState == TypeState.Defencer) GameManager.Instance.masterGold -= 10;
                        else GameManager.Instance.userGold -= 10;
                        CurrentTower.DelayLevel++;
                        CurrentTower.AttackDelay -= CurrentTower.DelayDecreaseValue[CurrentTower.DelayLevel - 1];
                        TowerDelay.text = "Delay : " + CurrentTower.AttackDelay;
                    }
                }
            }

        });

        DamageUpgrade.onClick.AddListener(() =>
        {
            int gold = GameManager.Instance.currentTypeState == TypeState.Defencer ? GameManager.Instance.masterGold : GameManager.Instance.userGold;
            if (gold >= 5)
            {
                if (GameManager.Instance.currentTypeState == TypeState.Defencer) GameManager.Instance.masterGold -= 5;
                else GameManager.Instance.userGold -= 5;
                if (CurrentTower != null)
                {
                    CurrentTower.DamageLevel++;
                    CurrentTower.Damage += CurrentTower.DamageIncreaseValue;
                    TowerDamage.text = "Damage : " + CurrentTower.Damage;
                }
            }
        });
    }

    public void SpawnTower(TowerSlot towerSlot)
    {
        TowerBase tower = Instantiate(SinglePrefab, towerSlot.transform).GetComponent<TowerBase>();
        tower.name = "싱글 타워";
        towerSlot.CurrentTower = tower;
        currentSlot = null;
        GameManager.Instance.CurrentRoundInstallTowerCount--;
        TowerSelectPanel.gameObject.SetActive(false);
    }

    #endregion 
}
