using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    [SerializeField] private int _IdleLength = 3;
    private float _timer = 3;

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();

        if (_timer <= 0)
        {
            _timer = _IdleLength;
            base.ChangeState(States.Wander);
        }
        else
            _timer -= Time.deltaTime;
    }
}
