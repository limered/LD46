using SystemBase;
using UnityEngine.UI;

namespace Assets.Systems.GameEnd
{
    public class EndGameComponent : GameComponent
    {
        public int LastGameScore;
        public Text ScoreDisplay;
    }
}