using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
	[SerializeField] private TrackGenerator _track;
	[SerializeField] private Animator _ac;
	[SerializeField] private int _maxPassengers = 4;
	public static int MaxPassengers;
	public static List<string> Passengers = new List<string>();

	private void Awake()
	{
		// lazy exposing of max :)
		MaxPassengers = _maxPassengers;
		CarEvents.AddPassenger = AddPassenger;
		CarEvents.RemovePassenger = RemovePassenger;
		// hack - get things moving
		GameEvents.StartGame += CarEvents.EndInteraction;
		TrackGenerator.OnPassengerApproaching += GoToHitchhikeMode;
	}

	private void Update()
	{
		_ac.SetFloat("speed", _track.CarSpeed);
	}

	public void GoToHitchhikeMode(string passengerToPickUp)
	{
		CarEvents.Passenger.SlowingToPickUp?.Invoke(passengerToPickUp);
		StartCoroutine(DelayToSlow(1f, passengerToPickUp));
	}

	private IEnumerator DelayToSlow(float delay, string passengerToPickUp)
	{
		yield return new WaitForSeconds(delay);
		CarEvents.Passenger.DelayedSlowingToPickUp?.Invoke(passengerToPickUp);
	}

	private void AddPassenger(string name)
	{
		if (Passengers.Count >= _maxPassengers)
		{
			return;
		}
		Passengers.Add(name);
		CarEvents.Passenger.Entered?.Invoke(name);
		CarEvents.EndInteraction?.Invoke();
	}

	private void RemovePassenger(string name)
	{
		if (!Passengers.Contains(name))
		{
			return;
		}
		Passengers.Remove(name);
		CarEvents.Passenger.Exited?.Invoke(name);
		CarEvents.EndInteraction?.Invoke();
	}
}