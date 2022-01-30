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

[SerializeField]
private GameSettings _gameSettings;
[SerializeField]
private float _inCarEventPollRate = 3;

	private void Awake()
	{
		// lazy exposing of max :)
		MaxPassengers = _maxPassengers;
		CarEvents.AddPassenger = AddPassenger;
		CarEvents.RemovePassenger = RemovePassenger;
		// hack - get things moving
		GameEvents.StartGame += CarEvents.EndInteraction;
		TrackGenerator.OnPassengerApproaching += GoToHitchhikeMode;
        
        CarEvents.MovingOff += ProcessPassengers;
        StartCoroutine(PollForCarEvent());
	}
    
    private IEnumerator PollForCarEvent()
    {
        var delay = new WaitForSeconds(_inCarEventPollRate);
        while (true)
        {
            yield return delay;
            if (TrackGenerator.NormalizedSpeed() > 0.5f && TrackGenerator.DistanceToNextPassenger() > _gameSettings.MinDistanceToTriggerInCarEvent && !UI_DialogueDisplay.ShowingDialogue)
            {
                CarEvents.CheckForInCarDialogue?.Invoke();
            }
        }
    }
    
    private void SetupDilemma(string newPassenger)
    {
        var newPass = new List<string>() { newPassenger };
        CarEvents.ShowDilemma(Passengers, newPass);
    }
    
    private void ProcessPassengers(List<string> inCarPassengers, List<string> outCarPassengers)
    {
        foreach(var p in inCarPassengers)
        {
            AddPassenger(p);
        }
        foreach (var p in outCarPassengers)
        {
            RemovePassenger(p);
        }
        CarEvents.EndInteraction?.Invoke();
    }

	private void Update()
	{
		_ac.SetFloat("speed", TrackGenerator.NormalizedSpeed());
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
        if (!Passengers.Contains(name))
        {
            Passengers.Add(name);
            CarEvents.Passenger.Entered?.Invoke(name);
        }
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