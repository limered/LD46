using SystemBase;
using UniRx;

namespace Assets.Systems.Beat
{
    public class BeatSystemConfig : GameComponent
    {
        public FloatReactiveProperty BPM = new FloatReactiveProperty(120);

        public ReactiveCommand BeatTrigger = new ReactiveCommand();
    }
}