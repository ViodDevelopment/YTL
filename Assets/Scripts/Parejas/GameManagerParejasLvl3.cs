﻿using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;



public class GameManagerParejasLvl3 : MonoBehaviour

{
    public GameObject dumi;

    public SceneManagement m_Scener;

    public GameObject m_Canvas;

    public List<Image> marcos = new List<Image>();

    public GameObject m_RealCanvas;

    [HideInInspector]

    public Animation m_Animation;

    public GameObject m_ImageZoom;

    public Image m_ImageZoomed;

    public Image planeImageWhenPair;

    public Text m_TextZoomed;

    private List<PalabraBD> listOfPalabras = new List<PalabraBD>();

    private List<PalabraBD> repetirPalabras = new List<PalabraBD>();

    int m_CurrentNumRep = 1;

    public int m_CurrentPairs;

    private bool completed;
    private bool repeating;
    private bool acabado = false;
    private bool cambiarTamanoFont = false;

    private int maxLetters = 0;


    #region Separador

    #endregion




    private int currentNumOfPairs = 0;

    public int m_NumPairs;

    private bool m_IsHorizontal;

    private float m_XPos;

    private float m_YPos;

    private bool m_FirstPair;

    #region Asignación de imagenes
    public List<GameObject> horizontal4Arriba = new List<GameObject>();
    public List<GameObject> horizontal4Abajo = new List<GameObject>();

    public List<GameObject> horizontal3Arriba = new List<GameObject>();
    public List<GameObject> horizontal3Abajo = new List<GameObject>();

    public List<GameObject> horizontal2Arriba = new List<GameObject>();
    public List<GameObject> horizontal2Abajo = new List<GameObject>();



    public List<GameObject> vertical4Left = new List<GameObject>();
    public List<GameObject> vertical4Right = new List<GameObject>();

    public List<GameObject> vertical3Left = new List<GameObject>();
    public List<GameObject> vertical3Right = new List<GameObject>();

    public List<GameObject> vertical2Left = new List<GameObject>();
    public List<GameObject> vertical2Right = new List<GameObject>();

    #endregion



    #region Points

    public Sprite m_CompletedPoint;

    public Sprite m_IncompletedPoint;

    public Transform m_SpawnImpar;

    public Transform m_SpawnPar;

    Transform m_CurrentSpawn;

    public GameObject m_Point;

    static int l_NumReps = GameManager.GetInstance().m_NeededToMinigame;

    GameObject[] m_Points = new GameObject[l_NumReps];

    public GameObject m_Siguiente;

    public GameObject m_Repetir;

    #endregion



