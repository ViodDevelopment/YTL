using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInSeconds : MonoBehaviour
{
    public float TimeInSeconds;

    void Start()
    {
        Destroy(this.gameObject, TimeInSeconds);  
    }
}
