using UnityEngine;
[CreateAssetMenu(fileName = "CharacterDialogue", menuName = "Dialogue/Character")]
public class BaseDialogues : UnityEngine.ScriptableObject
{
    public string CharacterName;
    [Space]
    [Header("HitchHike")]
    public DialogueMessage[] HitchhikeDialogue;

    [Header("Enter Car")]
    public DialogueMessage[] EnterCarDialogue;

    [Header("In Car")]
    public InCarDialogue[] InCarDialogues;

    [Header("Stopping")]
    public StoppingDialogue[] StoppingDialogues;

    [Header("Leave Car")]
    public DialogueMessage[] LeaveCarDialogue;

    public Dialogue_Hitchhike Hitchhike
    {
        get {
            if (_hitchike == null)
            {
                _hitchike = new Dialogue_Hitchhike(HitchhikeDialogue, CharacterName);
            }
            return _hitchike;
        }
    }
    private Dialogue_Hitchhike _hitchike;

    public Dialogue_InCar[] InCar
    {
        get
        {
            if (_inCar == null)
            {
                _inCar = new Dialogue_InCar[InCarDialogues.Length];
                for (var i = 0; i < _inCar.Length; i++)
                {
                    _inCar[i] = new Dialogue_InCar(InCarDialogues[i]);
                }
            }
            return _inCar;
        }
    }
    private Dialogue_InCar[] _inCar;

    public Dialogue_EnterCar EnterCar
    {
        get
        {
            if (_enterCar == null)
            {
                _enterCar = new Dialogue_EnterCar(EnterCarDialogue);
            }
            return _enterCar;
        }
    }
    private Dialogue_EnterCar _enterCar;

    public Dialogue_LeaveCar LeaveCar
    {
        get
        {
            if (_leaveCar == null)
            {
                _leaveCar = new Dialogue_LeaveCar(LeaveCarDialogue);
            }
            return _leaveCar;
        }
    }
    private Dialogue_LeaveCar _leaveCar;

    public Dialogue_Stopping[] Stopping
    {
        get
        {
            if (_stopping == null)
            {
                _stopping = new Dialogue_Stopping[StoppingDialogues.Length];
                for (var i=0; i<_stopping.Length; i++)
                {
                    _stopping[i] = new Dialogue_Stopping(StoppingDialogues[i]);
                }
            }
            return _stopping;
        }
    }
    private Dialogue_Stopping[] _stopping;

    
}
