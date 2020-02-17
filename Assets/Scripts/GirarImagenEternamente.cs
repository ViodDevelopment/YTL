using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GirarImagenEternamente : MonoBehaviour
{
    private bool activo = false;
    public bool parar = false;
    public bool acabado = false;
    public LoadingScene loadingScene;
    public Quaternion initForward;
    public float speed = 550;
    // Update is called once per frame

    private void Start()
    {
        initForward = transform.rotation;
    }
    void Update()
    {
        if (!acabado)
        {
            if (activo)
                gameObject.transform.RotateAround(gameObject.transform.position, Vector3.forward, -speed * Time.deltaTime);
            if (activo && parar)
            {
                if (Mathf.Abs(Quaternion.Angle(gameObject.transform.rotation, initForward)) < 5)
                {
                    gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
                    acabado = true;
                }
            }
        }
    }

    public void TurnActivo(bool _activo)
    {
        if (!loadingScene.doing)
        {
            activo = _activo;
            loadingScene.myimage = this;
            GameObject.Find("InicioPrep").GetComponent<Button>().interactable = false;
        }
    }

}
