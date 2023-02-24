using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformerCollider))]
public class PlatformerRigidbody : MonoBehaviour
{
    [SerializeField] private float _gravityMult = 1;
	[SerializeField] private float _startDelay = 0.1f;
	[SerializeField, Range(0, 1)] private float _groundFriction = 0.25f;
    [Tooltip("Increased accuracy, reduced performance.")]
    [SerializeField] private int _freeColliderIterations = 10;
	[SerializeField, ReadOnly] private Vector2 _velocity;
	[SerializeField, ReadOnly] private bool _climbing;
	[SerializeField, ReadOnly] private float _climb;
    
    private PlatformerCollider _collider;
    private bool _disabled;
    
    public bool Grounded => _collider.Grounded;
	public bool CanJump => _collider.Grounded || _collider.OnWall;
	public bool OnLeftWall => _collider.LeftWall;
	public bool OnRightWall =>  _collider.RightWall;
    public Vector2 Gravity => _gravityMult * Physics2D.gravity;
    public bool FacingRight { get; private set; }

    private bool Disabled => _disabled;
    
    private void Awake()
    {
        _collider = GetComponent<PlatformerCollider>();
    }

    private IEnumerator Start()
    {
        _disabled = true;
        yield return new WaitForSeconds(_startDelay);
        _disabled = false;
    }

    public void SetVelocityX(float value) => _velocity.x = value;
    public void SetVelocityY(float value) => _velocity.y = value;
	public void SetClimbing(bool climbing) => _climbing = climbing;
	public void SetClimb(float value) => _climb = value;

    public void UpdatePosition(float delta)
    {
        if (Disabled) return;
        
        _velocity += Gravity * delta;

	    if (Grounded)
	    {
	    	_velocity.x *= (1 - _groundFriction);
		    if (_velocity.y < 0) _velocity.y = 0;
	    }
	    
	    if (_collider.OnWall && _climbing) _velocity.y = 0;
        
        var pos = (Vector2)transform.position;
        var furthestPoint = pos + _velocity * delta;
        
	    var hit = _collider.CheckOverlap(furthestPoint);
	    bool validMovement = false;
        if (hit)
        {
        	for (int i = 1; i < _freeColliderIterations; i++)
        	{
	        	var t = 1f - (float)i / _freeColliderIterations;
	        	var posToTry = Vector2.Lerp(pos, furthestPoint, t);

	        	hit = _collider.CheckOverlap(posToTry);
	        	if (hit) continue;
            
	        	transform.position = posToTry;
	        	validMovement = true;
	        	break;
        	}
        }
        else
        {
	        transform.position = furthestPoint;
	        validMovement = true;
        }
	    if (_collider.OnWall && _climbing)
	    {
	    	if (_climb > 0)
	    	{
		    	transform.position += Vector3.up * delta;
	    	}
	    	else if (_climb < 0)
	    	{
	    		if (!Grounded)
	    		{
		    		transform.position += Vector3.down * delta;
	    		}
	    	}
	    }
	    else if (!validMovement && !Grounded)
	    {
	    	transform.position += Vector3.down * delta;
	    }
    }

    public void UpdateFacing()
    {
        var x = _velocity.x;
        if (x == 0) return;
        FacingRight = _velocity.x > 0;
    }
}
