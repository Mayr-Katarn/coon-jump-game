using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUiController : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private GameObject _gameUi;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameOverMenu;
    #endregion

    #region METHODS
    private void OnEnable()
    {
        EventManager.OnTogglePauseMenu.AddListener(TogglePauseMenu);
        EventManager.OnGameOver.AddListener(GameOverMenu);
    }

    private void TogglePauseMenu(bool isPause)
    {
        if (isPause) GameConfig.PauseGame();
        else GameConfig.ResumeGame();

        _gameUi.SetActive(!isPause);
        _pauseMenu.SetActive(isPause);
    }

    private void GameOverMenu()
    {
        GameConfig.PauseGame();
        _gameUi.SetActive(false);
        _gameOverMenu.SetActive(true);
    }
    #endregion
}
