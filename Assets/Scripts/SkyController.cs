using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SkyController : MonoBehaviour
{
	public static float NightProgress { get; private set; }
	[Header("Sun")] [SerializeField] private Material _sunlightMaterial;
	[SerializeField] private Light _sunlightDirectional;

	[SerializeField] private Transform _sun;
	[SerializeField] private Transform _sunLightStartRotation;
	[SerializeField] private Transform _sunlightEndRotation;

	[Header("Moon")] [SerializeField] private Material _moonlightMaterial;
	[SerializeField] private Transform _moon;
	[SerializeField] private Transform _moonLightStartRotation;
	[SerializeField] private Transform _moonlightEndRotation;
	[SerializeField] private Light _moonlightDirectional;
	[SerializeField] private Color _startColor;
	[SerializeField] private Color _endColor;
	[SerializeField] private float _maxContrast;
	[SerializeField] private float _minLightIntesity;
	[SerializeField] private float _maxLightIntesity;
	[SerializeField] private GameSettings _gameSettings;
	[SerializeField] private bool _updateInEditMode;
	private float progressOverride => _gameSettings.ForceSkyLightProgress;
	private float progressToFerry = 0f;
	private bool isLightUpdatePaused = false;

	private void Awake()
	{
		Thunder.OnThunderStart += OnThunderStart;
	}

	private void OnThunderStart()
	{
		StartCoroutine(LightningFlash());
	}

	IEnumerator LightningFlash()
	{
		float startingIntensity = _moonlightDirectional.intensity;
		isLightUpdatePaused = true;
		yield return new WaitForSeconds(0.2f);
		_moonlightDirectional.intensity = 10f;
		yield return new WaitForSeconds(0.1f);
		_moonlightDirectional.intensity = startingIntensity;
		isLightUpdatePaused = false;
	}

	void Update()
	{
		if (Application.isPlaying == false && _updateInEditMode)
		{
			progressToFerry = progressOverride;
		}
		else
		{
			progressToFerry = TrackGenerator.ProgressToFerry;
		}
		UpdateLights();
	}

	void UpdateLights()
	{
		// might be doing flash
		if (isLightUpdatePaused)
		{
			return;
		}
		SetLightSettings();
		SetSunProgress();
		SetMoonProgress();
	}

	void SetLightSettings()
	{
		bool isDay = progressToFerry < 0.5f;
		RenderSettings.skybox = isDay ? _sunlightMaterial : _moonlightMaterial;
		RenderSettings.sun = isDay ? _sunlightDirectional : _moonlightDirectional;
		// _sunlightDirectional.enabled = isDay;
		// _moonlightDirectional.enabled = !isDay;
	}

	void SetSunProgress()
	{
		var sunsetProgress = progressToFerry / 0.5f;
		var sunRotation =
			Quaternion.Lerp(_sunLightStartRotation.rotation, _sunlightEndRotation.rotation, sunsetProgress);
		_sun.rotation = sunRotation;
	}

	void SetMoonProgress()
	{
		NightProgress = Mathf.Lerp(0f, 1f, (progressToFerry - 0.5f) / 0.5f);
		var newColor = Color.Lerp(_startColor, _endColor, NightProgress);
		_moonlightMaterial.SetColor("_SkyColor", newColor);
		_moonlightMaterial.SetFloat("_Brightness", _maxContrast * NightProgress);
		_moonlightDirectional.intensity = _minLightIntesity + (_maxLightIntesity - _minLightIntesity) * NightProgress;
		var moonLightRotation =
			Quaternion.Lerp(_moonLightStartRotation.rotation, _moonlightEndRotation.rotation, NightProgress);
		_moon.rotation = moonLightRotation;
	}
}