using SystemBase;
using UnityEngine;

namespace Assets.Systems.Camera
{
    public class CameraColorComponent : GameComponent
    {
        public UnityEngine.Camera CameraComponent { get; set; }
        public Color TargetColor;
    }
}