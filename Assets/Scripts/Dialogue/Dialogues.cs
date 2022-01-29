public class Dialogue_EnterCar : Dialogue
{
    public Dialogue_EnterCar(DialogueMessage[] dialogue)
    {
        _dialogue = dialogue;
    }
    private DialogueMessage[] _dialogue;

    public new DialogueEvents.Trigger TriggerEvent => DialogueEvents.Trigger.EnterCar;
    public new DialogueEvents.Choice[] Choices => null;

    public new DialogueEvents.Condition Conditions => null;
}

public class Dialogue_LeaveCar : Dialogue
{
    public Dialogue_LeaveCar(DialogueMessage[] dialogue)
    {
        _dialogue = dialogue;
    }
    private DialogueMessage[] _dialogue;

    public new DialogueEvents.Trigger TriggerEvent => DialogueEvents.Trigger.LeaveCar;
    public new DialogueEvents.Choice[] Choices => null;

    public new DialogueEvents.Condition Conditions => null;
}

public class Dialogue_Stopping : Dialogue
{
    public Dialogue_Stopping(StoppingDialogue dialogue)
    {
        _dialogue = dialogue.Dialogue;
        _stoppingConditions = dialogue.Conditions;
    }

    public DialogueMessage[] _dialogue;
    public DialogueEvents.StoppingConditions _stoppingConditions;

    public new DialogueEvents.Trigger TriggerEvent => DialogueEvents.Trigger.Stopping;
    public new DialogueEvents.Choice[] Choices => null;

    public new DialogueEvents.Condition Conditions => _stoppingConditions;
}


public class Dialogue_InCar : Dialogue
{
    public Dialogue_InCar(InCarDialogue dialogue)
    {
        _dialogue = dialogue.Dialogue;
        _inCarConditions = dialogue.Conditions;
        _choices = dialogue.Choices;
    }
    private DialogueMessage[] _dialogue;
    private DialogueEvents.InCarConditions _inCarConditions;
    private DialogueEvents.Choice[] _choices;

    public new DialogueEvents.Trigger TriggerEvent => DialogueEvents.Trigger.InCar;
    public new DialogueEvents.Choice[] Choices => _choices;

    public new DialogueEvents.Condition Conditions => _inCarConditions;
}


public class Dialogue_Hitchhike : Dialogue
{
    public Dialogue_Hitchhike(DialogueMessage[] dialogue)
    {
        _dialogue = dialogue;
    }
    private DialogueMessage[] _dialogue;

    public new DialogueEvents.Trigger TriggerEvent => DialogueEvents.Trigger.Hitchhike;
    public new DialogueEvents.Condition Conditions => null;
    public new DialogueEvents.Choice[] Choices => new [] {
        new DialogueEvents.Choice { Resolution = DialogueEvents.ResolutionType.Accept, Context = ""},
        new DialogueEvents.Choice { Resolution = DialogueEvents.ResolutionType.Reject, Context = ""}
    };
}

[System.Serializable]
public class DialogueMessage
{
    public string CharacterName;
    public string Message;
    public MessageType MessageBoxType;
    
    public enum MessageType
    {
        Default,
        Happy,
        Angry
    }
}

[System.Serializable]
public class InCarDialogue
{
    public DialogueMessage[] Dialogue;
    public DialogueEvents.InCarConditions Conditions;
    public DialogueEvents.Choice[] Choices;
}

[System.Serializable]
public class StoppingDialogue
{
    public DialogueMessage[] Dialogue;
    public DialogueEvents.StoppingConditions Conditions;
}