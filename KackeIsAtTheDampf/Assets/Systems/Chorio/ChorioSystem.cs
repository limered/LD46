using System;
using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using SystemBase;
using Assets.Systems.Chorio.Generator;
using Assets.Systems.Key;
using Assets.Systems.Score;
using UniRx;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Assets.Systems.Chorio
{
    [GameSystem(typeof(BeatSystem))]
    public class ChorioSystem : GameSystem<BeatSystemConfig, KeyInfoComponent, ScoreComponent>
    {
        private readonly ReactiveProperty<KeyInfoComponent> _keyInfoComponent = new ReactiveProperty<KeyInfoComponent>();

        private IChorioGenerator _currentGenerator;

        public override void Register(BeatSystemConfig component)
        {
            component.WaitOn(_keyInfoComponent).Subscribe(infoComponent =>
                {
                    _currentGenerator = new FailingGenerator();

                    component.BeatTrigger
                        .Subscribe(beatInfo => OnBeat(beatInfo, component.TimePerBeat))
                        .AddTo(component);
                })
                .AddTo(component);
        }

        public override void Register(KeyInfoComponent component)
        {
            _keyInfoComponent.Value = component;
        }

        public override void Register(ScoreComponent component)
        {
            //return;
            component.HyperLevel.Subscribe(level =>
            {
                switch (level)
                {
                    case HyperLevel.Failing:
                        _currentGenerator = new FailingGenerator();
                        break;
                    case HyperLevel.Shitty:
                        _currentGenerator = new ShittyGenerator();
                        break;
                    case HyperLevel.Normal:
                        _currentGenerator = new NormalGenerator();
                        break;
                    case HyperLevel.Cool:
                        _currentGenerator = new CoolGenerator();
                        break;
                    case HyperLevel.Hot:
                        _currentGenerator = new CoolGenerator();
                        break;
                }
            }).AddTo(component);
        }

        private void OnBeat(BeatInfo beatInfo, float timePerBeat)
        {
            EvtNextBeatKeyAdded[] keys = _currentGenerator
                .GenerateTargetsForBeat(beatInfo, timePerBeat, _keyInfoComponent.Value);

            foreach (var beatKeyAdded in keys)
            {
                MessageBroker.Default.Publish(beatKeyAdded);
            }
        }
    }
}
