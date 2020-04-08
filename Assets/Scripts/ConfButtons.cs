using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ConfButtons : MonoBehaviour
{
    public RectTransform boton1;
    public RectTransform boton2;
    public RectTransform panel;

    float size;
    void Start()
    {
        size = (panel.rect.width /2);

        boton1.sizeDelta = new Vector2(size, boton1.sizeDelta.y);
        boton1.localPosition = new Vector2(-size, boton1.localPosition.y);
        boton2.sizeDelta = new Vector2(size, boton2.sizeDelta.y);
        boton2.localPosition = new Vector2(size, boton2.localPosition.y);

    }

 
   
}
