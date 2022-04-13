using System;
using System.Collections.Generic;
using Script.Utils;
using UnityEngine;

public class CucumberBlower : MonoBehaviour
{
    private Trigger2DCheck _check;

    private void Awake()
    {
        _check = GetComponent<Trigger2DCheck>();
    }

    private void OnEnable()
    {
        _check.OnTriggerEnterChanged += DoAction;
    }

    private void OnDisable()
    {
        _check.OnTriggerEnterChanged -= DoAction;
    }

    private void DoAction(List<Collider2D> list)
    {
        foreach (var col in list)
        {
            var bomb = col.transform.GetComponent<Bomb>();
            bomb.UnTrigger();//吹灭炸弹
        }
    }
}