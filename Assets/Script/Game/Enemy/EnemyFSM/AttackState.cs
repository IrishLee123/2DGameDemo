using UnityEngine;

/// <summary>
/// 攻击状态
/// </summary>
public class AttackState : EnemyBaseState
{
    private float _attackTimer, _waitTimer;

    public override void OnEnter(Enemy enemy)
    {
        _attackTimer = _waitTimer = 0;

        if (enemy.targetPoint.CompareTag("Player"))
            enemy.AttackAction();
        else
            enemy.SkillAction();
    }

    public override void OnUpdate(Enemy enemy)
    {
        _attackTimer += Time.deltaTime;
        if (_attackTimer < enemy.atkDuration) return;

        //TODO: 进行伤害判定

        _waitTimer += Time.deltaTime;
        if (_waitTimer < enemy.atkWaitDuration) return;

        var attackTarget = FindTarget(enemy);

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
    }
}