using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    #region Configuracion
    [HideInInspector]
    public static Configurartion configurartion;
    #endregion

    #region ScenesIndex
    [HideInInspector]
    public int PreparadosIndex, ListosIndex, YaIndex, GusanosIndex = 4, BurbujasIndex = 5, ColorIndex = 6;
    [HideInInspector]
    public int InicioIndex = 0, ParejasIndex = 2, BitIndex =1, PuzzleIndex = 3;
    #endregion  

    #region ButtonUI
    public Color m_BlackColor, m_PurpleColor, m_GrisColor, m_WhiteColor;

    public Sprite ActiveButton;
    public Sprite DesactivateButton;
    #endregion

    #region WordAdding(Cambiar de Sitio)
    public string Word;
    #endregion

    public static List<int> m_CurrentToMinigame = new List<int>();//0 Parejas, 1 Bit, 2 Puzzle
    public static List<PalabraBD> palabrasDisponibles = new List<PalabraBD>();
    public static List<PalabraBD> palabrasUserDisponibles = new List<PalabraBD>();
    public static List<FraseBD> frasesDisponibles = new List<FraseBD>();
    public static int currentMiniGame = 0;
    public static int fallosPuzzle = 0;
    public static int fallosParejas = 0;
    public static bool backFromActivity = false;
    public int m_NeededToMinigame = 5;//Siempre añadir un +1 a lo que necesitan
    [HideInInspector]
    public int m_BitLevel = 1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (m_CurrentToMinigame.Count == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    m_CurrentToMinigame.Add(0);
                }
            }
        }
        else
        {
            DestroyImmediate(this);
        }

       
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
                for (int i = 0; i < 3; i++)
                {
                    m_CurrentToMinigame.Add(0);
                }
            }
            return instance;
        }
    }

    public static void SumPointToMinigame(int _numOfMinigame)
    {
        m_CurrentToMinigame[_numOfMinigame]++;
        ManagamentFalseBD.management.SaveBolasMinijuegos();
    }

    public static void ResetPointToMinigame(int _numOfMinigame)
    {
        m_CurrentToMinigame[_numOfMinigame] = 0;
        ManagamentFalseBD.management.SaveBolasMinijuegos();
    }

    public void SaveWord(InputField input)
    {
        Word = input.text;
    }

    public bool InputRecieved()
    {
        if (Input.GetMouseButton(0) || (Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began))
            return true;

        return false;
    }

    public void ChangeConfig()
    {
        SingletonLenguage.GetInstance().SetLenguage(configurartion.currentLenguaje);
        SingletonLenguage.GetInstance().SetFont(configurartion.currentFont);
    }


}
