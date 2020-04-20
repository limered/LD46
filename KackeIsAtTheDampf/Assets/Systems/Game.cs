using GameState.States;
using SystemBase;
using SystemBase.StateMachineBase;
using Systems.GameState.Messages;
using StrongSystems.Audio.Helper;
using UnityEngine.SceneManagement;
using UniRx;
using Utils;
using UniRx.Triggers;
using UnityEngine;

namespace Systems
{
    public class Game : GameBase
    {
        public readonly StateContext<Game> GameStateContext = new StateContext<Game>();

        [ContextMenu("Start Game")]
        public void StartGame()
        {
            MessageBroker.Default.Publish(new GameMsgStart());
            SceneManager.LoadScene("MainFloor");
        }

        private void Awake()
        {
            IoC.RegisterSingleton(this);

            GameStateContext.Start(new Loading());

            InstantiateSystems();

            Init();

            MessageBroker.Default.Publish(new GameMsgFinishedLoading());
            GameStateContext.CurrentState.Where(state => state is StartScreen).Subscribe(_ => ListenToGameStartButtonPressed());
        }

        public override void Init()
        {
            base.Init();

            IoC.RegisterSingleton<ISFXComparer>(()=> new SFXComparer());
        }

        private void ListenToGameStartButtonPressed()
        {
            this.UpdateAsObservable().Subscribe(_ =>
            {
                if (Input.GetButtonDown("Start"))
                {
                    StartGame();
                }
            });
        }
    }
}