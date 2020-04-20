using SystemBase;
using System;
using UniRx;
using UniRx.Operators;
using UnityEngine;
using Assets.Systems.Score;
using Assets.Systems.BeatChecker;
using Assets.Systems.BeatChecker.Events;
using Object = UnityEngine.Object;
using Utils.Math;

namespace Assets.Systems.People
{
    [GameSystem]
    public class SoundSystem : GameSystem<ScoreComponent, VoiceSystemConfigComponent, SoundSystemConfigComponent>
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

            //=== Periodic shouts from people ===
            comp.WaitOn(_score, score =>
            {
                score.HyperLevel
                    .DistinctUntilChanged()
                    //Periodically play a sound for the current hype level
                    .Merge(score.HyperLevel.Sample(TimeSpan.FromSeconds(comp.SampleTime)))
                    //throttle hyper level changes
                    .Throttle(TimeSpan.FromSeconds(comp.ThrottleTimeBetweenHypeLevelChanges))

                    .Subscribe(level =>
                    {
                        var hypeSounds = comp.SoundsForHype(level);
                        var randomIndex = hypeSounds.RandomIndex();
                        player.panStereo = UnityEngine.Random.Range(0f, 1f);
                        player.pitch = comp.PitchRange.Value;
                        player.PlayOneShot(hypeSounds[randomIndex], comp.VolumeRange.Value);
                    })
                    .AddTo(comp);
            }).AddTo(comp);
        }

        public override void Register(SoundSystemConfigComponent comp)
        {
            var player = comp.gameObject.GetComponent<AudioSource>();

            //=== Sound effects for HyperLevel changes ===
            MessageBroker.Default.Receive<EvtHitMessage>()
                .Subscribe(key =>
                {
                    var sounds = comp.SoundsForState(key.State);
                    var randomIndex = sounds.RandomIndex();
                    player.panStereo = 0.5f;
                    player.pitch = sounds[randomIndex].Pitch;
                    player.PlayOneShot(sounds[randomIndex].Clip, sounds[randomIndex].Volume);
                })
                .AddTo(comp);
        }
    }
}