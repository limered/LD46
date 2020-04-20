using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemBase;
using Assets.Systems.Beat;
using UniRx;
using UnityEngine;

namespace Assets.Systems.Camera
{
    [GameSystem(typeof(BeatSystem))]
    public class CameraColorSystem : GameSystem<CameraColorComponent, BeatSystemConfig>
    {
        private readonly ReactiveProperty<BeatSystemConfig> _beatSystemObserver = new ReactiveProperty<BeatSystemConfig>();

        public override void Register(CameraColorComponent component)
        {
            component.WaitOn(_beatSystemObserver, beats => Register(component, beats))
                .AddTo(component);
        }

        private void Register(CameraColorComponent component, BeatSystemConfig beats)
        {
            component.CameraComponent = component.GetComponent<UnityEngine.Camera>();
            beats.BeatTrigger.Subscribe(beatInfo => OnBeat(beatInfo, component)).AddTo(component);
            SystemUpdate(component).Subscribe(FadeToTargetColor).AddTo(component);
        }

        private void FadeToTargetColor(CameraColorComponent obj)
        {
            Color.RGBToHSV(obj.CameraComponent.backgroundColor, out var currH, out var currS, out var currV);
            Color.RGBToHSV(obj.TargetColor, out var tarH, out var tarS, out var tarV);
            const float t = 0.03f;
            var h = tarH * t + currH * (1 - t);
            obj.CameraComponent.backgroundColor = Color.HSVToRGB(h, currS, currV);
        }

        private void OnBeat(BeatInfo beatInfo, CameraColorComponent component)
        {
            if (beatInfo.BeatNo % 2 != 0) return;

            Color.RGBToHSV(component.TargetColor, out var h, out var s, out var v);
            h *= 360f;
            h = (h + 481f) % 360f;
            h /= 360f;
            component.TargetColor = Color.HSVToRGB(h, 0.75f, 0.75f);
        }

        public override void Register(BeatSystemConfig component)
        {
            _beatSystemObserver.Value = component;
        }
    }
}
