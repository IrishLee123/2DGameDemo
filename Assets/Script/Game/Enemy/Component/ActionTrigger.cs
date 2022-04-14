using System.Collections.Generic;
using Script.Utils;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 当攻击行为触发时调用OnAction，便于不类型的Enemy作出不同响应
/// </summary>
public class ActionTrigger : MonoBehaviour
{
    public UnityEvent<List<Collider2D>> onAction;

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
        onAction.Invoke(list);
    }
}
