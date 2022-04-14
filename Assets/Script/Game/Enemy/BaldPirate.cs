using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldPirate : Enemy
{
    [HideInInspector] public Animator myAnimator;
    private static readonly int VelocityH = Animator.StringToHash("velocity_h");
    private static readonly int Hit = Animator.StringToHash("hit");
    private static readonly int AttackTrigger = Animator.StringToHash("kick");

    private Vector2 _lastPosition;

    protected override void Init()
    {
        base.Init();

        stateMap.Add(EnemyState.PatrolState, new PatrolState());
        stateMap.Add(EnemyState.ChasingState, new ChasingState());
        stateMap.Add(EnemyState.AttackState, new AttackState());

        myAnimator = GetComponentInChildren<Animator>();

        _lastPosition = transform.position;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        var position = transform.position;
        var delat = position.x - _lastPosition.x;
        myAnimator.SetFloat(VelocityH, Math.Abs(delat / Time.deltaTime)); //设置动画状态机参数
        _lastPosition = position;
    }

    public override void BeenHurt(float damage)
    {
        myAnimator.SetTrigger(Hit);
    }

    public override void AttackAction()
    {
        base.AttackAction();
        myAnimator.SetTrigger(AttackTrigger);
    }

    public override void SkillAction()
    {
        base.SkillAction();
        myAnimator.SetTrigger(AttackTrigger);
    }

    public void OnBlowOffBomb(List<Collider2D> list)
    {
        foreach (var col in list)
        {
            var bomb = col.transform.GetComponent<Bomb>();
            bomb.UnTrigger(); //吹灭炸弹
        }
    }

    public void OnAttackPlayer(List<Collider2D> list)
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
    }
}