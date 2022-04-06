using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using Script.Utils;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    private Rigidbody2D _rigidbody2D;

    public Trigger2DCheck GroundCheck;

    private bool _inTheAir = false;

    private void Awake()
    {
        _animator = transform.GetComponent<Animator>();
        _rigidbody2D = transform.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _animator.SetFloat("speed", Math.Abs(_rigidbody2D.velocity.x));
        _animator.SetFloat("velocityY", _rigidbody2D.velocity.y);
        _animator.SetBool("onGround", GroundCheck.Triggered);

        // 判断起飞后落地逻辑
        if (!_inTheAir)
        {
            _inTheAir = !GroundCheck.Triggered;
        }
        else
        {
            if (GroundCheck.Triggered)
            {
                ShowLandingEffect();
                _inTheAir = false;
            }
        }
    }

    public void ShowJumpEffect()
    {
        var pos = new Vector2();
        pos.x = transform.position.x;
        pos.y = transform.position.y - 0.5f;
        EffectController.Instance.ShowJumpEffect(pos);
    }

    public void ShowLandingEffect()
    {
        var pos = new Vector2();
        pos.x = transform.position.x;
        pos.y = transform.position.y - 0.7f;
        EffectController.Instance.ShowLandingEffect(pos);
    }
}