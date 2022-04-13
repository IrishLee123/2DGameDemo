using System.Linq;
using UnityEngine;

/// <summary>
/// 巡逻状态
/// </summary>
public class PatrolState : EnemyBaseState
{
    private static readonly float IdleDuration = 2f;
    private float _idleTimer;

    public override void OnEnter(Enemy enemy)
    {
        _idleTimer = 0;
        enemy.ResetTargetPoint();
    }

    public override void OnUpdate(Enemy enemy)
    {
        //先找引线点燃的炸弹
        var attackTarget= enemy.attackList.FirstOrDefault(transform => transform.CompareTag("Bomb") && transform.GetComponent<Bomb>().Triggered);
        //再找敌人
        if (attackTarget == null)
            attackTarget = enemy.attackList.FirstOrDefault(transform => transform.CompareTag("Player"));
        
        if (attackTarget)
        {
            enemy.targetPoint = attackTarget;//设置玩家为之为目标点
            enemy.TransitionToState(EnemyState.ChasingState);//切换追击状态
            return;
        }

        if (!enemy.ArriveTargetPoint()) // 检查是否到达目标点
        {
            enemy.MoveToTarget(); // 未到达目标点继续移动
            return;
        }

        _idleTimer += Time.deltaTime;
        if (_idleTimer < IdleDuration) //在切换至下一目标点之前先停留一段时间
        {
            return;
        }

        _idleTimer = 0;

        enemy.SetNextPoint(); // 到达目标点切换下一个目标点
    }

    public override void OnExit(Enemy enemy)
    {
    }
}