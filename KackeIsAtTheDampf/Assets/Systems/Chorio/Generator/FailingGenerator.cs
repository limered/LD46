﻿using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using Assets.Systems.Key;
using UnityEngine;

namespace Assets.Systems.Chorio.Generator
{
    public class FailingGenerator : BaseChorioGenerator
    {
        private int _lastBeatNumberGenerated;
        private int _minDistanceBetweenBeets = 2;
        public override EvtNextBeatKeyAdded[] GenerateTargetsForBeat(BeatInfo info, float timePerBeat, KeyInfoComponent _keyInfoComponent)
        {
            if (_lastBeatNumberGenerated + _minDistanceBetweenBeets > info.BeatNo) return new EvtNextBeatKeyAdded[0];
            var generatorVal = (int) (Random.value * 2f);
            if (generatorVal != 0) return new EvtNextBeatKeyAdded[0];

            _lastBeatNumberGenerated = info.BeatNo;
            var key = _keyInfoComponent.RelevantKeys[(int) (Random.value * _keyInfoComponent.RelevantKeys.Length)];
            return new[]
            {
                new EvtNextBeatKeyAdded
                {
                    Id = Time.frameCount,
                    Key = key,
                    BeatNo = info.BeatNo + BeatDistance,
                    PlannedBeatTime = info.BeatTime + (timePerBeat * BeatDistance),
                }
            };
        }
    }
}
