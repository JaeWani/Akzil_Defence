using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    [SerializeField] private TowerBase currentTower;
    public TowerBase CurrentTower 
    { 
        get { return currentTower; } 
        set 
        { 
            currentTower = value; 
            DelayDecreaseValue = DelayDecreaseValue * 1;
        } 
    }
    public bool CanUse = true;
    [SerializeField] private float delayDecreaseValue;
    public float DelayDecreaseValue
    {
        get { return delayDecreaseValue; }
        set
        {
            delayDecreaseValue = value;
            if (CurrentTower != null)
            {
                Debug.Log("앙가잇");
                CurrentTower.AttackDelay = CurrentTower.OrignalAttackDelay - delayDecreaseValue;
            }
        }
    }
    [SerializeField] private GameObject lockObject;

    public void SlotLock(float time)
    {
        if (CanUse) StartCoroutine(Lock());
        IEnumerator Lock()
        {
            CanUse = false;
            lockObject.SetActive(true);
            yield return new WaitForSeconds(time);
            CanUse = true;
            lockObject.SetActive(false);
        }
    }
    public void Reset()
    {
        if(CurrentTower != null)
        {
            Destroy(CurrentTower.gameObject);
            CurrentTower = null;
        }
    }
}
