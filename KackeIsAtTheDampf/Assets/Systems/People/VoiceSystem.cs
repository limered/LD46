using SystemBase;
using System;
using UniRx;
using UniRx.Operators;
using UnityEngine;
using Assets.Systems.Score;
using Object = UnityEngine.Object;
using Utils.Math;

namespace Assets.Systems.People
{
    [GameSystem]
    public class VoiceSystem : GameSystem<ScoreComponent, VoiceSystemConfigComponent>
    {
        private ReactiveProperty<ScoreComponent> _score = new ReactiveProperty<ScoreComponent>();

        ///react to Hype Level changes
        public override void Register(ScoreComponent comp)
        {
            _score.Value = comp;
        }

        public override void Register(VoiceSystemConfigComponent comp)
        {
            var player = comp.gameObject.GetComponent<AudioSource>();

            comp.WaitOn(_score, score =>
            {
                score.HyperLevel
                    .DistinctUntilChanged()

                    //Periodically play a sound for the current hype level
                    .Merge(score.HyperLevel.Sample(TimeSpan.FromSeconds(comp.SampleTime)))
                    //throttle hyper level changes
                    .Throttle(TimeSpan.FromSeconds(comp.ThrottleTimeBetweenHypeLevelChanges))

                    .Select(level => comp.SoundsForHype(level))
                    .Subscribe(hypeSounds =>
                    {
                        var randomIndex = hypeSounds.RandomIndex();
                        player.panStereo = UnityEngine.Random.Range(0f, 1f);
                        player.pitch = UnityEngine.Random.Range(
                            comp.PitchRange.Min,
                            comp.PitchRange.Max
                        );
                        player.PlayOneShot(hypeSounds[randomIndex], comp.Volume);
                    })
                    .AddTo(comp);
            }).AddTo(comp);
        }
    }
}