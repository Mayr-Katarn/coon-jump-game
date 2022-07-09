using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : UiButton
{
    public override void OnClick()
    {
        base.OnClick();
        EventManager.SendTogglePauseMenu(true);
    }
}
