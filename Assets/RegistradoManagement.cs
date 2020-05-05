using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistradoManagement : MonoBehaviour
{
    private void Awake()
    {
        if (GameManager.configurartion.registrado)
            this.gameObject.SetActive(false);
    }

    
}
