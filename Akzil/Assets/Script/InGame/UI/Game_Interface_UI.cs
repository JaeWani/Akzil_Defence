using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game_Interface_UI : MonoBehaviour
{
    #region Variable
    public static Game_Interface_UI Instance { get; private set; }

    [Header("Interface")]
    [SerializeField] private TextMeshProUGUI roundTimeText;
    [SerializeField] private TextMeshProUGUI roundText;
    #endregion

    #region Unity_Function
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Update()
    {
        SetRoundText();
    }
    #endregion

    #region Function
    public static void SetRoundTime(int roundTime) => Instance.roundTimeText.text = "Next Round : " + roundTime;
    private void SetRoundText()
    {
        roundText.text = GameManager.CurrentTurn == TurnState.AttackTurn ? "Round : Attack" : "Round : Defence";  
    }
    #endregion
}
