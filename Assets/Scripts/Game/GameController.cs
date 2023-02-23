using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField] private GameObject _playerUnitPrefab;
	[SerializeField] private Transform _playerUnitSpawnLocation;
	
	public GameObject PlayerUnitPrefab => _playerUnitPrefab;
	public Transform PlayerUnitSpawnLocation => _playerUnitSpawnLocation;
}
