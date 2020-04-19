using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemBase;
using Assets.Systems.Score;
using UniRx;
using UnityEngine;

namespace Assets.Systems.HyperMeterAnimation
{
    public class HyperMeterAnimationSystem : GameSystem<HyperMeterComponent, ScoreComponent>
    {
        private ReactiveProperty<HyperMeterComponent> _hyperMeterComponent = new ReactiveProperty<HyperMeterComponent>();

        public override void Register(HyperMeterComponent component)
        {
            _hyperMeterComponent.Value = component;
        }

        public override void Register(ScoreComponent component)
        {
            component.WaitOn(_hyperMeterComponent, meterComponent => Register(component, meterComponent))
                .AddTo(component);
        }

        private void Register(ScoreComponent component, HyperMeterComponent hyperMeterComponent)
        {
            component.HyperScore
                .Subscribe(score => CalculateTargetValue(score, hyperMeterComponent))
                .AddTo(component);

            SystemFixedUpdate(hyperMeterComponent)
                .Subscribe(AnimateHyperArrow)
                .AddTo(hyperMeterComponent);
        }

        private void AnimateHyperArrow(HyperMeterComponent component)
        {
            var transform = component.GetComponent<RectTransform>();
            var currentAngle = transform.eulerAngles.z;

        }

        private void CalculateTargetValue(float score, HyperMeterComponent hyperComponent)
        {
            hyperComponent.TargetAngle = -180 + (score / 100 * 180);
        }
    }

    public class HyperMeterComponent : GameComponent
    {
        public float TargetAngle;
    }
}
