using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WorldUtils : MonoBehaviour
{
	[SerializeField] private TrackGenerator _track;

	[Button("Clear track")]
	void ClearTrack()
	{
		_track.ClearTrack();
	}
}