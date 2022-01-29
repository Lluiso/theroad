using System.Linq;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    void Awake()
    {
        DialogueController.ShowDialogue += DisplayDialogue;
    }

    // Update is called once per frame
    private void DisplayDialogue(Dialogue dialogue, DialogueEvents.Choice[] choices)
    {
        var messages = dialogue.DialogueMessages?.Select(m => "<color=cyan>"+ m.CharacterName + "</color>:\t" + m.Message).ToArray() ?? null;
        if (messages != null && messages.Length > 0)
        {
            var log = string.Join("\n", messages);
            Debug.Log("Message:\n" + log + "\n");
        }
    }
}
