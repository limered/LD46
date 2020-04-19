using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using Assets.Systems.Key;
using UnityEngine;

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

    public class ShittyGenerator : IChorioGenerator
    {
        public EvtNextBeatKeyAdded[] GenerateTargetsForBeat(BeatInfo info, float timePerBeat, KeyInfoComponent _keyInfoComponent)
        {
            throw new System.NotImplementedException();
        }
    }

    public class NormalGenerator : IChorioGenerator
    {
        public EvtNextBeatKeyAdded[] GenerateTargetsForBeat(BeatInfo info, float timePerBeat, KeyInfoComponent _keyInfoComponent)
        {
            throw new System.NotImplementedException();
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
