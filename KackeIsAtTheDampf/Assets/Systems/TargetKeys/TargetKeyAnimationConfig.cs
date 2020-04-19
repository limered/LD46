using SystemBase;
using UnityEngine;

namespace Assets.Systems.TargetKeys
{
    public class TargetKeyAnimationConfig : GameComponent
    {
        public GameObject Line_Top;
        public GameObject Line_MIddle;
        public GameObject Line_Bottom;

        public GameObject TargetKeyPrefab;
        public float PreviewTime = 3;
    }
}