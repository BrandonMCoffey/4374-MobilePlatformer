using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathVolume : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log(other);
		if (other.GetComponent<PlayerController>())
		{
			GameController.LoseGame();
		}
	}
}
