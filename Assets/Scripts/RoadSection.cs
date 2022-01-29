using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TeamDuaLipa;
using Random = UnityEngine.Random;

public class RoadSection : MonoBehaviour
{
	[SerializeField] private GameSettings _settings;
	[SerializeField] private GameObject _groundLeft;
	[SerializeField] private GameObject _groundRight;

	public void GenerateVegetation()
	{
		CreateVegetationOnPlane(_groundLeft);
		CreateVegetationOnPlane(_groundRight, 0.2f);
	}

	void CreateVegetationOnPlane(GameObject _targetPlane, float countModifier = 1.0f)
	{
		var vegToSpawn = Random.Range(_settings.MinVegetationPerPlane, _settings.MaxVegetationPerPlane);
		vegToSpawn = Mathf.RoundToInt(vegToSpawn * countModifier);
		var spawnPoints = GetRandomPointsOnPlane(_targetPlane, vegToSpawn);
		for (int i = 0; i < spawnPoints.Count; i++)
		{
			var go = Instantiate(_settings.VegetationPrefabs.Random(), spawnPoints[i], quaternion.identity);
			go.transform.parent = _targetPlane.transform;
			go.transform.position = spawnPoints[i];
		}
	}

	List<Vector3> GetRandomPointsOnPlane(GameObject _targetPlane, int pointsRequired)
	{
		List<Vector3> RandomPoints = new List<Vector3>();
		Vector3 min = _targetPlane.GetComponent<MeshFilter>().sharedMesh.bounds.min *
		              (1f - _settings.VegetationPlaneMargin);
		Vector3 max = _targetPlane.GetComponent<MeshFilter>().sharedMesh.bounds.max *
		              (1f - _settings.VegetationPlaneMargin);
		var scale = _targetPlane.transform.localScale;
		var xScale = scale.x;
		var zScale = scale.z;
		for (int i = 0; i < pointsRequired; i++)
		{
			Vector3 RndPointonPlane = _targetPlane.transform.position - new Vector3(
				(Random.Range(min.x * xScale, max.x * xScale)), _targetPlane.transform.position.y,
				(Random.Range(min.z * zScale, max.z * zScale)));
			RandomPoints.Add(RndPointonPlane);
		}
		return RandomPoints;
	}
}