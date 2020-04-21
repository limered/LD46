using System.Collections.Generic;
using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using Assets.Systems.Key;
using UnityEngine;
using Utils.DotNet;

namespace Assets.Systems.Chorio.Generator
{
    public class NormalGenerator : BaseChorioGenerator
    {
        public override EvtNextBeatKeyAdded[] GenerateTargetsForBeat(BeatInfo info, float timePerBeat, KeyInfoComponent _keyInfoComponent)
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
                    BeatNo = info.BeatNo + BeatDistance + i,
                    PlannedBeatTime = info.BeatTime + (timePerBeat * (BeatDistance + i))
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
}