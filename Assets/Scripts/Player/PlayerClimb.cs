using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
	private PlatformerCollider _col;
	private PlatformerRigidbody _rb;
    
	private void Awake()
	{
		_col = GetComponent<PlatformerCollider>();
		_rb = GetComponent<PlatformerRigidbody>();
	}
	
	public void ProcessClimb()
	{
		if (_col.OnWall && InputManager.Holding)
		{
			_rb.SetClimbing(true);
			var y = InputManager.MoveDir.y;
			if (y > 5) y = 1;
			else if (y < 5) y = -1;
			else y = 0;
			_rb.SetClimb(y);
		}
		else
		{
			_rb.SetClimbing(false);
		}
	}
}
