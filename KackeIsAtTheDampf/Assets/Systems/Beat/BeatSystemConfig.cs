using SystemBase;
using UniRx;

namespace Assets.Systems.Beat
{
    public class BeatSystemConfig : GameComponent
    {
        public float YellowCheckDuration = 0.6f;
        public float GreenCheckDuration = 0.2f;

        public FloatReactiveProperty BPM = new FloatReactiveProperty(118);
        public float TimePerBeat { get; set; }
        public ReactiveProperty<BeatInfo> BeatTrigger = new ReactiveProperty<BeatInfo>();
    }

    public struct BeatInfo
    {
        public int BeatNo { get; set; }
        public float BeatTime { get; set; }
    }
}