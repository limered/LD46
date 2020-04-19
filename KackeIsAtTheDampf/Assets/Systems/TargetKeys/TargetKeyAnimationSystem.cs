using System;
using System.Linq;
using SystemBase;
using Assets.Systems.Beat;
using Assets.Systems.Chorio.Evt;
using UniRx;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
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
                targetKey = Object.Instantiate(config.TargetKeyPrefab, config.Line_Top.transform);
            }
            else if (new[] {"s", "d", "f"}.Contains(msg.Key))
            {
                targetKey = Object.Instantiate(config.TargetKeyPrefab, config.Line_MIddle.transform);
            }
            else if (new[] {"x", "c", "v"}.Contains(msg.Key))
            {
                targetKey = Object.Instantiate(config.TargetKeyPrefab, config.Line_Bottom.transform);
            }

            if (targetKey != null)
            {
                targetKey.transform.localPosition = new Vector3(800, 0);
                var animationComponent = targetKey.GetComponent<TargetKeyAnimationComponent>();
                animationComponent.Key = msg.Key;
                animationComponent.PressTime = msg.PlannedBeatTime;
                animationComponent.AnimationDuration = config.PreviewTime;
                animationComponent.TimeLeft = Time.realtimeSinceStartup - msg.PlannedBeatTime;
                targetKey.GetComponent<Text>().text = msg.Key;
            }
        }

        public override void Register(TargetKeyAnimationComponent component)
        {
            var xPos = component.transform.localPosition.x;
            component.AnimationPerSecond = xPos / component.AnimationDuration;

            SystemUpdate(component).Subscribe(OnAnimateKey).AddTo(component);
        }

        private void OnAnimateKey(TargetKeyAnimationComponent obj)
        {
            SystemUpdate(obj)
                .Subscribe(component =>
                {
                    var transform = component.GetComponent<RectTransform>();

                    if (!(Time.realtimeSinceStartup > component.PressTime - component.AnimationDuration)) return;

                    var newX = transform.anchoredPosition.x - component.AnimationPerSecond * Time.deltaTime * Time.deltaTime;
                    transform.anchoredPosition = new Vector2(newX, 0);

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
        }
    }
}
