public class DialogueEvents 
{
    public enum Trigger
    {
        Hitchhike,
        InCar,
        Stopping,
        EnterCar,
        LeaveCar
    }

    [System.Serializable]
    public class Choice
    {
        public ResolutionType Resolution;
        public string Context;
    }

    public enum ResolutionType
    {
        None,
        Reject,
        Accept,
        KickOutCar
    }

    public abstract class Condition
    {
        public virtual bool IsMet(string[] args)
        {
            return true;
        }
    }

    [System.Serializable]
    public class InCarConditions : Condition
    {
        public override bool IsMet(string[] args)
        {
            return true;
        }

        public int PassengerCount;
        public string[] CharactersInCar;
    }

    [System.Serializable]
    public class StoppingConditions : Condition
    {
        public override bool IsMet(string[] args)
        {
            return true;
        }

        public string[] CharacterBeingPickedUp;
    }
}
