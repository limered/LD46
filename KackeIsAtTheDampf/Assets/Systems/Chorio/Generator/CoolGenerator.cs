using System.Collections.Generic;
using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using Assets.Systems.Key;
using UnityEngine;
using Utils.DotNet;

namespace Assets.Systems.Chorio.Generator
{
    public class CoolGenerator : BaseChorioGenerator
    {
        public override EvtNextBeatKeyAdded[] GenerateTargetsForBeat(BeatInfo info, float timePerBeat, KeyInfoComponent _keyInfoComponent)
        {
            if (info.BeatNo % 6 != 0) return new EvtNextBeatKeyAdded[0];
            if (Random.value < 0.1) return new EvtNextBeatKeyAdded[0]; // pause

            var list = new List<EvtNextBeatKeyAdded>();
            var generation = Random.Range(0, 4);
            switch (generation)
            {
                case 0: list = GenerateRandomKeys(info, timePerBeat, _keyInfoComponent);
                    break;
                case 1:
                    list = GenerateRandomFast(info, timePerBeat, _keyInfoComponent);
                    break;
                case 2:
                    list = GenerateFirst(info, timePerBeat, _keyInfoComponent);
                    break;
                case 3:
                    list = GenerateSecond(info, timePerBeat, _keyInfoComponent);
                    break;
            }

            return list.ToArray();
        }

        private List<EvtNextBeatKeyAdded> GenerateRandomKeys(BeatInfo info, float timePerBeat, KeyInfoComponent _keyInfoComponent)
        {
            var initialKeyCount = Random.Range(4, 6);
            var list = new List<EvtNextBeatKeyAdded>();
            for (var i = 0; i < initialKeyCount; i++)
            {
                list.Add(new EvtNextBeatKeyAdded
                {
                    Id = Time.frameCount + 5000 + i,
                    Key = _keyInfoComponent.RelevantKeys.RandomElement(),
                    BeatNo = info.BeatNo + BeatDistance + i,
                    PlannedBeatTime = info.BeatTime + ((BeatDistance + i) * timePerBeat)
                });
            }

            return list;
        }

        private List<EvtNextBeatKeyAdded> GenerateRandomFast(BeatInfo info, float timePerBeat,
            KeyInfoComponent _keyInfoComponent)
        {
            var initialKeyCount = Random.Range(4, 6);
            var list = new List<EvtNextBeatKeyAdded>();
            for (var i = 0; i < initialKeyCount; i++)
            {
                list.Add(new EvtNextBeatKeyAdded
                {
                    Id = Time.frameCount + 5000 + i,
                    Key = _keyInfoComponent.RelevantKeys.RandomElement(),
                    BeatNo = info.BeatNo + BeatDistance + i,
                    PlannedBeatTime = info.BeatTime + ((BeatDistance + i * 0.5f) * timePerBeat)
                });
            }

            return list;
        }

        private List<EvtNextBeatKeyAdded> GenerateFirst(BeatInfo info, float timePerBeat,
            KeyInfoComponent _keyInfoComponent)
        {
            var initialKeyCount = Random.Range(3, 9);
            var list = new List<EvtNextBeatKeyAdded>();
            var startKey = Random.Range(0, _keyInfoComponent.RelevantKeys.Length);
            for (var i = 0; i < initialKeyCount; i++)
            {
                var key = (startKey + i) % _keyInfoComponent.RelevantKeys.Length;
                var delay = (BeatDistance + i * 0.5f);
                delay += (delay > BeatDistance + 1f) ? 0.5f : 0f;
                delay += (delay > BeatDistance + 3f) ? 0.5f : 0f;
                list.Add(new EvtNextBeatKeyAdded
                {
                    Id = Time.frameCount + 5000 + i,
                    Key = _keyInfoComponent.RelevantKeys[key],
                    BeatNo = info.BeatNo + BeatDistance + i,
                    PlannedBeatTime = info.BeatTime + (delay * timePerBeat)
                });
            }

            return list;
        }

        private List<EvtNextBeatKeyAdded> GenerateSecond(BeatInfo info, float timePerBeat,
            KeyInfoComponent _keyInfoComponent)
        {
            var initialKeyCount = Random.Range(3, 6);
            var list = new List<EvtNextBeatKeyAdded>();
            var startKey = Random.Range(0, _keyInfoComponent.RelevantKeys.Length);
            for (var i = 0; i < initialKeyCount; i++)
            {
                var key = (startKey + i) % _keyInfoComponent.RelevantKeys.Length;
                key = _keyInfoComponent.RelevantKeys.Length - key - 1;
                var delay = (BeatDistance + i);
                list.Add(new EvtNextBeatKeyAdded
                {
                    Id = Time.frameCount + 5000 + i,
                    Key = _keyInfoComponent.RelevantKeys[key],
                    BeatNo = info.BeatNo + BeatDistance + i,
                    PlannedBeatTime = info.BeatTime + (delay * timePerBeat)
                });
            }

            return list;
        }
    }
}