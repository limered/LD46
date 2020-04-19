using SystemBase;
using UniRx;

namespace Assets.Systems.Beat
{
    public class BeatSystemConfig : GameComponent
    {
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