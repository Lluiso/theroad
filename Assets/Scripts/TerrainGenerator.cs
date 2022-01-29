using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
	[SerializeField] private GameSettings _settings;
	[SerializeField] private float _segmentLength;
	[SerializeField] private float _visibleLength;
	[SerializeField] private GameObject _roadSectionPrefab;
	private int _numberOfSectionsToGenerate;

	public void DEBUGClearAndGenerate()
	{
		DestroyChildren();
		_numberOfSectionsToGenerate = (int)((_settings.StartingDistanceToFerryMeters / _segmentLength) + 1);
		Generate();
	}

	void Generate()
	{
		float nextZ = transform.position.z;
		for (int i = 0; i < _numberOfSectionsToGenerate; i++)
		{
			nextZ = _segmentLength * i;
			var newSpawnPos = transform.position;
			newSpawnPos.z = nextZ;
			var newRoadPiece = Instantiate(_roadSectionPrefab, transform, true);
			newRoadPiece.name = $"Road section {i}";
			newRoadPiece.transform.position = newSpawnPos;
		}
	}

	void DestroyChildren()
	{
		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(transform.GetChild(i).gameObject);
		}
	}
}