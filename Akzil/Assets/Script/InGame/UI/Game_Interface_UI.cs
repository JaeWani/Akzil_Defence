using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Game_Interface_UI : MonoBehaviour
{
    #region Variable
    public static Game_Interface_UI Instance { get; private set; }

    [Header("Interface")]
    [SerializeField] private TextMeshProUGUI roundTimeText;
    [SerializeField] private TextMeshProUGUI roundText;

    [SerializeField] private RectTransform roundStartEmotion;
    [SerializeField] private TextMeshProUGUI roundStartEmotionText;
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
    public static void SetRoundTime(int roundTime) => Instance.roundTimeText.text = "다음 라운드 : " + roundTime;
    private void SetRoundText()
    {
        roundText.text = GameManager.CurrentTurn == TurnState.AttackTurn ? "이번 라운드 : 공격" : "이번 라운드 : 수비";  
    }
    public Coroutine StartRoundEmotion(string text) => Instance._StartRoundEmotion(text);
    private Coroutine _StartRoundEmotion(string text)
    {
        return StartCoroutine(Emotion());
        IEnumerator Emotion()
        {
            roundStartEmotionText.text = text;
            roundStartEmotion.DOAnchorPosX(-1080, 0.5f);
            yield return new WaitForSeconds(0.5f + 2);
            roundStartEmotion.DOAnchorPosX(-1080, 0.5f);
            yield return new WaitForSeconds(0.5f + 1);
        }
    }
    #endregion
}
