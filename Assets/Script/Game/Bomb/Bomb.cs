using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float countdown;
    public float bombForce;
    public Vector3 centerOffset;

    [Header("Check")] public float radius;
    public LayerMask targetLayer;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;

    private float _timer;

    private bool _triggered;

    private void Awake()
    {
        _animator = transform.GetComponent<Animator>();
        _rigidbody2D = transform.GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _timer = countdown;
        _triggered = false;
    }

    private void Start()
    {
        Trigger();
    }

    private void Update()
    {
        if (!_triggered)
        {
            return;
        }
        
        if (_timer < 0)
        {
            return;
        }

        _timer -= Time.deltaTime;

        if (_timer < 0)
        {
            _animator.SetTrigger("explore");
            Expotion();
        }
    }

    public bool Triggered
    {
        get { return _triggered; }
    }

    public void Trigger()
    {
        if (_triggered)
        {
            return;
        }

        _triggered = true;

        _animator.SetBool("isOn", true);
    }

    public void UnTrigger()
    {
        if (!_triggered)
        {
            return;
        }

        _triggered = false;

        _animator.SetBool("isOn", false);
    }

    public void Expotion()
    {
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        foreach (var item in results)
        {
            // 炸弹不能自己炸自己
            if (item.gameObject == gameObject)
            {
                continue;
            }

            // 对爆炸范围内的物体施加冲量
            var rig = item.GetComponent<Rigidbody2D>();
            if (rig != null)
            {
                Vector3 dir = item.transform.position - transform.position - centerOffset;
                rig.AddForce(dir.normalized * bombForce, ForceMode2D.Impulse);

                // TODO: 计算位置信息对爆炸范围内的物体施加扭力
                // if (Math.Abs(dir.normalized.x) > 0.1 && Math.Abs(dir.normalized.y) > 0.1)
                // {
                //     double rad = Math.Atan(dir.normalized.y / dir.normalized.x);
                // }

                if (rig.transform.CompareTag("Bomb"))//炸弹爆炸可以引燃炸弹
                {
                    var bomb = rig.GetComponent<Bomb>();
                    bomb.Trigger();
                }
            }
        }

        _rigidbody2D.bodyType = RigidbodyType2D.Static;
    }

    public void OnExpFinish()
    {
        Destroy(gameObject);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + centerOffset, radius);
    }
}