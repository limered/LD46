using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameState.States;
using SystemBase;
using SystemBase.StateMachineBase;
using Systems.GameState.Messages;
using StrongSystems.Audio.Helper;
using UniRx;
using Utils;
using System;

namespace Systems.Hype
{
    public class HypeGeneratorComponent : GameComponent
    {
        public float HypeFactor = 1;

        private readonly Subject<float> _hyper = new Subject<float>();

        public IObservable<float> OnHype() => _hyper;
        public void TriggerHypeReaction() => _hyper.OnNext(HypeFactor);
    }
}