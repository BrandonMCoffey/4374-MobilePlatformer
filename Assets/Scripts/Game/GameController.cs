using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField] private GameObject _playerPrefab;
	[SerializeField] private Transform _playerSpawnLocation;
	
	public GameObject PlayerPrefab => _playerPrefab;
	public Transform PlayerSpawnLocation => _playerSpawnLocation;
}
