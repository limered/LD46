using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using SystemBase;
using UniRx;

namespace Assets.Systems.Chorio
{
    [GameSystem(typeof(BeatSystem))]
    public class ChorioSystem : GameSystem<BeatSystemConfig>
    {
        public override void Register(BeatSystemConfig component)
        {
            component.BeatTrigger
                .Subscribe(beatInfo => OnBeat(beatInfo, component.TimePerBeat))
                .AddTo(component);
        }

        private void OnBeat(BeatInfo beatInfo, float timePerBeat)
        {
            if (beatInfo.BeatNo % 4 != 0) return;

            MessageBroker.Default.Publish(new EvtNextBeatKeyAdded
            {
                Key = "g",
                PlannedBeatTime = beatInfo.BeatTime + timePerBeat * 2,
                BeatNo = beatInfo.BeatNo + 2
            });
        }
    }
}
