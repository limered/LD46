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

        public override void Register(BeatSystemConfig component)
        {
            component.BPM.Subscribe(bpm => BPMChanged(bpm, component));
        }

        private void BPMChanged(float bpm, BeatSystemConfig config)
        {
            var timePerBeat = 60f / bpm;
            _timerDisposable?.Dispose();
            _timerDisposable = Observable
                .Interval(TimeSpan.FromSeconds(timePerBeat))
                .Subscribe(_ => OnBeat(config));
        }

        private void OnBeat(BeatSystemConfig cofig)
        {
            cofig.BeatTrigger.ForceExecute();
        }
    }
}
