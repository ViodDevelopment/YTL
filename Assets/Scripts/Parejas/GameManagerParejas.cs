using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;



public class GameManagerParejas : MonoBehaviour

{
    public GameObject dumi;

    public SceneManagement m_Scener;

    public GameObject m_Canvas;

    public GameObject m_RealCanvas;

    [HideInInspector]

    public Animation m_Animation;

    public GameObject m_ImageZoom;

    public Image m_ImageZoomed;

    public Image planeImageWhenPair;

    public Text m_TextZoomed;

    public List<Texture2D> m_ImagePairs = new List<Texture2D>();

    public List<string> palabrasCastellano = new List<string>();

    public List<string> palabrasCatalan = new List<string>();

    public List<AudioClip> audiosCastellano = new List<AudioClip>();

    public List<AudioClip> audiosCatalan = new List<AudioClip>();

    List<Texture2D> RepeatList = new List<Texture2D>();

    List<string> repeatListPalabras = new List<string>();

    List<AudioClip> repeatListAudios = new List<AudioClip>();

    int m_CurrentNumRep = 1;

    public int m_CurrentPairs;

    private bool completed;
    private bool repeating;



    #region Separador

    #endregion



    public GameObject m_Plantilla;

    public GameObject m_PlantillaPareja;



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



    public List<GameObject> vertical4Left = new List<GameObject>();
    public List<GameObject> vertical4Right = new List<GameObject>();

    public List<GameObject> vertical3Left = new List<GameObject>();
    public List<GameObject> vertical3Right = new List<GameObject>();

    #endregion



    #region Points

    public Sprite m_CompletedPoint;

    public Sprite m_IncompletedPoint;

    public Transform m_SpawnImpar;

    public Transform m_SpawnPar;

    Transform m_CurrentSpawn;

    public GameObject m_Point;

    static int l_NumReps = GameManager.Instance.m_NeededToMinigame;

    GameObject[] m_Points = new GameObject[l_NumReps];

    public GameObject m_Siguiente;

    public GameObject m_Repetir;

    #endregion



    void Start()
    {

        Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Minute + Random.seed + 1);
        m_NumPairs = Random.Range(3, 5);
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



        for (int i = 0; i <= GameManager.m_CurrentToMinigame[0]; i++)
        {
            if (i > 0 && m_Points.Length > i - 1)
                m_Points[i - 1].GetComponent<Image>().sprite = m_CompletedPoint;

        }

        completed = false;
        repeating = false;

        InstantiatePairs();

