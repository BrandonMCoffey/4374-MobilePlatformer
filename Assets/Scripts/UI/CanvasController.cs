using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
	[SerializeField] private Canvas _loadingScreen;
	[SerializeField] private Canvas _playerHud;
	[SerializeField] private Canvas _winScreen;
	[SerializeField] private Canvas _loseScreen;
	
	private void Awake()
	{
		SetLoadingScreenActive(false);
		SetPlayerHudActive(false);
		SetWinScreenActive(false);
		SetLoseScreenActive(false);
	}
	
	public void SetLoadingScreenActive(bool active)
	{
		if (_loadingScreen) _loadingScreen.enabled = active;
	}
	public void SetPlayerHudActive(bool active)
	{
		if (_playerHud) _playerHud.enabled = active;
	}
	public void SetWinScreenActive(bool active)
	{
		if (_winScreen) _winScreen.enabled = active;
	}
	public void SetLoseScreenActive(bool active)
	{
		if (_loseScreen) _loseScreen.enabled = active;
	}
	
	public void RetryLevel() => GameController.RetryLevel();
	public void QuitToMainMenu() => GameController.ReturnToMainMenu();
}
