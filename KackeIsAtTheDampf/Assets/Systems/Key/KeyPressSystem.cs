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
        public override void Register(KeyInfoComponent component)
        {
            SystemUpdate(component)
                .Subscribe(OnUpdate)
                .AddTo(component);
        }

        private void OnUpdate(KeyInfoComponent component)
        {
            if (!Input.anyKeyDown) return;

            var input = Input.inputString;
            if (input.Length == 1 && component.RelevantKeys.Contains(input))
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
