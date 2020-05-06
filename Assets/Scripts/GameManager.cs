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

    public static int loadingScene = -1;

    public static List<int> m_CurrentToMinigame = new List<int>();//0 Parejas, 1 Bit, 2 Puzzle
    public static List<PalabraBD> palabrasDisponibles = new List<PalabraBD>();
    public static List<PalabraBD> palabrasUserDisponibles = new List<PalabraBD>();
    public static List<FraseBD> frasesDisponibles = new List<FraseBD>();
    public static int currentMiniGame = 0;
    public static int fallosPuzzle = 0;
    public static int fallosParejas = 0;
    public static bool backFromActivity = false;
    public static int lastLevelActivity = 0;
    public int m_NeededToMinigame = 5;//Siempre añadir un +1 a lo que necesitan
    [HideInInspector]
    public int m_BitLevel = 1;


    bool camAvaliable;
    WebCamTexture backCam;
    //[HideInInspector]
    public Texture PhotoFromCam;

    #region Registro
    public Button m_Enviar;
    string m_UserName = "";
    string m_UserMail = "";
    bool m_AcceptedPolitics;

    #endregion

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

    private void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            camAvaliable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);

            }

        }

        if (backCam == null)
        {
            return;
        }

        backCam.Play();
        backCam.Pause();
        InvokeRepeating("CamIsPlaying", 0f, 5f);
      
    }

    void CamIsPlaying()
    {
        Debug.Log("Camera trasera" + backCam.isPlaying);
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

    public void SaveUserName(InputField name)
    {
        m_UserName = name.text;
        CheckOK();
    }

    public void SaveUserMail(InputField mail)
    {
        m_UserMail = mail.text;
        CheckOK();
    }

    public void AcceptPolitics()
    {
        m_AcceptedPolitics = !m_AcceptedPolitics;
        CheckOK();
    }

    public void CheckOK()
    {
        if (m_UserName != "" && m_UserMail.Contains("@") && m_AcceptedPolitics)
            m_Enviar.interactable = true;
        else if (m_Enviar.IsInteractable())
            m_Enviar.interactable = false;
    }

    public void ReadPolitics()
    {
        Application.OpenURL("http://yotambienleo.com/politica-de-privacidad/");
    }
    public void SendMail()
    {
        // Nombre es la variable m_UserName
        // El Correo es la variable m_UserMail
        configurartion.registrado = true;
        ManagamentFalseBD.management.SaveConfig();
    }


}
