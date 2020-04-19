using Assets.Systems.Beat;
using Assets.Systems.BeatChecker.Events;
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
        private BeatSystemConfig _beatSystemConfig;

        public override void Register(BeatSystemConfig component)
        {
            _beatSystemConfig = component;

            MessageBroker.Default.Receive<EvtKeyPressed>()
                .Subscribe(CheckKEyEventAgainstQueue)
                .AddTo(component);

            MessageBroker.Default.Receive<EvtNextBeatKeyAdded>()
                .Subscribe(AddNextBeatKeyToQueue)
                .AddTo(component);

            SystemUpdate().Subscribe(CleanUpTriggerKeys).AddTo(component);
        }

        private void CleanUpTriggerKeys(float obj)
        {
            while (_nextKeysToPress.Any(info =>
                info.TimeToPress + _beatSystemConfig.YellowCheckDuration < Time.realtimeSinceStartup))
            {
                var fail = _nextKeysToPress.Dequeue();
                if (fail.State == BeatKeyState.None)
                {
                    fail.State = BeatKeyState.Red;
                    MessageBroker.Default.Publish(new EvtHitMessage
                    {
                        Id = fail.Id,
                        State = BeatKeyState.Red,
                        TimeStamp = Time.realtimeSinceStartup,
                        DistanceToOptimum = Math.Abs(fail.TimeToPress - Time.realtimeSinceStartup)
                    });
                }
            }
        }

        private void CheckKEyEventAgainstQueue(EvtKeyPressed obj)
        {
            if (!_nextKeysToPress.Any()) return;

            var nextKeyToPress = _nextKeysToPress.Peek();
            if (nextKeyToPress.State == BeatKeyState.Red) return;

            var nextTimeStamp = nextKeyToPress.TimeToPress;

            bool CheckIfInGreen()
            {
                return obj.Timestamp > nextTimeStamp - _beatSystemConfig.GreenCheckDuration
                       && obj.Timestamp < nextTimeStamp + _beatSystemConfig.GreenCheckDuration;
            }

            bool CheckIfInYellow()
            {
                return obj.Timestamp > nextTimeStamp - _beatSystemConfig.YellowCheckDuration
                       && obj.Timestamp < nextTimeStamp + _beatSystemConfig.YellowCheckDuration;
            }

            if (nextKeyToPress.KeyToPress != obj.Key)
            {
                nextKeyToPress.State = BeatKeyState.Red;
                MessageBroker.Default.Publish(new EvtHitMessage
                {
                    Id = nextKeyToPress.Id,
                    State = BeatKeyState.Red,
                    TimeStamp = obj.Timestamp,
                    DistanceToOptimum = Math.Abs(nextTimeStamp - obj.Timestamp)
                });
                return;
            }

            if (CheckIfInGreen())
            {
                var hit = _nextKeysToPress.Dequeue();
                hit.State = BeatKeyState.Green;
                MessageBroker.Default.Publish(new EvtHitMessage
                {
                    Id = nextKeyToPress.Id,
                    State = BeatKeyState.Green,
                    TimeStamp = obj.Timestamp,
                    DistanceToOptimum = Math.Abs(nextTimeStamp - obj.Timestamp)
                });
                return;
            }

            if (CheckIfInYellow())
            {
                var hit = _nextKeysToPress.Dequeue();
                hit.State = BeatKeyState.Yellow;
                MessageBroker.Default.Publish(new EvtHitMessage
                {
                    Id = nextKeyToPress.Id,
                    State = BeatKeyState.Yellow,
                    TimeStamp = obj.Timestamp,
                    DistanceToOptimum = Math.Abs(nextTimeStamp - obj.Timestamp)
                });
                return;
            }

            nextKeyToPress.State = BeatKeyState.Red;
            MessageBroker.Default.Publish(new EvtHitMessage
            {
                Id = nextKeyToPress.Id,
                State = BeatKeyState.Red,
                TimeStamp = obj.Timestamp,
                DistanceToOptimum = Math.Abs(nextTimeStamp - obj.Timestamp)
            });
        }

        private void AddNextBeatKeyToQueue(EvtNextBeatKeyAdded obj)
        {
            _nextKeysToPress.Enqueue(new BeatKeyInfo
            {
                Id = obj.Id,
                BeatNo = obj.BeatNo,
                TimeToPress = obj.PlannedBeatTime,
                KeyToPress = obj.Key,
                State = BeatKeyState.None,
            });
        }
    }

    public class BeatKeyInfo
    {
        public int Id;
        public int BeatNo;
        public float TimeToPress;
        public string KeyToPress;
        public BeatKeyState State;
    }
}
