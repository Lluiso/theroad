using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class BlackBarsController : MonoBehaviour
{


    [SerializeField]
    private RectTransform topBar;

    [SerializeField]
    private RectTransform bottomBar;

    [SerializeField]
    private CanvasGroup UICanvas;

    public float percentageScreenHidden = 0.2f;

    public float animationDuration = 1.5f;

    private float height;
    private bool _barsHidden = true;

    // Start is called before the first frame update
    private void Awake()
    {
        _barsHidden = true;
        CarEvents.StartInteraction += hideBars;
        CarEvents.EndInteraction += showBars;
    }

    void Start()
    {
        height = Screen.height * percentageScreenHidden;

        topBar.sizeDelta = new Vector2(0, height);
        bottomBar.sizeDelta = new Vector2(0, height);

        topBar.DOLocalMoveY(height, 0f).SetRelative();
        bottomBar.DOLocalMoveY(-height, 0f).SetRelative();

        GetComponent<CanvasGroup>().alpha = 1f;

        //StartCoroutine(waitAndShowBars());
    }


    IEnumerator waitAndShowBars()
    {
        yield return new WaitForSecondsRealtime(1f);
        showBars();
    }

    [Button]
    public void showBars()
    {
        if (!_barsHidden)
        {
            return;
        }
        _barsHidden = false;
        Debug.Log("Show Bars");
        UICanvas.DOFade(0.5f, animationDuration * 0.5f);
        topBar.DOLocalMoveY(-height, animationDuration).SetRelative();
        bottomBar.DOLocalMoveY(height, animationDuration).SetRelative();
    }
    [Button]
    public void hideBars()
    {
        if (_barsHidden)
        {
            return;
        }
        _barsHidden = true;
        Debug.Log("Hide Bars");
        UICanvas.DOFade(1f, animationDuration * 0.5f);
        topBar.DOLocalMoveY(height, animationDuration).SetRelative();
        bottomBar.DOLocalMoveY(-height, animationDuration).SetRelative();
    }


}
