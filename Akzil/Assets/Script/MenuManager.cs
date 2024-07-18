using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button StartBtn;
    [SerializeField] private Button Setting;
    [SerializeField] private Button ExitBtn;

    [SerializeField] private Image MatchingPanel;


    void Start()
    {
        StartBtn.onClick.AddListener(() => { StartCoroutine(enumerator()); SoundManager.Instance.Play("BTN", false); });
        ExitBtn.onClick.AddListener(() => { SoundManager.Instance.Play("BTN", false); Application.Quit(); });
    }

    IEnumerator enumerator()
    {
        MatchingPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
