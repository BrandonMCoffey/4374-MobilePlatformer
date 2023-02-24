using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformerCollider : MonoBehaviour
{
    [Header("Character Bounds")]
    [SerializeField] private Vector2 _offset = new Vector2(0, 0);
    [SerializeField] private Vector2 _size = new Vector2(1f, 1.3f);
    [SerializeField] private LayerMask _groundLayer = 1;
    [SerializeField] private float _groundStandBuffer = 0.1f;
    [SerializeField] private float _jumpCornerBuffer = 0.18f;
    [SerializeField] private float _crouchBuffer = 0.02f;
    [SerializeField] private float _stepHeight = 0.24f;
    [SerializeField] private float _undershoot = 0.05f;
    [SerializeField] private float _overshoot = 0.05f;
    [SerializeField] private int _extraRays = 3;

    [Header("Debug")]
    [SerializeField, ReadOnly] private bool _grounded;
    [SerializeField, ReadOnly] private bool _hitHead;
    [SerializeField, ReadOnly] private bool _leftWall;
    [SerializeField, ReadOnly] private bool _rightWall;
    [SerializeField, ReadOnly] private bool _landingThisFrame;
    [SerializeField, ReadOnly] private float _timeLastGrounded;

    public bool Grounded => _grounded;
	public bool OnWall => _leftWall || _rightWall;
    private Vector3 Center => transform.position + (Vector3)_offset;

    private RaySet _raysUp, _raysLeft, _raysRight, _raysDown;

    public void CheckCollider()
    {
        CalculateRays();

        _landingThisFrame = false;
        var groundedCheck = CheckCollisions(_raysDown);
        if (_grounded && !groundedCheck)
        {
            _timeLastGrounded = Time.time;
        }
        else if (!_grounded && groundedCheck)
        {
            _landingThisFrame = true;
        }
        _grounded = groundedCheck;
        _hitHead = CheckCollisions(_raysUp);
        _leftWall = CheckCollisions(_raysLeft);
        _rightWall = CheckCollisions(_raysRight);

        bool CheckCollisions(RaySet set)
        {
            var origins = set.GetOrigins(_extraRays);
	        return origins.Any(origin => Physics2D.Raycast(origin, set.Dir, _overshoot, _groundLayer));
        }
    }

    public Collider2D CheckOverlap(Vector2 position)
    {
	    return Physics2D.OverlapBox(position + _offset, _size, 0, _groundLayer);
    }

    private void CalculateRays()
    {
        var bounds = new Bounds(Center, _size);
        // Top Rays
        _raysUp = new RaySet
        {
            X = true,
            Start = bounds.min.x + _jumpCornerBuffer,
            End = bounds.max.x - _jumpCornerBuffer,
            Other = bounds.max.y,
            Dir = Vector2.up
        };
        // Left Rays
        _raysLeft = new RaySet
        {
            X = false,
            Start = bounds.min.y + _stepHeight,
            End = bounds.max.y - _crouchBuffer,
            Other = bounds.min.x,
            Dir = Vector2.left
        };
        // Right Rays
        _raysRight = new RaySet
        {
            X = false,
            Start = bounds.min.y + _stepHeight,
            End = bounds.max.y - _crouchBuffer,
            Other = bounds.max.x,
            Dir = Vector2.right
        };
        // Bottom Rays
        _raysDown = new RaySet
        {
            X = true,
            Start = bounds.min.x + _groundStandBuffer,
            End = bounds.max.x - _groundStandBuffer,
            Other = bounds.min.y,
            Dir = Vector2.down
        };
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        float halfX = _size.x * 0.5f - 0.01f;
        float halfY = _size.y * 0.5f - 0.01f;

        _jumpCornerBuffer = Mathf.Clamp(_jumpCornerBuffer, 0, halfX);
        _groundStandBuffer = Mathf.Clamp(_groundStandBuffer, 0, halfX);
        _crouchBuffer = Mathf.Clamp(_crouchBuffer, 0, halfY);
        _stepHeight = Mathf.Clamp(_stepHeight, 0, halfY);
        _overshoot = Mathf.Clamp01(_overshoot);
        _extraRays = Mathf.Clamp(_extraRays, 0, 50);
    }

    private void OnDrawGizmos()
    {
        // Bounds
        Gizmos.color = new Color(0.49f, 0.79f, 0.47f);
        Gizmos.DrawWireCube(Center, _size);
    }

    private void OnDrawGizmosSelected()
    {
        // Rays
        Gizmos.color = Color.red;
        CalculateRays();
        DrawRays(_raysUp);
        DrawRays(_raysLeft);
        DrawRays(_raysRight);
        DrawRays(_raysDown);

        void DrawRays(RaySet set)
        {
            var origins = set.GetOrigins(_extraRays);
            foreach (var origin in origins)
            {
                Gizmos.DrawRay(origin - set.Dir * _undershoot, set.Dir * (_undershoot + _overshoot));
            }
        }
    }
#endif
}

internal struct RaySet
{
    public bool X;
    public float Start;
    public float End;
    public float Other;
    public Vector2 Dir;

    public IEnumerable<Vector2> GetOrigins(int extraRays)
    {
        var list = new List<Vector2>(2 + extraRays);
        for (float value = Start; value <= End + 0.01f; value += (End - Start) / (1 + extraRays))
        {
            list.Add(new Vector2(X ? value : Other, X ? Other : value));
        }
        return list;
    }
}