using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScore : MonoBehaviour
{
    #region FIELDS
    private TMP_Text _scoreText;
    #endregion

    #region METHODS
    private void Start()
    {
        _scoreText = GetComponent<TMP_Text>();
        UpdateScore();
    }

    private void Update()
    {
        if (!GameConfig.IsGameOver) UpdateScore();
    }

    private void UpdateScore()
    {
        _scoreText.text = GameConfig.Score.ToString();
    }
    #endregion
}
