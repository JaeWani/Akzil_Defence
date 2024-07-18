using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuffTower : TowerBase
{
    public float TowerDelayDecreaseValue = 0.05f;
    protected override void Start()
    {
        base.Start();
        foreach(TowerSlot slot in GameManager.TowerSlots)
        {
            slot.DelayDecreaseValue += TowerDelayDecreaseValue;
        } 
    }
}
