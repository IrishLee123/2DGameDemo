using System;
using System.Collections.Generic;
using Script.Utils;
using UnityEngine;
using UnityEngine.Serialization;

public enum EnemyState
{
    PatrolState,
    ChasingState,
    AttackState
}

public class Enemy : MonoBehaviour
{
    private static readonly int velocity_h = Animator.StringToHash("velocity_h");

    [HideInInspector] public Animator myAnimator;

    [Header("Movement")] public float speed;
    public List<Transform> pointList;
    protected int CurrentPoint;

    public Transform targetPoint;
    public List<Transform> attackList = new List<Transform>();

    [Header("Environment Check")] public Trigger2DCheck frontCheck;

    private EnemyBaseState currentState;
    private Dictionary<EnemyState, EnemyBaseState> stateMap = new Dictionary<EnemyState, EnemyBaseState>();

    private Rigidbody2D _rigidbody2D;

    private Vector2 lastPosition;

    private void Awake()
    {
        stateMap.Add(EnemyState.PatrolState, new PatrolState());
        stateMap.Add(EnemyState.ChasingState, new ChasingState());
        stateMap.Add(EnemyState.AttackState, new AttackState());

        Init();
    }

    protected virtual void Init()
    {
        myAnimator = GetComponentInChildren<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
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

        TransitionToState(EnemyState.PatrolState);

        lastPosition = transform.position;
    }

    private void Update()
    {
        currentState?.OnUpdate(this);

        var position = transform.position;
        var delat = position.x - lastPosition.x;
        myAnimator.SetFloat(velocity_h, Math.Abs(delat/Time.deltaTime) ); //设置动画状态机参数
        lastPosition = position;
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
        CurrentPoint = 0;
        targetPoint = pointList[CurrentPoint];
        
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
        CurrentPoint++;
        if (CurrentPoint > pointList.Count - 1)
            CurrentPoint = 0;

        targetPoint = pointList[CurrentPoint];

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

    protected virtual void AttackAction()
    {
    }

    protected virtual void SkillAction()
    {
    }

    protected virtual void FlipDirection()
    {
        var scale = transform.localScale;
        if (targetPoint.position.x < transform.position.x)
            scale.x = -1;
        else
            scale.x = 1;
        transform.localScale = scale;
    }
}