using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupState : State
{
	private GameFSM _stateMachine;
	private GameController _controller;
	
	public GameSetupState(GameFSM stateMachine, GameController controller)
	{
		_stateMachine = stateMachine;
		_controller = controller;
	}
	
	protected override void OnEnter() { }
	protected override void OnTick()
	{
		_stateMachine.ChangeState(_stateMachine.PlayState);
	}
	
	protected override void OnFixedTick() { }
	protected override void OnExit() { }
}
