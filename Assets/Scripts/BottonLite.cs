using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottonLite : MonoBehaviour
{
    public SceneManagement sceneManagement;
    public int numButton;
    public GameObject panelToActivate;
    private Button myButton;
    private GirarImagenEternamente myGirar;

    public GameObject panelFullVersion;

    private void Start()
    {
        myButton = GetComponent<Button>();
        myGirar = GetComponent<GirarImagenEternamente>();
        switch (numButton)
        {
            case 0:
                if (GameManager.configuration.listosParejasCompletado)
                {
                    myButton.image.color = new Vector4(myButton.image.color.r, myButton.image.color.g, myButton.image.color.b, 0.6f);
                }
                break;
            case 1:
                if (GameManager.configuration.listosBitCompletado)
                {
                    myButton.image.color = new Vector4(myButton.image.color.r, myButton.image.color.g, myButton.image.color.b, 0.6f);

                }
                break;
            case 2:
                if (GameManager.configuration.listosPuzzleCompletado)
                {
                    myButton.image.color = new Vector4(myButton.image.color.r, myButton.image.color.g, myButton.image.color.b, 0.6f);

                }
                break;
            case 3:
                if (GameManager.configuration.yaParejasCompletado)
                {
                    myButton.image.color = new Vector4(myButton.image.color.r, myButton.image.color.g, myButton.image.color.b, 0.6f);

                }
                break;
            case 4:
                if (GameManager.configuration.yaBitCompletado)
                {
                    myButton.image.color = new Vector4(myButton.image.color.r, myButton.image.color.g, myButton.image.color.b, 0.6f);

                }
                break;
            case 5:
                if (GameManager.configuration.yaPuzzleCompletado)
                {
                    myButton.image.color = new Vector4(myButton.image.color.r, myButton.image.color.g, myButton.image.color.b, 0.6f);

                }
                break;
        }
    }

    public void SetAction()
    {
        switch(numButton)
        {
            case 0:
                if(!GameManager.configuration.listosParejasCompletado)
                {
                    myGirar.TurnActivo(true);
                    sceneManagement.LoadParejas(2);
                }
                else
                {
                    panelFullVersion.SetActive(true);
                }
                break;
            case 1:
                if(!GameManager.configuration.listosBitCompletado)
                {
                    myGirar.TurnActivo(true);
                    sceneManagement.LoadBit(2);
                }
                else
                {
                    panelFullVersion.SetActive(true);

                }
                break;
            case 2:
                if (!GameManager.configuration.listosPuzzleCompletado)
                {
                    myGirar.TurnActivo(true);
                    sceneManagement.LoadPuzzle(2);
                }
                else
                {
                    panelFullVersion.SetActive(true); 

                }
                break;
            case 3:
                if (!GameManager.configuration.yaParejasCompletado)
                {
                    myGirar.TurnActivo(true);
                    sceneManagement.LoadParejas(3);
                }
                else
                {
                    panelFullVersion.SetActive(true);

                }
                break;
            case 4:
                if (!GameManager.configuration.yaBitCompletado)
                {
                    myGirar.TurnActivo(true);
                    sceneManagement.LoadBit(3);
                }
                else
                {
                    panelFullVersion.SetActive(true);

                }
                break;
            case 5:
                if (!GameManager.configuration.yaPuzzleCompletado)
                {
                    myGirar.TurnActivo(true);
                    sceneManagement.LoadPuzzle(3);
                }
                else
                {
                    panelFullVersion.SetActive(true);

                }
                break;
        }
    }
}
