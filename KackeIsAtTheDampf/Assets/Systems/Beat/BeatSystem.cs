using System;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Assets.Systems.Beat
{
    [GameSystem]
    public class BeatSystem : GameSystem<BeatSystemConfig>
    {
        private IDisposable _timerDisposable;
        private int _beatNo;

        public override void Register(BeatSystemConfig component)
        {
            component.BPM.Subscribe(bpm => BPMChanged(bpm, component));
        }

        private void BPMChanged(float bpm, BeatSystemConfig config)
        {
            config.TimePerBeat = 60f / bpm;
            _timerDisposable?.Dispose();
            _timerDisposable = Observable
                .Interval(TimeSpan.FromSeconds(config.TimePerBeat))
                .Subscribe(_ => OnBeat(config))
                .AddTo(config);
        }

        private void OnBeat(BeatSystemConfig cofig)
        {
            cofig.BeatTrigger.Value = new BeatInfo
            {
                BeatNo = _beatNo++,
                BeatTime = Time.realtimeSinceStartup
            };
        }
    }
}
