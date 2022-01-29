using System.Linq;

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
        
    }

    [System.Serializable]
    public class InCarConditions : Condition
    {
        public bool IsMet(string[] currentPassengers)
        {
            if (currentPassengers.Length == PassengerCount)
            {
                return true;
            }
            foreach (var c in currentPassengers)
            {
                if (CharactersInCar.Any(inCar => c == inCar))
                {
                    // this passenger doesnt like someone in the car
                    return true;
                }
            }
            return false;
        }

        public int PassengerCount;
        public string[] CharactersInCar;
    }

    [System.Serializable]
    public class StoppingConditions : Condition
    {
        public bool IsMet(string newPassenger)
        {
            // check if this passenger cares about the character being picked up
            return CharacterBeingPickedUp.Any(c => c == newPassenger);
        }

        public string[] CharacterBeingPickedUp;
    }
}
