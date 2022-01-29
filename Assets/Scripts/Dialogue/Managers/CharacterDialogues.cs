using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogues
{
    public Dictionary<string, BaseDialogues> AllCharacters;
    public Dictionary<string, BaseDialogues> CurrentPassengers { get; private set; }

    public void Init()
    {
        AllCharacters = new Dictionary<string, BaseDialogues>();
        CurrentPassengers = new Dictionary<string, BaseDialogues>();

        var chars = Resources.LoadAll<BaseDialogues>("Characters/");
        Debug.Log("Found " + chars.Length + " Character Dialogues");

        foreach (var c in chars)
        {
            AllCharacters.Add(c.CharacterName, c);
        }

        CarEvents.Passenger.Entered += AddPassenger;
        CarEvents.Passenger.Exited += RemovePassenger;
    }

    public BaseDialogues GetDialogueForCharacter(string name)
    {
        if (!AllCharacters.ContainsKey(name))
        {
            return null;
        }
        return AllCharacters[name];
    }

    private void AddPassenger(string name)
    {
        if (!AllCharacters.ContainsKey(name))
        {
            Debug.LogError("No Dialogue found for " + name);
        }
        var dialogue = AllCharacters[name];
        CurrentPassengers.Add(name, dialogue);
    }

    private void RemovePassenger(string name)
    {
        if (CurrentPassengers.ContainsKey(name))
        {
            CurrentPassengers.Remove(name);
        }
    }
}
