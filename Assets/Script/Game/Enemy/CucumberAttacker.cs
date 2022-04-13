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
            foreach (var col in list)
            {
                if (col.transform.CompareTag("Player"))
                {
                    var player = col.transform.GetComponent<IHurtable>();
                    player.BeenHurt(1);
                }
            }
        });
    }
}