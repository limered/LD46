using SystemBase;
using UniRx;
using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Systems.Score;
using Utils.Math;

namespace Assets.Systems.People
{
    [RequireComponent(typeof(AudioSource))]
    public class VoiceSystemConfigComponent : GameComponent
    {
        public float ThrottleTimeBetweenHypeLevelChanges = 0.25f;

        ///Periodically play a sound for the current hype level
        public float SampleTime = 5f;

        ///Volume range with which the shouts are played
        public RandomFloatRange VolumeRange = new RandomFloatRange(0.25f, 0.75f);
        ///Pitch range with which the shouts are played
        public RandomFloatRange PitchRange = new RandomFloatRange(0.75f, 1.25f);


        public AudioClip[] FailingShouts = new AudioClip[0];
        public AudioClip[] ShittyShouts = new AudioClip[0];
        public AudioClip[] NormalShouts = new AudioClip[0];
        public AudioClip[] CoolShouts = new AudioClip[0];
        public AudioClip[] HotShouts = new AudioClip[0];
        public AudioClip[] HotDateShouts = new AudioClip[0];
        public AudioClip[] CopShouts = new AudioClip[0];
        public AudioClip[] BouncerShouts = new AudioClip[0];



        ///Helper function to get the right sounds easier
        public AudioClip[] SoundsForHype(HyperLevel level)
        {
            switch (level)
            {
                case HyperLevel.Failing: return FailingShouts;
                case HyperLevel.Shitty: return ShittyShouts;
                case HyperLevel.Normal: return NormalShouts;
                case HyperLevel.Cool: return CoolShouts;
                case HyperLevel.Hot: return HotShouts;

                default: throw new System.Exception("unknown HyperLevel " + level);
            }
        }

    }
}
