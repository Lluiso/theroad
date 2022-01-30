using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField]
    private int _maxPassengers = 4;
    public static int MaxPassengers;
    public static List<string> Passengers = new List<string>();

    private void Awake()
    {
        // lazy exposing of max :)
        MaxPassengers = _maxPassengers;

        CarEvents.AddPassenger = AddPassenger;
        CarEvents.RemovePassenger = RemovePassenger;

        TrackGenerator.OnPassengerApproaching += GoToHitchhikeMode;
    }

    private void Start()
    {
        CarEvents.EndInteraction?.Invoke();
    }

    public void GoToHitchhikeMode(string passengerToPickUp)
    {
        CarEvents.Passenger.SlowingToPickUp?.Invoke(passengerToPickUp);
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
