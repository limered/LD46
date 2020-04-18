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

namespace Systems.Hype
{
    [GameSystem]
    public class HypeSystem : GameSystem<HypeGeneratorComponent>
    {
        private float _totalHype;

        public override void Init()
        {
            //could be added to IoC
            IoC.RegisterSingleton(this);
        }

        public override void Register(HypeGeneratorComponent comp)
        {
            if(!comp) throw new System.Exception("missing "+nameof(HypeGeneratorComponent));

            comp.OnHype();
        }
    }
}