using Systems.GameState.Messages;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Systems.Menu
{
    public class StartOnClick : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("MainFloor");

            MessageBroker.Default.Publish(new GameMsgStart());
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("MainFloor");

            MessageBroker.Default.Publish(new GameMsgRestart());
        }
    }
}
