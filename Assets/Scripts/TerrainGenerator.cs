using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
	[SerializeField] private GameSettings _settings;
	[SerializeField] private float _segmentLength;
	[SerializeField] private float _roadSignIntervalsMeters;
	[SerializeField] private float _visibleLength;
	[SerializeField] private GameObject _roadSectionPrefab;
	[SerializeField] private GameObject _roadSignPrefab;
	private int _numberOfSectionsToGenerate;
	private Transform _roadSegmentsParent;

	public void DEBUGClearAndGenerate()
	{
		DestroyChildren();
		_numberOfSectionsToGenerate = (int)((_settings.StartingDistanceToFerryMeters / _segmentLength) + 1);
		Generate();
	}

	void Generate()
	{
		SpawnRoadSegments();
		SpawnRoadSigns();
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
	}
}