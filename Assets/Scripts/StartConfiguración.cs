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
        if (GameManager.configurartion != null && !done)
        {
            StartConfiguration();
            done = true;
            gameObject.GetComponent<StartConfiguración>().enabled = false;
        }
    }
    public void StartConfiguration()
    {
        int buttonActive = 0;
        switch (GameManager.configurartion.currentLenguaje)
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
                buttonsLenguaje[i].GetComponent<Image>().sprite = GameManager.Instance.DesactivateButton;
                buttonsLenguaje[i].GetComponentInChildren<Text>().color = GameManager.Instance.m_PurpleColor;
            }
            else
            {
                buttonsLenguaje[i].GetComponent<Image>().sprite = GameManager.Instance.ActiveButton;
                buttonsLenguaje[i].GetComponentInChildren<Text>().color = GameManager.Instance.m_WhiteColor;
            }
        }

        buttonActive = 0;

        switch (GameManager.configurartion.currentFont)
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
                buttonsFont[i].GetComponent<Image>().sprite = GameManager.Instance.DesactivateButton;
                buttonsFont[i].GetComponentInChildren<Text>().color = GameManager.Instance.m_PurpleColor;
            }
            else
            {
                buttonsFont[i].GetComponent<Image>().sprite = GameManager.Instance.ActiveButton;
                buttonsFont[i].GetComponentInChildren<Text>().color = GameManager.Instance.m_WhiteColor;
            }
        }

        buttonActive = 0;
        switch (GameManager.configurartion.repetitionsOfExercise)
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
                buttonsRepetitions[i].GetComponent<Image>().sprite = GameManager.Instance.DesactivateButton;
                buttonsRepetitions[i].GetComponentInChildren<Text>().color = GameManager.Instance.m_PurpleColor;
            }
            else
            {
                buttonsRepetitions[i].GetComponent<Image>().sprite = GameManager.Instance.ActiveButton;
                buttonsRepetitions[i].GetComponentInChildren<Text>().color = GameManager.Instance.m_WhiteColor;
            }
        }


        if (GameManager.configurartion.palabrasConArticulo)
        {
            buttonsWordsArticle[0].GetComponent<Image>().sprite = GameManager.Instance.DesactivateButton;
            buttonsWordsArticle[0].GetComponentInChildren<Text>().color = GameManager.Instance.m_PurpleColor;
            buttonsWordsArticle[1].GetComponent<Image>().sprite = GameManager.Instance.ActiveButton;
            buttonsWordsArticle[1].GetComponentInChildren<Text>().color = GameManager.Instance.m_WhiteColor;
        }
        else
        {
            buttonsWordsArticle[1].GetComponent<Image>().sprite = GameManager.Instance.DesactivateButton;
            buttonsWordsArticle[1].GetComponentInChildren<Text>().color = GameManager.Instance.m_PurpleColor;
            buttonsWordsArticle[0].GetComponent<Image>().sprite = GameManager.Instance.ActiveButton;
            buttonsWordsArticle[0].GetComponentInChildren<Text>().color = GameManager.Instance.m_WhiteColor;
        }

        if (!GameManager.configurartion.ayudaVisual)
        {
            buttonsVisualHelp[0].GetComponent<Image>().sprite = GameManager.Instance.DesactivateButton;
            buttonsVisualHelp[0].GetComponentInChildren<Text>().color = GameManager.Instance.m_PurpleColor;
            buttonsVisualHelp[1].GetComponent<Image>().sprite = GameManager.Instance.ActiveButton;
            buttonsVisualHelp[1].GetComponentInChildren<Text>().color = GameManager.Instance.m_WhiteColor;
        }
        else
        {
            buttonsVisualHelp[1].GetComponent<Image>().sprite = GameManager.Instance.DesactivateButton;
            buttonsVisualHelp[1].GetComponentInChildren<Text>().color = GameManager.Instance.m_PurpleColor;
            buttonsVisualHelp[0].GetComponent<Image>().sprite = GameManager.Instance.ActiveButton;
            buttonsVisualHelp[0].GetComponentInChildren<Text>().color = GameManager.Instance.m_WhiteColor;
        }

        if (!GameManager.configurartion.refuerzoPositivo)
        {
            buttonsPositiveRefuerzo[0].GetComponent<Image>().sprite = GameManager.Instance.DesactivateButton;
            buttonsPositiveRefuerzo[0].GetComponentInChildren<Text>().color = GameManager.Instance.m_PurpleColor;
            buttonsPositiveRefuerzo[1].GetComponent<Image>().sprite = GameManager.Instance.ActiveButton;
            buttonsPositiveRefuerzo[1].GetComponentInChildren<Text>().color = GameManager.Instance.m_WhiteColor;
        }
        else
        {
            buttonsPositiveRefuerzo[1].GetComponent<Image>().sprite = GameManager.Instance.DesactivateButton;
            buttonsPositiveRefuerzo[1].GetComponentInChildren<Text>().color = GameManager.Instance.m_PurpleColor;
            buttonsPositiveRefuerzo[0].GetComponent<Image>().sprite = GameManager.Instance.ActiveButton;
            buttonsPositiveRefuerzo[0].GetComponentInChildren<Text>().color = GameManager.Instance.m_WhiteColor;
        }

        buttonActive = 0;
        switch (GameManager.configurartion.paquete)
        {
            case 0:
                buttonActive = 0;
                break;
            case 1:
                buttonActive = 1;
                break;
            case 2:
                buttonActive = 2;
                break;
            case 3:
                buttonActive = 3;
                break;
            case 4:
                buttonActive = 4;
                break;
        }

        for (int i = 0; i < buttonsPaquet.Count; i++)
        {
            if (i != buttonActive)
            {
                buttonsPaquet[i].GetComponent<Image>().sprite = GameManager.Instance.DesactivateButton;
                buttonsPaquet[i].GetComponentInChildren<Text>().color = GameManager.Instance.m_PurpleColor;
            }
            else
            {
                buttonsPaquet[i].GetComponent<Image>().sprite = GameManager.Instance.ActiveButton;
                buttonsPaquet[i].GetComponentInChildren<Text>().color = GameManager.Instance.m_WhiteColor;
            }
        }


    }


}
