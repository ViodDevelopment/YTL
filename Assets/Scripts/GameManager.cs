using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    #region Configuracion
    [HideInInspector]
    public static Configuration configuration;
    #endregion

    #region ScenesIndex
    [HideInInspector]
    public int PreparadosIndex, ListosIndex, YaIndex, GusanosIndex = 4, BurbujasIndex = 5, ColorIndex = 6;
    [HideInInspector]
    public int InicioIndex = 0, ParejasIndex = 2, BitIndex = 1, PuzzleIndex = 3;
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

    public static List<int> m_CurrentToMinigame = new List<int>();//0 Parejas, 1 Bit, 2 Puzzle, 3 parejas2, 4 bit2, 5 puzzle2, 6 parejas3, 7 bit3, 8 puzzle3
    public static List<PalabraBD> palabrasDisponibles = new List<PalabraBD>();
    public static List<PalabraBD> palabrasUserDisponibles = new List<PalabraBD>();
    public static List<FraseBD> frasesDisponibles = new List<FraseBD>();
    public static int currentMiniGame = 0;
    public static int fallosPuzzle = 0;
    public static int fallosParejas = 0;
    public static bool backFromActivity = false;
    public static int lastLevelActivity = 0;
    public static bool backFromConf = false;
    public int m_NeededToMinigame = 5;//Siempre añadir un +1 a lo que necesitan
    [HideInInspector]
    public int m_BitLevel = 1;

    public static bool actualizacion = false;
    WebCamTexture backCam;
    //[HideInInspector]
    public Texture PhotoFromCam;
 
    public string m_UserName = "";
    public string m_UserMail = "";
    public bool m_AcceptedPolitics=false;
    public Button m_Enviar;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (m_CurrentToMinigame.Count == 0)
            {
                for (int i = 0; i < 9; i++)
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


    public static GameManager GetInstance()
    {

        if (instance == null)
        {
            instance = new GameManager();
            for (int i = 0; i < 9; i++)
            {
                m_CurrentToMinigame.Add(0);
            }
        }
        return instance;

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
        if (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            return true;

        return false;
    }

    public void ChangeConfig()
    {
        SingletonLenguage.GetInstance().SetLenguage(configuration.currentLenguaje);
        SingletonLenguage.GetInstance().SetFont(configuration.currentFont);
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
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("vioddevelopment@gmail.com");
        mail.To.Add("app@yotambienleo.com");
        mail.Subject = "Usuario y Correo";

        
        mail.Body = "Versión: Android Lite   Name: " + m_UserName + " Correo: " + m_UserMail;
#if UNITY_IOS
        mail.Body = "Versión: iOS Lite   Name: " + m_UserName + " Correo: " + m_UserMail;
#endif
        // you can use others too.
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("vioddevelopment@gmail.com", "Viod@1557") as ICredentialsByHost;
        smtpServer.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        { return true; };
        smtpServer.Send(mail);


        configuration.registrado = true;
        ManagamentFalseBD.management.SaveConfig();
    }

}
