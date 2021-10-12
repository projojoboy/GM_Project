using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController2D), typeof(Rigidbody2D))]
[RequireComponent(typeof(AttackController))]
public class AnimationController : MonoBehaviour
{
    private Animator _anim;
    private CharacterController2D _cc;
    private Rigidbody2D _rb;
    private AttackController _ac;

    [SerializeField] private float _velocityThreshold = .8f;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _cc = GetComponent<CharacterController2D>();
        _rb = GetComponent<Rigidbody2D>();
        _ac = GetComponent<AttackController>();
    }

    private void Update()
    {
        bool grounded = _cc.grounded;
        bool attack = _ac.attack;
        bool atkAnimDone = _ac.animationEnded;
        bool moving = (_rb.velocity.x > _velocityThreshold || _rb.velocity.x < -_velocityThreshold);
        bool jumping = (_rb.velocity.y > 0 && !grounded);
        bool falling = (_rb.velocity.y <= 0 && !grounded);

        if(moving && !_anim.GetBool("Moving") && _cc.horizontalMove != 0)
            _anim.SetBool("Moving", true);
        else if(!moving && _anim.GetBool("Moving") && _cc.horizontalMove == 0)
            _anim.SetBool("Moving", false);

        if(jumping)
            _anim.SetBool("Jumping", true);
        else if (!jumping)
            _anim.SetBool("Jumping", false);

        if (falling)
            _anim.SetBool("Falling", true);
        else if (!falling)
            _anim.SetBool("Falling", false);

        if (grounded)
            _anim.SetBool("Grounded", true);
        else if (!grounded)
            _anim.SetBool("Grounded", false);

        if (attack)
            _anim.SetBool("Attack", true);
        else if (!attack)
            _anim.SetBool("Attack", false);

        if (atkAnimDone)
            _anim.SetBool("Finished Attack Animation", true);
        else if (!atkAnimDone)
            _anim.SetBool("Finished Attack Animation", false);
    }
}
