﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SilabaUnseedColocarMarco : MonoBehaviour
{
    public List<Sprite> marcos = new List<Sprite>();
    public Image imagen;
    
    public void SetMarco(int _num)
    {
        switch(_num)
        {
            case -1:
                imagen.sprite = marcos[0];
                break;
            case 0:
                imagen.sprite = marcos[1];
                break;
            case 1:
                imagen.sprite = marcos[2];
                break;
        }
    }
}