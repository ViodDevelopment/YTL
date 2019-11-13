using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    Button m_MyButton;
    public int m_Position;
    public GameObject[] m_Buttons;

    int m_CurrentEditWord =1;

    private void Awake()
    {
        m_MyButton = GetComponent<Button>();
    }

    private void Start()
    {
        m_MyButton.onClick.AddListener(() => ButtonUI(m_Position, m_Buttons));
    }

    public void ButtonUI(int l_Position, GameObject[] l_Buttons)
    {
        for (int i = 0; i < l_Buttons.Length; i++)
        {
            if (i == l_Position)
            {
                l_Buttons[i].GetComponent<Image>().sprite = GameManager.Instance.ActiveButton;
                l_Buttons[i].GetComponentInChildren<Text>().color = GameManager.Instance.m_WhiteColor;
            }
            else
            {
                l_Buttons[i].GetComponent<Image>().sprite = GameManager.Instance.DesactivateButton;
                l_Buttons[i].GetComponentInChildren<Text>().color = GameManager.Instance.m_PurpleColor;
            }
        }
    }

    public void Idioma(string l_Idioma)
    {
        GameManager.Instance.Idioma = l_Idioma;
    }

    public void TipoLetraMayus(bool l_Mayus)
    {
        GameManager.Instance.Mayus = l_Mayus;
    }

    public void TipoLetra(Font l_Font)
    {
        GameManager.Instance.TipoLetra = l_Font;
    }

    public void Ayuda(bool l_Ayuda)
    {
        GameManager.Instance.Ayuda = l_Ayuda;
    }

    public void Animacion(bool l_Animacion)
    {
        GameManager.Instance.Dumi = l_Animacion;
    }

    public void Articulo(bool l_Articulo)
    {
        GameManager.Instance.Articulo = l_Articulo;
    }

    public void Repeticiones(int l_Repeticiones)
    {
        GameManager.Repeticiones = l_Repeticiones;
    }
   
    public void Pack(int l_Packs)
    {
        GameManager.Packs = l_Packs;
    }

    public void Dificultad(string Dificultad)
    {
        GameManager.Instance.WordDifficulty = Dificultad;
    }

    public void PalabraEditar(Text l_Texto)
    {
        m_CurrentEditWord++;
        if (m_CurrentEditWord > 8)
            m_CurrentEditWord = 1;
        l_Texto.text = m_CurrentEditWord.ToString() +" de 8";
    }

    public void SetFont(int _font)
    {
        switch (_font)
        {
            case 0:
                SingletonLenguage.GetInstance().SetFont(SingletonLenguage.OurFont.MAYUSCULA);
                break;
            case 1:
                SingletonLenguage.GetInstance().SetFont(SingletonLenguage.OurFont.IMPRENTA);
                break;
            case 2:
                SingletonLenguage.GetInstance().SetFont(SingletonLenguage.OurFont.MANUSCRITA);
                break;
        }
    }

    public void SetLenguage(int _lengauge)
    {
        switch(_lengauge)
        {
            case 0:
                SingletonLenguage.GetInstance().SetLenguage(SingletonLenguage.Lenguage.CASTELLANO);
                break;
            case 1:
                SingletonLenguage.GetInstance().SetLenguage(SingletonLenguage.Lenguage.CATALAN);
                break;
        }
    }

}
