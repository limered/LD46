using System;
using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;

namespace Systems.Animation
{
    [GameSystem]
    public class AnimationSystem : GameSystem<BasicToggleAnimationComponent, JoeAnimationComponent>
    {
        public override void Register(BasicToggleAnimationComponent component)
        {
            component.FixedUpdateAsObservable()
            .Select(_ => component.CurrentSprite != BasicToggleAnimationComponent.NotAnimating)
            .DistinctUntilChanged()
            .SelectMany(animating => animating ? Observable.FromCoroutine(() => Animate(component)) : Observable.Empty<Unit>())
            .Subscribe()
            .AddTo(component);

            component.OnSpriteIndexWithoutAnimation
            .Subscribe(index =>
            {
                for (var s = 0; s < component.Sprites.Length; s++)
                {
                    component.Sprites[s].SetActive(s == index);
                }
            })
            .AddTo(component);

            component.OnShowEndSprite
            .Subscribe(_ =>
            {
                if (component.EndSprite)
                {
                    for (var s = 0; s < component.Sprites.Length; s++)
                    {
                        component.Sprites[s].SetActive(false);
                    }
                    component.EndSprite.SetActive(true);
                }
            })
            .AddTo(component);

            if (component.StartAnimationOnAwake) component.StartAnimation();
        }

        public override void Register(JoeAnimationComponent comp)
        {
            var animator = comp.GetComponent<Animator>();

            comp.State
            .Subscribe(animationState =>
            {
                animator.Play(animationState);
            })
            .AddTo(comp);
        }

        private IEnumerator Animate(BasicToggleAnimationComponent component)
        {
            var steps = component.Sprites.Length;
            var time = component.AnimationTime;
            var delta = time / steps;

            component.StartAnimation();
            if (component.EndSprite) component.EndSprite.SetActive(true);

            for (var i = 0; i < steps; i++)
            {
                if (component.CurrentSprite == BasicToggleAnimationComponent.NotAnimating) break;
                for (var s = 0; s < component.Sprites.Length; s++)
                {
                    component.Sprites[s].SetActive(component.CurrentSprite == s);
                }

                yield return new WaitForSeconds(delta);
                if (component.CurrentSprite == BasicToggleAnimationComponent.NotAnimating) break;

                component.CurrentSprite += component.Reverse ? -1 : 1;
                component.CurrentSprite = Math.Max(0, component.CurrentSprite);
                component.CurrentSprite = Math.Min(component.CurrentSprite, steps - 1);

                if (i + 1 == steps && component.IsLoop)
                {
                    i = -1;
                    component.CurrentSprite = component.Reverse ? steps - 1 : 0;
                }
            }

            if (component.CurrentSprite != BasicToggleAnimationComponent.NotAnimating)
            {
                if (component.EndSprite)
                {
                    for (var s = 0; s < component.Sprites.Length; s++)
                    {
                        component.Sprites[s].SetActive(false);
                    }
                    component.EndSprite.SetActive(true);
                }

                component.StopAnimation();
            }
        }
    }
}