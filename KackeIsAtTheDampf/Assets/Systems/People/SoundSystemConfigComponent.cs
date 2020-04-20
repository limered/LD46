using SystemBase;
using UniRx;
using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Systems.Score;
using Assets.Systems.BeatChecker;
using Utils.Math;

namespace Assets.Systems.People
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundSystemConfigComponent : GameComponent
    {
        public RandomFloatRange VolumeRange = new RandomFloatRange(1f, 1f);
        ///Pitch range with which the shouts are played
        public RandomFloatRange PitchRange = new RandomFloatRange(0.75f, 1.25f);

        ///played when player hits the right key perfectly
        public AudioClip[] GreenSounds = new AudioClip[0];
        ///played when player hits the right key in OK range
        public AudioClip[] YellowSounds = new AudioClip[0];
        ///played when player hits the right key in OK range
        public AudioClip[] RedSounds = new AudioClip[0];

        public AudioClip[] SoundsForState(BeatKeyState keyState)
        {
            switch (keyState)
            {
                case BeatKeyState.Red: return RedSounds;
                case BeatKeyState.Yellow: return YellowSounds;
                case BeatKeyState.Green: return GreenSounds;


                default: throw new System.Exception("unknown BeatKeyState " + keyState);
            }
        }
    }
}
