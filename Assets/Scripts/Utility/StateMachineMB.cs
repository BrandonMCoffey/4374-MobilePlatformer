using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineMB : MonoBehaviour
{
	public State CurrentState { get; private set; }
	private State _previousState;
	
	private bool _inTransition = false;
	private State _incomingState;
	
	protected virtual void Update()
	{
		if (CurrentState != null && !_inTransition)
			CurrentState.Tick();
	}
	
	protected virtual void FixedUpdate()
	{
		if (CurrentState != null && !_inTransition)
			CurrentState.FixedTick();
	}
	
	protected virtual void OnDestroy()
	{
		CurrentState?.Exit();
	}
	
	public void ChangeState(State newState)
	{
		if (CurrentState == newState) return;
		if (_inTransition)
		{
			_incomingState = newState;
			return;
		}
		
		ChangeStateSequence(newState);
		
		if (_incomingState != null)
		{
			var state = _incomingState;
			_incomingState = null;
			ChangeState(state);
		}
	}
	
	public void ChangeStateToPrevious()
	{
		if (_previousState != null) ChangeState(_previousState);
		else Debug.LogWarning("No previous state to change to", gameObject);
	}
	
	private void ChangeStateSequence(State newState)
	{
		_inTransition = true;
		
		CurrentState?.Exit();
		StoreStateAsPrevious(CurrentState, newState);
		
		CurrentState = newState;
		
		CurrentState?.Enter();
		_inTransition = false;
	}
	
	private void StoreStateAsPrevious(State currentState, State newState)
	{
		// If there is no previous state, this is our first state
		if (_previousState == null && newState != null)
			_previousState = newState;
		// Otherwise, store our current state as the previous state
		else if (_previousState != null && CurrentState != null)
			_previousState = CurrentState;
	}
}
