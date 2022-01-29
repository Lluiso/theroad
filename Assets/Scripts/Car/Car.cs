using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public int MaxPassengers = 4;
    public static List<string> Passengers = new List<string>();

    private void Awake()
    {
        CarEvents.AddPassenger = AddPassenger;
        CarEvents.RemovePassenger = RemovePassenger;
    }

    private void AddPassenger(string name)
    {
        if (Passengers.Count >= MaxPassengers)
        {
            return;
        }
        Passengers.Add(name);
        CarEvents.Passenger.Entered?.Invoke(name);
    }

    private void RemovePassenger(string name)
    {
        if (Passengers.Contains(name))
        {
            return;
        }
        Passengers.Remove(name);
        CarEvents.Passenger.Exited?.Invoke(name);
    }
}
