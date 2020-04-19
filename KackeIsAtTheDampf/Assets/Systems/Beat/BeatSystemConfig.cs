﻿using SystemBase;
using UniRx;

namespace Assets.Systems.Beat
{
    public class BeatSystemConfig : GameComponent
    {
        public FloatReactiveProperty BPM = new FloatReactiveProperty(118);

        public IntReactiveProperty BeatTrigger = new IntReactiveProperty(0);
    }
}