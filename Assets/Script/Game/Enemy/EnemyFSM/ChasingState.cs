using System.Linq;
using UnityEngine;

/// <summary>
/// 追击状态
/// </summary>
public class ChasingState : EnemyBaseState
{
    private static readonly float WaitDuration = 2f;

    private float _timer;

    public override void OnEnter(Enemy enemy)
    {
        _timer = 0;
    }

    public override void OnUpdate(Enemy enemy)
    {
        //先找引线点燃的炸弹
        var attackTarget= enemy.attackList.FirstOrDefault(transform => transform.CompareTag("Bomb") && transform.GetComponent<Bomb>().Triggered);
        //再找敌人
        if (attackTarget == null)
            attackTarget = enemy.attackList.FirstOrDefault(transform => transform.CompareTag("Player"));

        if (!attackTarget) //是否仍存在攻击目标
        {
            _timer += Time.deltaTime;
            if (_timer < WaitDuration)
            {
                return;
            }

            enemy.ResetTargetPoint(); //重置目标点
            enemy.TransitionToState(EnemyState.PatrolState); //持续一段时间未发现敌人切换回巡逻状态
            return;
        }

        enemy.targetPoint = attackTarget; //持续更新目标
        _timer = 0; //有敌人重置等待时间

        if (!enemy.ArriveTargetPoint(1.2f))
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