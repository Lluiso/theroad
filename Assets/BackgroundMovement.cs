using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
	[SerializeField] private TrackGenerator _trackGenerator;
	[SerializeField] private float _multiplier;

	// Update is called once per frame
	void Update()
	{
		var zDelta = _trackGenerator.CarSpeed * Time.deltaTime * _multiplier;
		var newPos = transform.position;
		newPos.z -= zDelta;
		transform.position = newPos;
	}
}