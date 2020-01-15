using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirarImagenEternamente : MonoBehaviour
{
    private bool activo = false;
    public LoadingScene loadingScene;
    // Update is called once per frame
    void Update()
    {
        if (activo)
            gameObject.transform.RotateAround(gameObject.transform.position ,Vector3.forward, -500 * Time.deltaTime);
    }

    public void TurnActivo(bool _activo)
    {
        if(!loadingScene.doing)
            activo = _activo;
    }
}
