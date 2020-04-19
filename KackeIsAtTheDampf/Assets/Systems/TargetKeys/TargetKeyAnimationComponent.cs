using SystemBase;

namespace Assets.Systems.TargetKeys
{
    public class TargetKeyAnimationComponent : GameComponent
    {
        public string Key;
        public float TimeLeft;
        public float PressTime;
        public float AnimationPerSecond { get; set; }
        public float AnimationDuration { get; set; }
        public float AnimationStartTime { get; set; }
    }
}