using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSystem : MonoBehaviour
{
    void Start()
    {
        var throwComponent = gameObject.GetComponent<ThrowComponent>();
        throwComponent.ThrowTypes[0].gameObject.SetActive(true);
        throwComponent.ThrowTypes[1].gameObject.SetActive(false);
        throwComponent.ThrowTypes[2].gameObject.SetActive(false);
    }
}
