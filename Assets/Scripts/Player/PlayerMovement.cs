using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float _speed;
	
	private void Update()
	{
		transform.position += (Vector3)InputManager.MoveDir * _speed * Time.deltaTime;
	}
}
