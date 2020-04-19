using SystemBase;

namespace Assets.Systems.TargetKeys
{
    public class TargetKeyAnimationComponent : GameComponent
    {
        public int Id;
        public string Key;
        public float PressTime;
        public float AnimationPerSecond { get; set; }
        public float AnimationDuration { get; set; }
        public float AnimationStartTime { get; set; }
    }
}