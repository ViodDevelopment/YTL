using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BotonDropDown : MonoBehaviour
{
    public PalabraBD palabraBD;
    public Button myButton;
    public Text myText;
    // Start is called before the first frame update
    void Start()
    {
        myButton = gameObject.GetComponent<Button>();
        myText = gameObject.transform.GetComponentInChildren<Text>();
    }

    public void SetPalabra(PalabraBD _palabra)
    {
        palabraBD = _palabra;
        myText.text = palabraBD.palabraActual;
    }
    
}
