using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
	public float TimeLimitInSeconds => _timeLimitInSeconds;
	public float StartingDistanceToFerryMeters => _startingDistanceToFerryMeters;
	public float ForceSkyLightProgress => _forceSkylightProgress;
	public float ProgressForLightsOn => _progressForLightsOn;

	[Header("DEBUG")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _forceSkylightProgress;

	[Header("Starting values")]
	[SerializeField]
	private float _startingDistanceToFerryMeters;

	[SerializeField] private float _timeLimitInSeconds;

	[Header("Car")] [SerializeField] private float _progressForLightsOn;
}