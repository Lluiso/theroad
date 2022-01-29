using UnityEngine;

[ExecuteInEditMode]
public class TurnLightOnAtNight : MonoBehaviour
{
	[SerializeField] private GameSettings _settings;
	private Light _light;
	private void Awake()
	{
		_light = GetComponent<Light>();
		SetLightsEnabled(false);
	}
#if UNITY_EDITOR
	void Update()
	{
		if (_light == null)
		{
			_light = GetComponent<Light>();
		}
		SetLightsEnabled(_settings.ForceSkyLightProgress > _settings.ProgressForLightsOn);
	}
#endif

	void SetLightsEnabled(bool isEnabled)
	{
		_light.enabled = isEnabled;
	}
}