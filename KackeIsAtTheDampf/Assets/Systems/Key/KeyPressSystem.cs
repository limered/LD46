using Assets.Systems.Key.Events;
using System.Linq;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Assets.Systems.Key
{
    [GameSystem]
    public class KeyPressSystem : GameSystem<KeyInfoComponent>
    {
        private readonly string[] _relevantKeys = { "q", "w", "e", "a", "s", "d", "y", "x", "c" };

        public override void Register(KeyInfoComponent component)
        {
            SystemUpdate()
                .Subscribe(OnUpdate)
                .AddTo(component);
        }

        private void OnUpdate(float obj)
        {
            if (!Input.anyKeyDown) return;

            var input = Input.inputString;
            if (input.Length == 1 && _relevantKeys.Contains(input))
            {
                MessageBroker.Default.Publish(new EvtKeyPressed
                {
                    Key = input,
                    Timestamp = Time.realtimeSinceStartup
                });
            }
        }
    }
}
