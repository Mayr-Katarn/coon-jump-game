using UnityEngine.Events;

public class EventManager
{
    #region FIELDS
    public static UnityEvent<float> OnUpdateLevelHeight = new();
    public static UnityEvent<float> OnUpdateScore = new();
    public static UnityEvent<bool> OnTogglePauseMenu = new();
    public static UnityEvent OnGameOver = new();
    #endregion

    #region METHODS
    public static void SendUpdateLevelHeight(float y) => OnUpdateLevelHeight.Invoke(y);
    public static void SendUpdateScore(float score) => OnUpdateScore.Invoke(score);
    public static void SendTogglePauseMenu(bool isPause) => OnTogglePauseMenu.Invoke(isPause);
    public static void SendGameOver() => OnGameOver.Invoke();
    #endregion
}
