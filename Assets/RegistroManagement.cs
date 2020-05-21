using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class RegistroManagement : MonoBehaviour
{
    public GameObject Inicio;
    private void Awake()
    {
        if (GameManager.configuration != null)
        {
            if (GameManager.configuration.registrado)
            {
                this.gameObject.SetActive(false);
                Inicio.SetActive(true);
            }
        }
    }
}
