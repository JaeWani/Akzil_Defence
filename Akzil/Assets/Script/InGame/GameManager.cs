using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType { Basic, Speed, Boss }

public enum TurnState { Start, AttackTurn, DefenceTurn, End }

public class GameManager : MonoBehaviour
{
    #region Variable
    [SerializeField] private TurnState currentTurn = TurnState.Start;
    public TurnState CurrentTurn { get { return currentTurn; } set { currentTurn = value; } }

    private Queue<MonsterType> MonsterQueue = new Queue<MonsterType>();

    [Header ("Attack Turn")]
    private bool selectMonster = false;
    private int maxMonsterNumber = 6;

    [Header("Prefabs")]
    [SerializeField] private GameObject MonsterPrefab;


    #endregion

    #region Unity_Function
    private void Start()
    {

    }
    #endregion

    #region Function
    private void Progress(TurnState turn)
    {

    }

    private void StartMotion()
    {

    }

    private void AttackTurn()
    {
        StartCoroutine(StartTurn());
        IEnumerator StartTurn()
        {
            while (!selectMonster) yield return null; // 몬스터 선택하지 않을 시, 선택 할 때 까지 대기


        }
    }

    private void DefenceTurn()
    {

    }

    private void EndMotion()
    {

    }

    IEnumerator Spawn(int count, float delay)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(MonsterPrefab, new Vector2(-2.5f, -4.5f), Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(5);
        StartCoroutine(Spawn(5, 0.6f));
    }
    #endregion
}
