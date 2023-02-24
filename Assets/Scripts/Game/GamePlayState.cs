using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayState : State
{
	private GameFSM _stateMachine;
	private GameController _controller;
	private CanvasController _canvas;
	
	private GameObject _player;
	
	public GamePlayState(GameFSM stateMachine)
	{
		_stateMachine = stateMachine;
		_controller = stateMachine.Controller;
		_canvas = stateMachine.Canvas;
	}
	
	protected override void OnEnter()
	{
		if (_player == null)
		{
			_player = GameObject.Instantiate(_controller.PlayerPrefab, _controller.PlayerSpawnLocation);
		}
		else
		{
			_player.transform.localPosition = Vector3.zero;
		}
		_player.SetActive(true);
		MainCamera.Player = _player.transform;
		_canvas.SetPlayerHudActive(true);
	}
	protected override void OnTick() { }
	protected override void OnFixedTick() { }
	protected override void OnExit()
	{
		if (_player) _player.SetActive(false);
		_canvas.SetPlayerHudActive(false);
	}
}
