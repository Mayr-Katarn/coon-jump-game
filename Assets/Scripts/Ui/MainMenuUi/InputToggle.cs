using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputToggle : MonoBehaviour
{
    [SerializeField] private InputType _inputType;

    private Toggle _toggle;

    private void Start()
    {
        _toggle = GetComponent<Toggle>();
        OnValueChanged();
    }

    public void OnValueChanged()
    {
        if (_toggle.isOn)
        {
            GameConfig.InputType = _inputType;
        }
    }
}
