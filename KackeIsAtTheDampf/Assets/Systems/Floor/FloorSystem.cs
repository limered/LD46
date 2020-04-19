using SystemBase;
using Assets.Systems.Floor.Actions;
using Assets.Systems.Lights;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Systems.Floor
{
    [GameSystem]
    public class FloorSystem : GameSystem<FloorComponent>
    {
        public const int Dimension = 7;

        private readonly FloorTileComponent[,] _matrix = new FloorTileComponent[Dimension, Dimension];

        public override void Register(FloorComponent component)
        {
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    var position = new Vector3(i * component.Scale, 0, -j * component.Scale);
                    var tile = Object.Instantiate(component.FloorTilePrefab, position, Quaternion.identity, component.transform);
                    tile.transform.localScale = new Vector3(component.Scale, 0.1f, component.Scale);
                    _matrix[i, j] = tile.GetComponent<FloorTileComponent>();
                }
            }

            MessageBroker.Default.Receive<ActLightUp>()
                .Subscribe(OnLightUpAction)
                .AddTo(component);
        }

        private void OnLightUpAction(ActLightUp msg)
        {
            if (msg.X >= Dimension || msg.Y >= Dimension)
            {
                return;
            }

            var light = _matrix[msg.X, msg.Y];
            var blinkComponent = light.GetComponent<BlinkComponent>();
            blinkComponent.FadeDuration = msg.FallOff;
            blinkComponent.BlinkColor = msg.BlinkColor;
            blinkComponent.DelayInSec = msg.Delay;
            blinkComponent.Blink();
        }
    }
}
