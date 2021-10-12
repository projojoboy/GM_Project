using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public bool attack = false;
    public bool animationEnded = true;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Attack();   
    }

    private void FixedUpdate()
    {
        attack = false;
    }

    private void Attack()
    {
        attack = true;
    }

    public void EndAttackAnim() { animationEnded = true; }
    public void StartAttackAnim() { animationEnded = false; }
}
