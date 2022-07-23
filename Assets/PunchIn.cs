using DG.Tweening;
using UnityEngine;

public class PunchIn : MonoBehaviour
{
    private Vector3 startingValue;
    [SerializeField] private float _speed;
    [SerializeField] private Ease _ease;
    [SerializeField] private bool _runOnStart;

    private void Awake()
    {
        startingValue = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        if (_runOnStart)
        {
            DoEffect();
        }
    }

    [ContextMenu("Do effect")]
    void DoEffect()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(startingValue, _speed).SetEase(_ease);
        transform.DOScale(Vector3.zero, _speed).SetEase(_ease).SetDelay(_speed);
    }
}
