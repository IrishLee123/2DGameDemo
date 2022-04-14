using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaldPirate : Enemy
{
    [HideInInspector] public Animator myAnimator;
    private static readonly int VelocityH = Animator.StringToHash("velocity_h");
    private static readonly int Hit = Animator.StringToHash("hit");
    private static readonly int AttackTrigger = Animator.StringToHash("kick");

    [Header("Special Setting")] public float kickForce;

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

    public void OnKick(List<Collider2D> list)
    {
        bool isPlayer = true;

        //先找玩家
        var attackTarget = list.FirstOrDefault(col => col.transform.CompareTag("Player"));

        //没有敌人的时候再找引爆中的炸弹
        if (attackTarget == null)
        {
            isPlayer = false;
            attackTarget = list.FirstOrDefault(col =>
                col.transform.CompareTag("Bomb") && col.transform.GetComponent<Bomb>().Triggered);
        }

        if (!attackTarget) return;

        if (isPlayer)
        {
            //玩家受伤
            var player = attackTarget.transform.GetComponent<IHurtable>();
            player.BeenHurt(1);
        }
        else
        {
            //踢飞炸弹
            var rig = attackTarget.transform.GetComponent<Rigidbody2D>();
            Vector3 dir = rig.transform.position - transform.position;
            dir.y = 0.2f;
            rig.AddForce(dir.normalized * kickForce, ForceMode2D.Impulse);
        }
    }

    public override void OnFindTarget()
    {
        base.OnFindTarget();

        FlipDirection(); //根据目标设定朝向
        myAnimator.Play("BaldRun");
    }
}