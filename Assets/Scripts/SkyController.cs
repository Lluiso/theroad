using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SkyController : MonoBehaviour
{
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
		float nightProgress = Mathf.Lerp(0f, 1f, (progressToFerry - 0.5f) / 0.5f);
		var newColor = Color.Lerp(_startColor, _endColor, nightProgress);
		_moonlightMaterial.SetColor("_SkyColor", newColor);
		_moonlightMaterial.SetFloat("_Brightness", _maxContrast * nightProgress);
		_moonlightDirectional.intensity = _minLightIntesity + (_maxLightIntesity - _minLightIntesity) * nightProgress;
		var moonLightRotation =
			Quaternion.Lerp(_moonLightStartRotation.rotation, _moonlightEndRotation.rotation, nightProgress);
		_moon.rotation = moonLightRotation;
	}
}