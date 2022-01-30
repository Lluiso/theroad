
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Passenger : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private Canvas canvas;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameText;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector3 originalPos;

    private bool droppedOnSlot = false;
    public bool isInsideCar;
    public string _name;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        originalPos = transform.position;

        droppedOnSlot = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (!droppedOnSlot)
        {
            transform.DOMove(originalPos, 0.1f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    IEnumerator waitAndGoBack()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        if (!droppedOnSlot)
        {

        }
    }
    public void wasDroppedOnSlot(bool _isInsideCar)
    {

        Debug.Log("Dropped on slot!");
        droppedOnSlot = true;
        isInsideCar = _isInsideCar;
    }
    public void setInfo(bool _isInsideCar, string name, Sprite img, Canvas _canvas)
    {
        isInsideCar = _isInsideCar;
        nameText.text = name;
        _name = name;
        image.sprite = img;
        canvas = _canvas;
    }

}
