using System;
using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UniRx;
using UnityEngine;
using Utils.Plugins;

namespace Systems.Animation
{
    [RequireComponent(typeof(Animator))]
    public class JoeAnimationComponent : GameComponent
    {
        public StringReactiveProperty State = new StringReactiveProperty();
    }

    public static class Joe
    {
        public static class Dance
        {
            public const string Idle = "joe_idle01";
            public const string Normal = "joe_idle_normal01";
            public const string Cool = "joe_idle_cool01";
        }

        public static class Poses
        {
            public const string Peace = "joe_peace";
            public const string Powerstand = "joe_powerstand";
            public const string Leg = "joe_leg";
            public const string HorrorSit = "joe_horror_sit";
            public const string HorrorStand = "joe_horror_stand";
            public const string FingerHoch = "joe_finger_hoch";
            public const string FingerRechts = "joe_finger_rechts";
            public const string FingerRechtsStern = "joe_finger_rechts_stern";
            public const string FingerHochStern = "joe_finger_hoch_stern";
            public const string Drehen = "joe_drehen01";
        }
    }
}