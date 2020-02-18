using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    Button m_MyButton;
    public int m_Position;
    public GameObject[] m_Buttons;
    private ManagamentFalseBD bd;

    int m_CurrentEditWord = 1;

    private void Awake()
    {
        m_MyButton = GetComponent<Button>();
        bd = GameObject.Find("ManagementFalsaBD").GetComponent<ManagamentFalseBD>();
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
                l_Buttons[i].GetComponent<Image>().sprite = GameManager.GetInstance().ActiveButton;
                l_Buttons[i].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_WhiteColor;
            }
            else
            {
                l_Buttons[i].GetComponent<Image>().sprite = GameManager.GetInstance().DesactivateButton;
                l_Buttons[i].GetComponentInChildren<Text>().color = GameManager.GetInstance().m_PurpleColor;
            }
        }
    }

    public void Ayuda(bool l_Ayuda)
    {
        GameManager.configurartion.ayudaVisual = l_Ayuda;
        bd.SaveConfig();
    }

    public void Animacion(bool l_Animacion)
    {
        GameManager.configurartion.refuerzoPositivo = l_Animacion;
        bd.SaveConfig();
    }

    public void Articulo(bool l_Articulo)
    {
        GameManager.configurartion.palabrasConArticulo = l_Articulo;
        bd.SaveConfig();
    }

    public void Repeticiones(int l_Repeticiones)
    {
        GameManager.configurartion.repetitionsOfExercise = l_Repeticiones;
        bd.SaveConfig();
    }

    public void Pack(int l_Packs)
    {
        GameManager.configurartion.paquete = l_Packs;
        bd.SaveConfig();
        print(GameManager.configurartion.paquete);
    }

    public void Dificultad(int Dificultad)
    {
        GameManager.configurartion.difficult = Dificultad;
        bd.SaveConfig();
    }

    public void PalabraEditar(Text l_Texto)
    {
        m_CurrentEditWord++;
        if (m_CurrentEditWord > 8)
            m_CurrentEditWord = 1;
        l_Texto.text = m_CurrentEditWord.ToString() + " de 8";
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
        GameManager.configurartion.currentFont = SingletonLenguage.GetInstance().GetFont();
        bd.SaveConfig();

    }

    public void SetLenguage(int _lengauge)
    {
        switch (_lengauge)
        {
            case 0:
                SingletonLenguage.GetInstance().SetLenguage(SingletonLenguage.Lenguage.CASTELLANO);
                break;
            case 1:
                SingletonLenguage.GetInstance().SetLenguage(SingletonLenguage.Lenguage.CATALAN);
                break;
        }
        GameManager.configurartion.currentLenguaje = SingletonLenguage.GetInstance().GetLenguage();
        
        bd.SaveConfig();
    }

}
