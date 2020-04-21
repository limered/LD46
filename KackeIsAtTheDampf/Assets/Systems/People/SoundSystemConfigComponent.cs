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
        ///played when player hits the right key perfectly
        public AudioClipWithVolumeAndPitch[] GreenSounds = new AudioClipWithVolumeAndPitch[0];
        ///played when player hits the right key in OK range
        public AudioClipWithVolumeAndPitch[] YellowSounds = new AudioClipWithVolumeAndPitch[0];
        ///played when player hits the right key in OK range
        public AudioClipWithVolumeAndPitch[] RedSounds = new AudioClipWithVolumeAndPitch[0];

        public AudioClipWithVolumeAndPitch[] SoundsForState(BeatKeyState keyState)
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

    [Serializable]
    public struct AudioClipWithVolumeAndPitch
    {
        public AudioClip Clip;

        [Range(0f, 1f)]
        public float Volume;

        public float Pitch;
    }
}
