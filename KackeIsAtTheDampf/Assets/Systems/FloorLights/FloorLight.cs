using System;
using System.Collections;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Assets.Systems.FloorLights
{
    public class FloorLight : GameComponent
    {
        public float FadeDuration = 0.3f;
        public int AnimationSteps = 60;
        public Color BaseColor = Color.black;
        public Color BlinkColor = Color.red;
        public float Value;

        private Renderer _thisRenderer;
        private IDisposable _currentCoRoutine;

        protected override void OverwriteStart()
        {
            _thisRenderer = GetComponent<Renderer>();
        }

        public void Blink()
        {
            _currentCoRoutine?.Dispose();
            _currentCoRoutine = Observable.FromCoroutine(BlinkCoroutine).Subscribe();
        }

        private IEnumerator BlinkCoroutine()
        {
            var step = FadeDuration / AnimationSteps;
            for (var i = 0; i < AnimationSteps; i++)
            {
                var t = (float)i / AnimationSteps;
                var mix = 1f - Mathf.Sin((t * Mathf.PI) / 2f);
                Value = mix;
                _thisRenderer.material.color = Color32.Lerp(BaseColor, BlinkColor, mix);
                yield return new WaitForSeconds(step);
            }

            _thisRenderer.material.color = BaseColor;
        }
    }
}