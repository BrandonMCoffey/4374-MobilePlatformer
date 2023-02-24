using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
	[SerializeField] private float _camFollowSpeed = 5;
	[SerializeField] private Vector2 _offset = Vector2.zero;
	
	public static Transform Player;
	
	private void LateUpdate()
	{
		if (!Player) return;
		var pos = transform.position;
		var goal = Player.transform.position + (Vector3)_offset;
		goal.z = pos.z;
		transform.position = Vector3.Slerp(pos, goal, _camFollowSpeed * Time.deltaTime);
	}
}
