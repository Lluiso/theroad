using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorInspector : Editor
{
	public override void OnInspectorGUI()
	{
		TerrainGenerator generator = (TerrainGenerator)target;
		base.OnInspectorGUI();
		if (GUILayout.Button("Clear and generate"))
		{
			generator.DEBUGClearAndGenerate();
		}
	}
}