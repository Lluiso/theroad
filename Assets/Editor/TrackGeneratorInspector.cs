using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrackGenerator))]
public class TrackGeneratorInspector : Editor
{
	public override void OnInspectorGUI()
	{
		TrackGenerator generator = (TrackGenerator)target;
		base.OnInspectorGUI();
		if (GUILayout.Button("Clear and generate"))
		{
			generator.ClearAndGenerate();
		}
		if (GUILayout.Button("Clear"))
		{
			generator.ClearTrack();
		}
	}
}