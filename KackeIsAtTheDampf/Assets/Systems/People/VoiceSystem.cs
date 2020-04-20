using SystemBase;
using System;
using UniRx;
using UniRx.Operators;
using UnityEngine;
using Assets.Systems.Score;
using Object = UnityEngine.Object;

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
                    
                    .Select(level => comp.SoundsForHype(level))
                    .Subscribe(hypeSounds =>
                    {
                        var randomIndex = (int)Mathf.Floor(UnityEngine.Random.Range(0, hypeSounds.Length));
                        player.PlayOneShot(hypeSounds[randomIndex], comp.Volume);
                    })
                    .AddTo(comp);
            }).AddTo(comp);


        }


    }
}
