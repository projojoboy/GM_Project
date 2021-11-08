using UnityEngine;

[RequireComponent(typeof(EnemyController), typeof(Rigidbody2D))]
public class State : MonoBehaviour
{
    public EnemyController ec;
    public Rigidbody2D rb;

    public bool _hasEnteredState = false;
    public bool _hasExitState = false;

    private void Awake() { ec = GetComponent<EnemyController>(); rb = GetComponent<Rigidbody2D>(); }
    public virtual void OnStateEnter() { _hasEnteredState = true; _hasExitState = false; }
    public virtual void OnStateUpdate()
    {
        if (!_hasEnteredState)
            this.OnStateEnter();
    }
    public virtual void OnStateExit() { _hasEnteredState = false; _hasExitState = true; }
    public void ChangeState(States state) { ec.ChangeState(state); }
}
