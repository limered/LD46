using Assets.Systems.Beat;
using Assets.Systems.Floor.Actions;
using SystemBase;
using GameState.States;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Assets.Systems.Lights
{
    [GameSystem(typeof(BeatSystem))]
    public class BlinkSystem : GameSystem<BeatSystemConfig, BlinkComponent, SpotLightComponent>
    {
        private ReactiveProperty<BeatSystemConfig> _beatSystemConfig = new ReactiveProperty<BeatSystemConfig>(null);

        public override void Register(BlinkComponent component)
        {
            component.BlinkColor = component.BaseColor;
            component.Blink();
        }

        public override void Register(BeatSystemConfig component)
        {
            _beatSystemConfig.Value = component;

            IoC.Game.GameStateContext.CurrentState.Where(state => state is GameOver)
                .Subscribe(_ => _beatSystemConfig.Value = null).AddTo(component);
        }

        public override void Register(SpotLightComponent component)
        {
            component.WaitOn(_beatSystemConfig, config =>
            {
                _beatSystemConfig.Value.BeatTrigger
                    .Where(info => info.BeatNo % 2 == 0)
                    .Subscribe(info => { component.TargetHue = ((component.TargetHue * 360 + 255) % 360) / 360; })
                    .AddTo(component);

            }).AddTo(component);

            SystemUpdate(component).Subscribe(lightComponent =>
            {
                var image = component.GetComponent<Image>();
                Color.RGBToHSV(image.color, out var h, out var s, out var v);
                h = Mathf.Lerp(h, component.TargetHue, 0.03f);
                var newColor = Color.HSVToRGB(h, s, v);
                image.color = new Color(newColor.r, newColor.g, newColor.b, 0.3f);

            }).AddTo(component);
        }
    }
}
