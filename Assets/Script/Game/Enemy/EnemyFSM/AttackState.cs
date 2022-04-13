using System.Linq;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    private static readonly int attackTrigger = Animator.StringToHash("attack");
    private static readonly int skillTrigger = Animator.StringToHash("skill");

    private static readonly float AttackDuration = 0.5f; //攻击动画时长
    private static readonly float WaitDuration = 2f; //攻击间隔

    private float _attackTimer, _waitTimer;

    private bool attackPlayer = false;

    public override void OnEnter(Enemy enemy)
    {
        _attackTimer = _waitTimer = 0;

        attackPlayer = enemy.targetPoint.CompareTag("Player");

        enemy.myAnimator.SetTrigger(attackPlayer ? attackTrigger : skillTrigger);
    }

    public override void OnUpdate(Enemy enemy)
    {
        _attackTimer += Time.deltaTime;
        if (_attackTimer < AttackDuration) return;

        //TODO: 进行伤害判定

        _waitTimer += Time.deltaTime;
        if (_waitTimer < WaitDuration) return;

        //先找引线点燃的炸弹
        var attackTarget= enemy.attackList.FirstOrDefault(transform => transform.CompareTag("Bomb") && transform.GetComponent<Bomb>().Triggered);
        //再找敌人
        if (attackTarget == null)
            attackTarget = enemy.attackList.FirstOrDefault(transform => transform.CompareTag("Player"));

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