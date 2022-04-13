using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Enemy
{

    protected override void Init()
    {
        base.Init();
    }

    public override void SkillAction()
    {
        base.SkillAction();

        if (!targetPoint.CompareTag("Bomb")) return;
        
        var bomb = targetPoint.GetComponent<Bomb>();
        bomb.UnTrigger();
    }
}