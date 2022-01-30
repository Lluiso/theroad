using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassengerApproachingController : MonoBehaviour
{
    [SerializeField] private GameObject _avatar;
    [SerializeField] private Image _avatarImage;
    [SerializeField] private Button _stopButton;
    [SerializeField] private Button _ignoreButton;
    [SerializeField] private Image _ignoreFill;
    [SerializeField] private float _interactTimeLimit = 5f;
    [SerializeField] private GameSettings _settings;

    private string _passengerName;
    private Coroutine _coroutine;
    private bool _stopping;

    private void Awake()
    {
        _avatar.gameObject.SetActive(false);
        _stopButton.onClick.AddListener(StopAtPassenger);
        _ignoreButton.onClick.AddListener(IgnorePassenger);
        TrackGenerator.OnPassengerApproaching += ShowAvatar;
        TrackGenerator.OnNextToPassenger += CheckStop;
        _ignoreFill.fillAmount = 0f;
    }

    private void StopAtPassenger()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _stopping = true;
        _avatar.gameObject.SetActive(false);
    }

    private void CheckStop(string passenger)
    {
        if (_stopping && passenger == _passengerName)
        {
            CarEvents.Passenger.StoppedAt?.Invoke(_passengerName);
        }
        _stopping = false;
    }

    private void IgnorePassenger()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _stopping = false;

        CarEvents.EndInteraction?.Invoke();
        _avatar.gameObject.SetActive(false);
    }

    private void ShowAvatar(string passenger)
    {
        _stopping = false;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        var sprite = _settings.Passengers.First(p => p.Name == passenger).Avatar;
        _avatarImage.sprite = sprite;
        _ignoreFill.fillAmount = 0f;
        _passengerName = passenger;
        CarEvents.StartInteraction?.Invoke();
        _avatar.gameObject.SetActive(true);
        _coroutine = StartCoroutine(InteractTimer());
    }

    private IEnumerator InteractTimer()
    {
        var elapsed = 0f;
        while (elapsed < _interactTimeLimit)
        {
            elapsed += Time.deltaTime;
            var progress = elapsed / _interactTimeLimit;

            _ignoreFill.fillAmount = progress;
            yield return null;
        }
        IgnorePassenger();
    }
}
