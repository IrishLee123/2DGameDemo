using Script.Utils;
using UnityEngine;

public class CucumberAttacker : MonoBehaviour
{
    private Trigger2DCheck _check;

    private void Awake()
    {
        _check = GetComponent<Trigger2DCheck>();

        _check.OnTriggerEnterChanged += (list =>
        {
            // 计算伤害
        });
    }
}
