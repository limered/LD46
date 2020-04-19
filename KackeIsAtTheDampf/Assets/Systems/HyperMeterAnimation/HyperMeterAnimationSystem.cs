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
    [GameSystem(typeof(ScoreSystem))]
    public class HyperMeterAnimationSystem : GameSystem<HyperMeterComponent, ScoreComponent>
    {
        private readonly ReactiveProperty<HyperMeterComponent> _hyperMeterComponent = new ReactiveProperty<HyperMeterComponent>();

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
            SystemFixedUpdate(hyperMeterComponent)
                .Subscribe(hyper => AnimateHyperArrow(hyper, component))
                .AddTo(hyperMeterComponent);
        }

        private void AnimateHyperArrow(HyperMeterComponent component, ScoreComponent scoreComponent)
        {
            var targetAngle = 180 - (scoreComponent.HyperScore.Value / 100 * 180);

            var transform = component.GetComponent<RectTransform>();
            var currentAngle = transform.eulerAngles.z;
            var newZ = targetAngle * 0.1f + currentAngle * 0.9f;
            transform.eulerAngles = new Vector3(0,0, newZ);

        }

        private void CalculateTargetValue(float score, HyperMeterComponent hyperComponent)
        {
            Debug.Log(hyperComponent.TargetAngle);
            hyperComponent.TargetAngle = -180 + (score / 100 * 180);
        }
    }
}
