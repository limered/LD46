using SystemBase;
using UniRx;
using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Systems.Score;
using Utils.Math;

namespace Assets.Systems.Animation
{
    [RequireComponent(typeof(Animator))]
    public class SpeakerComponent : GameComponent
    {
        public string Animation = "boxes02";
    }
}