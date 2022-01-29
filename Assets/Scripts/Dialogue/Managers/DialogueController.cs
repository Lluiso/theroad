using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class DialogueController
{
    public static Action<Dialogue, DialogueEvents.Choice[]> ShowDialogue;

    private static CharacterDialogues _characterDialogues;
    private static int PassengerCount => Car.Passengers.Count;
    private static int MaxPassengers => Car.MaxPassengers;

    private static Dictionary<string, BaseDialogues> CurrentPassengerDialogues => _characterDialogues.CurrentPassengers;
    private static BaseDialogues PassengerDialogue(string name) => _characterDialogues.GetDialogueForCharacter(name);

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        // Load our dialogues
        _characterDialogues = new CharacterDialogues();
        _characterDialogues.Init();

        // subscribe to car events
        CarEvents.CheckForInCarDialogue += CheckForInCarDialogue;
        CarEvents.Passenger.Entered += OnPassengerEntered;
        CarEvents.Passenger.Exited += OnPassengerExited;
        CarEvents.Passenger.Rejected += OnPassengerExited;
        CarEvents.Passenger.SlowingToPickUp += OnSlowToPickUp;
        CarEvents.Passenger.StoppedAt += OnStoppedAt;
    }

    private static void CheckForInCarDialogue()
    {
        // check if any of our current passengers don't like each other
        var valid = new List<Dialogue_InCar>();
        foreach (var character in CurrentPassengerDialogues.Values)
        {
            var validDialogues = character.InCar.Where(d => d.CheckConditionsMet(CurrentPassengerDialogues.Keys.ToArray()) && !d.Seen).ToList();
            valid.AddRange(validDialogues);
        }
        Debug.Log("Valid dialogues for current passengers while driving: " + valid.Count);
        if (valid.Count > 0)
        {
            // pick random
            var rand = UnityEngine.Random.Range(0, valid.Count);
            TryShowDialogue(valid[rand], valid[rand].Choices);
        }
    }

    private static void OnPassengerEntered(string passengerName)
    {
        var dialogue = PassengerDialogue(passengerName);
        if (dialogue != null && dialogue.EnterCar != null)
        {
            TryShowDialogue(dialogue.EnterCar, dialogue.EnterCar.Choices);
        }
    }
    
    private static void OnPassengerExited(string passengerName)
    {
        var dialogue = PassengerDialogue(passengerName);
        if (dialogue != null && dialogue.LeaveCar != null)
        {
            TryShowDialogue(dialogue.LeaveCar, dialogue.LeaveCar.Choices);
        }
    }

    private static void OnSlowToPickUp(string newPassenger)
    {
        // check if any of our current passengers don't like this new one
        var valid = new List<Dialogue_Stopping>();
        foreach(var character in CurrentPassengerDialogues.Values)
        {
            var validDialogues = character.Stopping.Where(d => d.CheckConditionMet(newPassenger) && !d.Seen).ToList();
            valid.AddRange(validDialogues);
        }
        Debug.Log("Valid dialogues for current passengers when picking up new passenger: " + valid.Count);
        if (valid.Count > 0)
        {
            // pick random
            var rand = UnityEngine.Random.Range(0, valid.Count);
            TryShowDialogue(valid[rand], valid[rand].Choices);
        }
    }

    private static void OnStoppedAt(string newPassenger)
    {
        var dialogue = PassengerDialogue(newPassenger);
        if (dialogue != null && dialogue.Hitchhike != null)
        {
            TryShowDialogue(dialogue.Hitchhike, dialogue.Hitchhike.Choices);
        }
    }

    private static void TryShowDialogue(Dialogue dialogue, DialogueEvents.Choice[] choices)
    {
        // sanity check
        if (!dialogue.Seen)
        {
            ShowDialogue?.Invoke(dialogue, choices);
            dialogue.MarkSeen();          
        }
    }
}
