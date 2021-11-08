using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private States _currentState = States.Idle;

    public UnityEvent OnIdleEvent = new UnityEvent();
    public UnityEvent OnWanderEvent = new UnityEvent();
    public UnityEvent OnSearchEvent = new UnityEvent();
    public UnityEvent OnChaseEvent = new UnityEvent();
    public UnityEvent OnAttackEvent = new UnityEvent();

    // Update is called once per frame
    void Update()
    {
        switch(_currentState)
        {
            case States.Idle:
                OnIdleEvent?.Invoke();
                break;
            case States.Wander:
                OnWanderEvent?.Invoke();
                break;
            case States.Search:
                OnSearchEvent?.Invoke();
                break;
            case States.Chase:
                OnChaseEvent?.Invoke();
                break;
            case States.Attack:
                OnAttackEvent?.Invoke();
                break;
            case States.None:
                break;
        }
    }

    public void ChangeState(States stateToChangeTo) { _currentState = stateToChangeTo; Debug.Log("Changing Current State To: States." + stateToChangeTo); }
}

public enum States { Idle, Wander, Search, Chase, Attack, None };
