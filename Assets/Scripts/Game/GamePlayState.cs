using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayState : State
{
	private GameFSM _stateMachine;
	private GameController _controller;
	
	private GameObject _player;
	
	public GamePlayState(GameFSM stateMachine, GameController controller)
	{
		_stateMachine = stateMachine;
		_controller = controller;
	}
	
	protected override void OnEnter()
	{
		if (_player == null)
		{
			_player = GameObject.Instantiate(_controller.PlayerPrefab, _controller.PlayerSpawnLocation);
		}
		MainCamera.Player = _player.transform;
	}
	protected override void OnTick() { }
	protected override void OnFixedTick() { }
	protected override void OnExit() { }
}
