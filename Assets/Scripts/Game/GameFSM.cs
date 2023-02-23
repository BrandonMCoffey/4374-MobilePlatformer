using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFSM : StateMachineMB
{
	[SerializeField] private GameController _controller;
	
	public GameSetupState SetupState { get; private set; }
	public GamePlayState PlayState { get; private set; }
	
	private void OnValidate()
	{
		if (!_controller) _controller = FindObjectOfType<GameController>();
	}
	
	private void Awake()
	{
		if (!_controller) _controller = FindObjectOfType<GameController>();
		
		SetupState = new GameSetupState(this, _controller);
		PlayState = new GamePlayState(this, _controller);
	}
	
	private void Start()
	{
		ChangeState(SetupState);
	}
}
