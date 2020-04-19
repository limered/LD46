using SystemBase;
using UnityEngine;

namespace Assets.Systems.TargetKeys
{
    public class TargetKeyAnimationConfig : GameComponent
    {
        public GameObject Line_Top;
        public GameObject Line_MIddle;
        public GameObject Line_Bottom;

        public GameObject TargetKeyPinkPrefab;
        public GameObject TargetKeyGreenPrefab;
        public GameObject TargetKeyPurplePrefab;
        public float PreviewTime = 3;
    }
}