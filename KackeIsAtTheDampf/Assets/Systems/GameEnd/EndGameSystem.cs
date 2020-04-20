using Assets.Systems.Score;
using SystemBase;
using Assets.Systems.Beat;

namespace Assets.Systems.GameEnd
{
    [GameSystem(typeof(ScoreSystem))]
    public class EndGameSystem : GameSystem<ScoreComponent, EndGameComponent, BeatSystemConfig>
    {
        private ScoreComponent _score;

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
            //component.Music.clip.length
        }
    }
}
