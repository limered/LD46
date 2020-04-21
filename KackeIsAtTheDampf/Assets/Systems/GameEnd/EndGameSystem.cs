using Assets.Systems.Beat;
using Assets.Systems.Score;
using System;
using SystemBase;
using Systems.GameState.Messages;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Assets.Systems.GameEnd
{
    [GameSystem(typeof(ScoreSystem))]
    public class EndGameSystem : GameSystem<ScoreComponent, EndGameComponent, BeatSystemConfig, FadeToBlackComponent>
    {
        private ScoreComponent _score;
        private FadeToBlackComponent _fadeToBlackComponent;

        public override void Register(ScoreComponent component)
        {
            _score = component;

            _score.HyperLevel
                .Throttle(TimeSpan.FromSeconds(10))
                .Where(level => level == HyperLevel.Failing)
                .Subscribe(_ => OnEndGame(0))
                .AddTo(component);

            _score.HyperLevel
                .Throttle(TimeSpan.FromSeconds(20))
                .Where(level => level == HyperLevel.Hot)
                .Subscribe(_ => OnEndGame(Random.Range(2, 4)))
                .AddTo(component);
        }

        public override void Register(EndGameComponent component)
        {
            component.LastGameScore = (int)_score.FullScore.Value;
            component.ScoreDisplay.text = component.LastGameScore.ToString();
        }

        public override void Register(BeatSystemConfig component)
        {
            component.GameEndTimestamp
                .Where(f => f > 0)
                .Subscribe(f =>
                {
                    var timeToStop = f - Time.realtimeSinceStartup;
                    Observable.Timer(TimeSpan.FromSeconds(timeToStop))
                        .Subscribe(_ => OnEndGame(1))
                        .AddTo(component);
                })
                .AddTo(component);
        }

        public override void Register(FadeToBlackComponent component)
        {
            _fadeToBlackComponent = component;
        }

        private void OnEndGame(long gameEndNumber)
        {
            MessageBroker.Default.Publish(new ActStopTheBeat());

            _fadeToBlackComponent.UpdateAsObservable()
                .Subscribe(_ => OnNext(gameEndNumber))
                .AddTo(_fadeToBlackComponent);
        }

        private void OnNext(long gameEndNumber)
        {
            var current = _fadeToBlackComponent.GetComponent<Image>().color.a;
            var next = Mathf.Lerp(current, 1f, 0.03f);
            _fadeToBlackComponent.GetComponent<Image>().color = new Color(
                _fadeToBlackComponent.GetComponent<Image>().color.r,
                _fadeToBlackComponent.GetComponent<Image>().color.g,
                _fadeToBlackComponent.GetComponent<Image>().color.b,
                next);

            if (next > 0.999)
            {
                MessageBroker.Default.Publish(new GameMsgEnd());
                if (gameEndNumber == 0)
                {
                    SceneManager.LoadScene("End_Bouncer");
                    return;
                }
                if(gameEndNumber == 1)
                {
                    SceneManager.LoadScene("End_Police");
                    return;
                }
                if (gameEndNumber == 2)
                {
                    SceneManager.LoadScene("End_HotDateAndrew");
                    return;
                }
                if (gameEndNumber == 3)
                {
                    SceneManager.LoadScene("End_HotDateStacy");
                    return;
                }
            }
        }
    }
}
