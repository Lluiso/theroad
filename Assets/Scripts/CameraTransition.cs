using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField]
    private Transform _camera;
    [SerializeField]
    private Transform _cinematicTransform;
    [SerializeField]
    private Transform _standardTransfrom;

    [SerializeField]
    private float _transitionTime = 1f;

    private Coroutine _coroutine;
    // Start is called before the first frame update
    void Awake()
    {
        CarEvents.StartInteraction += GoToStandard;
        CarEvents.EndInteraction += GoToCinematic;

        _camera.position = _standardTransfrom.position;
        _camera.rotation = _standardTransfrom.rotation;
    }

    private void GoToStandard()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(Goto(_standardTransfrom));
    }

    private void GoToCinematic()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(Goto(_cinematicTransform));
    }

    private IEnumerator Goto(Transform to)
    {
        var elapsed = 0f;
        var from = _camera;
        while (elapsed < _transitionTime)
        {
            elapsed += Time.deltaTime;
            var progress = elapsed / _transitionTime;
            _camera.position = Vector3.Lerp(from.position, to.position, progress);
            _camera.rotation = Quaternion.Lerp(from.rotation, to.rotation, progress);
            yield return null;
        }
        _camera.position = to.position;
        _camera.rotation = to.rotation;
    }

}
