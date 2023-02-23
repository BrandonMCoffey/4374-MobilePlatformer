using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private PlatformerRigidbody _rb;
    
    private void Awake()
    {
        if (transform.parent) transform.parent.GetComponent<PlatformerRigidbody>();
        if (!_rb) _rb = GetComponent<PlatformerRigidbody>();
        if (!_rb) _rb = FindObjectOfType<PlatformerRigidbody>();
    }
    
    private void LateUpdate()
    {
        transform.localScale = new Vector3(_rb.FacingRight ? 1 : -1, 1, 1);
    }
}
