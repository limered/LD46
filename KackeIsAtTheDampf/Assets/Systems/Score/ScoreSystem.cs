using Assets.Systems.Beat;
using Assets.Systems.BeatChecker;
using Assets.Systems.BeatChecker.Events;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Assets.Systems.Score
{
    [GameSystem(typeof(BeatCheckerSystem), typeof(BeatSystem))]
    public class ScoreSystem : GameSystem<ScoreComponent>
    {
        public override void Register(ScoreComponent component)
        {
            MessageBroker.Default.Receive<EvtHitMessage>()
                .Subscribe(msg => OnHitRegistered(msg, component))
                .AddTo(component);
        }

        private void OnHitRegistered(EvtHitMessage msg, ScoreComponent component)
        {
            switch (msg.State)
            {
                case BeatKeyState.Red:
                    component.HyperScore.Value = Mathf.Max(component.MinHyperScore,
                        component.HyperScore.Value + component.RedHitPoints);
                    break;
                case BeatKeyState.Yellow:
                    component.HyperScore.Value = Mathf.Min(component.MaxHyperScore,
                        component.HyperScore.Value + component.YellowPoints);
                    break;
                case BeatKeyState.Green:
                    component.HyperScore.Value = Mathf.Min(component.MaxHyperScore,
                        component.HyperScore.Value + component.GreenPoints);
                    break;
                default:
                    component.HyperScore.Value = component.HyperScore.Value;
                    break;
            }

            var value = component.HyperScore.Value;
            if (value <= (int) HyperLevel.Failing)
            {
                component.HyperLevel.Value = HyperLevel.Failing;
            }
            else if (value < (int) HyperLevel.Normal)
            {
                component.HyperLevel.Value = HyperLevel.Shitty;
            }
            else if (value < (int) HyperLevel.Cool)
            {
                component.HyperLevel.Value = HyperLevel.Normal;
            }
            else if (value < (int) HyperLevel.Hot)
            {
                component.HyperLevel.Value = HyperLevel.Cool;
            }
            else if (value > (int) HyperLevel.Hot)
            {
                component.HyperLevel.Value = HyperLevel.Hot;
            }

            component.FullScore.Value += component.HyperScore.Value;
        }
    }

    public enum HyperLevel
    {
        Failing = 1,
        Shitty = 10,
        Normal = 30,
        Cool = 60,
        Hot = 90
    }
}
