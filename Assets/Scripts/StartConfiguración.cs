using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartConfiguración : MonoBehaviour
{
    public List<GameObject> buttonsLenguaje = new List<GameObject>();
    public List<GameObject> buttonsFont = new List<GameObject>();
    public List<GameObject> buttonsRepetitions = new List<GameObject>();
    public List<GameObject> buttonsWordsArticle = new List<GameObject>();
    public List<GameObject> buttonsVisualHelp = new List<GameObject>();
    public List<GameObject> buttonsPositiveRefuerzo = new List<GameObject>();
    public List<GameObject> buttonsPaquet = new List<GameObject>();
    private bool done = false;

    private void Update()
    {
        if (GameManager.configuration != null && !done)
        {
            StartConfiguration();
            done = true;
            gameObject.GetComponent<StartConfiguración>().enabled = false;
        }
    }
    public void StartConfiguration()
    {
        int buttonActive = 0;
        switch (GameManager.configuration.currentLenguaje)
        {
            case SingletonLenguage.Lenguage.CASTELLANO:
                buttonActive = 0;
                break;
            case SingletonLenguage.Lenguage.CATALAN:
                buttonActive = 1;
                break;
            case SingletonLenguage.Lenguage.INGLES:
                break;
            case SingletonLenguage.Lenguage.FRANCES:
                break;
        }
        for (int i = 0; i < buttonsLenguaje.Count; i++)
        {
            if (i != buttonActive)
            {
                buttonsLenguaje[i].GetComponent<Image>().sprite = GameManager.GetInstance().DesactivateButton;
                buttonsLenguaje[i].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_PurpleColor;
            }
            else
            {
                buttonsLenguaje[i].GetComponent<Image>().sprite = GameManager.GetInstance().ActiveButton;
                buttonsLenguaje[i].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_WhiteColor;
            }
        }

        buttonActive = 0;

        switch (GameManager.configuration.currentFont)
        {
            case SingletonLenguage.OurFont.MAYUSCULA:
                buttonActive = 0;
                break;
            case SingletonLenguage.OurFont.IMPRENTA:
                buttonActive = 1;
                break;
            case SingletonLenguage.OurFont.MANUSCRITA:
                buttonActive = 2;
                break;
        }

        for (int i = 0; i < buttonsFont.Count; i++)
        {
            if (i != buttonActive)
            {
                buttonsFont[i].GetComponent<Image>().sprite = GameManager.GetInstance().DesactivateButton;
                buttonsFont[i].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_PurpleColor;
            }
            else
            {
                buttonsFont[i].GetComponent<Image>().sprite = GameManager.GetInstance().ActiveButton;
                buttonsFont[i].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_WhiteColor;
            }
        }

        buttonActive = 0;
        switch (GameManager.configuration.repetitionsOfExercise)
        {
            case 2:
                buttonActive = 0;
                break;
            case 3:
                buttonActive = 1;
                break;
            case 4:
                buttonActive = 2;
                break;
            case 5:
                buttonActive = 3;
                break;
            case 6:
                buttonActive = 4;
                break;
        }

        for (int i = 0; i < buttonsRepetitions.Count; i++)
        {
            if (i != buttonActive)
            {
                buttonsRepetitions[i].GetComponent<Image>().sprite = GameManager.GetInstance().DesactivateButton;
                buttonsRepetitions[i].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_PurpleColor;
            }
            else
            {
                buttonsRepetitions[i].GetComponent<Image>().sprite = GameManager.GetInstance().ActiveButton;
                buttonsRepetitions[i].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_WhiteColor;
            }
        }
        /*

        if (GameManager.configuration.palabrasConArticulo)
        {
            buttonsWordsArticle[0].GetComponent<Image>().sprite = GameManager.GetInstance().DesactivateButton;
            buttonsWordsArticle[0].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_PurpleColor;
            if (GameManager.configuration.determinados)
            {
                buttonsWordsArticle[1].GetComponent<Image>().sprite = GameManager.GetInstance().ActiveButton;
                buttonsWordsArticle[1].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_WhiteColor;
                buttonsWordsArticle[2].GetComponent<Image>().sprite = GameManager.GetInstance().DesactivateButton;
                buttonsWordsArticle[2].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_PurpleColor;
            }
            else
            {
                buttonsWordsArticle[2].GetComponent<Image>().sprite = GameManager.GetInstance().ActiveButton;
                buttonsWordsArticle[2].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_WhiteColor;
                buttonsWordsArticle[1].GetComponent<Image>().sprite = GameManager.GetInstance().DesactivateButton;
                buttonsWordsArticle[1].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_PurpleColor;
            }
        }
        else
        {
            buttonsWordsArticle[1].GetComponent<Image>().sprite = GameManager.GetInstance().DesactivateButton;
            buttonsWordsArticle[1].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_PurpleColor;
            buttonsWordsArticle[0].GetComponent<Image>().sprite = GameManager.GetInstance().ActiveButton;
            buttonsWordsArticle[0].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_WhiteColor;
            buttonsWordsArticle[2].GetComponent<Image>().sprite = GameManager.GetInstance().DesactivateButton;
            buttonsWordsArticle[2].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_PurpleColor;
        }*/

        if (!GameManager.configuration.ayudaVisual)
        {
            buttonsVisualHelp[0].GetComponent<Image>().sprite = GameManager.GetInstance().DesactivateButton;
            buttonsVisualHelp[0].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_PurpleColor;
            buttonsVisualHelp[1].GetComponent<Image>().sprite = GameManager.GetInstance().ActiveButton;
            buttonsVisualHelp[1].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_WhiteColor;
        }
        else
        {
            buttonsVisualHelp[1].GetComponent<Image>().sprite = GameManager.GetInstance().DesactivateButton;
            buttonsVisualHelp[1].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_PurpleColor;
            buttonsVisualHelp[0].GetComponent<Image>().sprite = GameManager.GetInstance().ActiveButton;
            buttonsVisualHelp[0].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_WhiteColor;
        }

        if (!GameManager.configuration.refuerzoPositivo)
        {
            buttonsPositiveRefuerzo[0].GetComponent<Image>().sprite = GameManager.GetInstance().DesactivateButton;
            buttonsPositiveRefuerzo[0].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_PurpleColor;
            buttonsPositiveRefuerzo[1].GetComponent<Image>().sprite = GameManager.GetInstance().ActiveButton;
            buttonsPositiveRefuerzo[1].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_WhiteColor;
        }
        else
        {
            buttonsPositiveRefuerzo[1].GetComponent<Image>().sprite = GameManager.GetInstance().DesactivateButton;
            buttonsPositiveRefuerzo[1].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_PurpleColor;
            buttonsPositiveRefuerzo[0].GetComponent<Image>().sprite = GameManager.GetInstance().ActiveButton;
            buttonsPositiveRefuerzo[0].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_WhiteColor;
        }

        buttonActive = 0;
        switch (GameManager.configuration.paquete)
        {
            case -1:
                buttonActive = 0;
                break;
            case 0:
                buttonActive = 1;
                break;
            case 3:
                buttonActive = 2;
                break;
        }

        /*for (int i = 0; i < buttonsPaquet.Count; i++)
        {
            if (i != buttonActive)
            {
                buttonsPaquet[i].GetComponent<Image>().sprite = GameManager.GetInstance().DesactivateButton;
                buttonsPaquet[i].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_PurpleColor;
            }
            else
            {
                buttonsPaquet[i].GetComponent<Image>().sprite = GameManager.GetInstance().ActiveButton;
                buttonsPaquet[i].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_WhiteColor;
            }
        }
        */

    }


}
