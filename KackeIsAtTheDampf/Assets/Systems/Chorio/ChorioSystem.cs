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
                .Subscribe(OnBeat)
                .AddTo(component);
        }

        private void OnBeat(int beatNo)
        {
            if (beatNo % 4 != 0) return;

            MessageBroker.Default.Publish(new EvtNextBeatKeyAdded
            {
                Key = "g",
                BeatNo = beatNo + 2,
            });
        }
    }
}
