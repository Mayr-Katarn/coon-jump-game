using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    #region FIELDS
    public static GameConfig instance;
    private static bool _isGameOver = false;
    private static int _lastScore = 0;
    public static int bestScore = 0;
    public static bool IsGameOver
    {
        get => _isGameOver;
        set
        {
            _isGameOver = value;
            if (_isGameOver) EventManager.SendGameOver();
        }
    }

    public static int Score
    {
        get => _lastScore;
        set
        {
            _lastScore = value;
            if(_lastScore > bestScore) bestScore = _lastScore;
        }
    }
    #endregion

    #region METHODS
    private void Start()
    {
        instance = this;
    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1f;
    }
    public static void StartGame()
    {
        IsGameOver = false;
        Score = 0;
        Time.timeScale = 1f;
    }
    #endregion    
}
