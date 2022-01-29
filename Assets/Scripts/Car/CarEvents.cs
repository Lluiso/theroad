using System;

public static class CarEvents 
{
    public static class Passenger
    {
        // Enter/Exit
        public static Action<string> Entered;
        public static Action<string> Exited;
        public static Action<string> Rejected;

        // Slowing down to pick someone up
        public static Action<string> SlowingToPickUp;

        // stopped to pick up a hitchhiker, string = person to be picked up
        public static Action<string> StoppedAt;
    }

    // Check for dialogue between passengers/angel&Devil
    public static Action CheckForInCarDialogue;

    // Add a passenger to the car
    public static Action<string> AddPassenger;
    public static Action<string> RemovePassenger;
}
