using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
	[SerializeField] private bool _debug;
	[SerializeField] private float _tapLengthForJump = 0.1f;
	[SerializeField] private int _minDistForDash = 300;
	[SerializeField] private float _walkDirFade = 1;
	[SerializeField] private RectTransform _targetVisual;
	[SerializeField] private LineRenderer _dashLine;
	[SerializeField] private float _dashLineAlphaTime = 1;
	[SerializeField] private AnimationCurve _dashLineAlphaCurve;
	[SerializeField] private RectTransform _jumpVisual;
	[SerializeField] private Image _jumpVisualImage;
	[SerializeField] private float _jumpVisualTime = 1;
	[SerializeField] private AnimationCurve _jumpVisualAlphaCurve;
	[SerializeField] private AnimationCurve _jumpVisualScaleCurve;
	
	[SerializeField, ReadOnly] private bool _walking;
	[SerializeField, ReadOnly] private bool _dashing;
	[SerializeField, ReadOnly] private bool _startTap;
	[SerializeField, ReadOnly] private float _timeSinceTap;
	[SerializeField, ReadOnly] private Vector2 _initialPosition;
	[SerializeField, ReadOnly] private Vector2 _lastPosition;
	[SerializeField, ReadOnly] private float _dashLineTimer;
	
	public static bool Holding { get; private set; }
	public static bool StartJumpThisFrame { get; private set; }
	public static bool JumpThisFrame { get; private set; }
	public static bool DashThisFrame { get; set; }
	public static Vector2 MoveDir { get; private set; } = Vector2.zero;
	public static Vector2 DashDir { get; private set; } = Vector2.zero;
	
	private Coroutine _jumpCircleRoutine;
	
	private void Start()
	{
		_initialPosition = -Vector3.one;
		_jumpVisual.localScale = Vector3.zero;
	}
	
	private void OnTap(InputValue value)
	{
		bool pressed = value.isPressed;
		if (pressed)
		{
			// Start Tap
			_initialPosition = _lastPosition;
			_startTap = true;
			StartJumpThisFrame = true;
		}
		else
		{
			// Release Tap
			if (!_dashing && _timeSinceTap < _tapLengthForJump)
			{
				// Jump
				Log("Jump");
				JumpThisFrame = true;
				PlayJumpJircle(_initialPosition);
			}
			_initialPosition = -Vector3.one;
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
			if (_timeSinceTap < _tapLengthForJump && !_dashing)
			{
				// Check for Dash
				var dist = Vector2.Distance(_initialPosition, pos);
				if (dist > _minDistForDash)
				{
					// Dash (Or jump is dash up and grounded)
					_dashing = true;
					DashDir = (pos - _initialPosition).normalized;
					DrawDashLine(_initialPosition, pos);
					_initialPosition = pos;
					DashThisFrame = true;
					Log("Dash");
				}
			}
			// Update WalkToPosition
			MoveDir = (pos - _initialPosition).normalized;
			_walking = true;
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
		if (_dashLineTimer < _dashLineAlphaTime)
		{
			_dashLineTimer += Time.deltaTime;
			SetLineAlpha(1 - _dashLineTimer / _dashLineAlphaTime);
		}
	}
	
	private void DrawDashLine(Vector2 start, Vector2 end)
	{
		var p = Camera.main.ScreenToWorldPoint(start);
		p.z = -1;
		_dashLine.SetPosition(0, p);
		p = Camera.main.ScreenToWorldPoint(end);
		p.z = -1;
		_dashLine.SetPosition(1, p);
		
		_dashLineTimer = 0;
	}
	
	private void SetLineAlpha(float alpha)
	{
		var c = _dashLine.startColor;
		c.a = alpha;
		_dashLine.startColor = c;
		c = _dashLine.endColor;
		c.a = alpha;
		_dashLine.endColor = c;
	}
	
	private void PlayJumpJircle(Vector2 pos)
	{
		if (_jumpCircleRoutine != null) StopCoroutine(_jumpCircleRoutine);
		_jumpCircleRoutine = StartCoroutine(JumpCircleRoutine(pos));
	}
	
	private IEnumerator JumpCircleRoutine(Vector2 pos)
	{
		float mult = 1f / _jumpVisualTime;
		_jumpVisual.position = pos;
		for (float t = 0; t < 1; t += Time.deltaTime * mult)
		{
			var s = _jumpVisualScaleCurve.Evaluate(t);
			_jumpVisual.localScale = new Vector3(s, s, 1);
			var c = _jumpVisualImage.color;
			c.a = _jumpVisualAlphaCurve.Evaluate(t);
			_jumpVisualImage.color = c;
			yield return null;
		}
		_jumpVisual.localScale = Vector3.zero;
		_jumpCircleRoutine = null;
	}
	
	private void LateUpdate()
	{
		JumpThisFrame = false;
		StartJumpThisFrame = false;
		DashThisFrame = false;
		if (_walking)
		{
			_initialPosition = Vector2.Lerp(_initialPosition, _lastPosition, _walkDirFade * Time.deltaTime);
			MoveDir = (_lastPosition - _initialPosition) * 0.1f;
			if (MoveDir.magnitude > 1) MoveDir = MoveDir.normalized;
		}
		else MoveDir = Vector2.zero;
		if (_targetVisual) _targetVisual.position = _initialPosition;
	}
	
	private void OnWin() => GameController.WinGame();
	private void OnDeath() => GameController.LoseGame();

	private void Log(string message)
	{
		if (_debug) Debug.Log("Input: " + message, gameObject);
	}
}
