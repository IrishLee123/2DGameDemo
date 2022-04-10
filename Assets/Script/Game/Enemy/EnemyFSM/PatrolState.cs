using UnityEngine;

/// <summary>
/// 巡逻状态
/// </summary>
public class PatrolState : EnemyBaseState
{
    private static readonly int State = Animator.StringToHash("State");

    private static readonly float IdleDuration = 2f;
    private float _idleTimer;

    public override void OnEnter(Enemy enemy)
    {
        _idleTimer = 0;
        enemy.ResetTargetPoint();
    }

    public override void OnUpdate(Enemy enemy)
    {
        Transform attackTarget = null;
        for (var i = 0; i < enemy.attackList.Count; i++)
        {
            if (!enemy.attackList[i].CompareTag("Player")) continue;//检查视野范围内是否存在玩家
            attackTarget = enemy.attackList[i];
            break;
        }

        if (attackTarget)
        {
            enemy.targetPoint = attackTarget;//设置玩家为之为目标点
            enemy.TransitionToState(EnemyState.ChasingState);//切换追击状态
            return;
        }

        if (!enemy.ArriveTargetPoint()) // 检查是否到达目标点
        {
            enemy.MoveToTarget(); // 未到达目标点继续移动
            enemy.myAnimator.SetInteger(State, 1); //设置动画状态机为跑步
            return;
        }

        _idleTimer += Time.deltaTime;
        if (_idleTimer < IdleDuration) //在切换至下一目标点之前先停留一段时间
        {
            enemy.myAnimator.SetInteger(State, 0); //设置动画状态机为静止
            return;
        }

        _idleTimer = 0;

        enemy.SetNextPoint(); // 到达目标点切换下一个目标点
    }

    public override void OnExit(Enemy enemy)
    {
    }
}