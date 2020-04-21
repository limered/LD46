using System;
using System.Linq;
using SystemBase;
using Assets.Systems.Beat;
using Assets.Systems.BeatChecker;
using Assets.Systems.BeatChecker.Events;
using Assets.Systems.Chorio.Evt;
using GameState.States;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Object = UnityEngine.Object;

namespace Assets.Systems.TargetKeys
{
    [GameSystem(typeof(BeatSystem))]
    public class TargetKeyAnimationSystem : GameSystem<BeatSystemConfig, TargetKeyAnimationConfig, TargetKeyAnimationComponent>
    {
        private readonly ReactiveProperty<BeatSystemConfig> _beatSystemConfig = new ReactiveProperty<BeatSystemConfig>();

        public override void Register(TargetKeyAnimationConfig component)
        {
            component.WaitOn(_beatSystemConfig, config => Register(component, config))
                .AddTo(component);
        }

        private void Register(TargetKeyAnimationConfig config, BeatSystemConfig beatSystemConfig)
        {
            MessageBroker.Default.Receive<EvtNextBeatKeyAdded>()
                .Subscribe(evt => OnNewTargetKeyAdded(evt, config))
                .AddTo(config);
        }

        private void OnNewTargetKeyAdded(EvtNextBeatKeyAdded msg, TargetKeyAnimationConfig config)
        {
            GameObject targetKey = null;
            if (new[] {"w", "e", "r"}.Contains(msg.Key))
            {
                targetKey = Object.Instantiate(config.TargetKeyPinkPrefab, config.Line_Top.transform);
            }
            else if (new[] {"s", "d", "f"}.Contains(msg.Key))
            {
                targetKey = Object.Instantiate(config.TargetKeyPurplePrefab, config.Line_MIddle.transform);
            }
            else if (new[] {"x", "c", "v"}.Contains(msg.Key))
            {
                targetKey = Object.Instantiate(config.TargetKeyGreenPrefab, config.Line_Bottom.transform);
            }

            if (targetKey != null)
            {
                targetKey.transform.localPosition = new Vector3(800, 0);
                targetKey.GetComponentInChildren<Text>().text = msg.Key.ToUpper();

                var animationComponent = targetKey.GetComponent<TargetKeyAnimationComponent>();
                animationComponent.Key = msg.Key;
                animationComponent.PressTime = msg.PlannedBeatTime;
                animationComponent.Backgrounds[1].gameObject.SetActive(false);
                animationComponent.Backgrounds[2].gameObject.SetActive(false);
                animationComponent.Backgrounds[3].gameObject.SetActive(false);
                animationComponent.AnimationDuration = config.PreviewTime;
                animationComponent.AnimationStartTime = msg.PlannedBeatTime - config.PreviewTime;
                animationComponent.AnimationPerSecond = 800 / config.PreviewTime;
                animationComponent.Id = msg.Id;
            }
        }

        public override void Register(TargetKeyAnimationComponent component)
        {
            SystemUpdate(component).Subscribe(OnAnimateKey).AddTo(component);

            MessageBroker.Default.Receive<EvtHitMessage>()
                .Where(message => message.Id == component.Id)
                .Subscribe(msg => ChangeSpriteToFinal(msg, component))
                .AddTo(component);
        }

        private void ChangeSpriteToFinal(EvtHitMessage obj, TargetKeyAnimationComponent targetComponent)
        {
            targetComponent.Backgrounds[0].gameObject.SetActive(false);

            switch (obj.State)
            {
                case BeatKeyState.Green:
                    targetComponent.Backgrounds[1].gameObject.SetActive(true);
                    break;
                case BeatKeyState.Yellow:
                    targetComponent.Backgrounds[2].gameObject.SetActive(true);
                    break;
                case BeatKeyState.Red:
                    targetComponent.Backgrounds[3].gameObject.SetActive(true);
                    break;
            }
        }

        private void OnAnimateKey(TargetKeyAnimationComponent obj)
        {
            SystemUpdate(obj)
                .Subscribe(component =>
                {
                    var timeDelta = component.AnimationStartTime - Time.realtimeSinceStartup;
                    if (timeDelta > 0) return;

                    var animationDistance = 800 + timeDelta * component.AnimationPerSecond;

                    var transform = component.GetComponent<RectTransform>();
                    //var newX = transform.anchoredPosition.x - component.AnimationPerSecond * Time.deltaTime * Time.deltaTime;
                    transform.anchoredPosition = new Vector2(animationDistance, 0);

                    if (transform.anchoredPosition.x < -800)
                    {
                        Object.Destroy(component.gameObject);
                    }
                })
                .AddTo(obj);
        }


        public override void Register(BeatSystemConfig component)
        {
            _beatSystemConfig.Value = component;

            IoC.Game.GameStateContext.CurrentState
                .Where(state => state is GameOver)
                .Subscribe(_ => _beatSystemConfig.Value = null)
                .AddTo(component);
        }
    }
}
