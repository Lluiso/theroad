using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
	public float TimeLimitInSeconds => _timeLimitInSeconds;
	public float StartingDistanceToFerryMeters => _startingDistanceToFerryMeters;
	public float ForceSkyLightProgress => _forceSkylightProgress;
	public float ProgressForLightsOn => _progressForLightsOn;

	public List<GameObject> VegetationPrefabs => _vegetationPrefabs;
	public float VegetationPlaneMargin => _vegetationPlaneMargin;
	public int MinVegetationPerPlane => _minVegetationPerPlane;
	public int MaxVegetationPerPlane => _maxVegetationPerPlane;

	[Header("DEBUG")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _forceSkylightProgress;

	public bool SkipToNight;

	[Header("Weather")] public float RainStartProgress;

	[Header("Passengers")] public List<float> PassengerSpawnPoints;
	public List<Passenger> Passengers;
	public float DistanceToAlertApproachingCharacter;
	public float DistanceFromCharacterToStop;

	[Header("Starting values")]
	[SerializeField]
	private float _startingDistanceToFerryMeters;

	[Header("Vegetation")]
	[SerializeField]
	private float _vegetationPlaneMargin;

	[SerializeField] private int _minVegetationPerPlane;
	[SerializeField] private int _maxVegetationPerPlane;

	[SerializeField] private List<GameObject> _vegetationPrefabs;

	[SerializeField] private float _timeLimitInSeconds;

	[Header("Car")] [SerializeField] private float _progressForLightsOn;

	[System.Serializable]
	public class Passenger
	{
		public string Name;
		public Sprite Avatar;
	}
}