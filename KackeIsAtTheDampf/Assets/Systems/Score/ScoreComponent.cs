using SystemBase;
using UniRx;

namespace Assets.Systems.Score
{
    public class ScoreComponent : GameComponent
    {
        public float RedHitPoints = -10;
        public float YellowPoints = 2;
        public float GreenPoints = 5;

        public float MaxHyperScore = 100;
        public float MinHyperScore = 0;

        public FloatReactiveProperty HyperScore = new FloatReactiveProperty(5);
        public ReactiveProperty<HyperLevel> HyperLevel = new ReactiveProperty<HyperLevel>(Score.HyperLevel.Normal);
        public FloatReactiveProperty FullScore = new FloatReactiveProperty(0);
    }
}