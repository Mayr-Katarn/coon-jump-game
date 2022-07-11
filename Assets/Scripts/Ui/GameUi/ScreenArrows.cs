using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenArrows : MonoBehaviour
{
    #region FIELDS
    private readonly float _maxInputForce = 1;
    private readonly float _minInputForce = -1;
    public static float inputForce = 0;
    private bool _isLeftButtonDown = false;
    private bool _isRightButtonDown = false;
    #endregion

    #region METHODS
    private void Update()
    {
        InputCatcher();
    }

    private void InputCatcher()
    {
        if (_isLeftButtonDown && !_isRightButtonDown && inputForce > _minInputForce)
        {
            if (inputForce > 0) inputForce = 0;
            inputForce -= 3f * Time.deltaTime;
        }
        else if (_isRightButtonDown && !_isLeftButtonDown && inputForce < _maxInputForce)
        {
            if (inputForce < 0) inputForce = 0;
            inputForce += 3f * Time.deltaTime;
        }
        else if (!_isRightButtonDown && !_isLeftButtonDown && inputForce != 0)
        {
            inputForce = 0;
        }
    }

    public void OnLeftButtonDown()
    {
        _isLeftButtonDown = true;
    }
    public void OnLeftButtonUp()
    {
        _isLeftButtonDown = false;
    }

    public void OnRightButtonDown()
    {
        _isRightButtonDown = true;
    }
    public void OnRightButtonUp()
    {
        _isRightButtonDown = false;
    }
    #endregion
}
