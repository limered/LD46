using System.Collections.Generic;
using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using Assets.Systems.Key;
using UnityEngine;

namespace Assets.Systems.Chorio.Generator
{
    public class ShittyGenerator : BaseChorioGenerator
    {
        private int _lastBeatNumberGenerated;
        private int _minDistanceBetweenBeets = 2;
        public override EvtNextBeatKeyAdded[] GenerateTargetsForBeat(BeatInfo info, float timePerBeat, KeyInfoComponent _keyInfoComponent)
        {
            if (_lastBeatNumberGenerated + _minDistanceBetweenBeets > info.BeatNo) return new EvtNextBeatKeyAdded[0];
            var generatorVal = (int)(Random.value * 4f);
            if (generatorVal >= 3) return new EvtNextBeatKeyAdded[0];

            _lastBeatNumberGenerated = info.BeatNo;
            var key = _keyInfoComponent.RelevantKeys[(int)(Random.value * _keyInfoComponent.RelevantKeys.Length)];
            var list = new List<EvtNextBeatKeyAdded>
            {
                new EvtNextBeatKeyAdded
                {
                    Id = Time.frameCount + 500000,
                    Key = key,
                    BeatNo = info.BeatNo + BeatDistance,
                    PlannedBeatTime = info.BeatTime + (timePerBeat * BeatDistance)
                }
            };

            if (!(Random.value < 0.1f)) return list.ToArray();

            list.Add(new EvtNextBeatKeyAdded
            {
                Id = Time.frameCount + 500001,
                Key = key,
                BeatNo = info.BeatNo + BeatDistance + 1,
                PlannedBeatTime = info.BeatTime + (timePerBeat * (BeatDistance + 1f))
            });

            return list.ToArray();
        }
    }
}