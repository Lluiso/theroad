using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecisionButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _button;

    public void Set(Action callback, string text)
    {
        _button.onClick.AddListener(() => callback?.Invoke());
        _text.SetText(text);
    }
}
