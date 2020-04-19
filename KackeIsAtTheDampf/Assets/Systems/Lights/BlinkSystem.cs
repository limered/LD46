using Assets.Systems.Beat;
using Assets.Systems.Floor.Actions;
using SystemBase;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Systems.Lights
{
    [GameSystem(typeof(BeatSystem))]
    public class BlinkSystem : GameSystem<BeatSystemConfig, BlinkComponent>
    {
        private readonly ReactiveProperty<BeatSystemConfig> _beatSystemConfig = new ReactiveProperty<BeatSystemConfig>(null);

        public override void Register(BlinkComponent component)
        {
            component.BlinkColor = component.BaseColor;
            component.Blink();
        }

        public override void Register(BeatSystemConfig component)
        {
            component.BeatTrigger.Subscribe(b =>
                {
                    MessageBroker.Default.Publish(new ActLightUp
                    {
                        X = Random.Range(0, 7),
                        Y = Random.Range(0, 7),
                        FallOff = 0.4f,
                        Delay = 0,
                        BlinkColor = Color.red
                    });
                }
            );
        }
    }
}
