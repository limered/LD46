﻿using System.Collections.Generic;
using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using Assets.Systems.Key;
using UnityEngine;
using Utils.DotNet;

namespace Assets.Systems.Chorio.Generator
{
    public class FailingGenerator : IChorioGenerator
    {
        private int _lastBeatNumberGenerated;
        private int _minDistanceBetweenBeets = 2;
        public EvtNextBeatKeyAdded[] GenerateTargetsForBeat(BeatInfo info, float timePerBeat, KeyInfoComponent _keyInfoComponent)
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
                    BeatNo = info.BeatNo + 4,
                    PlannedBeatTime = info.BeatTime + (timePerBeat * 4),
                }
            };
        }
    }

    public class NormalGenerator : IChorioGenerator
    {
        public EvtNextBeatKeyAdded[] GenerateTargetsForBeat(BeatInfo info, float timePerBeat, KeyInfoComponent _keyInfoComponent)
        {
            if (info.BeatNo % 4 != 0) return new EvtNextBeatKeyAdded[0];
            if (Random.value < 0.1) return new EvtNextBeatKeyAdded[0]; // pause

            var keyCount = Random.Range(2, 5);
            var list = new List<EvtNextBeatKeyAdded>();
            for (var i = 0; i < keyCount; i++)
            {
                list.Add(new EvtNextBeatKeyAdded
                {
                    Id = Time.frameCount + 5000 + i,
                    Key = _keyInfoComponent.RelevantKeys.RandomElement(),
                    BeatNo = info.BeatNo + 4 + i,
                    PlannedBeatTime = info.BeatTime + (timePerBeat * (4+i))
                });
            }

            var additionalBeatRnd = Random.value;
            if (additionalBeatRnd < 0.03)
            {
                var element = list.RandomElement();
                list.Add(new EvtNextBeatKeyAdded
                {
                    Id = Time.frameCount + 60000,
                    Key = element.Key,
                    BeatNo = element.BeatNo,
                    PlannedBeatTime = element.PlannedBeatTime + (timePerBeat * 0.5f),
                });
            }

            return list.ToArray();
        }
    }

    public class CoolGenerator : IChorioGenerator
    {
        public EvtNextBeatKeyAdded[] GenerateTargetsForBeat(BeatInfo info, float timePerBeat, KeyInfoComponent _keyInfoComponent)
        {
            throw new System.NotImplementedException();
        }
    }

    public class HotGenerator : IChorioGenerator
    {
        public EvtNextBeatKeyAdded[] GenerateTargetsForBeat(BeatInfo info, float timePerBeat, KeyInfoComponent _keyInfoComponent)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IChorioGenerator
    {
        EvtNextBeatKeyAdded[] GenerateTargetsForBeat(BeatInfo info, float timePerBeat, KeyInfoComponent _keyInfoComponent);
    }
}
