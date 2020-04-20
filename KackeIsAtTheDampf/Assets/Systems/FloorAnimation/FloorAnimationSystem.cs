using Assets.Systems.Beat;
using Assets.Systems.Score;
using System;
using SystemBase;
using Assets.Systems.Floor.Actions;
using Boo.Lang;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Systems.FloorAnimation
{
    [GameSystem(typeof(BeatSystem), typeof(ScoreSystem))]
    public class FloorAnimationSystem : GameSystem<BeatSystemConfig, ScoreComponent>
    {
        private readonly ReactiveProperty<BeatSystemConfig> _beatSystemConfig = new ReactiveProperty<BeatSystemConfig>();

        private IAnimator _animator = new ShittyAnimator();

        public override void Register(BeatSystemConfig component)
        {
            _beatSystemConfig.Value = component;
        }

        public override void Register(ScoreComponent component)
        {
            component.WaitOn(_beatSystemConfig, beat => Register(component, beat))
                .AddTo(component);

            component.HyperLevel.Subscribe(ChangeAnimator).AddTo(component);
        }

        private void ChangeAnimator(HyperLevel obj)
        {
            switch (obj)
            {
                case HyperLevel.Failing:
                    _animator = new ShittyAnimator();
                    break;
                case HyperLevel.Shitty:
                    _animator = new ShittyAnimator();
                    break;
                case HyperLevel.Normal:
                    _animator = new NormalAnimator();
                    break;
                case HyperLevel.Cool:
                    _animator = new CoolAnimator();
                    break;
                case HyperLevel.Hot:
                    _animator = new HotAnimator();
                    break;
            }
        }

        private void Register(ScoreComponent score, BeatSystemConfig beat)
        {
            beat.BeatTrigger.Subscribe(beatInfo => _animator.Animate(score, beatInfo)).AddTo(beat);
        }
    }

    internal interface IAnimator
    {
        void Animate(ScoreComponent score, BeatInfo beat);
    }

    public class HotAnimator : IAnimator
    {
        private int _currentAnimation;
        private const int _animationCount = 5;

        private readonly Action<float>[] _animations = new Action<float>[_animationCount];

        public HotAnimator()
        {
            _animations[0] = (hue) =>
            {
                for (var i = 0; i < 49; i++)
                {
                    var x = i / 7;
                    var y = i % 7;
                    MessageBroker.Default.Publish(new ActLightUp
                    {
                        X = x,
                        Y = y,
                        FallOff = 1f,
                        Delay = i/49f,
                        BlinkColor = Color.HSVToRGB(hue, 1, 1),
                    });
                }
            };
            _animations[1] = (hue) =>
            {
                for (var i = 0; i < 49; i++)
                {
                    var x = i % 7;
                    var y = i / 7;
                    MessageBroker.Default.Publish(new ActLightUp
                    {
                        X = x,
                        Y = y,
                        FallOff = 1f,
                        Delay = i / 49f,
                        BlinkColor = Color.HSVToRGB(hue, 1, 1),
                    });
                }
            };
            _animations[2] = (hue) =>
            {
                for (var i = 0; i < 49; i++)
                {
                    var x = i / 7;
                    var y = i % 7;
                    MessageBroker.Default.Publish(new ActLightUp
                    {
                        X = x,
                        Y = y,
                        FallOff = 1f,
                        Delay = 1- (i / 49f),
                        BlinkColor = Color.HSVToRGB(hue, 1, 1),
                    });
                }
            };
            _animations[3] = (hue) =>
            {
                for (var i = 0; i < 49; i++)
                {
                    var x = i % 7;
                    var y = i / 7;
                    MessageBroker.Default.Publish(new ActLightUp
                    {
                        X = x,
                        Y = y,
                        FallOff = 1f,
                        Delay = 1- (i / 49f),
                        BlinkColor = Color.HSVToRGB(hue, 1, 1),
                    });
                }
            };
            _animations[4] = (hue) =>
            {
                for (var i = 0; i < 49; i++)
                {
                    var x = i % 7;
                    var y = i / 7;
                    MessageBroker.Default.Publish(new ActLightUp
                    {
                        X = x,
                        Y = y,
                        FallOff = 0.3f,
                        Delay = 0,
                        BlinkColor = Color.HSVToRGB(hue, 1, 1),
                    });
                    MessageBroker.Default.Publish(new ActLightUp
                    {
                        X = x,
                        Y = y,
                        FallOff = 0.3f,
                        Delay = 0.3f,
                        BlinkColor = Color.HSVToRGB(hue, 1, 1),
                    });
                    MessageBroker.Default.Publish(new ActLightUp
                    {
                        X = x,
                        Y = y,
                        FallOff = 1f,
                        Delay = 0.6f,
                        BlinkColor = Color.HSVToRGB(hue, 1, 1),
                    });
                }
            };
        }

        public void Animate(ScoreComponent score, BeatInfo beat)
        {
            if (beat.BeatNo % 2 != 0) return;
            var hue = Random.value;
            _currentAnimation = (_currentAnimation + 1) % _animationCount;
            _animations[_currentAnimation](hue);
        }
    }

    public class CoolAnimator : IAnimator
    {
        public void Animate(ScoreComponent score, BeatInfo beat)
        {
            if (beat.BeatNo % 2 != 0) return;
            var hue = Random.value;
            
            MessageBroker.Default.Publish(new ActLightUp
            {
                X = 3,
                Y = 3,
                FallOff = 1f,
                Delay = 0,
                BlinkColor = Color.HSVToRGB(hue, 1, 1),
            });

            var arr = new List<Vector2Int> { new Vector2Int(3, 3) };
            for (int i = 2; i < 5; i++)
            {
                for (int j = 2; j < 5; j++)
                {
                    if (arr.Contains(new Vector2Int(i, j)))continue;
                    MessageBroker.Default.Publish(new ActLightUp
                    {
                        X = i,
                        Y = j,
                        FallOff = 1f,
                        Delay = 0.2f,
                        BlinkColor = Color.HSVToRGB(hue, 1, 1),
                    });
                    arr.Add(new Vector2Int(i, j));
                }
            }

            for (int i = 1; i < 6; i++)
            {
                for (int j = 1; j < 6; j++)
                {
                    if (arr.Contains(new Vector2Int(i, j))) continue;
                    MessageBroker.Default.Publish(new ActLightUp
                    {
                        X = i,
                        Y = j,
                        FallOff = 1f,
                        Delay = 0.4f,
                        BlinkColor = Color.HSVToRGB(hue, 1, 1),
                    });
                    arr.Add(new Vector2Int(i, j));
                }
            }

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (arr.Contains(new Vector2Int(i, j))) continue;
                    MessageBroker.Default.Publish(new ActLightUp
                    {
                        X = i,
                        Y = j,
                        FallOff = 1f,
                        Delay = 0.6f,
                        BlinkColor = Color.HSVToRGB(hue, 1, 1),
                    });
                    arr.Add(new Vector2Int(i, j));
                }
            }
        }
    }

    public class NormalAnimator : IAnimator
    {
        public void Animate(ScoreComponent score, BeatInfo beat)
        {
            if (beat.BeatNo % 2 != 0) return;
            var hue = Random.value;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if(j % 2 == 0 && i % 2 == 0) continue;
                    if(j % 2 == 1 && i % 2 == 1) continue;
                    MessageBroker.Default.Publish(new ActLightUp
                    {
                        X = i,
                        Y = j,
                        FallOff = 1f,
                        Delay = 0,
                        BlinkColor = Color.HSVToRGB(hue, 1, 1),
                    });
                }
            }
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (j % 2 == 0 && i % 2 == 1) continue;
                    if (j % 2 == 1 && i % 2 == 0) continue;
                    MessageBroker.Default.Publish(new ActLightUp
                    {
                        X = i,
                        Y = j,
                        FallOff = 1f,
                        Delay = 0.5f,
                        BlinkColor = Color.HSVToRGB(hue, 1, 1),
                    });
                }
            }
        }
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
