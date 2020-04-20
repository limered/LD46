using Assets.Systems.Beat;
using Assets.Systems.Score;
using System;
using SystemBase;
using Assets.Systems.Floor.Actions;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Systems.FloorAnimation
{
    [GameSystem(typeof(BeatSystem), typeof(ScoreSystem))]
    public class FloorAnimationSystem : GameSystem<BeatSystemConfig, ScoreComponent>
    {
        private readonly ReactiveProperty<BeatSystemConfig> _beatSystemConfig = new ReactiveProperty<BeatSystemConfig>();

        private IAnimator _animator;

        public override void Register(BeatSystemConfig component)
        {
            _beatSystemConfig.Value = component;
        }

        public override void Register(ScoreComponent component)
        {
            component.WaitOn(_beatSystemConfig, beat => Register(component, beat))
                .AddTo(component);
        }

        private void Register(ScoreComponent score, BeatSystemConfig beat)
        {
            _animator = new ShittyAnimator();
            beat.BeatTrigger.Subscribe(beatInfo => _animator.Animate(score, beatInfo)).AddTo(beat);
        }
    }

    internal interface IAnimator
    {
        void Animate(ScoreComponent score, BeatInfo beat);
    }

    public class ShittyAnimator : IAnimator
    {
        public void Animate(ScoreComponent score, BeatInfo beat)
        {
            var hue = Random.value;
            MessageBroker.Default.Publish(new ActLightUp
            {
                X = Random.Range(0, 7),
                Y = Random.Range(0, 7),
                FallOff = 1.5f,
                Delay = 0,
                BlinkColor = Color.HSVToRGB(hue, 1, 1),
            });
            
        }
    }
}
