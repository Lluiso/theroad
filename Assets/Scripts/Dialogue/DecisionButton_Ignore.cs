using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecisionButton_Ignore : DecisionButton
{
    [SerializeField] private Image _image;
    private void Awake()
    {
        _image.fillAmount = 0f;
    }

    public void SetTimer(float amount)
    {
        _image.fillAmount = amount;
    }
}