    void Start()
    {
        InitPaabras();
        Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Minute + Random.seed + 1);
        if (PaquetePalabrasParejas.GetInstance("3").acabado)
            m_NumPairs = Random.Range(3, 5);
        else
            m_NumPairs = PaquetePalabrasParejas.GetInstance("3").parejas; Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Minute + Random.seed + 1);
        Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Minute + Random.seed + 1);

        if (l_NumReps % 2 == 0)
        {

            m_CurrentSpawn = m_SpawnPar;

            m_CurrentSpawn.GetComponent<RectTransform>().anchoredPosition -= new Vector2((75 * (l_NumReps / 2 - 1)), 0);

        }

        else
        {

            m_CurrentSpawn = m_SpawnImpar;

            m_CurrentSpawn.GetComponent<RectTransform>().anchoredPosition -= new Vector2((75 * (l_NumReps / 2)), 0);

        }



        for (int i = 0; i < l_NumReps; i++)
        {

            m_Points[i] = Instantiate(m_Point, m_CurrentSpawn.transform);

            m_Points[i].GetComponent<RectTransform>().anchoredPosition += new Vector2(m_Points[i].transform.position.x + (i * 75), 0);

        }



        for (int i = 0; i <= GameManager.m_CurrentToMinigame[6]; i++)
        {
            if (i > 0 && m_Points.Length > i - 1)
                m_Points[i - 1].GetComponent<Image>().sprite = m_CompletedPoint;

        }

        completed = false;
        repeating = false;

        InstantiatePairs();

        m_Animation = m_RealCanvas.GetComponent<Animation>();

    }

    private void InitPaabras()
    {
        listOfPalabras.Clear();
        foreach (PalabraBD p in PaquetePalabrasParejas.GetInstance("3").currentParejasPaquet)
        {

            if (p.paquet == GameManager.configuration.paquete)
            {
                if (p.image1 != "")
                {
                    listOfPalabras.Add(p);
                }
            }
            else if(GameManager.configuration.paquete == -1)
            {
                if (p.image1 != "")
                {
                    listOfPalabras.Add(p);
                }
            }
        }

        foreach (PalabraBD p in GameManager.palabrasUserDisponibles)
        {
            if (SingletonLenguage.GetInstance().GetLenguage() == SingletonLenguage.Lenguage.CASTELLANO)
            {
                if (p.nameSpanish != "" && (GameManager.configuration.paquete == 0 || GameManager.configuration.paquete == -1))
                    listOfPalabras.Add(p);
            }
            else if (SingletonLenguage.GetInstance().GetLenguage() == SingletonLenguage.Lenguage.CATALAN)
            {
                if (p.nameCatalan != "" && (GameManager.configuration.paquete == 0 || GameManager.configuration.paquete == -1))
                    listOfPalabras.Add(p);
            }
        }
    }



    private void Update()
    {

        if (completed)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                completed = false;
                StartCoroutine(WaitSeconds(1.5f));
            }
        }
        else if (m_ImageZoom.activeSelf && GameManager.InputRecieved() && !m_RealCanvas.GetComponent<Animation>().isPlaying && m_CurrentPairs != m_NumPairs)
        {

            m_ImageZoom.SetActive(false);

            planeImageWhenPair.gameObject.SetActive(false);



        }

        if (acabado)
        {
            if (!GameManager.configuration.refuerzoPositivo)
            {
                ActivateButtons();
                acabado = false;
            }
            else if (GameObject.Find("Dumi(Clone)") == null)
            {
                ActivateButtons();
                acabado = false;
            }
        }

    }



    public void InstantiatePairs()
    {
        m_CurrentPairs = 0;
        completed = false;
        repeating = false;
        m_ImageZoom.SetActive(false);


        planeImageWhenPair.gameObject.SetActive(false);
        Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Minute);

        if (GameManager.m_CurrentToMinigame[6] > 0 && m_Points.Length > GameManager.m_CurrentToMinigame[6] - 1)
            m_Points[GameManager.m_CurrentToMinigame[6] - 1].GetComponent<Image>().sprite = m_CompletedPoint;

        m_CurrentNumRep = 1;

        currentNumOfPairs = 0;



        List<PalabraBD> l_Pairs = new List<PalabraBD>();

        foreach (PalabraBD p in listOfPalabras)
        {
            PalabraBD pal = new PalabraBD();
            CopyWords(p, ref pal);
            l_Pairs.Add(pal);
            Random.InitState(Random.seed + 1);
            switch (Random.Range(0, 3))
            {
                case 0:
                    l_Pairs[l_Pairs.Count - 1].image1 = l_Pairs[l_Pairs.Count - 1].image1;
                    break;
                case 1:
                    l_Pairs[l_Pairs.Count - 1].image1 = l_Pairs[l_Pairs.Count - 1].image2;
                    break;
                case 2:
                    l_Pairs[l_Pairs.Count - 1].image1 = l_Pairs[l_Pairs.Count - 1].image3;
                    break;
            }
        }


        List<PalabraBD> l_SecondPair = new List<PalabraBD>();

        List<PalabraBD> l_ThirdPair = new List<PalabraBD>();

        repetirPalabras = l_ThirdPair;


        if (PaquetePalabrasParejas.GetInstance("3").acabado)
        {
            Random.InitState(Random.Range(-15, 15));
            if (Random.Range(0, 2) == 1)
            {
                m_IsHorizontal = true;
            }
            else
            {
                m_IsHorizontal = false;
            }
            Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Minute + Random.seed + 1);
            m_NumPairs = Random.Range(3, 5);
        }
        else
        {
            m_IsHorizontal = PaquetePalabrasParejas.GetInstance("3").pantallasHorizontal[0];
            m_NumPairs = PaquetePalabrasParejas.GetInstance("3").parejas;
        }

        m_FirstPair = true;


        if (m_IsHorizontal)
        {

            m_XPos = Screen.width / (m_NumPairs * 2.235f);
            m_YPos = Screen.height / 5;

            for (int i = 0; i < 2; i++)
            {

                int k = 1;

                if (m_FirstPair)
                {

                    for (int j = 0; j < m_NumPairs; j++)
                    {

                        int l_RandomPair = Random.Range(0, l_Pairs.Count);

                        InstiantateCopy(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, m_NumPairs, currentNumOfPairs, j);




                        l_SecondPair.Add(l_Pairs[l_RandomPair]);

                        l_ThirdPair.Add(l_Pairs[l_RandomPair]);

                        l_Pairs.RemoveAt(l_RandomPair);


                        k++;
                        k++;
                    }

                }

                else
                {

                    for (int j = 0; j < m_NumPairs; j++)
                    {

                        int l_RandomPair = Random.Range(0, l_SecondPair.Count);

                        InstiantatePair(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, m_NumPairs, currentNumOfPairs, j);


                        currentNumOfPairs++;

                        l_SecondPair.RemoveAt(l_RandomPair);
                        ;

                        k++;
                        k++;
                    }

                }

                m_FirstPair = false;
                m_YPos *= 3f;

            }
        }

        else
        {
            m_XPos = Screen.width / 4.515f;

            m_YPos = Screen.height / (m_NumPairs * 2.3f);

            for (int i = 0; i < 2; i++)
            {

                int k = 1;

                if (m_FirstPair)
                {

                    for (int j = 0; j < m_NumPairs; j++)
                    {


                        int l_RandomPair = Random.Range(0, l_Pairs.Count);

                        InstiantatePair(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, m_NumPairs, currentNumOfPairs, j);


                        currentNumOfPairs++;



                        l_SecondPair.Add(l_Pairs[l_RandomPair]);

                        l_ThirdPair.Add(l_Pairs[l_RandomPair]);

                        l_Pairs.RemoveAt(l_RandomPair);


                        k++;
                        k++;
                    }

                }

                else
                {

                    for (int j = 0; j < m_NumPairs; j++)
                    {

                        int l_RandomPair = Random.Range(0, l_SecondPair.Count);

                        InstiantateCopy(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, m_NumPairs, currentNumOfPairs, j);


                        l_SecondPair.RemoveAt(l_RandomPair);

                        k++;
                        k++;

                    }

                }
                m_FirstPair = false;
                m_XPos *= 3f;

            }

        }
        ReduceFont();

    }



    public void NextPairs(bool next = false)
    {
        RevertFont();
        m_CurrentPairs = 0;
        completed = false;
        repeating = false;
        m_ImageZoom.SetActive(false);


        planeImageWhenPair.gameObject.SetActive(false);
        Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Minute);

        if (GameManager.m_CurrentToMinigame[6] > 0 && m_Points.Length > GameManager.m_CurrentToMinigame[6] - 1)
            m_Points[GameManager.m_CurrentToMinigame[6] - 1].GetComponent<Image>().sprite = m_CompletedPoint;

        m_CurrentNumRep = 1;

        if (GameManager.m_CurrentToMinigame[6] >= GameManager.GetInstance().m_NeededToMinigame)
        {
            GameManager.ResetPointToMinigame(6);
            m_Scener.NextGame();
        }
        else
        {

            currentNumOfPairs = 0;



            List<PalabraBD> l_Pairs = new List<PalabraBD>();

            foreach (PalabraBD p in listOfPalabras)
            {
                PalabraBD pal = new PalabraBD();
                CopyWords(p, ref pal);
                l_Pairs.Add(pal);
                Random.InitState(Random.seed + 1);
                switch (Random.Range(0, 3))
                {
                    case 0:
                        l_Pairs[l_Pairs.Count - 1].image1 = l_Pairs[l_Pairs.Count - 1].image1;
                        break;
                    case 1:
                        l_Pairs[l_Pairs.Count - 1].image1 = l_Pairs[l_Pairs.Count - 1].image2;
                        break;
                    case 2:
                        l_Pairs[l_Pairs.Count - 1].image1 = l_Pairs[l_Pairs.Count - 1].image3;
                        break;
                }
            }


            List<PalabraBD> l_SecondPair = new List<PalabraBD>();

            List<PalabraBD> l_ThirdPair = new List<PalabraBD>();

            repetirPalabras = l_ThirdPair;


            if (PaquetePalabrasParejas.GetInstance("3").acabado)
            {
                Random.InitState(Random.seed + Random.Range(-2, 2));
                if (Random.Range(0, 2) == 1)
                {
                    m_IsHorizontal = true;
                }
                else
                {
                    m_IsHorizontal = false;
                }
                Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Minute + Random.seed + 1);
                m_NumPairs = Random.Range(3, 5);
            }
            else
            {
                m_IsHorizontal = PaquetePalabrasParejas.GetInstance("3").pantallasHorizontal[0];
                m_NumPairs = PaquetePalabrasParejas.GetInstance("3").parejas;
            }

            m_FirstPair = true;


            if (m_IsHorizontal)
            {

                m_XPos = Screen.width / (m_NumPairs * 2.235f);
                m_YPos = Screen.height / 5;

                for (int i = 0; i < 2; i++)
                {

                    int k = 1;

                    if (m_FirstPair)
                    {

                        for (int j = 0; j < m_NumPairs; j++)
                        {

                            int l_RandomPair = Random.Range(0, l_Pairs.Count);

                            InstiantateCopy(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, m_NumPairs, currentNumOfPairs, j);




                            l_SecondPair.Add(l_Pairs[l_RandomPair]);

                            l_ThirdPair.Add(l_Pairs[l_RandomPair]);

                            l_Pairs.RemoveAt(l_RandomPair);


                            k++;
                            k++;
                        }

                    }

                    else
                    {

                        for (int j = 0; j < m_NumPairs; j++)
                        {

                            int l_RandomPair = Random.Range(0, l_SecondPair.Count);

                            InstiantatePair(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, m_NumPairs, currentNumOfPairs, j);


                            currentNumOfPairs++;

                            l_SecondPair.RemoveAt(l_RandomPair);
                            ;

                            k++;
                            k++;
                        }

                    }

                    m_FirstPair = false;
                    m_YPos *= 3f;

                }
            }

            else
            {
                m_XPos = Screen.width / 4.515f;

                m_YPos = Screen.height / (m_NumPairs * 2.3f);

                for (int i = 0; i < 2; i++)
                {

                    int k = 1;

                    if (m_FirstPair)
                    {

                        for (int j = 0; j < m_NumPairs; j++)
                        {


                            int l_RandomPair = Random.Range(0, l_Pairs.Count);

                            InstiantatePair(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, m_NumPairs, currentNumOfPairs, j);


                            currentNumOfPairs++;



                            l_SecondPair.Add(l_Pairs[l_RandomPair]);

                            l_ThirdPair.Add(l_Pairs[l_RandomPair]);

                            l_Pairs.RemoveAt(l_RandomPair);


                            k++;
                            k++;
                        }

                    }

                    else
                    {

                        for (int j = 0; j < m_NumPairs; j++)
                        {

                            int l_RandomPair = Random.Range(0, l_SecondPair.Count);

                            InstiantateCopy(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, m_NumPairs, currentNumOfPairs, j);


                            l_SecondPair.RemoveAt(l_RandomPair);

                            k++;
                            k++;

                        }

                    }
                    m_FirstPair = false;
                    m_XPos *= 3f;

                }

            }
        }
        ReduceFont();
    }



    public void RepeatPairs()
    {
        RevertFont();
        Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Minute);

        m_CurrentNumRep++;

        m_CurrentPairs = 0;
        completed = false;
        repeating = true;
        m_ImageZoom.SetActive(false);
        planeImageWhenPair.gameObject.SetActive(false);

        currentNumOfPairs = 0;



        List<PalabraBD> l_Pairs = new List<PalabraBD>();

        foreach (PalabraBD p in repetirPalabras)
        {
            l_Pairs.Add(p);
        }


        List<PalabraBD> l_SecondPair = new List<PalabraBD>();

        List<PalabraBD> l_ThirdPair = new List<PalabraBD>();


        m_FirstPair = true;

        if (m_IsHorizontal)
        {
            m_XPos = Screen.width / (m_NumPairs * 2.0f);
            m_YPos = Screen.height / 4;

            for (int i = 0; i < 2; i++)
            {
                int k = 1;

                if (m_FirstPair)
                {

                    for (int j = 0; j < m_NumPairs; j++)
                    {

                        int l_RandomPair = Random.Range(0, l_Pairs.Count);

                        InstiantateCopy(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, m_NumPairs, currentNumOfPairs, j);



                        l_SecondPair.Add(l_Pairs[l_RandomPair]);

                        l_ThirdPair.Add(l_Pairs[l_RandomPair]);

                        l_Pairs.RemoveAt(l_RandomPair);

                        k++;
                        k++;
                    }
                }

                else
                {

                    for (int j = 0; j < m_NumPairs; j++)
                    {
                        int l_RandomPair = Random.Range(0, l_SecondPair.Count);

                        InstiantatePair(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, m_NumPairs, currentNumOfPairs, j);


                        currentNumOfPairs++;



                        l_SecondPair.RemoveAt(l_RandomPair);

                        k++;
                        k++;
                    }

                }

                m_FirstPair = false;
                m_YPos *= 3f;

            }

        }

        else
        {

            m_XPos = Screen.width / 4;
            m_YPos = Screen.height / (m_NumPairs * 2.0f);

            for (int i = 0; i < 2; i++)
            {
                int k = 1;

                if (m_FirstPair)
                {

                    for (int j = 0; j < m_NumPairs; j++)
                    {

                        int l_RandomPair = Random.Range(0, l_Pairs.Count);

                        InstiantatePair(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, m_NumPairs, currentNumOfPairs, j);


                        currentNumOfPairs++;



                        l_SecondPair.Add(l_Pairs[l_RandomPair]);

                        l_ThirdPair.Add(l_Pairs[l_RandomPair]);

                        l_Pairs.RemoveAt(l_RandomPair);


                        k++;
                        k++;
                    }
                }

                else
                {

                    for (int j = 0; j < m_NumPairs; j++)
                    {
                        int l_RandomPair = Random.Range(0, l_SecondPair.Count);

                        InstiantateCopy(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, m_NumPairs, currentNumOfPairs, j);

                        l_SecondPair.RemoveAt(l_RandomPair);

                        k++;
                        k++;
                    }
                }

                m_FirstPair = false;

                m_XPos *= 3f;

            }

        }
        ReduceFont();

    }



    public void ActivateButtons()
    {

        m_Siguiente.SetActive(true);

        if (m_CurrentNumRep < GameManager.configuration.repetitionsOfExercise)
        {
            m_Repetir.SetActive(true);
        }
    }

    private void ReduceFont()
    {
        if (cambiarTamanoFont)
        {
            if (m_IsHorizontal)
            {
                switch (m_NumPairs)
                {
                    case 3:
                        for (int i = 0; i < horizontal3Abajo.Count; i++)
                        {
                            horizontal3Abajo[i].GetComponentInChildren<Text>().transform.localScale *= 1 / (0.15f * maxLetters);
                        }
                        break;
                    case 4:
                        for (int i = 0; i < horizontal4Abajo.Count; i++)
                        {
                            horizontal4Abajo[i].GetComponentInChildren<Text>().transform.localScale *= 1 / (0.15f * maxLetters);
                        }
                        break;
                }
            }
            else
            {
                switch (m_NumPairs)
                {
                    case 3:
                        for (int i = 0; i < horizontal3Abajo.Count; i++)
                        {
                            vertical3Right[i].GetComponentInChildren<Text>().transform.localScale *= 1 / (0.15f * maxLetters);
                        }
                        break;
                    case 4:
                        for (int i = 0; i < horizontal4Abajo.Count; i++)
                        {
                            vertical4Right[i].GetComponentInChildren<Text>().transform.localScale *= 1 / (0.15f * maxLetters);
                        }
                        break;
                }
            }
            //solo lo hago con las horizontales
            m_TextZoomed.transform.localScale *= 1 / (0.15f * maxLetters);
            cambiarTamanoFont = false;
            maxLetters = 0;
        }
    }

    private void RevertFont()
    {

        if (m_IsHorizontal)
        {
            switch (m_NumPairs)
            {
                case 2:
                    for (int i = 0; i < horizontal2Abajo.Count; i++)
                    {
                        horizontal2Abajo[i].GetComponentInChildren<Text>().transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                    }
                    break;
                case 3:
                    for (int i = 0; i < horizontal3Abajo.Count; i++)
                    {
                        horizontal3Abajo[i].GetComponentInChildren<Text>().transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                    }
                    break;
                case 4:
                    for (int i = 0; i < horizontal4Abajo.Count; i++)
                    {
                        horizontal4Abajo[i].GetComponentInChildren<Text>().transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                    }
                    break;

            }

        }
        else
        {
            switch (m_NumPairs)
            {

                case 2:
                    for (int i = 0; i < vertical2Right.Count; i++)
                    {
                        vertical2Right[i].GetComponentInChildren<Text>().transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                    }
                    break;
                case 3:
                    for (int i = 0; i < vertical3Right.Count; i++)
                    {
                        vertical3Right[i].GetComponentInChildren<Text>().transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                    }
                    break;

                case 4:
                    for (int i = 0; i < vertical4Right.Count; i++)
                    {
                        vertical4Right[i].GetComponentInChildren<Text>().transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                    }
                    break;


            }
        }
        m_TextZoomed.gameObject.transform.localScale = new Vector3(0.3150725f, 0.3150725f, 0.3150725f);
    }



    IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (GameManager.configuration.refuerzoPositivo)
        {
            if (GameObject.Find("Dumi(Clone)") == null)
            {
                GameObject pinguino = Instantiate(dumi, dumi.transform.position, dumi.transform.rotation);
                pinguino.GetComponent<Dumi>().AudioPositivo();
            }
        }
        if (!repeating)
        {
            if (m_Points.Length > GameManager.m_CurrentToMinigame[6] - 1)
                GameManager.SumPointToMinigame(6);
            if (GameManager.m_CurrentToMinigame[6] > 0 && m_Points.Length > GameManager.m_CurrentToMinigame[6] - 1)
                m_Points[GameManager.m_CurrentToMinigame[6] - 1].GetComponent<Image>().sprite = m_CompletedPoint;
            if (!PaquetePalabrasParejas.GetInstance("3").acabado)
            {
                PaquetePalabrasParejas.GetInstance("3").pantallasHorizontal.RemoveAt(0);
                if (PaquetePalabrasParejas.GetInstance("3").pantallasHorizontal.Count == 0)
                {
                    PaquetePalabrasParejas.GetInstance("3").CrearNuevoPaquete();
                    InitPaabras();
                }
                PaquetePalabrasParejas.GetInstance("3").CrearBinario();
            }
        }
        acabado = true;
    }



    public void PairDone()
    {
        m_CurrentPairs++;
        m_Animation.Play();
        if (m_CurrentPairs == m_NumPairs)
            completed = true;
    }

    private void InstiantatePair(bool _firstTime, bool _horizontal, int l_RandomPair, List<PalabraBD> l_Pairs, List<PalabraBD> l_SecondPair, List<PalabraBD> l_ThirdPair, int _numofPairs, int _currentPair, int numJ)
    {
        if (_horizontal)
        {
            switch (_numofPairs)
            {
                case 2:
                    if (_firstTime)
                    {
                        if (l_Pairs[l_RandomPair].user)
                        {
                            horizontal2Arriba[_currentPair].GetComponent<Image>().sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(l_Pairs[l_RandomPair].GetSprite(l_Pairs[l_RandomPair].image1));
                        }
                        else
                            horizontal2Arriba[_currentPair].GetComponent<Image>().sprite = l_Pairs[l_RandomPair].GetSprite(l_Pairs[l_RandomPair].image1);

                        horizontal2Arriba[_currentPair].name = numJ.ToString();

                        horizontal2Arriba[_currentPair].GetComponent<PairsLvl3>().nombre = l_Pairs[l_RandomPair].palabraActual;

                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_Pairs[l_RandomPair].actualArticulo != null)
                            {
                                horizontal2Arriba[_currentPair].GetComponent<PairsLvl3>().articulo = l_Pairs[l_RandomPair].actualArticulo;
                                horizontal2Arriba[_currentPair].GetComponent<PairsLvl3>().audioArt = l_Pairs[l_RandomPair].GetAudioArticulo();
                                horizontal2Arriba[_currentPair].GetComponent<PairsLvl3>().onlyArt = l_Pairs[l_RandomPair].onlyArticulo;
                            }
                        }
                        horizontal2Arriba[_currentPair].GetComponent<PairsLvl3>().color = l_Pairs[l_RandomPair].color;

                        horizontal2Arriba[_currentPair].GetComponent<PairsLvl3>().audioClip = l_Pairs[l_RandomPair].GetAudioClip(l_Pairs[l_RandomPair].audio);

                        horizontal2Arriba[_currentPair].GetComponent<PairsLvl3>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        horizontal2Arriba[_currentPair].SetActive(true);

                        PonerColorMarco(l_Pairs[l_RandomPair].color, horizontal2Arriba[_currentPair].transform.GetChild(0).GetComponent<Image>());

                    }

                    else
                    {
                        if (l_SecondPair[l_RandomPair].user)
                        {
                            horizontal2Arriba[_currentPair].GetComponent<Image>().sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(l_SecondPair[l_RandomPair].GetSprite(l_SecondPair[l_RandomPair].image1));
                        }
                        else
                            horizontal2Arriba[_currentPair].GetComponent<Image>().sprite = l_SecondPair[l_RandomPair].GetSprite(l_SecondPair[l_RandomPair].image1);

                        horizontal2Arriba[_currentPair].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();

                        horizontal2Arriba[_currentPair].GetComponent<PairsLvl3>().nombre = l_SecondPair[l_RandomPair].palabraActual;

                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_SecondPair[l_RandomPair].actualArticulo != null)
                            {
                                horizontal2Arriba[_currentPair].GetComponent<PairsLvl3>().articulo = l_SecondPair[l_RandomPair].actualArticulo;
                                horizontal2Arriba[_currentPair].GetComponent<PairsLvl3>().audioArt = l_SecondPair[l_RandomPair].GetAudioArticulo();
                                horizontal2Arriba[_currentPair].GetComponent<PairsLvl3>().onlyArt = l_SecondPair[l_RandomPair].onlyArticulo;

                            }
                        }
                        horizontal2Arriba[_currentPair].GetComponent<PairsLvl3>().color = l_SecondPair[l_RandomPair].color;

                        horizontal2Arriba[_currentPair].GetComponent<PairsLvl3>().audioClip = l_SecondPair[l_RandomPair].GetAudioClip(l_SecondPair[l_RandomPair].audio);

                        horizontal2Arriba[_currentPair].GetComponent<PairsLvl3>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        horizontal2Arriba[_currentPair].SetActive(true);

                        PonerColorMarco(l_SecondPair[l_RandomPair].color, horizontal2Arriba[_currentPair].transform.GetChild(0).GetComponent<Image>());

                    }
                    break;
                case 3:
                    if (_firstTime)
                    {
                        if (l_Pairs[l_RandomPair].user)
                        {
                            horizontal3Arriba[_currentPair].GetComponent<Image>().sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(l_Pairs[l_RandomPair].GetSprite(l_Pairs[l_RandomPair].image1));
                        }
                        else
                            horizontal3Arriba[_currentPair].GetComponent<Image>().sprite = l_Pairs[l_RandomPair].GetSprite(l_Pairs[l_RandomPair].image1);

                        horizontal3Arriba[_currentPair].name = numJ.ToString();

                        horizontal3Arriba[_currentPair].GetComponent<PairsLvl3>().nombre = l_Pairs[l_RandomPair].palabraActual;

                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_Pairs[l_RandomPair].actualArticulo != null)
                            {
                                horizontal3Arriba[_currentPair].GetComponent<PairsLvl3>().articulo = l_Pairs[l_RandomPair].actualArticulo;
                                horizontal3Arriba[_currentPair].GetComponent<PairsLvl3>().audioArt = l_Pairs[l_RandomPair].GetAudioArticulo();
                                horizontal3Arriba[_currentPair].GetComponent<PairsLvl3>().onlyArt = l_Pairs[l_RandomPair].onlyArticulo;

                            }
                        }

                        horizontal3Arriba[_currentPair].GetComponent<PairsLvl3>().color = l_Pairs[l_RandomPair].color;

                        horizontal3Arriba[_currentPair].GetComponent<PairsLvl3>().audioClip = l_Pairs[l_RandomPair].GetAudioClip(l_Pairs[l_RandomPair].audio);

                        horizontal3Arriba[_currentPair].GetComponent<PairsLvl3>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        horizontal3Arriba[_currentPair].SetActive(true);

                        PonerColorMarco(l_Pairs[l_RandomPair].color, horizontal3Arriba[_currentPair].transform.GetChild(0).GetComponent<Image>());

                    }

                    else
                    {
                        if (l_SecondPair[l_RandomPair].user)
                        {
                            horizontal3Arriba[_currentPair].GetComponent<Image>().sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(l_SecondPair[l_RandomPair].GetSprite(l_SecondPair[l_RandomPair].image1));
                        }
                        else
                            horizontal3Arriba[_currentPair].GetComponent<Image>().sprite = l_SecondPair[l_RandomPair].GetSprite(l_SecondPair[l_RandomPair].image1);

                        horizontal3Arriba[_currentPair].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();

                        horizontal3Arriba[_currentPair].GetComponent<PairsLvl3>().nombre = l_SecondPair[l_RandomPair].palabraActual;

                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_SecondPair[l_RandomPair].actualArticulo != null)
                            {
                                horizontal3Arriba[_currentPair].GetComponent<PairsLvl3>().articulo = l_SecondPair[l_RandomPair].actualArticulo;
                                horizontal3Arriba[_currentPair].GetComponent<PairsLvl3>().audioArt = l_SecondPair[l_RandomPair].GetAudioArticulo();
                                horizontal3Arriba[_currentPair].GetComponent<PairsLvl3>().onlyArt = l_SecondPair[l_RandomPair].onlyArticulo;

                            }
                        }
                        horizontal3Arriba[_currentPair].GetComponent<PairsLvl3>().color = l_SecondPair[l_RandomPair].color;

                        horizontal3Arriba[_currentPair].GetComponent<PairsLvl3>().audioClip = l_SecondPair[l_RandomPair].GetAudioClip(l_SecondPair[l_RandomPair].audio);

                        horizontal3Arriba[_currentPair].GetComponent<PairsLvl3>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        horizontal3Arriba[_currentPair].SetActive(true);

                        PonerColorMarco(l_SecondPair[l_RandomPair].color, horizontal3Arriba[_currentPair].transform.GetChild(0).GetComponent<Image>());

                    }
                    break;

                case 4:
                    if (_firstTime)
                    {

                        if (l_Pairs[l_RandomPair].user)
                        {
                            horizontal4Arriba[_currentPair].GetComponent<Image>().sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(l_Pairs[l_RandomPair].GetSprite(l_Pairs[l_RandomPair].image1));
                        }
                        else
                            horizontal4Arriba[_currentPair].GetComponent<Image>().sprite = l_Pairs[l_RandomPair].GetSprite(l_Pairs[l_RandomPair].image1);

                        horizontal4Arriba[_currentPair].name = numJ.ToString();

                        horizontal4Arriba[_currentPair].GetComponent<PairsLvl3>().nombre = l_Pairs[l_RandomPair].palabraActual;

                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_Pairs[l_RandomPair].actualArticulo != null)
                            {
                                horizontal4Arriba[_currentPair].GetComponent<PairsLvl3>().articulo = l_Pairs[l_RandomPair].actualArticulo;
                                horizontal4Arriba[_currentPair].GetComponent<PairsLvl3>().audioArt = l_Pairs[l_RandomPair].GetAudioArticulo();
                                horizontal4Arriba[_currentPair].GetComponent<PairsLvl3>().onlyArt = l_Pairs[l_RandomPair].onlyArticulo;

                            }
                        }

                        horizontal4Arriba[_currentPair].GetComponent<PairsLvl3>().color = l_Pairs[l_RandomPair].color;

                        horizontal4Arriba[_currentPair].GetComponent<PairsLvl3>().audioClip = l_Pairs[l_RandomPair].GetAudioClip(l_Pairs[l_RandomPair].audio);

                        horizontal4Arriba[_currentPair].GetComponent<PairsLvl3>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        horizontal4Arriba[_currentPair].SetActive(true);

                        PonerColorMarco(l_Pairs[l_RandomPair].color, horizontal4Arriba[_currentPair].transform.GetChild(0).GetComponent<Image>());

                    }
                    else
                    {
                        if (l_SecondPair[l_RandomPair].user)
                        {
                            horizontal4Arriba[_currentPair].GetComponent<Image>().sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(l_SecondPair[l_RandomPair].GetSprite(l_SecondPair[l_RandomPair].image1));
                        }
                        else
                            horizontal4Arriba[_currentPair].GetComponent<Image>().sprite = l_SecondPair[l_RandomPair].GetSprite(l_SecondPair[l_RandomPair].image1);

                        horizontal4Arriba[_currentPair].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();

                        horizontal4Arriba[_currentPair].GetComponent<PairsLvl3>().nombre = l_SecondPair[l_RandomPair].palabraActual;

                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_SecondPair[l_RandomPair].actualArticulo != null)
                            {
                                horizontal4Arriba[_currentPair].GetComponent<PairsLvl3>().articulo = l_SecondPair[l_RandomPair].actualArticulo;
                                horizontal4Arriba[_currentPair].GetComponent<PairsLvl3>().audioArt = l_SecondPair[l_RandomPair].GetAudioArticulo();
                                horizontal4Arriba[_currentPair].GetComponent<PairsLvl3>().onlyArt = l_SecondPair[l_RandomPair].onlyArticulo;

                            }
                        }

                        horizontal4Arriba[_currentPair].GetComponent<PairsLvl3>().color = l_SecondPair[l_RandomPair].color;

                        horizontal4Arriba[_currentPair].GetComponent<PairsLvl3>().audioClip = l_SecondPair[l_RandomPair].GetAudioClip(l_SecondPair[l_RandomPair].audio);

                        horizontal4Arriba[_currentPair].GetComponent<PairsLvl3>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        horizontal4Arriba[_currentPair].SetActive(true);

                        PonerColorMarco(l_SecondPair[l_RandomPair].color, horizontal4Arriba[_currentPair].transform.GetChild(0).GetComponent<Image>());

                    }
                    break;
            }
        }
        else
        {
            switch (_numofPairs)
            {
                case 2:
                    if (_firstTime)
                    {

                        if (l_Pairs[l_RandomPair].user)
                        {
                            vertical2Left[_currentPair].GetComponent<Image>().sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(l_Pairs[l_RandomPair].GetSprite(l_Pairs[l_RandomPair].image1));
                        }
                        else
                            vertical2Left[_currentPair].GetComponent<Image>().sprite = l_Pairs[l_RandomPair].GetSprite(l_Pairs[l_RandomPair].image1);

                        vertical2Left[_currentPair].name = numJ.ToString();

                        vertical2Left[_currentPair].GetComponent<PairsLvl3>().nombre = l_Pairs[l_RandomPair].palabraActual;

                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_Pairs[l_RandomPair].actualArticulo != null)
                            {
                                vertical2Left[_currentPair].GetComponent<PairsLvl3>().articulo = l_Pairs[l_RandomPair].actualArticulo;
                                vertical2Left[_currentPair].GetComponent<PairsLvl3>().audioArt = l_Pairs[l_RandomPair].GetAudioArticulo();
                                vertical2Left[_currentPair].GetComponent<PairsLvl3>().onlyArt = l_Pairs[l_RandomPair].onlyArticulo;

                            }
                        }
                        vertical2Left[_currentPair].GetComponent<PairsLvl3>().color = l_Pairs[l_RandomPair].color;

                        vertical2Left[_currentPair].GetComponent<PairsLvl3>().audioClip = l_Pairs[l_RandomPair].GetAudioClip(l_Pairs[l_RandomPair].audio);

                        vertical2Left[_currentPair].GetComponent<PairsLvl3>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        vertical2Left[_currentPair].SetActive(true);

                        PonerColorMarco(l_Pairs[l_RandomPair].color, vertical2Left[_currentPair].transform.GetChild(0).GetComponent<Image>());

                    }
                    else
                    {
                        if (l_SecondPair[l_RandomPair].user)
                        {
                            vertical2Left[_currentPair].GetComponent<Image>().sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(l_SecondPair[l_RandomPair].GetSprite(l_SecondPair[l_RandomPair].image1));
                        }
                        else
                            vertical2Left[_currentPair].GetComponent<Image>().sprite = l_SecondPair[l_RandomPair].GetSprite(l_SecondPair[l_RandomPair].image1);

                        vertical2Left[_currentPair].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();

                        vertical2Left[_currentPair].GetComponent<PairsLvl3>().nombre = l_SecondPair[l_RandomPair].palabraActual;

                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_SecondPair[l_RandomPair].actualArticulo != null)
                            {
                                vertical2Left[_currentPair].GetComponent<PairsLvl3>().articulo = l_SecondPair[l_RandomPair].actualArticulo;
                                vertical2Left[_currentPair].GetComponent<PairsLvl3>().audioArt = l_SecondPair[l_RandomPair].GetAudioArticulo();
                                vertical2Left[_currentPair].GetComponent<PairsLvl3>().onlyArt = l_SecondPair[l_RandomPair].onlyArticulo;

                            }
                        }

                        vertical2Left[_currentPair].GetComponent<PairsLvl3>().color = l_SecondPair[l_RandomPair].color;

                        vertical2Left[_currentPair].GetComponent<PairsLvl3>().audioClip = l_SecondPair[l_RandomPair].GetAudioClip(l_SecondPair[l_RandomPair].audio);

                        vertical2Left[_currentPair].GetComponent<PairsLvl3>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        vertical2Left[_currentPair].SetActive(true);

                        PonerColorMarco(l_SecondPair[l_RandomPair].color, vertical2Left[_currentPair].transform.GetChild(0).GetComponent<Image>());

                    }
                    break;
                case 3:
                    if (_firstTime)
                    {

                        if (l_Pairs[l_RandomPair].user)
                        {
                            vertical3Left[_currentPair].GetComponent<Image>().sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(l_Pairs[l_RandomPair].GetSprite(l_Pairs[l_RandomPair].image1));
                        }
                        else
                            vertical3Left[_currentPair].GetComponent<Image>().sprite = l_Pairs[l_RandomPair].GetSprite(l_Pairs[l_RandomPair].image1);

                        vertical3Left[_currentPair].name = numJ.ToString();

                        vertical3Left[_currentPair].GetComponent<PairsLvl3>().nombre = l_Pairs[l_RandomPair].palabraActual;

                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_Pairs[l_RandomPair].actualArticulo != null)
                            {
                                vertical3Left[_currentPair].GetComponent<PairsLvl3>().articulo = l_Pairs[l_RandomPair].actualArticulo;
                                vertical3Left[_currentPair].GetComponent<PairsLvl3>().audioArt = l_Pairs[l_RandomPair].GetAudioArticulo();
                                vertical3Left[_currentPair].GetComponent<PairsLvl3>().onlyArt = l_Pairs[l_RandomPair].onlyArticulo;

                            }
                        }

                        vertical3Left[_currentPair].GetComponent<PairsLvl3>().color = l_Pairs[l_RandomPair].color;

                        vertical3Left[_currentPair].GetComponent<PairsLvl3>().audioClip = l_Pairs[l_RandomPair].GetAudioClip(l_Pairs[l_RandomPair].audio);

                        vertical3Left[_currentPair].GetComponent<PairsLvl3>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        vertical3Left[_currentPair].SetActive(true);

                        PonerColorMarco(l_Pairs[l_RandomPair].color, vertical3Left[_currentPair].transform.GetChild(0).GetComponent<Image>());

                    }
                    else
                    {
                        if (l_SecondPair[l_RandomPair].user)
                        {
                            vertical3Left[_currentPair].GetComponent<Image>().sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(l_SecondPair[l_RandomPair].GetSprite(l_SecondPair[l_RandomPair].image1));
                        }
                        else
                            vertical3Left[_currentPair].GetComponent<Image>().sprite = l_SecondPair[l_RandomPair].GetSprite(l_SecondPair[l_RandomPair].image1);

                        vertical3Left[_currentPair].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();

                        vertical3Left[_currentPair].GetComponent<PairsLvl3>().nombre = l_SecondPair[l_RandomPair].palabraActual;

                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_SecondPair[l_RandomPair].actualArticulo != null)
                            {
                                vertical3Left[_currentPair].GetComponent<PairsLvl3>().articulo = l_SecondPair[l_RandomPair].actualArticulo;
                                vertical3Left[_currentPair].GetComponent<PairsLvl3>().audioArt = l_SecondPair[l_RandomPair].GetAudioArticulo();
                                vertical3Left[_currentPair].GetComponent<PairsLvl3>().onlyArt = l_SecondPair[l_RandomPair].onlyArticulo;

                            }
                        }

                        vertical3Left[_currentPair].GetComponent<PairsLvl3>().color = l_SecondPair[l_RandomPair].color;

                        vertical3Left[_currentPair].GetComponent<PairsLvl3>().audioClip = l_SecondPair[l_RandomPair].GetAudioClip(l_SecondPair[l_RandomPair].audio);

                        vertical3Left[_currentPair].GetComponent<PairsLvl3>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        vertical3Left[_currentPair].SetActive(true);

                        PonerColorMarco(l_SecondPair[l_RandomPair].color, vertical3Left[_currentPair].transform.GetChild(0).GetComponent<Image>());

                    }
                    break;
                case 4:
                    if (_firstTime)
                    {

                        if (l_Pairs[l_RandomPair].user)
                        {
                            vertical4Left[_currentPair].GetComponent<Image>().sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(l_Pairs[l_RandomPair].GetSprite(l_Pairs[l_RandomPair].image1));
                        }
                        else
                            vertical4Left[_currentPair].GetComponent<Image>().sprite = l_Pairs[l_RandomPair].GetSprite(l_Pairs[l_RandomPair].image1);

                        vertical4Left[_currentPair].name = numJ.ToString();

                        vertical4Left[_currentPair].GetComponent<PairsLvl3>().nombre = l_Pairs[l_RandomPair].palabraActual;

                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_Pairs[l_RandomPair].actualArticulo != null)
                            {
                                vertical4Left[_currentPair].GetComponent<PairsLvl3>().articulo = l_Pairs[l_RandomPair].actualArticulo;
                                vertical4Left[_currentPair].GetComponent<PairsLvl3>().audioArt = l_Pairs[l_RandomPair].GetAudioArticulo();
                                vertical4Left[_currentPair].GetComponent<PairsLvl3>().onlyArt = l_Pairs[l_RandomPair].onlyArticulo;

                            }
                        }
                        vertical4Left[_currentPair].GetComponent<PairsLvl3>().color = l_Pairs[l_RandomPair].color;

                        vertical4Left[_currentPair].GetComponent<PairsLvl3>().audioClip = l_Pairs[l_RandomPair].GetAudioClip(l_Pairs[l_RandomPair].audio);

                        vertical4Left[_currentPair].GetComponent<PairsLvl3>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        vertical4Left[_currentPair].SetActive(true);

                        PonerColorMarco(l_Pairs[l_RandomPair].color, vertical4Left[_currentPair].transform.GetChild(0).GetComponent<Image>());

                    }
                    else
                    {
                        if (l_SecondPair[l_RandomPair].user)
                        {
                            vertical4Left[_currentPair].GetComponent<Image>().sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(l_SecondPair[l_RandomPair].GetSprite(l_SecondPair[l_RandomPair].image1));
                        }
                        else
                            vertical4Left[_currentPair].GetComponent<Image>().sprite = l_SecondPair[l_RandomPair].GetSprite(l_SecondPair[l_RandomPair].image1);

                        vertical4Left[_currentPair].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();

                        vertical4Left[_currentPair].GetComponent<PairsLvl3>().nombre = l_SecondPair[l_RandomPair].palabraActual;

                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_SecondPair[l_RandomPair].actualArticulo != null)
                            {
                                vertical4Left[_currentPair].GetComponent<PairsLvl3>().articulo = l_SecondPair[l_RandomPair].actualArticulo;
                                vertical4Left[_currentPair].GetComponent<PairsLvl3>().audioArt = l_SecondPair[l_RandomPair].GetAudioArticulo();
                                vertical4Left[_currentPair].GetComponent<PairsLvl3>().onlyArt = l_SecondPair[l_RandomPair].onlyArticulo;

                            }
                        }
                        vertical4Left[_currentPair].GetComponent<PairsLvl3>().color = l_SecondPair[l_RandomPair].color;

                        vertical4Left[_currentPair].GetComponent<PairsLvl3>().audioClip = l_SecondPair[l_RandomPair].GetAudioClip(l_SecondPair[l_RandomPair].audio);

                        vertical4Left[_currentPair].GetComponent<PairsLvl3>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        vertical4Left[_currentPair].SetActive(true);

                        PonerColorMarco(l_SecondPair[l_RandomPair].color, vertical4Left[_currentPair].transform.GetChild(0).GetComponent<Image>());

                    }
                    break;
            }
        }
    }

    private void InstiantateCopy(bool _firstTime, bool _horizontal, int l_RandomPair, List<PalabraBD> l_Pairs, List<PalabraBD> l_SecondPair, List<PalabraBD> l_ThirdPair, int _numofPairs, int _currentPair, int numJ)
    {

        if (_firstTime)
        {
            if (l_Pairs[l_RandomPair].palabraActual.Length > 0)
            {
                cambiarTamanoFont = true;
                if (l_Pairs[l_RandomPair].palabraActual.Length > maxLetters)
                    maxLetters = l_Pairs[l_RandomPair].palabraActual.Length;
            }
        }
        else
        {
            if (l_SecondPair[l_RandomPair].palabraActual.Length > 0)
            {
                cambiarTamanoFont = true;
                if (l_SecondPair[l_RandomPair].palabraActual.Length > maxLetters)
                    maxLetters = l_SecondPair[l_RandomPair].palabraActual.Length;
            }
        }

        if (maxLetters < 6)
            maxLetters = 6;

        if (_horizontal)
        {
            switch (_numofPairs)
            {
                case 2:
                    if (_firstTime)
                    {
                        horizontal2Abajo[numJ].name = numJ.ToString();
                        horizontal2Abajo[numJ].GetComponentInChildren<Text>().text = l_Pairs[l_RandomPair].palabraActual;
                        if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
                            horizontal2Abajo[numJ].GetComponentInChildren<Text>().transform.localScale *= 1.5f;

                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_Pairs[l_RandomPair].actualArticulo != null)
                            {

                                horizontal2Abajo[numJ].GetComponentInChildren<Text>().text = l_Pairs[l_RandomPair].actualArticulo + l_Pairs[l_RandomPair].palabraActual;
                                ModifyTextPair(horizontal2Abajo[numJ].GetComponentInChildren<Text>());

                            }
                        }

                        horizontal2Abajo[numJ].GetComponentInChildren<ConvertFont>().Convert();
                        horizontal2Abajo[numJ].SetActive(true);
                        PonerColorMarco(l_Pairs[l_RandomPair].color, horizontal2Abajo[numJ].transform.GetChild(1).GetComponent<Image>());

                    }
                    else
                    {
                        horizontal2Abajo[numJ].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();
                        horizontal2Abajo[numJ].GetComponentInChildren<Text>().text = l_SecondPair[l_RandomPair].palabraActual;
                        if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
                            horizontal2Abajo[numJ].GetComponentInChildren<Text>().transform.localScale *= 1.5f;
                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_SecondPair[l_RandomPair].actualArticulo != null)
                            {

                                horizontal2Abajo[numJ].GetComponentInChildren<Text>().text = l_SecondPair[l_RandomPair].actualArticulo + l_SecondPair[l_RandomPair].palabraActual;
                                ModifyTextPair(horizontal2Abajo[numJ].GetComponentInChildren<Text>());

                            }
                        }
                        horizontal2Abajo[numJ].GetComponentInChildren<ConvertFont>().Convert();
                        horizontal2Abajo[numJ].SetActive(true);
                        PonerColorMarco(l_SecondPair[l_RandomPair].color, horizontal2Abajo[numJ].transform.GetChild(1).GetComponent<Image>());
                    }
                    break;
                case 3:
                    if (_firstTime)
                    {
                        horizontal3Abajo[numJ].name = numJ.ToString();
                        horizontal3Abajo[numJ].GetComponentInChildren<Text>().text = l_Pairs[l_RandomPair].palabraActual;
                        if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
                            horizontal3Abajo[numJ].GetComponentInChildren<Text>().transform.localScale *= 1.5f;
                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_Pairs[l_RandomPair].actualArticulo != null)
                            {

                                horizontal3Abajo[numJ].GetComponentInChildren<Text>().text = l_Pairs[l_RandomPair].actualArticulo + l_Pairs[l_RandomPair].palabraActual;
                                ModifyTextPair(horizontal3Abajo[numJ].GetComponentInChildren<Text>());

                            }
                        }
                        horizontal3Abajo[numJ].GetComponentInChildren<ConvertFont>().Convert();
                        horizontal3Abajo[numJ].SetActive(true);
                        PonerColorMarco(l_Pairs[l_RandomPair].color, horizontal3Abajo[numJ].transform.GetChild(1).GetComponent<Image>());

                    }
                    else
                    {
                        horizontal3Abajo[numJ].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();
                        horizontal3Abajo[numJ].GetComponentInChildren<Text>().text = l_SecondPair[l_RandomPair].palabraActual;
                        if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
                            horizontal3Abajo[numJ].GetComponentInChildren<Text>().transform.localScale *= 1.5f;
                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_SecondPair[l_RandomPair].actualArticulo != null)
                            {

                                horizontal3Abajo[numJ].GetComponentInChildren<Text>().text = l_SecondPair[l_RandomPair].actualArticulo + l_SecondPair[l_RandomPair].palabraActual;
                                ModifyTextPair(horizontal3Abajo[numJ].GetComponentInChildren<Text>());

                            }
                        }
                        horizontal3Abajo[numJ].GetComponentInChildren<ConvertFont>().Convert();
                        horizontal3Abajo[numJ].SetActive(true);
                        PonerColorMarco(l_SecondPair[l_RandomPair].color, horizontal3Abajo[numJ].transform.GetChild(1).GetComponent<Image>());
                    }
                    break;
                case 4:
                    if (_firstTime)
                    {
                        horizontal4Abajo[numJ].name = numJ.ToString();
                        horizontal4Abajo[numJ].GetComponentInChildren<Text>().text = l_Pairs[l_RandomPair].palabraActual;
                        if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
                            horizontal4Abajo[numJ].GetComponentInChildren<Text>().transform.localScale *= 1.5f;
                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_Pairs[l_RandomPair].actualArticulo != null)
                            {

                                horizontal4Abajo[numJ].GetComponentInChildren<Text>().text = l_Pairs[l_RandomPair].actualArticulo + l_Pairs[l_RandomPair].palabraActual;
                                ModifyTextPair(horizontal2Abajo[numJ].GetComponentInChildren<Text>());

                            }
                        }
                        horizontal4Abajo[numJ].GetComponentInChildren<ConvertFont>().Convert();
                        horizontal4Abajo[numJ].SetActive(true);
                        PonerColorMarco(l_Pairs[l_RandomPair].color, horizontal4Abajo[numJ].transform.GetChild(1).GetComponent<Image>());

                    }
                    else
                    {
                        horizontal4Abajo[numJ].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();
                        horizontal4Abajo[numJ].GetComponentInChildren<Text>().text = l_SecondPair[l_RandomPair].palabraActual;
                        if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
                            horizontal4Abajo[numJ].GetComponentInChildren<Text>().transform.localScale *= 1.5f;
                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_SecondPair[l_RandomPair].actualArticulo != null)
                            {

                                horizontal4Abajo[numJ].GetComponentInChildren<Text>().text = l_SecondPair[l_RandomPair].actualArticulo + l_SecondPair[l_RandomPair].palabraActual;
                                ModifyTextPair(horizontal4Abajo[numJ].GetComponentInChildren<Text>());

                            }
                        }
                        horizontal4Abajo[numJ].GetComponentInChildren<ConvertFont>().Convert();
                        horizontal4Abajo[numJ].SetActive(true);
                        PonerColorMarco(l_SecondPair[l_RandomPair].color, horizontal4Abajo[numJ].transform.GetChild(1).GetComponent<Image>());

                    }
                    break;
            }
        }

        else
        {
            switch (_numofPairs)
            {
                case 2:

                    if (_firstTime)
                    {
                        vertical2Right[numJ].name = numJ.ToString();
                        vertical2Right[numJ].GetComponentInChildren<Text>().text = l_Pairs[l_RandomPair].palabraActual;
                        if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
                            vertical2Right[numJ].GetComponentInChildren<Text>().transform.localScale *= 1.5f;
                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_Pairs[l_RandomPair].actualArticulo != null)
                            {

                                vertical2Right[numJ].GetComponentInChildren<Text>().text = l_Pairs[l_RandomPair].actualArticulo + l_Pairs[l_RandomPair].palabraActual;
                                ModifyTextPair(vertical2Right[numJ].GetComponentInChildren<Text>());

                            }
                        }
                        vertical2Right[numJ].GetComponentInChildren<ConvertFont>().Convert();
                        vertical2Right[numJ].SetActive(true);
                        PonerColorMarco(l_Pairs[l_RandomPair].color, vertical2Right[numJ].transform.GetChild(1).GetComponent<Image>());

                    }
                    else
                    {
                        vertical2Right[numJ].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();
                        vertical2Right[numJ].GetComponentInChildren<Text>().text = l_SecondPair[l_RandomPair].palabraActual;
                        if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
                            vertical2Right[numJ].GetComponentInChildren<Text>().transform.localScale *= 1.5f;
                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_SecondPair[l_RandomPair].actualArticulo != null)
                            {

                                vertical2Right[numJ].GetComponentInChildren<Text>().text = l_SecondPair[l_RandomPair].actualArticulo + l_SecondPair[l_RandomPair].palabraActual;
                                ModifyTextPair(vertical2Right[numJ].GetComponentInChildren<Text>());

                            }
                        }
                        vertical2Right[numJ].GetComponentInChildren<ConvertFont>().Convert();
                        vertical2Right[numJ].SetActive(true);
                        PonerColorMarco(l_SecondPair[l_RandomPair].color, vertical2Right[numJ].transform.GetChild(1).GetComponent<Image>());

                    }
                    break;
                case 3:

                    if (_firstTime)
                    {
                        vertical3Right[numJ].name = numJ.ToString();
                        vertical3Right[numJ].GetComponentInChildren<Text>().text = l_Pairs[l_RandomPair].palabraActual;
                        if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
                            vertical3Right[numJ].GetComponentInChildren<Text>().transform.localScale *= 1.5f;
                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_Pairs[l_RandomPair].actualArticulo != null)
                            {

                                vertical3Right[numJ].GetComponentInChildren<Text>().text = l_Pairs[l_RandomPair].actualArticulo + l_Pairs[l_RandomPair].palabraActual;
                                ModifyTextPair(vertical3Right[numJ].GetComponentInChildren<Text>());

                            }
                        }
                        vertical3Right[numJ].GetComponentInChildren<ConvertFont>().Convert();
                        vertical3Right[numJ].SetActive(true);
                        PonerColorMarco(l_Pairs[l_RandomPair].color, vertical3Right[numJ].transform.GetChild(1).GetComponent<Image>());

                    }
                    else
                    {
                        vertical3Right[numJ].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();
                        vertical3Right[numJ].GetComponentInChildren<Text>().text = l_SecondPair[l_RandomPair].palabraActual;
                        if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
                            vertical3Right[numJ].GetComponentInChildren<Text>().transform.localScale *= 1.5f;
                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_SecondPair[l_RandomPair].actualArticulo != null)
                            {

                                vertical3Right[numJ].GetComponentInChildren<Text>().text = l_SecondPair[l_RandomPair].actualArticulo + l_SecondPair[l_RandomPair].palabraActual;
                                ModifyTextPair(vertical3Right[numJ].GetComponentInChildren<Text>());

                            }
                        }
                        vertical3Right[numJ].GetComponentInChildren<ConvertFont>().Convert();
                        vertical3Right[numJ].SetActive(true);
                        PonerColorMarco(l_SecondPair[l_RandomPair].color, vertical3Right[numJ].transform.GetChild(1).GetComponent<Image>());

                    }
                    break;
                case 4:

                    if (_firstTime)
                    {
                        vertical4Right[numJ].name = numJ.ToString();
                        vertical4Right[numJ].GetComponentInChildren<Text>().text = l_Pairs[l_RandomPair].palabraActual;
                        if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
                            vertical4Right[numJ].GetComponentInChildren<Text>().transform.localScale *= 1.5f;
                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_Pairs[l_RandomPair].actualArticulo != null)
                            {

                                vertical4Right[numJ].GetComponentInChildren<Text>().text = l_Pairs[l_RandomPair].actualArticulo + l_Pairs[l_RandomPair].palabraActual;
                                ModifyTextPair(vertical4Right[numJ].GetComponentInChildren<Text>());

                            }
                        }
                        vertical4Right[numJ].GetComponentInChildren<ConvertFont>().Convert();
                        vertical4Right[numJ].SetActive(true);
                        PonerColorMarco(l_Pairs[l_RandomPair].color, vertical4Right[numJ].transform.GetChild(1).GetComponent<Image>());

                    }
                    else
                    {
                        vertical4Right[numJ].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();
                        vertical4Right[numJ].GetComponentInChildren<Text>().text = l_SecondPair[l_RandomPair].palabraActual;
                        if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
                            vertical4Right[numJ].GetComponentInChildren<Text>().transform.localScale *= 1.5f;
                        if (GameManager.configuration.palabrasConArticulo)
                        {
                            if (l_SecondPair[l_RandomPair].actualArticulo != null)
                            {

                                vertical4Right[numJ].GetComponentInChildren<Text>().text = l_SecondPair[l_RandomPair].actualArticulo + l_SecondPair[l_RandomPair].palabraActual;
                                ModifyTextPair(vertical4Right[numJ].GetComponentInChildren<Text>());

                            }
                        }
                        vertical4Right[numJ].GetComponentInChildren<ConvertFont>().Convert();
                        vertical4Right[numJ].SetActive(true);
                        PonerColorMarco(l_SecondPair[l_RandomPair].color, vertical4Right[numJ].transform.GetChild(1).GetComponent<Image>());

                    }
                    break;
            }
        }
    }


    public void Clear()
    {
        foreach (var item in horizontal4Arriba)
        {
            item.SetActive(false);
        }
        foreach (var item in horizontal4Abajo)
        {
            item.SetActive(false);
        }
        foreach (var item in horizontal3Arriba)
        {
            item.SetActive(false);
        }
        foreach (var item in horizontal3Abajo)
        {
            item.SetActive(false);
        }
        foreach (var item in vertical4Left)
        {
            item.SetActive(false);
        }
        foreach (var item in vertical4Right)
        {
            item.SetActive(false);
        }
        foreach (var item in vertical3Left)
        {
            item.SetActive(false);
        }
        foreach (var item in vertical3Right)
        {
            item.SetActive(false);
        }

    }


    private void ModifyTextPair(Text _text)
    {
        Text texto = _text;
        if (texto.text.Length < 9 && texto.text.Length > 7)
            texto.gameObject.transform.localScale = texto.gameObject.transform.localScale * 0.9f;
        else if (texto.text.Length < 12 && texto.text.Length > 7)
        {
            texto.gameObject.transform.localScale = texto.gameObject.transform.localScale * 0.83f;
        }
        else if (texto.text.Length > 7)
        {
            texto.gameObject.transform.localScale = texto.gameObject.transform.localScale * 0.72f;

        }

    }

    private void CopyWords(PalabraBD toCopy, ref PalabraBD palabra)
    {
        palabra.image1 = toCopy.image1;
        palabra.image2 = toCopy.image2;
        palabra.image3 = toCopy.image3;
        palabra.audio = toCopy.audio;
        palabra.imagePuzzle = toCopy.imagePuzzle;
        palabra.piecesPuzzle = toCopy.piecesPuzzle;
        palabra.palabraActual = toCopy.palabraActual;
        palabra.color = toCopy.color;
        palabra.user = toCopy.user;
        palabra.nameSpanish = toCopy.nameSpanish;
        palabra.nameCatalan = toCopy.nameCatalan;
        if (toCopy.articulos != null)
        {
            foreach (var item in toCopy.articulos)
            {
                palabra.articulos.Add(new Articulo());
                palabra.articulos[palabra.articulos.Count - 1].articuloSpanish = item.articuloSpanish;
                palabra.articulos[palabra.articulos.Count - 1].audiosArticuloSpanish = item.audiosArticuloSpanish;
                palabra.articulos[palabra.articulos.Count - 1].articuloCatalan = item.articuloCatalan;
                palabra.articulos[palabra.articulos.Count - 1].audiosArticuloCatalan = item.audiosArticuloCatalan;
            }
        }
        palabra.SetPalabraActual();
    }

    public void PonerColorMarco(string _color, Image _marco)
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString(_color, out color);
        _marco.color = color;

    }

    public void CallCoroutine(AudioClip clip, bool _artOnly)
    {
        if (!_artOnly)
            StartCoroutine(WaitForArticle(clip));
    }

    IEnumerator WaitForArticle(AudioClip clip)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.clip = clip;
        audioSource.Play();
    }
}