/// <summary>
/// 巡逻状态
/// </summary>
public class PatrolState : EnemyBaseState
{
    public override void OnEnter(Enemy enemy)
    {
        enemy.ResetTargetPoint();
    }

    public override void OnUpdate(Enemy enemy)
    {
        enemy.MoveToTarget();
    }

    public override void OnExit(Enemy enemy)
    {
    }
}