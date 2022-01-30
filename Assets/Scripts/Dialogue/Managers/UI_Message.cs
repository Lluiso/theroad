using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UI_Message : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Animation _animation;

    [SerializeField] private RectTransform _rt;
    [SerializeField] private RectTransform _textRT;
    [SerializeField] private RectTransform _targetRT;
    [SerializeField] private float _offset = 50f;

    public void Set(string text, bool isRightAligned)
    {
        _text.SetText(text);
        //_text.alignment = isRighAligned ? TextAlignmentOptions.TopRight : TextAlignmentOptions.TopLeft;
        _animation[_animation.clip.name].normalizedTime = 0f;
        _animation[_animation.clip.name].normalizedSpeed = 0f;

        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return null;
        var targetRT = _textRT.sizeDelta;
        targetRT.y += (-_targetRT.offsetMin.y + -_targetRT.offsetMax.y) + _offset;
        Debug.Log(_rt.sizeDelta + " -> " + targetRT);

        _rt.sizeDelta = targetRT;
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());

        _animation[_animation.clip.name].normalizedSpeed = 1f;
        _animation.Play();
    }
}
