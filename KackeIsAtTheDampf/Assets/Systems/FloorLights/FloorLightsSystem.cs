using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemBase;
using Assets.Systems.Beat;
using UniRx;

namespace Assets.Systems.FloorLights
{
    [GameSystem(typeof(BeatSystem))]
    public class FloorLightsSystem : GameSystem<BeatSystemConfig,FloorLight>
    {
        private readonly ReactiveProperty<BeatSystemConfig> _beatSystemConfig = new ReactiveProperty<BeatSystemConfig>(null);

        public override void Register(FloorLight component)
        {
            component.WaitOn(_beatSystemConfig, config => OnBeatSystemAdded(config, component));
        }

        private void OnBeatSystemAdded(BeatSystemConfig config, FloorLight light)
        {
            config.BeatTrigger.Subscribe(_ => OnBeat(light));
        }

        private void OnBeat(FloorLight light)
        {
            light.Blink();
        }

        public override void Register(BeatSystemConfig component)
        {
            _beatSystemConfig.Value = component;
        }
    }
}
