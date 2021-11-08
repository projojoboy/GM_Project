using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : State
{
    [SerializeField] private float _wanderSpeed;
    [SerializeField] private GameObject[] _waypoints = null;

    private GameObject _targetWaypoint = null;

    private bool _isFacingRight = false;



    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();

        _targetWaypoint = GetTargetWaypoint();

        if(_isFacingRight)
            rb.velocity = Vector2.right * _wanderSpeed * Time.fixedDeltaTime;
        else
            rb.velocity = Vector2.right * -_wanderSpeed * Time.fixedDeltaTime;

        if (CheckForWall())
            Flip();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    private bool CheckForWall()
    {
        
        return false;
    }

    private GameObject GetTargetWaypoint()
    {
        if(_waypoints.Length == 0)
            return null;

        int rand = Random.Range(0, _waypoints.Length);

        Vector2 thisPos = new Vector2(transform.position.x, 0);
        Vector2 waypointPos = new Vector2(_waypoints[rand].transform.position.x, 0);

        if (Vector2.Distance(thisPos, waypointPos) <= 1.5f)
        {
            if(rand == 1) { rand = 0; }
            else { rand = 1; }
        }

        return _waypoints[rand];
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
