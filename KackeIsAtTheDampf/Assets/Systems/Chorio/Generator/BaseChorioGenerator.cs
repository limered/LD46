using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using Assets.Systems.Key;

namespace Assets.Systems.Chorio.Generator
{
    public abstract class BaseChorioGenerator : IChorioGenerator
    {
        public const int BeatDistance = 6;
        public abstract EvtNextBeatKeyAdded[] GenerateTargetsForBeat(BeatInfo info, float timePerBeat,
            KeyInfoComponent _keyInfoComponent);
    }
}