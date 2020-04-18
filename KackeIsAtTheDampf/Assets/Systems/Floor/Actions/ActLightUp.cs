using UnityEngine;

namespace Assets.Systems.Floor.Actions
{
    public struct ActLightUp
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float FallOff { get; set; }
        public float Delay { get; set; }
        public Color BlinkColor { get; set; }
    }
}
