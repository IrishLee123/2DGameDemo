using System.Linq;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    private static readonly int attackTrigger = Animator.StringToHash("attack");

    private static readonly float AttackDuration = 0.5f; //攻击动画时长
    private static readonly float WaitDuration = 2f; //攻击间隔

    private float _attackTimer, _waitTimer;

    public override void OnEnter(Enemy enemy)
    {
        Debug.Log("enter attack state.");
        _attackTimer = _waitTimer = 0;
        enemy.myAnimator.SetTrigger(attackTrigger); //设置动画状态机为攻击动画
    }

    public override void OnUpdate(Enemy enemy)
    {
        _attackTimer += Time.deltaTime;
        if (_attackTimer < AttackDuration) return;

        //TODO: 进行伤害判定

        _waitTimer += Time.deltaTime;
        if (_waitTimer < WaitDuration) return;

        var attackTarget = enemy.attackList.FirstOrDefault(t => t.CompareTag("Player"));

        if (!attackTarget) //是否仍存在攻击目标
        {
            enemy.ResetTargetPoint(); //重置目标点
            enemy.TransitionToState(EnemyState.PatrolState); //没有目标回到巡逻状态
            return;
        }

        enemy.TransitionToState(EnemyState.ChasingState); //有目标切换回追击状态
    }

    public override void OnExit(Enemy enemy)
    {
        Debug.Log("exit attack state.");
    }
}