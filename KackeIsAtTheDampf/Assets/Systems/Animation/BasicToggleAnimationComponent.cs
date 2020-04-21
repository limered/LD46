using System;
using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UniRx;
using UnityEngine;
using Utils.Plugins;

namespace Assets.Systems.Animation
{
    public class BasicToggleAnimationComponent : GameComponent
    {
        public const int NotAnimating = -1;
        public bool StartAnimationOnAwake;
        public float AnimationTime = 1f;
        public GameObject[] Sprites;
        public GameObject EndSprite;
        public bool Reverse;
        public bool IsLoop;

        #region Helpers
        public IntReactiveProperty OnSpriteIndexWithoutAnimation { get; } = new IntReactiveProperty(0);

        public ReactiveProperty<Unit> OnShowEndSprite { get; } = new ReactiveProperty<Unit>();

        public int CurrentSprite = NotAnimating;

        public void SetSpriteWithoutAnimation(int index)
        {
            OnSpriteIndexWithoutAnimation.Value = index;
        }

        public void ShowEndSprite()
        {
            OnShowEndSprite.Value = Unit.Default;
        }

        public void StartAnimation()
        {
            CurrentSprite = Reverse ? Sprites.Length - 1 : 0;
        }

        public void StopAnimation()
        {
            CurrentSprite = NotAnimating;
            OnShowEndSprite.Fire();
        }
        #endregion
    }

}