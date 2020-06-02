using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonPinguinoLite : MonoBehaviour
{
    public GameObject nivel;
    public GameObject inicio;
    public GameObject backgro;
    public GameObject inicioBack;
    public GameObject animacion;
    public GameObject panelToActivate;

    public int numButton;
    // Start is called before the first frame update
    void Start()
    {
        switch(numButton)
        {
            case 0:
                if(GameManager.configuration.listosBitCompletado && GameManager.configuration.listosParejasCompletado && GameManager.configuration.listosPuzzleCompletado)
                {
                    animacion.GetComponent<Animator>().enabled = false;
                    animacion.GetComponent<SpriteRenderer>().color = new Vector4(animacion.GetComponent<SpriteRenderer>().color.r, animacion.GetComponent<SpriteRenderer>().color.g, animacion.GetComponent<SpriteRenderer>().color.b, 0.6f);
                }
                    break;
            case 1:
                if (GameManager.configuration.yaBitCompletado && GameManager.configuration.yaParejasCompletado && GameManager.configuration.yaPuzzleCompletado)
                {
                    animacion.GetComponent<Animator>().enabled = false;
                    animacion.GetComponent<SpriteRenderer>().color = new Vector4(animacion.GetComponent<SpriteRenderer>().color.r, animacion.GetComponent<SpriteRenderer>().color.g, animacion.GetComponent<SpriteRenderer>().color.b, 0.6f);
                }
                break;
        }
    }

    public void SetAction()
    {
        switch (numButton)
        {
            case 0:
                if (GameManager.configuration.listosBitCompletado && GameManager.configuration.listosParejasCompletado && GameManager.configuration.listosPuzzleCompletado)
                {
                    panelToActivate.SetActive(true);
                }
                else
                {
                    nivel.SetActive(true);
                    inicio.SetActive(false);
                    backgro.SetActive(true);
                    inicioBack.SetActive(true);
                }
                break;
            case 1:
                if (GameManager.configuration.yaBitCompletado && GameManager.configuration.yaParejasCompletado && GameManager.configuration.yaPuzzleCompletado)
                {
                    panelToActivate.SetActive(true);
                }
                else
                {
                    nivel.SetActive(true);
                    inicio.SetActive(false);
                    backgro.SetActive(true);
                    inicioBack.SetActive(true);
                }
                break;
        }
    }


}
