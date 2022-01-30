
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public bool isInsideCar;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<Passenger>().wasDroppedOnSlot(isInsideCar);
            eventData.pointerDrag.GetComponent<Transform>().DOMove(transform.position, 0.1f);
        }
    }

}
