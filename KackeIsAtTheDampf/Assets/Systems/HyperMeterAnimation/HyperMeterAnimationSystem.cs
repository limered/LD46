using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemBase;
using Assets.Systems.Score;
using GameState.States;
using UniRx;
using UnityEngine;
using Utils;

namespace Assets.Systems.HyperMeterAnimation
{
    [GameSystem(typeof(ScoreSystem))]
    public class HyperMeterAnimationSystem : GameSystem<HyperMeterComponent, ScoreComponent>
    {
        private readonly ReactiveProperty<HyperMeterComponent> _hyperMeterComponent = new ReactiveProperty<HyperMeterComponent>();

        public override void Register(HyperMeterComponent component)
        {
            _hyperMeterComponent.Value = component;

            IoC.Game.GameStateContext.CurrentState
                .Where(state => state is GameOver)
                .Subscribe(_ => _hyperMeterComponent.Value = null)
                .AddTo(component);
        }

        public override void Register(ScoreComponent component)
        {
            component.WaitOn(_hyperMeterComponent, meterComponent => Register(component, meterComponent))
                .AddTo(component);
        }

        private void Register(ScoreComponent component, HyperMeterComponent hyperMeterComponent)
        {
            component.HyperScore.Subscribe(score => CalculateTargetValue(score, hyperMeterComponent))
                .AddTo(hyperMeterComponent);

            SystemFixedUpdate(hyperMeterComponent)
                .Subscribe(AnimateHyperArrow)
                .AddTo(hyperMeterComponent);
        }

        private void AnimateHyperArrow(HyperMeterComponent component)
        {
            var transform = component.GetComponent<RectTransform>();
            var currentAngle = transform.eulerAngles.z;
            var newZ = component.TargetAngle * 0.1f + currentAngle * 0.9f;
            transform.eulerAngles = new Vector3(0,0, newZ);

        }

        private void CalculateTargetValue(float score, HyperMeterComponent hyperComponent)
        {
            hyperComponent.TargetAngle = 180 - (score / 100 * 180);
        }
    }
}
