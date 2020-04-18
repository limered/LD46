using Assets.Systems.Beat;
using System;
using System.Linq;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Assets.Systems.Key
{
    [GameSystem(typeof(BeatSystem))]
    public class KeyPressSystem : GameSystem<BeatSystemConfig>
    {
        private PressedKey _lastPressedKey;
        private float _greenThreshold = 20f;
        private float _yellowThreshold = 60f;
        private readonly string[] _relevantKeys = { "r", "t", "y", "u", "f", "g", "h", "j", "v", "b", "n", "m" };

        public override void Register(BeatSystemConfig component)
        {
            component.BeatTrigger
                .Subscribe(OnBeat)
                .AddTo(component);

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
                _lastPressedKey = new PressedKey
                {
                    Key = input,
                    Timestamp = Time.frameCount
                };
            }
        }

        private void OnBeat(Unit _)
        {
            var beatTime = Time.frameCount;
            var timeDiff = Math.Abs(beatTime - _lastPressedKey.Timestamp);
            if (timeDiff < _greenThreshold)
            {
                Debug.Log("green " + timeDiff);
            }
            else if (timeDiff < _yellowThreshold)
            {
                Debug.Log("yellow " + timeDiff);
            }
            else
            {
                Debug.Log(timeDiff);
            }
        }

        public struct PressedKey
        {
            public string Key { get; set; }
            public float Timestamp { get; set; }
        }
    }
}
