using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private DashMode _mode;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashHorzSpeed;
    [SerializeField] private float _dashVertSpeed;
	[SerializeField, ReadOnly] private Vector2 _dashDir;
	[SerializeField, ReadOnly]private bool _canDash;
	[SerializeField, ReadOnly]private bool _dashing;
    
    private PlatformerRigidbody _rb;
    private float _timeDashed;

	private bool CanDash => _canDash && !_dashing;
    
    private void Awake()
    {
        _rb = GetComponent<PlatformerRigidbody>();
    }

    public void ProcessDash()
	{
        if (CanDash && InputManager.DashThisFrame)
        {
	        _dashing = true;
            _timeDashed = Time.time;
            CheckDashDirection();
        }
        if (_dashing && Time.time - _timeDashed < _dashTime)
        {
            if (_dashDir.x != 0) _rb.SetVelocityX(_dashDir.x * _dashHorzSpeed);
	        if (_dashDir.y != 0) _rb.SetVelocityY(_dashDir.y * _dashVertSpeed);
        }
        else if (_dashing)
        {
	        _dashing = false;
	        _canDash = false;
	        if (_dashDir.x != 0) _rb.SetVelocityX(_dashDir.x * _dashHorzSpeed * 0.5f);
	        if (_dashDir.y != 0) _rb.SetVelocityY(_dashDir.y * _dashVertSpeed * 0.5f);

        }
		if (_rb.Grounded && !_canDash) _canDash = true;
    }

    private void CheckDashDirection()
    {
	    var dir = InputManager.DashDir;
        switch (_mode)
        {
            case DashMode.HorizontalOnly:
            {
                bool right = dir.x >= 0;
                if (dir.x == 0) right = _rb.FacingRight;
                _dashDir = new Vector2(right ? 1 : -1, 0);
                break;
            }
            case DashMode.HorizontalAndUp:
            {
                var intensityX = dir.x;
                if (dir.x < 0) intensityX = -dir.x;
                
                if (dir.y > intensityX)
                {
                    _dashDir = new Vector2(0, 1);
                }
                else
                {
                    bool right = dir.x >= 0;
                    if (dir.x == 0) right = _rb.FacingRight;
                    _dashDir = new Vector2(right ? 1 : -1, 0);
                }
                break;
            }
            case DashMode.FourDirectional:
            {
                var intensityX = dir.x;
                if (dir.x < 0) intensityX = -dir.x;
                var intensityY = dir.y;
                if (dir.y < 0) intensityY = -dir.y;
                
                if (intensityY > intensityX)
                {
                    _dashDir = new Vector2(0, dir.y > 0 ? 1 : -1);
                }
                else
                {
                    bool right = dir.x >= 0;
                    if (dir.x == 0) right = _rb.FacingRight;
                    _dashDir = new Vector2(right ? 1 : -1, 0);
                }
                break;
            }
            case DashMode.EightDirectional:
            {
	            _dashDir = new Vector2(QuantizeAxis(dir.x), QuantizeAxis(dir.y)).normalized;
                break;
            }
            case DashMode.JoystickDirectional:
            {
                _dashDir = dir.normalized;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private static float QuantizeAxis(float input)
    {
        if (input < -0.5f) return -1;
        if (input > 0.5f) return 1;
        return 0;
    }
}

internal enum DashMode
{
    HorizontalOnly,
    HorizontalAndUp,
    FourDirectional,
    EightDirectional,
    JoystickDirectional
}
