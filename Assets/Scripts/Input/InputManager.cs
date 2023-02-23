using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	[SerializeField] private float _tapLengthForTap = 0.1f;
	
	private bool _startTap;
	private Vector2 _initialPosition;
	private Vector2 _lastPosition;
	
	public static Vector2 MoveDir { get; private set; } = Vector2.zero;
	
	private void OnTap(InputValue value)
	{
		bool pressed = value.isPressed;
		if (pressed)
		{
			_initialPosition = _lastPosition;
			_startTap = true;
		}
	}
	
	private void OnDrag(InputValue value)
	{
		var pos = value.Get<Vector2>();
		if (_startTap)
		{
			_initialPosition = pos;
			_startTap = false;
		}
		_lastPosition = pos;
	}
	
	private void OnJoystickDrag(InputValue value)
	{
	}
}
