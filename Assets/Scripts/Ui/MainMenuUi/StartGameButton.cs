using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : UiButton
{
    public override void OnClick()
    {
        base.OnClick();
        SceneManager.LoadScene(SceneType.Game.ToString());
    }
}
