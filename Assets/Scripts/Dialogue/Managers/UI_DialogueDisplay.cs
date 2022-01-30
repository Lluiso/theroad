using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DialogueDisplay : MonoBehaviour
{
    [SerializeField] private DialogueBox[] _boxes;
    [SerializeField] private GameObject _decisionButtonParent;
    [SerializeField] private GameObject _decisionButton;
    [SerializeField] private GameObject _ignoreButton;
    [SerializeField] private Transform _dialogueParent;
    [SerializeField] private AnimationCurve _delayPerWordCurve;
    [SerializeField] private int _maxWordCount = 15;
    [SerializeField] private float _maxWordTime = 3f;
    [SerializeField] private int _maxCharsPerLine = 20;
    [SerializeField] private float _slowDownSpeed = 0.1f;
    [SerializeField] private float _timeAvailableForDecision = 5f;
    private bool _decisionMade;
    private Dictionary<int, string> _context = new Dictionary<int, string>();

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

    private void ShowDialogue(Dialogue dialogue, DialogueEvents.Choice[] choices)
    {
        ClearMessages();
        StartCoroutine(ShowDialogueMessages(dialogue, choices));
    }

    private void ClearMessages()
    {
        for (var i=0; i< _currentGameObjects.Count; i++)
        {
            Destroy(_currentGameObjects[i]);
        }
        _currentGameObjects.Clear();
        _context.Clear();
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

    private IEnumerator ShowDialogueMessages(Dialogue dialogue, DialogueEvents.Choice[] choices)
    {
        _decisionMade = false;
        var lastCharacterToSpeak = dialogue.DialogueMessages.First().CharacterName;
        bool isRight = false;
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
        if (choices != null)
        {
            StartCoroutine(ShowDecisionButtons(choices));
        }
        else
        {
            StartCoroutine(CleanUp());
        }
    }

    private IEnumerator ShowDecisionButtons(DialogueEvents.Choice[] choices)
    {
        Time.timeScale = _slowDownSpeed;
        bool canIgnore = true;
        Action ignoredEvent = null;
        DecisionButton_Ignore ignoreButton = null;
        _context = new Dictionary<int, string>();
        int index = 0;
        foreach (var c in choices)
        {
            index++;
            int indexCopy = index;
            _context.Add(index, c.Context);
            switch (c.Resolution)
            {
                case DialogueEvents.ResolutionType.Accept:
                    var acceptButton = Instantiate(_decisionButton, _decisionButtonParent.transform).GetComponent<DecisionButton>();
                    acceptButton.Set(() => OnButtonPress(CarEvents.AddPassenger, indexCopy), "Accept");
                    _currentGameObjects.Add(acceptButton.gameObject);
                    break;
                case DialogueEvents.ResolutionType.KickOutCar:
                    var kickOutButton = Instantiate(_decisionButton, _decisionButtonParent.transform).GetComponent<DecisionButton>();
                    kickOutButton.Set(() => OnButtonPress(CarEvents.RemovePassenger, indexCopy), "Kick Out " + c.Context);
                    _currentGameObjects.Add(kickOutButton.gameObject);
                    break;
                case DialogueEvents.ResolutionType.Reject:
                    var rejectButton = Instantiate(_decisionButton, _decisionButtonParent.transform).GetComponent<DecisionButton>();
                    rejectButton.Set(() => OnButtonPress(CarEvents.Passenger.Exited, indexCopy), "Reject");
                    _currentGameObjects.Add(rejectButton.gameObject);
                    break;
                case DialogueEvents.ResolutionType.None:
                    canIgnore = false;
                    // todo scope
                    // todo figure out if we want some over-arching resolution if you ignore the event
                    ignoreButton = Instantiate(_ignoreButton, _decisionButtonParent.transform).GetComponent<DecisionButton_Ignore>();
                    ignoredEvent = () => OnButtonPress(null, indexCopy);
                    ignoreButton.Set(() => OnButtonPress(null, indexCopy), "Ignore");
                    _currentGameObjects.Add(ignoreButton.gameObject);
                    break;
            }
        }
        var elapsed = 0f;
        if (!canIgnore)
        {
            while (!_decisionMade && elapsed <= _timeAvailableForDecision)
            {
                elapsed += Time.unscaledDeltaTime;
                var progress = elapsed / _timeAvailableForDecision;
                ignoreButton.SetTimer(progress);
                yield return null;
            }
            if (!_decisionMade)
            {
                ignoredEvent?.Invoke();
            }
        }
    }

    private void OnButtonPress(Action<string> call, int index)
    {
        Debug.Log(index);
        if (call != null && _context.ContainsKey(index))
        {
            var context = _context[index];
            call?.Invoke(context);
        }
        else
        {
            StartCoroutine(CleanUp(0f));
        }
        Time.timeScale = 1f;
    }


    private IEnumerator CleanUp(float delay = 0.5f)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }
        ClearMessages();
    }
}
