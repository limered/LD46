using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using SystemBase;
using Assets.Systems.Chorio.Generator;
using Assets.Systems.Key;
using UniRx;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Assets.Systems.Chorio
{
    [GameSystem(typeof(BeatSystem))]
    public class ChorioSystem : GameSystem<BeatSystemConfig, KeyInfoComponent>
    {
        private readonly ReactiveProperty<KeyInfoComponent> _keyInfoComponent = new ReactiveProperty<KeyInfoComponent>();

        private IChorioGenerator _currentGenerator;

        public override void Register(BeatSystemConfig component)
        {
            component.WaitOn(_keyInfoComponent).Subscribe(infoComponent =>
                {
                    _currentGenerator = new NormalGenerator();

                    component.BeatTrigger
                        .Subscribe(beatInfo => OnBeat(beatInfo, component.TimePerBeat))
                        .AddTo(component);
                })
                .AddTo(component);
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

        public override void Register(KeyInfoComponent component)
        {
            _keyInfoComponent.Value = component;
        }
    }
}
