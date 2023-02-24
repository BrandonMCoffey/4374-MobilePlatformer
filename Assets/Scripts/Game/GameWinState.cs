using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinState : State
{
	private GameFSM _stateMachine;
	private GameController _controller;
	private CanvasController _canvas;
	
	public GameWinState(GameFSM stateMachine)
	{
		_stateMachine = stateMachine;
		_controller = stateMachine.Controller;
		_canvas = stateMachine.Canvas;
	}
	
	protected override void OnEnter()
	{
		_canvas.SetWinScreenActive(true);
	}
	protected override void OnTick() { }
	protected override void OnFixedTick() { }
	protected override void OnExit() 
	{
		_canvas.SetWinScreenActive(false);
	}
}
