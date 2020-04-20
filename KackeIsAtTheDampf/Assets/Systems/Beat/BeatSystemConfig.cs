using SystemBase;
using UniRx;
using UnityEngine;

namespace Assets.Systems.Beat
{
    public class BeatSystemConfig : GameComponent
    {
        public float YellowCheckDuration = 0.6f;
        public float GreenCheckDuration = 0.2f;

        public FloatReactiveProperty BPM = new FloatReactiveProperty(129);
        public float TimePerBeat;
        public ReactiveProperty<BeatInfo> BeatTrigger = new ReactiveProperty<BeatInfo>();

        public AudioSource Music;
        public FloatReactiveProperty GameEndTimestamp = new FloatReactiveProperty();
    }

    public struct BeatInfo
    {
        public int BeatNo { get; set; }
        public float BeatTime { get; set; }
    }
}