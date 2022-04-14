using System.Linq;
using UnityEngine;

/// <summary>
/// 追击状态
/// </summary>
public class ChasingState : EnemyBaseState
{
    private float _timer;

    public override void OnEnter(Enemy enemy)
    {
        _timer = 0;
    }

    public override void OnUpdate(Enemy enemy)
    {
        var attackTarget = FindTarget(enemy);

        if (!attackTarget) //是否仍存在攻击目标
        {
            _timer += Time.deltaTime;
            if (_timer < enemy.lossTargetWaitDuration)
            {
                return;
            }

            enemy.ResetTargetPoint(); //重置目标点

            enemy.TransitionToState(EnemyState.PatrolState); //持续一段时间未发现敌人切换回巡逻状态

            return;
        }

        enemy.targetPoint = attackTarget; //持续更新目标
        _timer = 0; //有敌人重置等待时间

        if (!enemy.ArriveTargetPoint(enemy.atkRange))
        {
            enemy.MoveToTarget(); //继续朝目标移动
            return;
        }

        enemy.TransitionToState(EnemyState.AttackState); //追上目标，进入攻击状态
    }

    public override void OnExit(Enemy enemy)
    {
    }
}