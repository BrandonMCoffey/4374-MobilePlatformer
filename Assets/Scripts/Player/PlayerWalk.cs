using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
	[SerializeField] private float _groundWalkSpeed = 5;
	[SerializeField] private float _airWalkSpeed = 2;
    
    private PlatformerRigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<PlatformerRigidbody>();
    }
    
    public void ProcessWalk()
	{
		var speed = _rb.Grounded ? _groundWalkSpeed : _airWalkSpeed;
	    var x = InputManager.MoveDir.x * speed;
	    if (x != 0) _rb.SetVelocityX(x);
    }
}
