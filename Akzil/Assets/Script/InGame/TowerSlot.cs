using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    public TowerBase currentTower;
    public bool CanUse = true;
    [SerializeField] private GameObject lockObject;

    public void SlotLock(float time)
    {
        if(CanUse) StartCoroutine(Lock());
        IEnumerator Lock()
        {
            CanUse = false;
            lockObject.SetActive(true);
            yield return new WaitForSeconds(time);
            CanUse = true;
            lockObject.SetActive(false);
        }
    }
}
