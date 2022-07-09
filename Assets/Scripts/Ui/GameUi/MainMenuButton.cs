using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : UiButton
{
    public override void OnClick()
    {
        base.OnClick();
        SceneManager.LoadScene(SceneType.MainMenu.ToString());
    }
}
