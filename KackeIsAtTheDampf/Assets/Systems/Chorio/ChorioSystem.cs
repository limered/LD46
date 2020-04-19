using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using SystemBase;
using Assets.Systems.Key;
using UniRx;
using UnityEngine;

namespace Assets.Systems.Chorio
{
    [GameSystem(typeof(BeatSystem))]
    public class ChorioSystem : GameSystem<BeatSystemConfig, KeyInfoComponent>
    {
        private readonly ReactiveProperty<KeyInfoComponent> _keyInfoComponent = new ReactiveProperty<KeyInfoComponent>();

        public override void Register(BeatSystemConfig component)
        {
            component.WaitOn(_keyInfoComponent).Subscribe(infoComponent =>
                {
                    component.BeatTrigger
                        .Subscribe(beatInfo => OnBeat(beatInfo, component.TimePerBeat))
                        .AddTo(component);
                })
                .AddTo(component);
        }

        private void OnBeat(BeatInfo beatInfo, float timePerBeat)
        {
            if (beatInfo.BeatNo % 4 != 0) return;

            MessageBroker.Default.Publish(new EvtNextBeatKeyAdded
            {
                Id = Time.frameCount,
                Key = _keyInfoComponent.Value.RelevantKeys[(int)(Random.value * _keyInfoComponent.Value.RelevantKeys.Length)],
                PlannedBeatTime = beatInfo.BeatTime + timePerBeat * 10,
                BeatNo = beatInfo.BeatNo + 10
            });
        }

        public override void Register(KeyInfoComponent component)
        {
            _keyInfoComponent.Value = component;
        }
    }
}
