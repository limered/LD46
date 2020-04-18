using SystemBase;
using Assets.Systems.Beat;
using UniRx;

namespace Assets.Systems.Lights
{
    [GameSystem(typeof(BeatSystem))]
    public class BeatBlinkSystem : GameSystem<BeatSystemConfig,BeatBlinkComponent>
    {
        private readonly ReactiveProperty<BeatSystemConfig> _beatSystemConfig = new ReactiveProperty<BeatSystemConfig>(null);

        public override void Register(BeatBlinkComponent component)
        {
            component.WaitOn(_beatSystemConfig, config => OnBeatSystemAdded(config, component));
        }

        private void OnBeatSystemAdded(BeatSystemConfig config, BeatBlinkComponent light)
        {
            config.BeatTrigger.Subscribe(_ => OnBeat(light));
        }

        private void OnBeat(BeatBlinkComponent light)
        {
            light.Blink();
        }

        public override void Register(BeatSystemConfig component)
        {
            _beatSystemConfig.Value = component;
        }
    }
}
