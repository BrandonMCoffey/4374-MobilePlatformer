using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.SoundSystem;

public class GameController : MonoBehaviour
{
	public static GameController Instance;
	
	[SerializeField] private GameFSM _stateMachine;
	[SerializeField] private GameObject _playerPrefab;
	[SerializeField] private Transform _playerSpawnLocation;
	[SerializeField] private GameObject _loadingScreen;
	[SerializeField] private MusicTrack _music;
	[SerializeField] private SfxReference _lose;
	[SerializeField] private SfxReference _win;
	
	private static int _level;
	
	public GameFSM StateMachine => _stateMachine;
	public GameObject PlayerPrefab => _playerPrefab;
	public Transform PlayerSpawnLocation => _playerSpawnLocation;
	public GameObject LoadingScreen => _loadingScreen;
	
	private void OnValidate()
	{
		NullCheck();
	}
	
	private void Awake()
	{
		NullCheck();
		Instance = this;
		_level = SceneManager.GetActiveScene().buildIndex;
	}
	
	private void Start()
	{
		_music.Play();
	}
	
	public static void WinGame()
	{
		var sm = Instance.StateMachine;
		if (sm.GameOver) return;
		sm.ChangeState(sm.WinState);
		Instance._win.Play();
	}
	
	public static void LoseGame()
	{
		var sm = Instance.StateMachine;
		if (sm.GameOver) return;
		sm.ChangeState(sm.LoseState);
		Instance._lose.Play();
	}
	
	public static void RetryLevel()
	{
		Debug.Log("Retry");
		var sm = Instance.StateMachine;
		sm.ChangeState(sm.PlayState);
	}
	
	public static void NextLevel()
	{
		SceneManager.LoadScene(_level + 1);
	}
	
	public static void ReturnToMainMenu()
	{
		SceneManager.LoadScene(0);
	}
	
	private void NullCheck()
	{
		if (!_stateMachine) _stateMachine = FindObjectOfType<GameFSM>();
	}
}
