using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using Assets.Systems.Key;

namespace Assets.Systems.Chorio.Generator
{
    public interface IChorioGenerator
    {
        EvtNextBeatKeyAdded[] GenerateTargetsForBeat(BeatInfo info, float timePerBeat, KeyInfoComponent _keyInfoComponent);
    }
}