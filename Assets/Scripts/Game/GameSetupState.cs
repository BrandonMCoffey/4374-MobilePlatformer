using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupState : State
{
	private GameFSM _stateMachine;
	private GameController _controller;
	private CanvasController _canvas;
	
	private GameObject _loadingScreen;
	
	public GameSetupState(GameFSM stateMachine)
	{
		_stateMachine = stateMachine;
		_controller = stateMachine.Controller;
		_canvas = stateMachine.Canvas;
	}
	
	protected override void OnEnter()
	{
		_canvas.SetLoadingScreenActive(true);
		Time.timeScale = 1;
	}
	protected override void OnTick()
	{
		_stateMachine.ChangeState(_stateMachine.PlayState);
	}
	
	protected override void OnFixedTick() { }
	protected override void OnExit()
	{
		_canvas.SetLoadingScreenActive(false);
	}
}
