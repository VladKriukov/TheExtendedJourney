using UnityEngine;

public class PossibleChunks : MonoBehaviour
{
    // the possible terrains from this chunk that the next spawner can spawn
    public GameObject[] leftItems;
    public GameObject[] rightItems;
    public GameObject[] forewardItems;
    public GameObject[] backwardItems;
    // based on the type of terrain given above, this might be necessary to be a -1 or a +1 if it is a slope
    // this is to make sure the next terrain spawns at the right altitude
    public int[] requiredLeftAltitudeSteps;
    public int[] requiredRightAltitudeSteps;
    public int[] requiredForewardAltitudeSteps;
    public int[] requiredBackwardAltitudeSteps;

    public int leftStartingStep;
    public int rightStartingStep;
    // the slope starting types to connect to the rails
    public enum LeftStartingSlopeType
    {
        Straight, Up, Down, NA
    }
    public LeftStartingSlopeType leftSlopeMatch;
    public enum RightStartingSlopeType
    {
        Straight, Up, Down, NA
    }
    public RightStartingSlopeType rightSlopeMatch;
}