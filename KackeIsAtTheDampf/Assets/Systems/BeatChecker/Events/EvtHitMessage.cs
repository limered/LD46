namespace Assets.Systems.BeatChecker.Events
{
    public class EvtHitMessage
    {
        public float TimeStamp;
        public BeatKeyState State;
        public float DistanceToOptimum;
        public int Id { get; set; }
    }
}
