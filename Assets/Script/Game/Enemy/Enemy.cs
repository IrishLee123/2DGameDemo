using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Script.Utils;
using UnityEngine;

public enum EnemyState
{
    PatrolState,
    ChasingState,
    AttackState
}

public enum EnemyAtkPriority
{
    PlayerFirst,
    BombFirst,
    DistanceFirst
}

public class Enemy : MonoBehaviour, IHurtable
{
    [Header("Setting")] [Tooltip("敌人血量")] public float hp;
    [Tooltip("移动速度")] public float speed;
    [Tooltip("巡逻目标点")] public List<Transform> pointList;
    [Tooltip("攻击方式")] public EnemyAtkPriority atkPriority;
    [Tooltip("巡逻时在目标点停留的时长")] public float arriveWaitDuration;
    [Tooltip("追击时丢失目标后等待的时长")] public float lossTargetWaitDuration;
    [Tooltip("攻击动作持续的时长")] public float atkDuration;
    [Tooltip("两次攻击间隔时长")] public float atkWaitDuration;
    [Tooltip("攻击距离")] public float atkRange;

    public Transform targetPoint;
    public List<Transform> attackList = new List<Transform>();

    [Header("Environment Check")] public Trigger2DCheck frontCheck;

    protected Dictionary<EnemyState, EnemyBaseState> stateMap = new Dictionary<EnemyState, EnemyBaseState>();
    protected EnemyBaseState currentState;

    protected int currentPointIdx;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        frontCheck.OnTriggerChanged += (list =>
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (i >= attackList.Count)
                {
                    attackList.Add(list[i].transform);
                    continue;
                }

                if (attackList[i] == list[i].gameObject.transform)
                    continue;

                attackList[i] = list[i].transform;
            }

            // 删除多余元素
            if (attackList.Count > list.Count)
            {
                attackList.RemoveRange(list.Count, attackList.Count - list.Count);
            }
        });
    }

    private void Start()
    {
        TransitionToState(EnemyState.PatrolState);
    }

    private void Update()
    {
        currentState?.OnUpdate(this);

        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
    }

    public bool TransitionToState(EnemyState state)
    {
        if (!stateMap.ContainsKey(state))
            return false;

        currentState?.OnExit(this);

        currentState = stateMap[state];
        currentState.OnEnter(this);

        return true;
    }

    public void ResetTargetPoint()
    {
        currentPointIdx = 0;
        targetPoint = pointList[currentPointIdx];

        // 更新朝向
        FlipDirection();
    }

    public bool ArriveTargetPoint(float range = 0.01f)
    {
        var position = transform.position;
        var targetPos = new Vector2(targetPoint.position.x, position.y);
        return Mathf.Abs(targetPos.x - transform.transform.position.x) < range;
    }

    public void SetNextPoint()
    {
        // 更新目标点
        currentPointIdx++;
        if (currentPointIdx > pointList.Count - 1)
            currentPointIdx = 0;

        targetPoint = pointList[currentPointIdx];

        // 更新朝向
        FlipDirection();
    }

    public void MoveToTarget()
    {
        var position = transform.position;
        var targetPos = new Vector2(targetPoint.position.x, position.y);
        position = Vector2.MoveTowards(position, targetPos, speed * Time.deltaTime);
        transform.position = position;
    }

    public virtual void AttackAction()
    {
    }

    public virtual void SkillAction()
    {
    }

    public void FlipDirection()
    {
        var scale = transform.localScale;
        if (targetPoint.position.x < transform.position.x)
            scale.x = -1;
        else
            scale.x = 1;
        transform.localScale = scale;
    }

    public virtual void BeenHurt(float damage)
    {
    }

    public virtual void OnFindTarget()
    {
    }
}