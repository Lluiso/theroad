using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeatherController : MonoBehaviour
{
	[SerializeField] private GameSettings _settings;
	private Camera _mainCam;
	[SerializeField] private GameObject _thunderPrefab;

	[SerializeField] private AudioSource _thunderSounds;
	[SerializeField] private AudioSource _rainSounds;
	[SerializeField] private List<ParticleSystem> _rainParticles;
	[SerializeField] private float _minRainModifier;
	[SerializeField] private float _maxRainModifier;
	[SerializeField] private float _minRainEmission;
	[SerializeField] private float _maxRainEmission;

	[Header("Camera shake")]
	[SerializeField]
	float shakeDuration = 0f;

	[SerializeField] float shakeAmount = 0.7f;
	[SerializeField] float decreaseFactor = 1.0f;

	private float shakeDurationRemaining;
	Vector3 cameraStartPos;

	private void Awake()
	{
		_mainCam = Camera.main;
	}

	private void Update()
	{
		if (TrackGenerator.ProgressToFerry > _settings.RainStartProgress)
		{
			if (_rainSounds.isPlaying == false)
			{
				_rainSounds.Play();
			}
			foreach (var ps in _rainParticles)
			{
				ps.Play();
				UpdateRainStrength();
			}
		}
	}

	void UpdateRainStrength()
	{
		foreach (var ps in _rainParticles)
		{
			var emission = ps.emission;
			emission.rateOverTimeMultiplier =
				_minRainEmission + (_maxRainEmission - _minRainEmission) * SkyController.NightProgress;
			var velocity = ps.velocityOverLifetime;
			velocity.speedModifierMultiplier =
				_minRainModifier + (_maxRainModifier - _minRainModifier) * SkyController.NightProgress;
			// ps.velocityOverLifetime = velocity;
		}
	}

	[Button("Spawn thunder")]
	public void SpawnThunder()
	{
		StartCoroutine(SpawnThunderSequence());
	}

	IEnumerator SpawnThunderSequence()
	{
		Instantiate(_thunderPrefab);
		yield return new WaitForSeconds(1f);
		_thunderSounds.Play();
		cameraStartPos = _mainCam.transform.position;
		StartCoroutine(ShakeCamera());
	}

	IEnumerator ShakeCamera()
	{
		shakeDurationRemaining = shakeDuration;
		while (shakeDurationRemaining > 0.01f)
		{
			_mainCam.transform.position = cameraStartPos + Random.insideUnitSphere * shakeAmount;
			shakeDurationRemaining -= Time.deltaTime * decreaseFactor;
			yield return null;
		}
		_mainCam.transform.position = cameraStartPos;
	}
}