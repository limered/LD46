using System;
using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;
using Assets.Systems.Score;
using Assets.Systems.BeatChecker;
using Assets.Systems.Beat;
using Assets.Systems.BeatChecker.Events;
using Utils.Math;

namespace Assets.Systems.Animation
{
    [GameSystem]
    public class AnimationSystem : GameSystem<BasicToggleAnimationComponent, JoeAnimationComponent, ScoreComponent, BeatSystemConfig, SpeakerComponent>
    {
        private ReactiveProperty<BeatSystemConfig> _beats = new ReactiveProperty<BeatSystemConfig>();
        private ReactiveProperty<ScoreComponent> _score = new ReactiveProperty<ScoreComponent>();

        public override void Register(BasicToggleAnimationComponent component)
        {
            component.FixedUpdateAsObservable()
                .Select(_ => component.CurrentSprite != BasicToggleAnimationComponent.NotAnimating)
                .DistinctUntilChanged()
                .SelectMany(animating => animating ? Observable.FromCoroutine(() => Animate(component)) : Observable.Empty<Unit>())
                .Subscribe()
                .AddTo(component);

            component.OnSpriteIndexWithoutAnimation
                .Subscribe(index =>
                {
                    for (var s = 0; s < component.Sprites.Length; s++)
                    {
                        component.Sprites[s].SetActive(s == index);
                    }
                })
                .AddTo(component);

            component.OnShowEndSprite
                .Subscribe(_ =>
                {
                    if (component.EndSprite)
                    {
                        for (var s = 0; s < component.Sprites.Length; s++)
                        {
                            component.Sprites[s].SetActive(false);
                        }
                        component.EndSprite.SetActive(true);
                    }
                })
                .AddTo(component);

            if (component.StartAnimationOnAwake) component.StartAnimation();
        }

        public override void Register(JoeAnimationComponent comp)
        {
            var animator = comp.GetComponent<Animator>();

            comp.WaitOn(_score, score =>
            {
                //##### Everytime the HYPE level changes #####
                score.HyperLevel
                    .Subscribe(level =>
                    {
                        comp.Dance.Value = (new Dictionary<HyperLevel, string> {
                            { HyperLevel.Failing, Joe.Dance.Idle },
                            { HyperLevel.Shitty, Joe.Dance.Idle },
                            { HyperLevel.Normal, Joe.Dance.Normal },
                            { HyperLevel.Cool, Joe.Dance.Cool },
                            { HyperLevel.Hot, Joe.Dance.Cool },
                        })[score.HyperLevel.Value];
                    })
                    .AddTo(comp);

                //##### Every Key Hit #####
                MessageBroker.Default.Receive<EvtHitMessage>()
                    .Subscribe(key =>
                    {
                        var isHype = score.HyperLevel.Value == HyperLevel.Hot;

                        comp.Pose.Value = (new Dictionary<BeatKeyState, string[]> {
                            //=== Wrong key or too late ===
                            { BeatKeyState.Red, new [] { Joe.Poses.HorrorStand, Joe.Poses.HorrorSit } },
                            //=== key timing still OK ===
                            { BeatKeyState.Yellow, new [] {
                                Joe.Poses.Leg,
                                Joe.Poses.Powerstand,
                                Joe.Poses.Peace,
                             }},
                             //=== Perfect key hit ===
                            { BeatKeyState.Green, new [] {
                                Joe.Poses.Drehen,
                                (isHype ? Joe.Poses.FingerHochStern : Joe.Poses.FingerHoch),
                                (isHype ? Joe.Poses.FingerRechtsStern : Joe.Poses.FingerRechts)
                             }}
                        })[key.State].RandomElement();
                    })
                    .AddTo(comp);
            }).AddTo(comp);

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(_ => comp.Dance.Value)
                .Merge(comp.Pose)
                .ThrottleFirst(TimeSpan.FromSeconds(comp.PoseTime))
                .Merge(comp.Pose)
                .DistinctUntilChanged()
                .Subscribe(animationState =>
                {
                    animator.Play(animationState);
                })
                .AddTo(comp);
        }

        public override void Register(ScoreComponent comp)
        {
            _score.Value = comp;
        }

        public override void Register(SpeakerComponent comp)
        {
            //FIXME: speaker animation just loops although looping is disabled

            // comp.WaitOn(_beats, beats =>
            // {
            //     var animator = comp.GetComponent<Animator>();
            //     beats.BeatTrigger
            //         .Where(beat => beat.BeatNo % 4 == 0)
            //         .Subscribe(beat =>
            //         {
            //             Debug.Log(beat.BeatNo);
            //             animator.Play(comp.Animation);
            //         })
            //         .AddTo(comp);
            // }).AddTo(comp);
        }

        public override void Register(BeatSystemConfig comp)
        {
            _beats.Value = comp;
        }

        private IEnumerator Animate(BasicToggleAnimationComponent component)
        {
            var steps = component.Sprites.Length;
            var time = component.AnimationTime;
            var delta = time / steps;

            component.StartAnimation();
            if (component.EndSprite) component.EndSprite.SetActive(true);

            for (var i = 0; i < steps; i++)
            {
                if (component.CurrentSprite == BasicToggleAnimationComponent.NotAnimating) break;
                for (var s = 0; s < component.Sprites.Length; s++)
                {
                    component.Sprites[s].SetActive(component.CurrentSprite == s);
                }

                yield return new WaitForSeconds(delta);
                if (component.CurrentSprite == BasicToggleAnimationComponent.NotAnimating) break;

                component.CurrentSprite += component.Reverse ? -1 : 1;
                component.CurrentSprite = Math.Max(0, component.CurrentSprite);
                component.CurrentSprite = Math.Min(component.CurrentSprite, steps - 1);

                if (i + 1 == steps && component.IsLoop)
                {
                    i = -1;
                    component.CurrentSprite = component.Reverse ? steps - 1 : 0;
                }
            }

            if (component.CurrentSprite != BasicToggleAnimationComponent.NotAnimating)
            {
                if (component.EndSprite)
                {
                    for (var s = 0; s < component.Sprites.Length; s++)
                    {
                        component.Sprites[s].SetActive(false);
                    }
                    component.EndSprite.SetActive(true);
                }

                component.StopAnimation();
            }
        }
    }
}