using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshProUGUI))]
public class TextCopy : MonoBehaviour
{
    private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _textToCopy;

    void Update()
    {
        if (_text == null)
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        _text.SetText(_textToCopy.text);
        _text.alignment = _textToCopy.alignment;
    }
}
