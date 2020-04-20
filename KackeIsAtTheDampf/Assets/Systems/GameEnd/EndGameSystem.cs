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
        }

        public override void Register(EndGameComponent component)
        {
            component.LastGameScore = (int)_score.FullScore.Value;
            component.ScoreDisplay.text = component.LastGameScore.ToString();
        }

        public override void Register(BeatSystemConfig component)
        {
            var timeToStop = component.GameEndTimestamp - Time.realtimeSinceStartup;
            Observable.Timer(TimeSpan.FromSeconds(timeToStop))
                .Subscribe(OnEndGame)
                .AddTo(component);
        }

        private void OnEndGame(long obj)
        {
            MessageBroker.Default.Publish(new ActStopTheBeat());

            _fadeToBlackComponent.UpdateAsObservable()
                .Subscribe(OnNext)
                .AddTo(_fadeToBlackComponent);
        }

        private void OnNext(Unit obj)
        {
            var current = _fadeToBlackComponent.GetComponent<Image>().color.a;
            var next = Mathf.Lerp(current, 1f, 0.01f);
            _fadeToBlackComponent.GetComponent<Image>().color = new Color(
                _fadeToBlackComponent.GetComponent<Image>().color.r,
                _fadeToBlackComponent.GetComponent<Image>().color.g,
                _fadeToBlackComponent.GetComponent<Image>().color.b,
                next);

            if (next > 0.999)
            {
                MessageBroker.Default.Publish(new GameMsgEnd());
                SceneManager.LoadScene("End_Police");
            }
        }

        public override void Register(FadeToBlackComponent component)
        {
            _fadeToBlackComponent = component;
        }
    }
}
