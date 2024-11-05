
/// <summary>
/// this is the componenent of the UI that controls the dialogue in game
/// ye
/// </summary>
/// 
[System.Serializable]
public class beatProcessing
{
    [System.Serializable]
    public class beat
    {
        public float beatMoment;
        public float beatTime;
        public float beatSpeed;
        public bool side;
    }

    [System.Serializable]
    public class Beats
    {
        public beat[] Start;
    }


}

