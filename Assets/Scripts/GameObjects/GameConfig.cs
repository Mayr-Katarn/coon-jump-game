using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    #region FIELDS
    public static GameConfig instance;
    public static bool isGamePaused = false;
    private static bool _isGameOver = false;
    private static int _lastScore = 0;
    public static int bestScore = 0;
    private static InputType _inputType = InputType.Arrows;
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

    public static InputType InputType
    {
        get => _inputType;
        set => _inputType = value;
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
        isGamePaused = true;
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
    }
    public static void StartGame()
    {
        IsGameOver = false;
        Score = 0;
        ResumeGame();
    }
    #endregion    
}
