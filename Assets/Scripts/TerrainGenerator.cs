using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
	[SerializeField] private int _sectionsToGenerate;
	[SerializeField] private int _sectionLengthWorldUnits;
	[SerializeField] private GameObject _roadSectionPrefab;

	public void DEBUGClearAndGenerate()
	{
		DestroyChildren();
		Generate();
	}

	void Generate()
	{
		float nextZ = transform.position.z;
		for (int i = 0; i < _sectionsToGenerate; i++)
		{
			nextZ = _sectionLengthWorldUnits * i;
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