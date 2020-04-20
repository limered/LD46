using Assets.Systems.Score;
using System;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Assets.Systems.Dancer
{
    [GameSystem]
    public class DancerSystem : GameSystem<DancerComponent, ScoreComponent>
    {
        private readonly ReactiveProperty<ScoreComponent> _score = new ReactiveProperty<ScoreComponent>();

        public override void Register(DancerComponent component)
        {
            component.WaitOn(_score, score => Register(score, component)).AddTo(component);
        }

        private void Register(ScoreComponent score, DancerComponent dancerComponent)
        {
            dancerComponent.TargetPosition = dancerComponent.StartPosition;

            score.HyperLevel
                .Subscribe(hypeLevel => OnHypeLevelChange(hypeLevel, dancerComponent))
                .AddTo(dancerComponent);

            SystemUpdate(dancerComponent).Subscribe(AnimateDancer).AddTo(dancerComponent);
        }

        private void AnimateDancer(DancerComponent dancer)
        {
            dancer.transform.position = Vector3.Lerp(dancer.transform.position, dancer.TargetPosition, 0.05f);
        }

        private void OnHypeLevelChange(HyperLevel hypeLevel, DancerComponent dancerComponent)
        {
            switch (dancerComponent.Type)
            {
                case DanceType.Shitty when hypeLevel == HyperLevel.Shitty:
                    dancerComponent.TargetPosition = dancerComponent.DancePosition;
                    break;
                case DanceType.Shitty when hypeLevel != HyperLevel.Shitty:
                    dancerComponent.TargetPosition = dancerComponent.StartPosition;
                    break;
                case DanceType.Normal when hypeLevel == HyperLevel.Normal:
                    dancerComponent.TargetPosition = dancerComponent.DancePosition;
                    break;
                case DanceType.Normal when hypeLevel != HyperLevel.Normal:
                    dancerComponent.TargetPosition = dancerComponent.StartPosition;
                    break;
                case DanceType.Cool when hypeLevel == HyperLevel.Cool || hypeLevel == HyperLevel.Hot:
                    dancerComponent.TargetPosition = dancerComponent.DancePosition;
                    break;
                case DanceType.Cool when hypeLevel != HyperLevel.Cool && hypeLevel != HyperLevel.Hot:
                    dancerComponent.TargetPosition = dancerComponent.StartPosition;
                    break;
                case DanceType.Hot when hypeLevel == HyperLevel.Hot:
                    dancerComponent.TargetPosition = dancerComponent.DancePosition;
                    break;
                case DanceType.Hot when hypeLevel != HyperLevel.Hot:
                    dancerComponent.TargetPosition = dancerComponent.StartPosition;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Register(ScoreComponent component)
        {
            _score.Value = component;
        }
    }
}
