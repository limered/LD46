using System.Collections.Generic;
using System.Linq;
using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using Assets.Systems.Key;
using UnityEngine;
using Utils.DotNet;

namespace Assets.Systems.Chorio.Generator
{
    public class HotGenerator : IChorioGenerator
    {
        public EvtNextBeatKeyAdded[] GenerateTargetsForBeat(BeatInfo info, float timePerBeat, KeyInfoComponent _keyInfoComponent)
        {
            if (info.BeatNo % 5 != 0) return new EvtNextBeatKeyAdded[0];
            if (Random.value < 0.05) return new EvtNextBeatKeyAdded[0]; // pause

            var list = new List<EvtNextBeatKeyAdded>();
            var generation = Random.Range(0, 5);
            switch (generation)
            {
                case 0:
                    list = GenerateRandomKeys(info, timePerBeat, _keyInfoComponent);
                    break;
                case 1:
                    list = GenerateFastRandom(info, timePerBeat, _keyInfoComponent);
                    break;
                case 2:
                    list = GenerateFastRandom2(info, timePerBeat, _keyInfoComponent);
                    break;
                case 3:
                    list = OneKeyTrigger(info, timePerBeat, _keyInfoComponent);
                    break;
                case 4:
                    list = OneKeyTriggerBeat(info, timePerBeat, _keyInfoComponent);
                    break;
            }

            return list.ToArray();
        }

        private static List<EvtNextBeatKeyAdded> GenerateRandomKeys(BeatInfo info, float timePerBeat, KeyInfoComponent _keyInfoComponent)
        {
            var initialKeyCount = Random.Range(4, 6);
            var list = new List<EvtNextBeatKeyAdded>();
            for (var i = 0; i < initialKeyCount; i++)
            {
                list.Add(new EvtNextBeatKeyAdded
                {
                    Id = Time.frameCount + 5000 + i,
                    Key = _keyInfoComponent.RelevantKeys.RandomElement(),
                    BeatNo = info.BeatNo + 4 + i,
                    PlannedBeatTime = info.BeatTime + ((4 + i) * timePerBeat)
                });
            }

            return list;
        }

        private List<EvtNextBeatKeyAdded> GenerateFastRandom(BeatInfo info, float timePerBeat,
            KeyInfoComponent _keyInfoComponent)
        {
            var initialKeyCount = Random.Range(4, 6);
            var list = new List<EvtNextBeatKeyAdded>();
            string[] keys = _keyInfoComponent.RelevantKeys.Take(3)
                .Concat(_keyInfoComponent.RelevantKeys.Skip(6).Take(3))
                .ToArray();
            for (var i = 0; i < initialKeyCount; i++)
            {
                list.Add(new EvtNextBeatKeyAdded
                {
                    Id = Time.frameCount + 5000 + i,
                    Key = keys.RandomElement(),
                    BeatNo = info.BeatNo + 4 + i,
                    PlannedBeatTime = info.BeatTime + ((4f + i * 0.5f) * timePerBeat)
                });
            }

            return list;
        }

        private List<EvtNextBeatKeyAdded> GenerateFastRandom2(BeatInfo info, float timePerBeat,
            KeyInfoComponent _keyInfoComponent)
        {
            var initialKeyCount = Random.Range(7, 9);
            var list = new List<EvtNextBeatKeyAdded>();
            string[] keys = _keyInfoComponent.RelevantKeys.Take(3)
                .Concat(_keyInfoComponent.RelevantKeys.Skip(6).Take(3))
                .ToArray();
            for (var i = 0; i < initialKeyCount; i++)
            {
                list.Add(new EvtNextBeatKeyAdded
                {
                    Id = Time.frameCount + 5000 + i,
                    Key = keys.RandomElement(),
                    BeatNo = info.BeatNo + 4 + i,
                    PlannedBeatTime = info.BeatTime + ((4f + i * 0.5f) * timePerBeat)
                });
            }

            return list;
        }

        private List<EvtNextBeatKeyAdded> OneKeyTrigger(BeatInfo info, float timePerBeat,
            KeyInfoComponent _keyInfoComponent)
        {
            var initialKeyCount = Random.Range(7, 12);
            var list = new List<EvtNextBeatKeyAdded>();
            var key = _keyInfoComponent.RelevantKeys.RandomElement();
            for (var i = 0; i < initialKeyCount; i++)
            {
                list.Add(new EvtNextBeatKeyAdded
                {
                    Id = Time.frameCount + 8000 + i,
                    Key = key,
                    BeatNo = info.BeatNo + 4 + i,
                    PlannedBeatTime = info.BeatTime + ((4f + i * 0.25f) * timePerBeat)
                });
            }

            return list;
        }

        private List<EvtNextBeatKeyAdded> OneKeyTriggerBeat(BeatInfo info, float timePerBeat,
            KeyInfoComponent _keyInfoComponent)
        {
            var initialKeyCount = new []{0f, 1f, 1.5f, 2.5f, 3.5f, 4f};
            var list = new List<EvtNextBeatKeyAdded>();
            var key = _keyInfoComponent.RelevantKeys.RandomElement();
            var key2 = _keyInfoComponent.RelevantKeys.RandomElement();
            for (var i = 0; i < initialKeyCount.Length; i++)
            {
                list.Add(new EvtNextBeatKeyAdded
                {
                    Id = Time.frameCount + 8000 + i,
                    Key = Random.value < 0.1 ? key2 : key,
                    BeatNo = info.BeatNo + 4 + i,
                    PlannedBeatTime = info.BeatTime + ((4f + initialKeyCount[i]) * timePerBeat)
                });
            }

            return list;
        }
    }
}