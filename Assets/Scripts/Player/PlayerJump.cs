using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float _jumpHeight = 2;

    [SerializeField] private bool _secondaryJumps;
    [SerializeField, ShowIf("_secondaryJumps")] private int _airJumps = 0;
    [SerializeField, ShowIf("_secondaryJumps")] private float _minJumpDelay = 0.2f;

    private float _lastJumpTime;
    private int _remainingAirJumps;
    private PlatformerRigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<PlatformerRigidbody>();
    }

    private void Start()
    {
        _remainingAirJumps = _airJumps;
    }

    public void ProcessJump()
    {
        if (_secondaryJumps)
        {
            if (_remainingAirJumps != _airJumps && _rb.Grounded)
            {
                _remainingAirJumps = _airJumps;
            }
            if (Time.time - _lastJumpTime < _minJumpDelay) return;
        }
	    if (InputManager.JumpThisFrame)
        {
            if (_rb.Grounded)
            {
                Jump();
            }
            else if (_secondaryJumps && _remainingAirJumps > 0)
            {
                Jump();
                _remainingAirJumps--;
            }
        }
	    if (_rb.Grounded && InputManager.DashThisFrame && InputManager.DashDir.normalized.y > 0.8f)
	    {
	    	Jump();
	    	InputManager.DashThisFrame = false;
	    }
    }
    
    private void Jump()
    {
        _rb.SetVelocityY(Mathf.Sqrt(-2f * _rb.Gravity.y * _jumpHeight));
        if (_secondaryJumps) _lastJumpTime = Time.time;
    }
}
