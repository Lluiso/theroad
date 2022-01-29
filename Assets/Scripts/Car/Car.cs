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
    }

    public void GoToHitchhikeMode(string passengerToPickUp)
    {
        CarEvents.Passenger.SlowingToPickUp?.Invoke(passengerToPickUp);
    }

    public void StopAtPassenger(string passengerToPickUp)
    {
        CarEvents.Passenger.StoppedAt?.Invoke(passengerToPickUp);
    }

    private void AddPassenger(string name)
    {
        if (Passengers.Count >= _maxPassengers)
        {
            return;
        }
        Passengers.Add(name);
        CarEvents.Passenger.Entered?.Invoke(name);
    }

    private void RemovePassenger(string name)
    {
        if (!Passengers.Contains(name))
        {
            return;
        }
        Passengers.Remove(name);
        CarEvents.Passenger.Exited?.Invoke(name);
    }
}
