using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFSM : StateMachineMB
{
	[SerializeField] private GameController _controller;
	[SerializeField] private CanvasController _canvas;
	
	public GameController Controller => _controller;
	public CanvasController Canvas => _canvas;
	
	public GameSetupState SetupState { get; private set; }
	public GamePlayState PlayState { get; private set; }
	public GameWinState WinState { get; private set; }
	public GameLoseState LoseState { get; private set; }
	
	public bool GameOver => CurrentState == WinState || CurrentState == LoseState;
	
	private void OnValidate()
	{
		NullCheck();
	}
	
	private void Awake()
	{
		NullCheck();
		SetupState = new GameSetupState(this);
		PlayState = new GamePlayState(this);
		WinState = new GameWinState(this);
		LoseState = new GameLoseState(this);
	}
	
	private void Start()
	{
		ChangeState(SetupState);
	}
	
	private void NullCheck()
	{
		if (!_controller) _controller = FindObjectOfType<GameController>();
		if (!_canvas) _canvas = FindObjectOfType<CanvasController>();
	}
}