using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistroManagement : MonoBehaviour
{
    public GameObject Inicio;
    private void Awake()
    {
        if (GameManager.configurartion.registrado)
        {
            this.gameObject.SetActive(false);
            Inicio.SetActive(true);
        }
    }
}
