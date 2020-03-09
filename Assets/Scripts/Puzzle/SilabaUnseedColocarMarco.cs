using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SilabaUnseedColocarMarco : MonoBehaviour
{
    public List<Sprite> marcos = new List<Sprite>();
    public Image imagen;
    public Text texto;
    public Image fondo;
    public List<Sprite> spritesFondo = new List<Sprite>();


    public void SetMarco(int _num)
    {
        switch(_num)
        {
            case -1:
                imagen.sprite = marcos[0];
                fondo.sprite = spritesFondo[0];
                fondo.rectTransform.anchoredPosition = new Vector3(4, 0, 0);

                texto.transform.position += new Vector3(-0.13f, 0, 0);
                break;
            case 0:
                imagen.sprite = marcos[1];
                fondo.sprite = spritesFondo[1];
                fondo.rectTransform.sizeDelta = new Vector2(315, 287.5f);
                imagen.rectTransform.sizeDelta = new Vector2(315, 319);
                fondo.rectTransform.anchoredPosition = new Vector3(0, 0, 0);

                break;
            case 1:
                imagen.sprite = marcos[2];
                fondo.sprite = spritesFondo[2];
                fondo.rectTransform.anchoredPosition = new Vector3(-4, 0, 0);
                texto.transform.position += new Vector3(0.13f, 0, 0);
                break;
            case 2:
                imagen.sprite = marcos[3];
                fondo.sprite = spritesFondo[3];
                fondo.rectTransform.sizeDelta = new Vector3(imagen.rectTransform.sizeDelta.x * 1.1f, imagen.rectTransform.sizeDelta.y, 0);
                imagen.rectTransform.sizeDelta = new Vector3(imagen.rectTransform.sizeDelta.x*1.2f, imagen.rectTransform.sizeDelta.y, 0);
                break;
            case 3: //Monnosilabo
                imagen.sprite = marcos[4];
                fondo.sprite = spritesFondo[3];
                fondo.rectTransform.localScale = new Vector3(0.65f, 0.7f, 0);
                break;
        }
    }
}
