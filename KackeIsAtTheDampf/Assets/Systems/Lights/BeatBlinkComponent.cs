using System;
using System.Collections;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Assets.Systems.Lights
{
    public class BeatBlinkComponent : GameComponent
    {
        public float FadeDuration = 0.3f;
        public int AnimationSteps = 60;
        public float DelayInSec = 0;
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
            yield return new WaitForSecondsRealtime(DelayInSec);
            var step = FadeDuration / AnimationSteps;
            for (var i = 0; i < AnimationSteps; i++)
            {
                var t = (float)i / AnimationSteps;
                var mix = 1f - Mathf.Sin((t * Mathf.PI) / 2f);
                Value = mix;
                var emission = Color32.Lerp(BaseColor, BlinkColor, mix);
                _thisRenderer.material.SetColor("_EmissionColor", emission);
                yield return new WaitForSecondsRealtime(step);
            }

            _thisRenderer.material.color = BaseColor;
        }
    }
}