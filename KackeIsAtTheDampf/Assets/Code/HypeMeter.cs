using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HypeMeter : MonoBehaviour
{
    [Range(0, 1)]
    FloatReactiveProperty Hype = new FloatReactiveProperty(1);


    void Start()
    {
        Hype.Subscribe().AddTo(this);
    }

    
    void Update()
    {
        
    }
}
