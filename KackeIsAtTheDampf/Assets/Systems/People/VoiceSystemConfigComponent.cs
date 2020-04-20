using SystemBase;
using UniRx;
using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Systems.Score;

namespace Assets.Systems.People
{
    [RequireComponent(typeof(AudioSource))]
    public class VoiceSystemConfigComponent : GameComponent
    {
        [Range(0f, 1f)]
        public float Volume = 1f;

        public float ThrottleTimeBetweenHypeLevelChanges = 1;

        ///Periodically play a sound for the current hype level
        public float SampleTime = 4;

        public RandomFloatRange PitchRange = new RandomFloatRange { Min = 0, Max = 1 };

        public AudioClip[] FailingSounds = new AudioClip[0];
        public AudioClip[] ShittySounds = new AudioClip[0];
        public AudioClip[] NormalSounds = new AudioClip[0];
        public AudioClip[] CoolSounds = new AudioClip[0];
        public AudioClip[] HotSounds = new AudioClip[0];


        ///Helper function to get the right sounds easier
        public AudioClip[] SoundsForHype(HyperLevel level)
        {
            switch (level)
            {
                case HyperLevel.Failing: return FailingSounds;
                case HyperLevel.Shitty: return ShittySounds;
                case HyperLevel.Normal: return NormalSounds;
                case HyperLevel.Cool: return CoolSounds;
                case HyperLevel.Hot: return HotSounds;

                default: throw new System.Exception("unknown HyperLevel " + level);
            }
        }

    }

    [Serializable]
    public struct RandomFloatRange
    {
        public float Min;
        public float Max;
    }
}
