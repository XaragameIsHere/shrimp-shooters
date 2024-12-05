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
        public string type;
        public float beatMoment;
        public float beatSpeed;
        public string side;
        public float holdTime;
        public float pressTime;
        public float beatTime;
        public string message;
    }
    
    [System.Serializable]
    public class Beats
    {
        public beat[] Start;
    }


}


