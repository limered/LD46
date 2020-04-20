using GameState.States;
using System;
using SystemBase;
using UniRx;
using UnityEngine;
using Utils;

namespace Assets.Systems.Beat
{
    [GameSystem]
    public class BeatSystem : GameSystem<BeatSystemConfig>
    {
        private IDisposable _timerDisposable;
        private int _beatNo;

        public override void Register(BeatSystemConfig component)
        {
            IoC.Game.GameStateContext.AfterStateChange.Where(state => state is Running)
                .Subscribe(_ => StartGame(component))
                .AddTo(component);

            IoC.Game.GameStateContext.CurrentState.Where(state => state is GameOver)
                .Subscribe(_ => EndGame())
                .AddTo(component);

            MessageBroker.Default.Receive<ActStopTheBeat>()
                .Subscribe(_ => EndGame())
                .AddTo(component);
        }

        private void EndGame()
        {
            _timerDisposable?.Dispose();
        }

        private void StartGame(BeatSystemConfig config)
        {
            config.TimePerBeat = 60f / config.BPM.Value;
            _beatNo = 0;
            _timerDisposable = Observable
                .Interval(TimeSpan.FromSeconds(config.TimePerBeat))
                .Subscribe(_ => OnBeat(config))
                .AddTo(config);
        }

        private void OnBeat(BeatSystemConfig cofig)
        {
            if (_beatNo == 0)
            {
                cofig.Music.Play();
                cofig.GameEndTimestamp = Time.realtimeSinceStartup + 20; //cofig.Music.clip.length;
            }

            cofig.BeatTrigger.Value = new BeatInfo
            {
                BeatNo = _beatNo++,
                BeatTime = Time.realtimeSinceStartup
            };
        }
    }
}
