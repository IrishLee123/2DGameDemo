using System;
using Script.Utils;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour, IHurtable
{
    public float speed;
    public float jumpForce;
    public float hp;

    private Rigidbody2D _rigidbody;

    [Header("Environment Check")] public Trigger2DCheck groundCheck;

    [Header("AttackSetting")] public GameObject bombPrefab;
    public float attackInterval;

    private PlayerAnimation _playerAnimation;

    private void Awake()
    {
        _rigidbody = transform.GetComponent<Rigidbody2D>();
        _playerAnimation = transform.GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
        }

        // float horizontalInput = Input.GetAxisRaw("Horizontal"); // -1 ~ 1
        float horizontalInput = Input.GetAxis("Horizontal"); // -1 ~ 1

        _rigidbody.velocity = new Vector2(horizontalInput * speed, _rigidbody.velocity.y);
        if (horizontalInput != 0)
        {
            var scaleX = horizontalInput < 0 ? -1 : 1;
            transform.localScale = new Vector3(scaleX, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && groundCheck.Triggered)
        {
            var velocity = _rigidbody.velocity;
            velocity = new Vector2(velocity.x, velocity.y + jumpForce);
            _rigidbody.velocity = velocity;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    private float _attackTimer;

    private void Attack()
    {
        if (_attackTimer > 0)
        {
            return;
        }

        GameObject bomb = Instantiate(bombPrefab);
        bomb.transform.position = transform.position;
        _attackTimer = attackInterval;
    }

    private void FixedUpdate()
    {
    }

    public void BeenHurt(float damage)
    {
        hp -= damage;
        _playerAnimation.Hurt();
        // Debug.Log("HP: " + hp);
    }
}