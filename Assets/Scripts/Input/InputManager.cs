using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	[SerializeField] private float _tapLengthForJump = 0.1f;
	[SerializeField] private float _tapLengthForWalk = 0.25f;
	[SerializeField] private int _minDistForDash = 300;
	[SerializeField] private float _walkDirFade = 1;
	
	[SerializeField, ReadOnly] private bool _walking;
	[SerializeField, ReadOnly] private bool _dashing;
	[SerializeField, ReadOnly] private bool _startTap;
	[SerializeField, ReadOnly] private float _timeSinceTap;
	[SerializeField, ReadOnly] private Vector2 _initialPosition;
	[SerializeField, ReadOnly] private Vector2 _lastPosition;
	
	public static bool Holding { get; private set; }
	public static bool JumpThisFrame { get; private set; }
	public static bool DashThisFrame { get; set; }
	public static Vector2 MoveDir { get; private set; } = Vector2.zero;
	public static Vector2 DashDir { get; private set; } = Vector2.zero;
	
	private void OnTap(InputValue value)
	{
		bool pressed = value.isPressed;
		if (pressed)
		{
			// Start Tap
			_initialPosition = _lastPosition;
			_startTap = true;
		}
		else
		{
			// Release Tap
			if (!_dashing && _timeSinceTap < _tapLengthForJump)
			{
				// Jump
				Debug.Log("Jump");
				JumpThisFrame = true;
			}
			_walking = false;
		}
		Holding = pressed;
		_dashing = false;
		_timeSinceTap = 0;
	}
	
	private void OnDrag(InputValue value)
	{
		var pos = value.Get<Vector2>();
		if (_startTap)
		{
			_initialPosition = pos;
			_startTap = false;
		}
		else if (Holding)
		{
			if (_timeSinceTap < _tapLengthForWalk && !_dashing)
			{
				// Check for Dash
				var dist = Vector2.Distance(_initialPosition, pos);
				if (dist > _minDistForDash)
				{
					// Dash (Or jump is dash up and grounded)
					_dashing = true;
					DashDir = (pos - _initialPosition).normalized;
					_initialPosition = pos;
					DashThisFrame = true;
					Debug.Log("Dash");
				}
			}
			else
			{
				// Update WalkToPosition
				MoveDir = (pos - _initialPosition).normalized;
				_walking = true;
			}
		}
		_lastPosition = pos;
	}
	
	private void OnJoystickDrag(InputValue value)
	{
	}
	
	private void Update()
	{
		if (Holding)
		{
			_timeSinceTap += Time.deltaTime;
		}
	}
	
	private void LateUpdate()
	{
		JumpThisFrame = false;
		DashThisFrame = false;
		if (_walking)
		{
			_initialPosition = Vector2.Lerp(_initialPosition, _lastPosition, _walkDirFade * Time.deltaTime);
			MoveDir = (_lastPosition - _initialPosition) * 0.1f;
			if (MoveDir.magnitude > 1) MoveDir = MoveDir.normalized;
		}
		else MoveDir = Vector2.zero;
	}
	
	private void OnWin() => GameController.WinGame();
	private void OnDeath() => GameController.LoseGame();
}
