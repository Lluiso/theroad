using System;
using System.Collections.Generic;
using System.Linq;
using TeamDuaLipa;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
	public float CarSpeed => _carSpeed;
	public static Action<string> OnPassengerApproaching;
	public static float ProgressToFerry { get; private set; }
	[SerializeField] private GameSettings _settings;
	[SerializeField] private float _segmentLength;
	[SerializeField] private float _roadSignIntervalsMeters;
	[SerializeField] private float _visibleLength;
	[SerializeField] private GameObject _roadSectionPrefab;
	[SerializeField] private GameObject _roadSignPrefab;
	[SerializeField] private GameObject _passengerPrefab;
	[SerializeField] private Transform _car;
	[SerializeField] private float _carSpeed;
	[SerializeField] private float _roadLightPrefab;
	[SerializeField] private float _roadLightsInterval;
	[SerializeField] private float _lightX;
	[SerializeField] private bool _generateOnAwake;
	[SerializeField] private List<Transform> _passengers = new List<Transform>();
	private int _numberOfSectionsToGenerate;
	private Transform _roadSegmentsParent;
	private float distanceCovered = 0f;
	private float distanceToNextCharacter;
	private string lastPassengerAlertFired;

	private void Awake()
	{
		if (_generateOnAwake)
		{
			ClearAndGenerate();
		}
	}

	public void ClearTrack()
	{
		DestroyChildren();
	}

	public void ClearAndGenerate()
	{
		DestroyChildren();
		_numberOfSectionsToGenerate = (int)((_settings.StartingDistanceToFerryMeters / _segmentLength) + 1);
		Generate();
	}

	void Update()
	{
		MoveTrack();
		CheckForApproachingPassenger();
	}

	private void CheckForApproachingPassenger()
	{
		Transform nextPassenger;
		for (int i = 0; i < _passengers.Count; i++)
		{
			var passenger = _passengers[i];
			if (passenger.position.z < _car.transform.position.z)
			{
				continue;
			}
			nextPassenger = passenger;
			distanceToNextCharacter = passenger.transform.position.z - _car.position.z;
			if (lastPassengerAlertFired != nextPassenger.gameObject.name &&
			    distanceToNextCharacter <= _settings.DistanceToAlertApproachingCharacter)
			{
				var passengerName = nextPassenger.gameObject.name;
				print($"approaching {passengerName}");
				lastPassengerAlertFired = passengerName;
				OnPassengerApproaching?.Invoke(passengerName);
			}
			break;
		}
	}

	void MoveTrack()
	{
		var newPos = transform.position;
		var zDelta = _carSpeed * Time.deltaTime;
		distanceCovered += zDelta;
		newPos.z -= zDelta;
		transform.position = newPos;
		ProgressToFerry = distanceCovered / _settings.StartingDistanceToFerryMeters;
	}

	void Generate()
	{
		SpawnRoadSegments();
		SpawnRoadSigns();
		SpawnLights();
		SpawnPassengers();
	}

	void SpawnPassengers()
	{
		_passengers = new List<Transform>();
		var container = MakeNewContainer("Passengers");
		List<string> _spawnedPassengerNames = new List<string>();
		foreach (var spawnPointPercentage in _settings.PassengerSpawnPoints)
		{
			var namesRemaining = _settings.PassengerNames.Where(n => _spawnedPassengerNames.Contains(n) == false);
			var randomName = namesRemaining.Random();
			_spawnedPassengerNames.Add(randomName);
			var newGO = Instantiate(_passengerPrefab, container);
			newGO.name = randomName;
			var z = _settings.StartingDistanceToFerryMeters * spawnPointPercentage;
			var newPos = transform.position;
			newPos.z = z;
			newGO.transform.position = newPos;
			_passengers.Add(newGO.transform);
		}
	}

	void SpawnLights()
	{
		// var container = MakeNewContainer("Road lights");
		// var currentDistance = 0f;
		// while (currentDistance < _settings.StartingDistanceToFerryMeters)
		// {
		// 	currentDistance += _roadLightsInterval;
		// 	Instantiate(_roadLightPrefab);
		// }
	}

	void SpawnRoadSigns()
	{
		var container = MakeNewContainer("Road Signs");
		var currentDistance = 0f;
		while (currentDistance < _settings.StartingDistanceToFerryMeters)
		{
			currentDistance += _roadSignIntervalsMeters;
			var distanceRemaining =
				Mathf.RoundToInt((_settings.StartingDistanceToFerryMeters - currentDistance) / 1000);
			if (distanceRemaining != 0)
			{
				var newSignGO = Instantiate(_roadSignPrefab);
				newSignGO.name = $"{distanceRemaining} km";
				newSignGO.transform.parent = container;
				var newPos = newSignGO.transform.position;
				newPos.z = currentDistance;
				newSignGO.transform.position = newPos;
				newSignGO.GetComponent<SetText>().Set($"Ferry \n{distanceRemaining} km");
			}
		}
	}

	void SpawnRoadSegments()
	{
		_roadSegmentsParent = MakeNewContainer("Road Segments");
		float nextZ = transform.position.z;

		// road & ground sections
		for (int i = 0; i < _numberOfSectionsToGenerate; i++)
		{
			nextZ = _segmentLength * i;
			var newSpawnPos = transform.position;
			newSpawnPos.z = nextZ;
			var newRoadPiece = Instantiate(_roadSectionPrefab, transform, true);
			newRoadPiece.GetComponent<RoadSection>().GenerateVegetation();
			newRoadPiece.name = $"Road section {i}";
			newRoadPiece.transform.position = newSpawnPos;
			newRoadPiece.transform.parent = _roadSegmentsParent;
		}
	}

	Transform MakeNewContainer(string newName)
	{
		var go = new GameObject(newName).transform;
		go.transform.position = Vector3.zero;
		go.parent = transform;
		return go;
	}

	void DestroyChildren()
	{
		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(transform.GetChild(i).gameObject);
		}
		_passengers.Clear();
	}
}