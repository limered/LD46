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
        private readonly FloorTileComponent[,] _matrix = new FloorTileComponent[8, 8];

        public override void Register(FloorComponent component)
        {
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    var position = new Vector3(i, 0, -j);
                    var tile = Object.Instantiate(component.FloorTilePrefab, position, Quaternion.identity,
                        component.transform);
                    _matrix[i, j] = tile.GetComponent<FloorTileComponent>();
                }
            }

            MessageBroker.Default.Receive<ActLightUp>()
                .Subscribe(OnLightUpAction)
                .AddTo(component);
        }

        private void OnLightUpAction(ActLightUp msg)
        {
            if (msg.X > 7 || msg.Y > 7)
            {
                return;
            }

            var light = _matrix[msg.X, msg.Y];
            var blinkComponent = light.GetComponent<BeatBlinkComponent>();
            blinkComponent.FadeDuration = msg.FallOff;
            blinkComponent.BlinkColor = msg.BlinkColor;
            blinkComponent.DelayInSec = msg.Delay;
            blinkComponent.Blink();
        }
    }
}
