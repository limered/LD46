using SystemBase;
using UnityEngine;

namespace Assets.Systems.Dancer
{
    public class DancerComponent : GameComponent
    {
        public DanceType Type;
        public Vector3 StartPosition;
        public Vector3 DancePosition;
        public Vector3 TargetPosition { get; set; }
    }
}
