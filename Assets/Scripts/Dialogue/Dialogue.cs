public class Dialogue
{
    public DialogueEvents.Trigger TriggerEvent;
    public DialogueEvents.Choice[] Choices;

    public DialogueEvents.Condition Conditions;

    public DialogueMessage[] DialogueMessages;

    public void MarkSeen()
    {
        Seen = true;
    }
    public bool Seen { get; private set; } = false;
}
