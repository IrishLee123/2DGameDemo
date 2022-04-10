using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Enemy
{
    private Rigidbody2D _rigidbody2D;

    protected override void Init()
    {
        base.Init();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
}