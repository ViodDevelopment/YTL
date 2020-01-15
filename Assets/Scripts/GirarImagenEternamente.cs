using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirarImagenEternamente : MonoBehaviour
{
    public bool activo = false;
    // Update is called once per frame
    void Update()
    {
        if (activo)
            gameObject.transform.RotateAround(gameObject.transform.position ,Vector3.forward, -500 * Time.deltaTime);
    }
}
