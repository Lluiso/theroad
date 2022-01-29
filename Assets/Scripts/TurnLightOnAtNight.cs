using UnityEngine;

[ExecuteInEditMode]
public class TurnLightOnAtNight : MonoBehaviour
{
	[SerializeField] private GameSettings _settings;
	[SerializeField] private GameObject _emissionlight;
	private Light _light;

	private void Awake()
	{
		_light = GetComponent<Light>();
		SetLightsEnabled(false);
	}

	void Update()
	{
		if (_light == null)
		{
			_light = GetComponent<Light>();
		}
		SetLightsEnabled(TerrainGenerator.ProgressToFerry > _settings.ProgressForLightsOn);
	}

	void SetLightsEnabled(bool isEnabled)
	{
		_light.enabled = isEnabled;
		if (_emissionlight != null)
		{
			_emissionlight.SetActive(isEnabled);
		}
	}
}