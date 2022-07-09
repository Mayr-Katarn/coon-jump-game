using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : UiButton
{
    public override void OnClick()
    {
        base.OnClick();
        EventManager.SendTogglePauseMenu(false);
    }
}