        m_Animation = m_RealCanvas.GetComponent<Animation>();

    }



    private void Update()
    {

        if (completed)
        {

            completed = false;
            StartCoroutine(WaitSeconds(1.5f));

        }
        else if (m_ImageZoom.activeSelf && GameManager.Instance.InputRecieved() && !m_RealCanvas.GetComponent<Animation>().isPlaying && m_CurrentPairs != m_NumPairs)
        {

            m_ImageZoom.SetActive(false);

            planeImageWhenPair.gameObject.SetActive(false);



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

        if (GameManager.m_CurrentToMinigame[0] > 0 && m_Points.Length > GameManager.m_CurrentToMinigame[0] - 1)
            m_Points[GameManager.m_CurrentToMinigame[0] - 1].GetComponent<Image>().sprite = m_CompletedPoint;

        m_CurrentNumRep = 1;

        currentNumOfPairs = 0;



        List<Texture2D> l_Pairs = new List<Texture2D>();

        List<string> l_Palabras = new List<string>();

        List<AudioClip> l_Audios = new List<AudioClip>();



        foreach (Texture2D item in m_ImagePairs)
        {

            l_Pairs.Add(item);

        }



        foreach (string item in ObtainListOfPalabras())
        {

            l_Palabras.Add(item);

        }



        foreach (AudioClip item in ObtainListOfAudios())
        {

            l_Audios.Add(item);

        }


        /* foreach (Texture2D item in m_ImagePairs)

         {

             l_Pairs.Add(item);

         }*/



        List<Texture2D> l_SecondPair = new List<Texture2D>();

        List<Texture2D> l_ThirdPair = new List<Texture2D>();

        RepeatList = l_ThirdPair;



        List<string> l_SecondPalabra = new List<string>();

        List<string> l_ThirdPalabra = new List<string>();

        repeatListPalabras = l_ThirdPalabra;



        List<AudioClip> l_SecondAudio = new List<AudioClip>();

        List<AudioClip> l_ThirdAudio = new List<AudioClip>();

        repeatListAudios = l_ThirdAudio;


        if (Random.Range(0, 2) == 1)
        {
            m_IsHorizontal = true;
        }
        else
        {
            m_IsHorizontal = false;
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

                        InstiantateCopy(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, l_Palabras, l_SecondPalabra, l_Audios, l_SecondAudio, m_NumPairs, currentNumOfPairs, j);




                        l_SecondPair.Add(l_Pairs[l_RandomPair]);

                        l_ThirdPair.Add(l_Pairs[l_RandomPair]);

                        l_Pairs.RemoveAt(l_RandomPair);



                        l_SecondPalabra.Add(l_Palabras[l_RandomPair]);

                        l_ThirdPalabra.Add(l_Palabras[l_RandomPair]);

                        l_Palabras.RemoveAt(l_RandomPair);



                        l_SecondAudio.Add(l_Audios[l_RandomPair]);

                        l_ThirdAudio.Add(l_Audios[l_RandomPair]);

                        l_Audios.RemoveAt(l_RandomPair);

                        k++;
                        k++;
                    }

                }

                else
                {

                    for (int j = 0; j < m_NumPairs; j++)
                    {

                        int l_RandomPair = Random.Range(0, l_SecondPair.Count);

                        InstiantatePair(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, l_Palabras, l_SecondPalabra, l_Audios, l_SecondAudio, m_NumPairs, currentNumOfPairs, j);


                        currentNumOfPairs++;

                        l_SecondPair.RemoveAt(l_RandomPair);

                        l_SecondPalabra.RemoveAt(l_RandomPair);

                        l_SecondAudio.RemoveAt(l_RandomPair);

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

                        InstiantatePair(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, l_Palabras, l_SecondPalabra, l_Audios, l_SecondAudio, m_NumPairs, currentNumOfPairs, j);


                        currentNumOfPairs++;



                        l_SecondPair.Add(l_Pairs[l_RandomPair]);

                        l_ThirdPair.Add(l_Pairs[l_RandomPair]);

                        l_Pairs.RemoveAt(l_RandomPair);



                        l_SecondPalabra.Add(l_Palabras[l_RandomPair]);

                        l_ThirdPalabra.Add(l_Palabras[l_RandomPair]);

                        l_Palabras.RemoveAt(l_RandomPair);



                        l_SecondAudio.Add(l_Audios[l_RandomPair]);

                        l_ThirdAudio.Add(l_Audios[l_RandomPair]);

                        l_Audios.RemoveAt(l_RandomPair);

                        k++;
                        k++;
                    }

                }

                else
                {

                    for (int j = 0; j < m_NumPairs; j++)
                    {

                        int l_RandomPair = Random.Range(0, l_SecondPair.Count);

                        InstiantateCopy(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, l_Palabras, l_SecondPalabra, l_Audios, l_SecondAudio, m_NumPairs, currentNumOfPairs, j);


                        l_SecondPair.RemoveAt(l_RandomPair);

                        l_SecondPalabra.RemoveAt(l_RandomPair);

                        l_SecondAudio.RemoveAt(l_RandomPair);

                        k++;
                        k++;

                    }

                }
                m_FirstPair = false;
                m_XPos *= 3f;

            }

        }

    }



    public void NextPairs(bool next = false)
    {

        Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Minute);

        currentNumOfPairs = 0;

        m_CurrentPairs = 0;
        completed = false;
        repeating = false;
        m_ImageZoom.SetActive(false);
        planeImageWhenPair.gameObject.SetActive(false);

        m_CurrentNumRep = 1;



        if (GameManager.m_CurrentToMinigame[0] >= GameManager.Instance.m_NeededToMinigame)
        {
            GameManager.ResetPointToMinigame(0);
            m_Scener.NextGame();
        }
        else

        {
            if (GameManager.m_CurrentToMinigame[0] > 0)
                m_Points[GameManager.m_CurrentToMinigame[0] - 1].GetComponent<Image>().sprite = m_CompletedPoint;

            List<Texture2D> l_Pairs = new List<Texture2D>();

            List<string> l_Palabras = new List<string>();

            List<AudioClip> l_Audios = new List<AudioClip>();



            foreach (Texture2D item in m_ImagePairs)
            {

                l_Pairs.Add(item);

            }



            foreach (string item in ObtainListOfPalabras())
            {

                l_Palabras.Add(item);

            }



            foreach (AudioClip item in ObtainListOfAudios())
            {

                l_Audios.Add(item);

            }



            List<Texture2D> l_SecondPair = new List<Texture2D>();

            List<Texture2D> l_ThirdPair = new List<Texture2D>();

            RepeatList = l_ThirdPair;



            List<string> l_SecondPalabra = new List<string>();

            List<string> l_ThirdPalabra = new List<string>();

            repeatListPalabras = l_ThirdPalabra;



            List<AudioClip> l_SecondAudio = new List<AudioClip>();

            List<AudioClip> l_ThirdAudio = new List<AudioClip>();

            repeatListAudios = l_ThirdAudio;



            if (Random.Range(0, 2) == 1)
            {
                m_IsHorizontal = true;
            }
            else
            {
                m_IsHorizontal = false;
            }

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

                            InstiantateCopy(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, l_Palabras, l_SecondPalabra, l_Audios, l_SecondAudio, m_NumPairs, currentNumOfPairs, j);




                            l_SecondPair.Add(l_Pairs[l_RandomPair]);

                            l_ThirdPair.Add(l_Pairs[l_RandomPair]);

                            l_Pairs.RemoveAt(l_RandomPair);



                            l_SecondPalabra.Add(l_Palabras[l_RandomPair]);

                            l_ThirdPalabra.Add(l_Palabras[l_RandomPair]);

                            l_Palabras.RemoveAt(l_RandomPair);



                            l_SecondAudio.Add(l_Audios[l_RandomPair]);

                            l_ThirdAudio.Add(l_Audios[l_RandomPair]);

                            l_Audios.RemoveAt(l_RandomPair);

                            k++;
                            k++;
                        }

                    }

                    else
                    {

                        for (int j = 0; j < m_NumPairs; j++)
                        {

                            int l_RandomPair = Random.Range(0, l_SecondPair.Count);

                            InstiantatePair(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, l_Palabras, l_SecondPalabra, l_Audios, l_SecondAudio, m_NumPairs, currentNumOfPairs, j);

                            currentNumOfPairs++;



                            l_SecondPair.RemoveAt(l_RandomPair);

                            l_SecondPalabra.RemoveAt(l_RandomPair);

                            l_SecondAudio.RemoveAt(l_RandomPair);

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

                            InstiantatePair(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, l_Palabras, l_SecondPalabra, l_Audios, l_SecondAudio, m_NumPairs, currentNumOfPairs, j);


                            currentNumOfPairs++;



                            l_SecondPair.Add(l_Pairs[l_RandomPair]);

                            l_ThirdPair.Add(l_Pairs[l_RandomPair]);

                            l_Pairs.RemoveAt(l_RandomPair);



                            l_SecondPalabra.Add(l_Palabras[l_RandomPair]);

                            l_ThirdPalabra.Add(l_Palabras[l_RandomPair]);

                            l_Palabras.RemoveAt(l_RandomPair);



                            l_SecondAudio.Add(l_Audios[l_RandomPair]);

                            l_ThirdAudio.Add(l_Audios[l_RandomPair]);

                            l_Audios.RemoveAt(l_RandomPair);

                            k++;
                            k++;
                        }

                    }

                    else
                    {

                        for (int j = 0; j < m_NumPairs; j++)
                        {


                            int l_RandomPair = Random.Range(0, l_SecondPair.Count);

                            InstiantateCopy(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, l_Palabras, l_SecondPalabra, l_Audios, l_SecondAudio, m_NumPairs, currentNumOfPairs, j);


                            l_SecondPair.RemoveAt(l_RandomPair);

                            l_SecondPalabra.RemoveAt(l_RandomPair);

                            l_SecondAudio.RemoveAt(l_RandomPair);

                            k++;
                            k++;
                        }

                    }
                    m_FirstPair = false;
                    m_XPos *= 3f;

                }

            }

        }

    }



    public void RepeatPairs()
    {

        Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Minute);

        m_CurrentNumRep++;

        m_CurrentPairs = 0;
        completed = false;
        repeating = true;
        m_ImageZoom.SetActive(false);
        planeImageWhenPair.gameObject.SetActive(false);

        currentNumOfPairs = 0;



        List<Texture2D> l_Pairs = new List<Texture2D>();

        List<string> l_Palabras = new List<string>();

        List<AudioClip> l_Audios = new List<AudioClip>();



        foreach (Texture2D item in RepeatList)
        {

            l_Pairs.Add(item);

        }



        foreach (string item in repeatListPalabras)
        {

            l_Palabras.Add(item);

        }



        foreach (AudioClip item in repeatListAudios)
        {

            l_Audios.Add(item);

        }

        List<Texture2D> l_SecondPair = new List<Texture2D>();

        List<Texture2D> l_ThirdPair = new List<Texture2D>();



        List<string> l_SecondPalabra = new List<string>();

        List<string> l_ThirdPalabra = new List<string>();



        List<AudioClip> l_SecondAudio = new List<AudioClip>();

        List<AudioClip> l_ThirdAudio = new List<AudioClip>();


        if (Random.Range(0, 2) == 1)
        {
            m_IsHorizontal = true;
        }
        else
        {
            m_IsHorizontal = false;
        }

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

                        InstiantateCopy(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, l_Palabras, l_SecondPalabra, l_Audios, l_SecondAudio, m_NumPairs, currentNumOfPairs, j);



                        l_SecondPair.Add(l_Pairs[l_RandomPair]);

                        l_ThirdPair.Add(l_Pairs[l_RandomPair]);

                        l_Pairs.RemoveAt(l_RandomPair);



                        l_SecondPalabra.Add(l_Palabras[l_RandomPair]);

                        l_ThirdPalabra.Add(l_Palabras[l_RandomPair]);

                        l_Palabras.RemoveAt(l_RandomPair);



                        l_SecondAudio.Add(l_Audios[l_RandomPair]);

                        l_ThirdAudio.Add(l_Audios[l_RandomPair]);

                        l_Audios.RemoveAt(l_RandomPair);

                        k++;
                        k++;
                    }
                }

                else
                {

                    for (int j = 0; j < m_NumPairs; j++)
                    {
                        int l_RandomPair = Random.Range(0, l_SecondPair.Count);

                        InstiantatePair(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, l_Palabras, l_SecondPalabra, l_Audios, l_SecondAudio, m_NumPairs, currentNumOfPairs, j);


                        currentNumOfPairs++;



                        l_SecondPair.RemoveAt(l_RandomPair);

                        l_SecondPalabra.RemoveAt(l_RandomPair);

                        l_SecondAudio.RemoveAt(l_RandomPair);

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

                        InstiantatePair(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, l_Palabras, l_SecondPalabra, l_Audios, l_SecondAudio, m_NumPairs, currentNumOfPairs, j);


                        currentNumOfPairs++;



                        l_SecondPair.Add(l_Pairs[l_RandomPair]);

                        l_ThirdPair.Add(l_Pairs[l_RandomPair]);

                        l_Pairs.RemoveAt(l_RandomPair);



                        l_SecondPalabra.Add(l_Palabras[l_RandomPair]);

                        l_ThirdPalabra.Add(l_Palabras[l_RandomPair]);

                        l_Palabras.RemoveAt(l_RandomPair);



                        l_SecondAudio.Add(l_Audios[l_RandomPair]);

                        l_ThirdAudio.Add(l_Audios[l_RandomPair]);

                        l_Audios.RemoveAt(l_RandomPair);

                        k++;
                        k++;
                    }
                }

                else
                {

                    for (int j = 0; j < m_NumPairs; j++)
                    {
                        int l_RandomPair = Random.Range(0, l_SecondPair.Count);

                        InstiantateCopy(m_FirstPair, m_IsHorizontal, l_RandomPair, l_Pairs, l_SecondPair, l_ThirdPair, l_Palabras, l_SecondPalabra, l_Audios, l_SecondAudio, m_NumPairs, currentNumOfPairs, j);

                        l_SecondPair.RemoveAt(l_RandomPair);

                        l_SecondPalabra.RemoveAt(l_RandomPair);

                        l_SecondAudio.RemoveAt(l_RandomPair);

                        k++;
                        k++;
                    }
                }

                m_FirstPair = false;

                m_XPos *= 3f;

            }

        }

    }



    public void ActivateButtons()
    {

        m_Siguiente.SetActive(true);

        if (m_CurrentNumRep < GameManager.Repeticiones)
        {
            m_Repetir.SetActive(true);
        }
    }



    IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (GameObject.Find("Dumi(Clone)") == null)
        {
            GameObject pinguino = Instantiate(dumi, dumi.transform.position, dumi.transform.rotation);
            pinguino.GetComponent<Dumi>().AudioPositivo();
        }
        if (!repeating)
        {
            if (m_Points.Length > GameManager.m_CurrentToMinigame[1] - 1)
                GameManager.SumPointToMinigame(0);
            if (GameManager.m_CurrentToMinigame[0] > 0 && m_Points.Length > GameManager.m_CurrentToMinigame[0] - 1)
                m_Points[GameManager.m_CurrentToMinigame[0] - 1].GetComponent<Image>().sprite = m_CompletedPoint;
        }
        ActivateButtons();
    }



    public void PairDone()
    {
        m_CurrentPairs++;
        m_Animation.Play();
        if (m_CurrentPairs == m_NumPairs)
            completed = true;
    }

    private void InstiantatePair(bool _firstTime, bool _horizontal, int l_RandomPair, List<Texture2D> l_Pairs, List<Texture2D> l_SecondPair, List<Texture2D> l_ThirdPair, List<string> l_Palabras, List<string> l_SecondPalabra, List<AudioClip> l_Audios, List<AudioClip> l_SecondAudio, int _numofPairs, int _currentPair, int numJ)
    {
        if (_horizontal)
        {
            switch (_numofPairs)
            {
                case 3:
                    if (_firstTime)
                    {
                        horizontal3Arriba[_currentPair].GetComponent<Image>().sprite = Sprite.Create(l_Pairs[l_RandomPair], new Rect(0, 0, l_Pairs[l_RandomPair].width / 1.02f, l_Pairs[l_RandomPair].height / 1.02f), Vector2.zero);

                        horizontal3Arriba[_currentPair].name = numJ.ToString();

                        horizontal3Arriba[_currentPair].GetComponent<Pairs>().nombre = l_Palabras[l_RandomPair];

                        horizontal3Arriba[_currentPair].GetComponent<Pairs>().audioClip = l_Audios[l_RandomPair];

                        horizontal3Arriba[_currentPair].GetComponent<Pairs>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        horizontal3Arriba[_currentPair].SetActive(true);
                    }

                    else
                    {
                        horizontal3Arriba[_currentPair].GetComponent<Image>().sprite = Sprite.Create(l_SecondPair[l_RandomPair], new Rect(0, 0, l_SecondPair[l_RandomPair].width / 1.02f, l_SecondPair[l_RandomPair].height / 1.02f), Vector2.zero);

                        horizontal3Arriba[_currentPair].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();

                        horizontal3Arriba[_currentPair].GetComponent<Pairs>().nombre = l_SecondPalabra[l_RandomPair];

                        horizontal3Arriba[_currentPair].GetComponent<Pairs>().audioClip = l_SecondAudio[l_RandomPair];

                        horizontal3Arriba[_currentPair].GetComponent<Pairs>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        horizontal3Arriba[_currentPair].SetActive(true);
                    }
                    break;

                case 4:
                    if (_firstTime)
                    {

                        horizontal4Arriba[_currentPair].GetComponent<Image>().sprite = Sprite.Create(l_Pairs[l_RandomPair], new Rect(0, 0, l_Pairs[l_RandomPair].width / 1.02f, l_Pairs[l_RandomPair].height / 1.02f), Vector2.zero);

                        horizontal4Arriba[_currentPair].name = numJ.ToString();

                        horizontal4Arriba[_currentPair].GetComponent<Pairs>().nombre = l_Palabras[l_RandomPair];

                        horizontal4Arriba[_currentPair].GetComponent<Pairs>().audioClip = l_Audios[l_RandomPair];

                        horizontal4Arriba[_currentPair].GetComponent<Pairs>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        horizontal4Arriba[_currentPair].SetActive(true);

                    }
                    else
                    {
                        horizontal4Arriba[_currentPair].GetComponent<Image>().sprite = Sprite.Create(l_SecondPair[l_RandomPair], new Rect(0, 0, l_SecondPair[l_RandomPair].width / 1.02f, l_SecondPair[l_RandomPair].height / 1.02f), Vector2.zero);

                        horizontal4Arriba[_currentPair].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();

                        horizontal4Arriba[_currentPair].GetComponent<Pairs>().nombre = l_SecondPalabra[l_RandomPair];

                        horizontal4Arriba[_currentPair].GetComponent<Pairs>().audioClip = l_SecondAudio[l_RandomPair];

                        horizontal4Arriba[_currentPair].GetComponent<Pairs>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        horizontal4Arriba[_currentPair].SetActive(true);
                    }
                    break;
            }
        }
        else
        {
            switch (_numofPairs)
            {
                case 3:
                    if (_firstTime)
                    {

                        vertical3Left[_currentPair].GetComponent<Image>().sprite = Sprite.Create(l_Pairs[l_RandomPair], new Rect(0, 0, l_Pairs[l_RandomPair].width / 1.02f, l_Pairs[l_RandomPair].height / 1.02f), Vector2.zero);

                        vertical3Left[_currentPair].name = numJ.ToString();

                        vertical3Left[_currentPair].GetComponent<Pairs>().nombre = l_Palabras[l_RandomPair];

                        vertical3Left[_currentPair].GetComponent<Pairs>().audioClip = l_Audios[l_RandomPair];

                        vertical3Left[_currentPair].GetComponent<Pairs>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        vertical3Left[_currentPair].SetActive(true);

                    }
                    else
                    {
                        vertical3Left[_currentPair].GetComponent<Image>().sprite = Sprite.Create(l_SecondPair[l_RandomPair], new Rect(0, 0, l_SecondPair[l_RandomPair].width / 1.02f, l_SecondPair[l_RandomPair].height / 1.02f), Vector2.zero);

                        vertical3Left[_currentPair].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();

                        vertical3Left[_currentPair].GetComponent<Pairs>().nombre = l_SecondPalabra[l_RandomPair];

                        vertical3Left[_currentPair].GetComponent<Pairs>().audioClip = l_SecondAudio[l_RandomPair];

                        vertical3Left[_currentPair].GetComponent<Pairs>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        vertical3Left[_currentPair].SetActive(true);

                    }
                    break;
                case 4:
                    if (_firstTime)
                    {

                        vertical4Left[_currentPair].GetComponent<Image>().sprite = Sprite.Create(l_Pairs[l_RandomPair], new Rect(0, 0, l_Pairs[l_RandomPair].width / 1.02f, l_Pairs[l_RandomPair].height / 1.02f), Vector2.zero);

                        vertical4Left[_currentPair].name = numJ.ToString();

                        vertical4Left[_currentPair].GetComponent<Pairs>().nombre = l_Palabras[l_RandomPair];

                        vertical4Left[_currentPair].GetComponent<Pairs>().audioClip = l_Audios[l_RandomPair];

                        vertical4Left[_currentPair].GetComponent<Pairs>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        vertical4Left[_currentPair].SetActive(true);

                    }
                    else
                    {
                        vertical4Left[_currentPair].GetComponent<Image>().sprite = Sprite.Create(l_SecondPair[l_RandomPair], new Rect(0, 0, l_SecondPair[l_RandomPair].width / 1.02f, l_SecondPair[l_RandomPair].height / 1.02f), Vector2.zero);

                        vertical4Left[_currentPair].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();

                        vertical4Left[_currentPair].GetComponent<Pairs>().nombre = l_SecondPalabra[l_RandomPair];

                        vertical4Left[_currentPair].GetComponent<Pairs>().audioClip = l_SecondAudio[l_RandomPair];

                        vertical4Left[_currentPair].GetComponent<Pairs>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();

                        vertical4Left[_currentPair].SetActive(true);
                    }
                    break;
            }
        }
    }

    private void InstiantateCopy(bool _firstTime, bool _horizontal, int l_RandomPair, List<Texture2D> l_Pairs, List<Texture2D> l_SecondPair, List<Texture2D> l_ThirdPair, List<string> l_Palabras, List<string> l_SecondPalabra, List<AudioClip> l_Audios, List<AudioClip> l_SecondAudio, int _numofPairs, int _currentPair, int numJ)
    {
        if (_horizontal)
        {
            switch (_numofPairs)
            {
                case 3:
                    if (_firstTime)
                    {
                        horizontal3Abajo[numJ].GetComponent<Image>().sprite = Sprite.Create(l_Pairs[l_RandomPair], new Rect(0, 0, l_Pairs[l_RandomPair].width / 1.02f, l_Pairs[l_RandomPair].height / 1.02f), Vector2.zero);
                        horizontal3Abajo[numJ].name = numJ.ToString();
                        horizontal3Abajo[numJ].SetActive(true);
                    }
                    else
                    {
                        horizontal3Abajo[numJ].GetComponent<Image>().sprite = Sprite.Create(l_SecondPair[l_RandomPair], new Rect(0, 0, l_SecondPair[l_RandomPair].width / 1.02f, l_SecondPair[l_RandomPair].height / 1.02f), Vector2.zero);
                        horizontal3Abajo[numJ].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();
                        horizontal3Abajo[numJ].SetActive(true);
                    }
                    break;
                case 4:
                    if (_firstTime)
                    {
                        horizontal4Abajo[numJ].GetComponent<Image>().sprite = Sprite.Create(l_Pairs[l_RandomPair], new Rect(0, 0, l_Pairs[l_RandomPair].width / 1.02f, l_Pairs[l_RandomPair].height / 1.02f), Vector2.zero);
                        horizontal4Abajo[numJ].name = numJ.ToString();
                        horizontal4Abajo[numJ].SetActive(true);
                    }
                    else
                    {
                        horizontal4Abajo[numJ].GetComponent<Image>().sprite = Sprite.Create(l_SecondPair[l_RandomPair], new Rect(0, 0, l_SecondPair[l_RandomPair].width / 1.02f, l_SecondPair[l_RandomPair].height / 1.02f), Vector2.zero);
                        horizontal4Abajo[numJ].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();
                        horizontal4Abajo[numJ].SetActive(true);
                    }
                    break;
            }
        }

        else
        {
            switch (_numofPairs)
            {
                case 3:

                    if (_firstTime)
                    {
                        vertical3Right[numJ].GetComponent<Image>().sprite = Sprite.Create(l_Pairs[l_RandomPair], new Rect(0, 0, l_Pairs[l_RandomPair].width / 1.02f, l_Pairs[l_RandomPair].height / 1.02f), Vector2.zero);
                        vertical3Right[numJ].name = numJ.ToString();
                        vertical3Right[numJ].SetActive(true);
                    }
                    else
                    {
                        vertical3Right[numJ].GetComponent<Image>().sprite = Sprite.Create(l_SecondPair[l_RandomPair], new Rect(0, 0, l_SecondPair[l_RandomPair].width / 1.02f, l_SecondPair[l_RandomPair].height / 1.02f), Vector2.zero);
                        vertical3Right[numJ].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();
                        vertical3Right[numJ].SetActive(true);
                    }
                    break;
                case 4:

                    if (_firstTime)
                    {
                        vertical4Right[numJ].GetComponent<Image>().sprite = Sprite.Create(l_Pairs[l_RandomPair], new Rect(0, 0, l_Pairs[l_RandomPair].width / 1.02f, l_Pairs[l_RandomPair].height / 1.02f), Vector2.zero);
                        vertical4Right[numJ].name = numJ.ToString();
                        vertical4Right[numJ].SetActive(true);
                    }
                    else
                    {
                        vertical4Right[numJ].GetComponent<Image>().sprite = Sprite.Create(l_SecondPair[l_RandomPair], new Rect(0, 0, l_SecondPair[l_RandomPair].width / 1.02f, l_SecondPair[l_RandomPair].height / 1.02f), Vector2.zero);
                        vertical4Right[numJ].name = l_ThirdPair.IndexOf(l_SecondPair[l_RandomPair]).ToString();
                        vertical4Right[numJ].SetActive(true);
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


    private List<string> ObtainListOfPalabras()
    {

        switch (SingletonLenguage.GetInstance().GetLenguage())
        {
            case SingletonLenguage.Lenguage.CASTELLANO:
                return palabrasCastellano;

            case SingletonLenguage.Lenguage.CATALAN:
                return palabrasCatalan;

            default:
                return palabrasCastellano;
        }

    }

    private List<AudioClip> ObtainListOfAudios()
    {
        switch (SingletonLenguage.GetInstance().GetLenguage())
        {
            case SingletonLenguage.Lenguage.CASTELLANO:
                return audiosCastellano;

            case SingletonLenguage.Lenguage.CATALAN:
                return audiosCatalan;

            default:
                return audiosCastellano;
        }
    }
}