using System;
using System.Collections;
using UnityEngine;

public class Thunder : MonoBehaviour
{
	public static Action OnThunderStart;
	[SerializeField] private SpriteRenderer _thunderSprite;
	[SerializeField] private Transform _lightningMask;
	[SerializeField] private float _lightningRevealPerFrame;
	[SerializeField] private float _lightningFadeoutPerFrame;
	[SerializeField] private float _lightDelay;
	[SerializeField] private float _soundDelay;
	private Vector3 _targetScale;

	private void Start()
	{
		_targetScale = _lightningMask.localScale;
		_lightningMask.localScale = Vector3.zero;
		StartCoroutine(LightningRoutine());
	}

	private IEnumerator LightningRoutine()
	{
		OnThunderStart?.Invoke();
		var revealProgress = 0f;
		while (revealProgress < 1f)
		{
			revealProgress += _lightningRevealPerFrame;
			_lightningMask.localScale = _targetScale * revealProgress;
			yield return null;
		}
		while (_thunderSprite.color.a > 0.01f)
		{
			var newColor = _thunderSprite.color;
			newColor.a -= _lightningFadeoutPerFrame;
			_thunderSprite.color = newColor;
			yield return null;
		}
	}
}