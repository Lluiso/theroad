using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BlackBarsController : MonoBehaviour
{


    [SerializeField]
    private RectTransform topBar;

    [SerializeField]
    private RectTransform bottomBar;

    public float percentageScreenHidden = 0.2f;

    public float animationDuration = 1.5f;

    private float height;

    // Start is called before the first frame update
    void Start()
    {
        height = Screen.height * percentageScreenHidden;

        topBar.sizeDelta = new Vector2(0, height);
        bottomBar.sizeDelta = new Vector2(0, height);

        topBar.DOLocalMoveY(height, 0f).SetRelative();
        bottomBar.DOLocalMoveY(-height, 0f).SetRelative();


        GetComponent<CanvasGroup>().alpha = 1f;

        StartCoroutine(waitAndShowBars());
    }


    IEnumerator waitAndShowBars()
    {
        yield return new WaitForSecondsRealtime(1f);
        showBars();
    }

    public void showBars()
    {
        topBar.DOLocalMoveY(-height, animationDuration).SetRelative();
        bottomBar.DOLocalMoveY(height, animationDuration).SetRelative();
    }

    public void hideBars()
    {
        topBar.DOLocalMoveY(height, animationDuration).SetRelative();
        bottomBar.DOLocalMoveY(-height, animationDuration).SetRelative();
    }


}
