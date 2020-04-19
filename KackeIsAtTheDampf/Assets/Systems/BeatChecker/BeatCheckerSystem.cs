using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using Assets.Systems.Key;
using Assets.Systems.Key.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Assets.Systems.BeatChecker
{
    [GameSystem(typeof(KeyPressSystem), typeof(BeatSystem))]
    public class BeatCheckerSystem : GameSystem<BeatSystemConfig>
    {
        private float _yellowCheckDuration = 0.6f;
        private float _greenCheckDuration = 0.2f;

        private readonly Queue<BeatKeyInfo> _nextKeysToPress = new Queue<BeatKeyInfo>();
        private BeatInfo _lastBeatInfo;
        private EvtKeyPressed _lastKeyPressed;
        private BeatSystemConfig _beatSystemConfig;

        public override void Register(BeatSystemConfig component)
        {
            _beatSystemConfig = component;

            component.BeatTrigger
                .Where(i => _nextKeysToPress.Any() && _nextKeysToPress.Peek().BeatNo == i.BeatNo)
                .Delay(TimeSpan.FromSeconds(_yellowCheckDuration))
                .Subscribe(OnBeatDelayed)
                .AddTo(component);

            component.BeatTrigger
                .Subscribe(i =>
                {
                    _lastBeatInfo = i;
                })
                .AddTo(component);

            MessageBroker.Default.Receive<EvtKeyPressed>()
                .Subscribe(CheckKEyEventAgainstQueue)
                .AddTo(component);

            MessageBroker.Default.Receive<EvtNextBeatKeyAdded>()
                .Subscribe(AddNextBeatKeyToQueue)
                .AddTo(component);
        }

        private void CheckKEyEventAgainstQueue(EvtKeyPressed obj)
        {
            _lastKeyPressed = obj;

            if (!_nextKeysToPress.Any()) return;

            var nextBeatKey = GetNextKeyToPressTime();
            if (nextBeatKey - _yellowCheckDuration > obj.Timestamp)
            {
                _nextKeysToPress.Peek().State = BeatKeyState.Red;

                Debug.Log("Fail " + nextBeatKey);
            }
        }

        private float GetNextKeyToPressTime()
        {
            var nextKey = _nextKeysToPress.Peek();
            var beatDelta = nextKey.BeatNo - _lastBeatInfo.BeatNo;
            var timePerBeat = 60f / _beatSystemConfig.BPM.Value;
            return _lastBeatInfo.BeatTime + beatDelta * timePerBeat;
        }

        private void AddNextBeatKeyToQueue(EvtNextBeatKeyAdded obj)
        {
            _nextKeysToPress.Enqueue(new BeatKeyInfo
            {
                BeatNo = obj.BeatNo,
                TimeToPress = obj.PlannedBeatTime,
                KeyToPress = obj.Key,
                State = BeatKeyState.None,
            });

            Debug.Log("Enque " + obj.BeatNo);
        }

        private void OnBeatDelayed(BeatInfo beatInfo)
        {
            if (!_nextKeysToPress.Any()) return;

            Debug.Log("Trigger " + beatInfo.BeatTime);

            var currentBeatInfo = _nextKeysToPress.Dequeue();
            if (currentBeatInfo.State == BeatKeyState.Red)
            {
                // Send Fail
                Debug.Log("Fail 1 " + beatInfo.BeatTime);
                return;
            }
            if (currentBeatInfo.KeyToPress != _lastKeyPressed.Key)
            {
                currentBeatInfo.State = BeatKeyState.Red;
                // Send Fail
                Debug.Log("Fail 2 " + beatInfo.BeatTime);
                return;
            }

            var timeDelta = Time.realtimeSinceStartup - _lastKeyPressed.Timestamp;
            if (timeDelta < _greenCheckDuration * 2)
            {
                currentBeatInfo.State = BeatKeyState.Green;
                Debug.Log("Green " + beatInfo.BeatTime);
                // Send MaxPts
            }
            else if (timeDelta < _yellowCheckDuration * 2)
            {
                currentBeatInfo.State = BeatKeyState.Yellow;
                // Send Normal Points
                Debug.Log("Yellow " + beatInfo.BeatTime);
            }
            else
            {
                currentBeatInfo.State = BeatKeyState.Red;
                // Send Fail
                Debug.Log("Fail 3 " + beatInfo.BeatTime);
            }
        }
    }

    public class BeatKeyInfo
    {
        public int BeatNo;
        public float TimeToPress;
        public string KeyToPress;
        public BeatKeyState State;
    }

    public enum BeatKeyState
    {
        None, Prepare, Green, Yellow, Red
    }
}
