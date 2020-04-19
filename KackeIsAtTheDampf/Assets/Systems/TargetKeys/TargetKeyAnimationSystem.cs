using System;
using SystemBase;
using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using UniRx;
using UnityEngine.UI;

namespace Assets.Systems.TargetKeys
{
    public class TargetKeyAnimationSystem : GameSystem<BeatSystemConfig, TargetKeyAnimationConfig, TargetKeyAnimationComponent>
    {
        private ReactiveProperty<BeatSystemConfig> _beatSystemConfig = new ReactiveProperty<BeatSystemConfig>();

        public override void Register(TargetKeyAnimationConfig component)
        {
            component.WaitOn(_beatSystemConfig, config => Register(component, config)).AddTo(component);
        }

        private void Register(TargetKeyAnimationConfig config, BeatSystemConfig beatSystemConfig)
        {
            MessageBroker.Default.Receive<EvtNextBeatKeyAdded>()
                .Subscribe(OnNewTargetKeyAdded)
                .AddTo(config);
        }

        public override void Register(TargetKeyAnimationComponent component)
        {
            throw new NotImplementedException();
        }

        private void OnNewTargetKeyAdded(EvtNextBeatKeyAdded obj)
        {
            throw new NotImplementedException();
        }

        public override void Register(BeatSystemConfig component)
        {
            _beatSystemConfig.Value = component;
        }
    }
}
