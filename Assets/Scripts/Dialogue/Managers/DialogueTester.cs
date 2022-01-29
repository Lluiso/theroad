using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{
    public class DialogueTester : MonoBehaviour
    {
        public TestSteps[] Steps;
        IEnumerator Start()
        {
            foreach (var s in Steps)
            {
                yield return new WaitForSeconds(s.Delay);
                Debug.Log("Running Test: " + s.TriggerType + " ( " + s.Context + " )");
                switch (s.TriggerType)
                {
                    case TestSteps.TriggerTypes.GetInCar:
                        CarEvents.AddPassenger(s.Context);
                        break;
                    case TestSteps.TriggerTypes.ExitCar:
                        CarEvents.RemovePassenger(s.Context);
                        break;
                    case TestSteps.TriggerTypes.InCar:
                        CarEvents.CheckForInCarDialogue();
                        break;
                    case TestSteps.TriggerTypes.Stopping:
                        CarEvents.Passenger.SlowingToPickUp(s.Context);
                        break;
                    case TestSteps.TriggerTypes.Stopped:
                        CarEvents.Passenger.StoppedAt(s.Context);
                        break;
                }
            }
        }

        [System.Serializable]
        public class TestSteps
        {
            public float Delay;
            public TriggerTypes TriggerType;
            public string Context;

            public enum TriggerTypes
            {
                GetInCar,
                ExitCar,
                InCar,
                Stopping,
                Stopped
            }
        }
    }
}
