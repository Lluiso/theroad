using UnityEngine;
using TMPro;

public class UI_Message : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void Set(string text, bool isRighAligned)
    {
        _text.SetText(text);
        //_text.alignment = isRighAligned ? TextAlignmentOptions.TopRight : TextAlignmentOptions.TopLeft;
    }
}
