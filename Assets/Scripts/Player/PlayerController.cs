using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _acceleration = 90f;
    [SerializeField] private float _jumpHeight = 2;
    
    private PlatformerRigidbody _rb;
    private PlatformerCollider _collider;
    
    private PlayerWalk _walk;
    private PlayerJump _jump;
	private PlayerClimb _climb;
    private PlayerDash _dash;

    private void Awake()
    {
        _rb = GetComponent<PlatformerRigidbody>();
        _collider = GetComponent<PlatformerCollider>();

        _walk = GetComponent<PlayerWalk>();
	    _jump = GetComponent<PlayerJump>();
	    _climb = GetComponent<PlayerClimb>();
        _dash = GetComponent<PlayerDash>();
    }

    private void Update()
    {
        if (_collider) _collider.CheckCollider();
        if (_walk) _walk.ProcessWalk();
        if (_jump) _jump.ProcessJump();
	    if (_climb) _climb.ProcessClimb();
        if (_dash) _dash.ProcessDash();
        if (_rb)
        {
            _rb.UpdatePosition(Time.deltaTime);
            _rb.UpdateFacing();
        }
    }
}
