using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DialogueDisplay : MonoBehaviour
{
    [SerializeField] private DialogueBox[] _boxes;
    [SerializeField] private Transform _dialogueParent;
    [SerializeField] private AnimationCurve _delayPerWordCurve;
    [SerializeField] private int _maxWordCount = 15;
    [SerializeField] private float _maxWordTime = 3f;
    [SerializeField] private int _maxCharsPerLine = 20;

    private List<GameObject> _currentGameObjects = new List<GameObject>();

    [System.Serializable]
    public class DialogueBox
    {
        public DialogueMessage.MessageType Type;
        public GameObject LeftMessagePrefab;
        public GameObject RightMessagePrefab;
    }

    private void Awake()
    {
        DialogueController.ShowDialogue += ShowDialogue;
    }

    private void ShowDialogue(Dialogue dialogue)
    {
        ClearMessages();
        StartCoroutine(ShowDialogueMessages(dialogue));
    }

    private void ClearMessages()
    {
        for (var i=0; i< _currentGameObjects.Count; i++)
        {
            Destroy(_currentGameObjects[i]);
        }
        _currentGameObjects.Clear();
    }

    private string FormatMessage(DialogueMessage message)
    {
        var formattedMessage = message.Message;
        if (message.Message.Length > _maxCharsPerLine)
        {
            var words = message.Message.Split(' ');
            var newStr = "";
            var currentLineLength = 0;
            foreach (var word in words)
            {
                if (currentLineLength + word.Length > _maxCharsPerLine)
                {
                    newStr += "\n" + word;
                    currentLineLength = word.Length;
                }
                else
                {
                    if (string.IsNullOrEmpty(newStr))
                    {
                        newStr += word;
                    }
                    else
                    {
                        newStr += " " + word;
                    }
                    currentLineLength += word.Length;
                }
            }
            formattedMessage = newStr;
        }
        return $"<b>{message.CharacterName}</b>\n{formattedMessage}";
    }

    private float GetMessageDelay(DialogueMessage message)
    {
        var words = message.Message.Split(' ').Length;
        var delay = _delayPerWordCurve.Evaluate((float)words / _maxWordCount) * _maxWordTime;
        return delay;
    }

    private IEnumerator ShowDialogueMessages(Dialogue dialogue)
    {
        var lastCharacterToSpeak = dialogue.DialogueMessages.First().CharacterName;
        bool isRight = true;
        foreach (var message in dialogue.DialogueMessages)
        {
            var box = _boxes.FirstOrDefault(b => b.Type == message.MessageBoxType);
            if (box == null)
            {
                Debug.LogError("Box type not found: " + message.MessageBoxType);
                box = _boxes.FirstOrDefault(b => b.Type == DialogueMessage.MessageType.Default);
            }

            if (message.CharacterName != lastCharacterToSpeak)
            {
                isRight = !isRight;
                lastCharacterToSpeak = message.CharacterName;
            }
            var prefab = isRight ? box.RightMessagePrefab : box.LeftMessagePrefab;
            var go = Instantiate(prefab, _dialogueParent);
            go.GetComponent<UI_Message>().Set(FormatMessage(message), isRight);
            _currentGameObjects.Add(go);
            yield return new WaitForSeconds(GetMessageDelay(message));
        }
    }

    private IEnumerator ShowDecisionButtons()
    {
        // todo slow down time
        // show buttons
        // wait for input/call reaction events
        yield return null;
    }
}
