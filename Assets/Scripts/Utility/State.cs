using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
	public float StateDuration { get; private set; } = 0;
	
	public void Enter()
	{
		StateDuration = 0;
		OnEnter();
	}
	protected abstract void OnEnter();
	
	public void Tick()
	{
		StateDuration += Time.deltaTime;
		OnTick();
	}
	protected abstract void OnTick();
	
	public void FixedTick()
	{
		OnFixedTick();
	}
	protected abstract void OnFixedTick();
	
	public void Exit()
	{
		StateDuration = 0;
		OnExit();
	}
	protected abstract void OnExit();
}
