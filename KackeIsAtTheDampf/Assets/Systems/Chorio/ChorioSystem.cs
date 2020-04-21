using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using Assets.Systems.Chorio.Generator;
using Assets.Systems.Key;
using Assets.Systems.Score;
using SystemBase;
using GameState.States;
using UniRx;
using Utils;

namespace Assets.Systems.Chorio
{
    [GameSystem(typeof(BeatSystem))]
    public class ChorioSystem : GameSystem<BeatSystemConfig, KeyInfoComponent, ScoreComponent>
    {
        private readonly ReactiveProperty<KeyInfoComponent> _keyInfoComponent = new ReactiveProperty<KeyInfoComponent>();

        private IChorioGenerator _currentGenerator;

        private int _waitForBeatsCount;
        private const int WaitTimeOnChange = 4;

        public override void Register(BeatSystemConfig component)
        {
            component.WaitOn(_keyInfoComponent).Subscribe(infoComponent =>
                {
                    _currentGenerator = new FailingGenerator();

                    component.BeatTrigger
                        .Subscribe(beatInfo => OnBeat(beatInfo, component.TimePerBeat))
                        .AddTo(component);
                })
                .AddTo(component);
        }

        public override void Register(KeyInfoComponent component)
        {
            _keyInfoComponent.Value = component;

            IoC.Game.GameStateContext.CurrentState
                .Where(state => state is GameOver)
                .Subscribe(_ => _keyInfoComponent.Value = null)
                .AddTo(component);
        }

        public override void Register(ScoreComponent component)
        {
            //return;
            component.HyperLevel.Subscribe(level =>
            {
                switch (level)
                {
                    case HyperLevel.Failing:
                        _currentGenerator = new FailingGenerator();
                        break;

                    case HyperLevel.Shitty:
                        _currentGenerator = new ShittyGenerator();
                        break;

                    case HyperLevel.Normal:
                        _currentGenerator = new NormalGenerator();
                        break;

                    case HyperLevel.Cool:
                        _currentGenerator = new CoolGenerator();
                        break;

                    case HyperLevel.Hot:
                        _currentGenerator = new HotGenerator();
                        break;
                }

                _waitForBeatsCount = WaitTimeOnChange;
            }).AddTo(component);
        }

        private void OnBeat(BeatInfo beatInfo, float timePerBeat)
        {
            if (_waitForBeatsCount-- > 0) return;

            EvtNextBeatKeyAdded[] keys = _currentGenerator
                .GenerateTargetsForBeat(beatInfo, timePerBeat, _keyInfoComponent.Value);

            foreach (var beatKeyAdded in keys)
            {
                MessageBroker.Default.Publish(beatKeyAdded);
            }
        }
    }
}
