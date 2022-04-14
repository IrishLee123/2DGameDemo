using System.Linq;
using FSM;
using UnityEngine;

public abstract class EnemyBaseState : BaseState<Enemy>
{
    /// <summary>
    /// 根据配置的优先级寻找攻击目标
    /// </summary>
    /// <param name="enemy"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected Transform FindTarget<T>(T enemy) where T : Enemy
    {
        Transform attackTarget; 
        if (enemy.atkPriority == EnemyAtkPriority.PlayerFirst)
            attackTarget = FindPlayerFirst(enemy);
        else if (enemy.atkPriority == EnemyAtkPriority.BombFirst)
            attackTarget = FindBombFirst(enemy);
        else
            attackTarget = FindTargetByDistance(enemy);
        
        return attackTarget;
    }

    protected Transform FindPlayerFirst<T>(T enemy) where T : Enemy
    {
        //先找引线点燃的炸弹
        var attackTarget = enemy.attackList.FirstOrDefault(transform =>
            transform.CompareTag("Bomb") && transform.GetComponent<Bomb>().Triggered);

        //没有炸弹的时候再找敌人
        if (attackTarget == null)
            attackTarget = enemy.attackList.FirstOrDefault(transform => transform.CompareTag("Player"));

        return attackTarget; //返回结果
    }

    protected Transform FindBombFirst<T>(T enemy) where T : Enemy
    {
        //先找敌人
        var attackTarget = enemy.attackList.FirstOrDefault(transform => transform.CompareTag("Player"));

        //没有敌人的时候再找引爆中的炸弹
        if (attackTarget == null)
            attackTarget = enemy.attackList.FirstOrDefault(transform =>
                transform.CompareTag("Bomb") && transform.GetComponent<Bomb>().Triggered);

        return attackTarget; //返回结果
    }

    protected Transform FindTargetByDistance<T>(T enemy) where T : Enemy
    {
        Transform attackTarget = null;
        float minDistance = 9999;
        foreach (var transform in enemy.attackList) //循环攻击列表中的目标，找到最近的目标
        {
            var dir = transform.position - enemy.transform.position;

            if (dir.magnitude > minDistance) continue;

            //更新最近目标点
            attackTarget = transform;
            minDistance = dir.magnitude;
        }

        return attackTarget;
    }
}