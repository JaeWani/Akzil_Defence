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
    [SerializeField] private Image roundTimeSlider;
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
        SetRoundText(GameManager.Instance.WaveCount);
    }
    #endregion

    #region Function
    public static void SetRoundTime(float currnet, float max) => Instance._SetRoundTime(currnet, max);
    private void _SetRoundTime(float current, float max)
    {
        roundTimeSlider.fillAmount = 1 - (current/max);   
    }
    private void SetRoundText(int round)
    {
        roundText.text = "라운드 " + round;
    }
    public static Coroutine StartRoundEmotion(string text, float fontSize) => Instance._StartRoundEmotion(text,fontSize);
    private Coroutine _StartRoundEmotion(string text, float fontSize)
    {
        roundStartEmotionText.fontSize = fontSize;
        return StartCoroutine(Emotion());
        IEnumerator Emotion()
        {
            roundStartEmotionText.text = text;
            roundStartEmotion.DOAnchorPosX(0, 0.5f);
            yield return new WaitForSeconds(0.5f + 2);
            roundStartEmotion.DOAnchorPosX(+1080, 0.5f);
            yield return new WaitForSeconds(0.5f + 1);
            roundStartEmotion.DOAnchorPosX(-1090, 0);
        }
    }
    #endregion
}
