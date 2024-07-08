using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject MonsterPrefab;

    private void Start()
    {
        StartCoroutine(Spawn(5, 0.6f));
    }

    IEnumerator Spawn(int count, float delay)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(MonsterPrefab, new Vector2(-2.5f, -4.5f), Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(5);
        StartCoroutine(Spawn(5, 0.6f));
    }
}
