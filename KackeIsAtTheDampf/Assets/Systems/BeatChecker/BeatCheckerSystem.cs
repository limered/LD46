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
    public enum BeatKeyState
    {
        None, Prepare, Green, Yellow, Red
    }

    [GameSystem(typeof(KeyPressSystem), typeof(BeatSystem))]
    public class BeatCheckerSystem : GameSystem<BeatSystemConfig>
    {
        private readonly Queue<BeatKeyInfo> _nextKeysToPress = new Queue<BeatKeyInfo>();
        private BeatInfo _lastBeatInfo;
        private EvtKeyPressed _lastKeyPressed;
        private BeatSystemConfig _beatSystemConfig;

        public override void Register(BeatSystemConfig component)
        {
            _beatSystemConfig = component;

            component.BeatTrigger
                .Where(i => _nextKeysToPress.Any(info => info.BeatNo == i.BeatNo))
                .Select(info => new Tuple<BeatInfo, BeatKeyInfo>(info, _nextKeysToPress.Dequeue()))
                .Delay(TimeSpan.FromSeconds(component.YellowCheckDuration))
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
            if (nextBeatKey - _beatSystemConfig.YellowCheckDuration > obj.Timestamp)
            {
                _nextKeysToPress.Peek().State = BeatKeyState.Red;

                //Debug.Log("Fail " + nextBeatKey);
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

            //Debug.Log("Enque " + obj.BeatNo);
            foreach (var beatKeyInfo in _nextKeysToPress.ToArray())
            {
                Debug.Log(beatKeyInfo.BeatNo);
            }
        }

        private void OnBeatDelayed(Tuple<BeatInfo, BeatKeyInfo> beatInfoTuple)
        {
            var beatInfo = beatInfoTuple.Item1;
            var currentBeatInfo = beatInfoTuple.Item2;

            //Debug.Log("Trigger " + beatInfo.BeatNo);

            if (currentBeatInfo.State == BeatKeyState.Red)
            {
                // Send Fail
                //Debug.Log("Fail 1 " + beatInfo.BeatNo);
                return;
            }
            if (currentBeatInfo.KeyToPress != _lastKeyPressed.Key)
            {
                currentBeatInfo.State = BeatKeyState.Red;
                // Send Fail
                //Debug.Log("Fail 2 " + beatInfo.BeatNo);
                return;
            }

            var timeDelta = Time.realtimeSinceStartup - _lastKeyPressed.Timestamp;
            if (timeDelta < _beatSystemConfig.GreenCheckDuration * 2)
            {
                currentBeatInfo.State = BeatKeyState.Green;
                Debug.Log("Green " + beatInfo.BeatNo);
                // Send MaxPts
            }
            else if (timeDelta < _beatSystemConfig.YellowCheckDuration * 2)
            {
                currentBeatInfo.State = BeatKeyState.Yellow;
                // Send Normal Points
                Debug.Log("Yellow " + beatInfo.BeatNo);
            }
            else
            {
                currentBeatInfo.State = BeatKeyState.Red;
                // Send Fail
                //Debug.Log("Fail 3 " + beatInfo.BeatNo);
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
}
