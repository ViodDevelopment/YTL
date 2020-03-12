using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManagerPuzzle : MonoBehaviour
{
    public GameObject dumi;
    public Animation m_AnimationCenter;
    public Image m_ImageAnim;
    public Text m_TextAnim;
    public List<Image> marcos = new List<Image>();
    private List<PalabraBD> palabrasDisponibles = new List<PalabraBD>();

    List<GameObject> m_Words = new List<GameObject>();
    public SceneManagement m_Scener;
    int m_CurrentNumRep = 1;
    public GameObject m_ImageTemplate;
    public GameObject m_ColliderTemplate;
    public GameObject m_CollidersSpawns;
    public GameObject m_ImagesSpawn;
    public GameObject m_Canvas;

    private PalabraBD palabraActual = new PalabraBD();
    Texture2D m_ImagePuzzle;
    public GameObject m_Word;
    public Transform m_WordTransform;
    public GameObject m_UnseenWord;
    public Transform m_UnseenWordTransform;

    [HideInInspector]
    public int m_Puntuacion = 0;
    public int m_NumPieces = 4;
    int m_NumPiecesX;
    int m_NumPiecesY;
    bool m_Completed;
    private bool repeating;
    int numRandom = 0;

    public Sprite m_CompletedPoint;
    public Transform m_SpawnImpar;
    public Transform m_SpawnPar;
    Transform m_CurrentSpawn;
    public GameObject m_Point;
    static int l_NumReps = 3;
    GameObject[] m_Points = new GameObject[l_NumReps];

    List<GameObject> m_Images = new List<GameObject>();
    List<GameObject> m_Colliders = new List<GameObject>();

    public GameObject m_Siguiente;
    public GameObject m_Repetir;
    public Image m_ActivitiesButton;

    public int[] PuzzlePiecesPossibilities;

    private bool acabado = false;
    private Vector3 startSizeText;
    public GameObject m_Saver;
    private int lvl;

    private void Start()
    {
        lvl = 1;
        InitBaseOfDates();
        startSizeText = m_TextAnim.transform.localScale;
        Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Minute);
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

        for (int i = 0; i <= GameManager.m_CurrentToMinigame[2]; i++)
        {
            if (i > 0 && m_Points.Length > i - 1)
                m_Points[i - 1].GetComponent<Image>().sprite = m_CompletedPoint;
        }
        repeating = false;
        m_Completed = false;
        InicioPuzzle();

    }

    private void InitBaseOfDates()
    {
        palabrasDisponibles.Clear();

        if (PaquetePuzzle.GetInstance(lvl).acabado)
        {
            foreach (PalabraBD p in GameManager.palabrasDisponibles)
            {
                if (p.paquet == GameManager.configurartion.paquete)
                {

                    if (p.imagePuzzle != 0)
                    {

                        for (int i = 0; i < p.piecesPuzzle.Count; i++)
                        {
                            if (p.piecesPuzzle[i] >= 4)
                            {
                                palabrasDisponibles.Add(p);
                                break;
                            }
                        }
                    }
                }
                else if (GameManager.configurartion.paquete == -1)
                {
                    if (p.imagePuzzle != 0)
                    {

                        for (int i = 0; i < p.piecesPuzzle.Count; i++)
                        {
                            if (p.piecesPuzzle[i] >= 4)
                            {
                                palabrasDisponibles.Add(p);
                                break;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            foreach (PalabraBD p in PaquetePuzzle.GetInstance(lvl).currentPuzzlePaquet)
            {
                if (p.paquet == GameManager.configurartion.paquete)
                {

                    palabrasDisponibles.Add(p);

                }
                else if (GameManager.configurartion.paquete == -1)
                {

                    palabrasDisponibles.Add(p);

                }
            }
        }
        //ACTIVAR CUANDO VAYAN LAS PIEZAS DEL PUZZLE CON LAS PALABRAS DEL USUARIO
        /*foreach (PalabraBD p in GameManager.palabrasUserDisponibles)
        {
            if (SingletonLenguage.GetInstance().GetLenguage() == SingletonLenguage.Lenguage.CASTELLANO)
            {
                if (p.nameSpanish != "")
                    palabrasDisponibles.Add(p);
            }
            else if (SingletonLenguage.GetInstance().GetLenguage() == SingletonLenguage.Lenguage.CATALAN)
            {
                if (p.nameCatalan != "")
                    palabrasDisponibles.Add(p);
            }
        }*/
    }

    private void Update()
    {
        if (!m_Completed)
        {
            PuzzleComplete();
        }

        if (m_Canvas.activeSelf && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
            PassPuzzle();

        if (acabado)
        {
            if (!GameManager.configurartion.refuerzoPositivo)
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

        /*if (Input.GetKey(KeyCode.P))
            PassPuzzle();

        if (Input.GetKey(KeyCode.R))
            RepeatPuzzle();*/

    }

    public void PuzzleComplete()
    {
        if (m_Puntuacion == m_NumPieces)
        {
            m_Words[m_Words.Count - 2].GetComponent<MoveTouch>().canMove = true;
        }
        else if (m_Puntuacion == m_NumPieces + 1 && !m_Completed)
        {
            AudioSource l_AS = GetComponent<AudioSource>();
            l_AS.clip = palabrasDisponibles[numRandom].GetAudioClip(palabrasDisponibles[numRandom].audio);
            l_AS.Play();

            m_Completed = true;
            m_ImageAnim.gameObject.SetActive(true);
            m_AnimationCenter.Play();
            m_ImagesSpawn.SetActive(false);
            m_CollidersSpawns.SetActive(false);
            foreach (Transform child in m_Saver.transform)
            {
                Destroy(child.gameObject);
            }

            StartCoroutine(WaitSeconds(2));

        }
    }

    public void ImagesCollsInstantiation()
    {
        int l_CurrentPiece = 0;
        int k = 0;

        if (m_ImagePuzzle == null)
        {
            Random.InitState(Random.seed + Random.Range(-5, 5));
            numRandom = Random.Range(0, palabrasDisponibles.Count);

        }
        else
        {
            bool same = true;
            int count = 0;
            int rand = numRandom;
            while (same)
            {
                count++;
                Random.InitState(Random.seed + Random.Range(-5, 5));
                numRandom = Random.Range(0, palabrasDisponibles.Count);
                if (rand != numRandom)
                    same = false;
            }
        }
        CopyWords(palabrasDisponibles[numRandom], ref palabraActual);

        Random.InitState(Random.seed + Random.Range(-5, 5));
        m_NumPieces = palabraActual.piecesPuzzle[Random.Range(0, palabraActual.piecesPuzzle.Count)];
        HowManyPieces(m_NumPieces);
        RectTransform l_Colliders = m_CollidersSpawns.GetComponent<RectTransform>();
        RectTransform l_Images = m_ImagesSpawn.GetComponent<RectTransform>();
        float sizeX = -l_Colliders.sizeDelta.x / (m_NumPiecesX);
        float sizeY = l_Colliders.sizeDelta.y / m_NumPiecesY;
        float l_Width = l_Colliders.sizeDelta.x / (m_NumPiecesX);
        float l_Height = l_Colliders.sizeDelta.y / m_NumPiecesY;

        int randomImage = palabraActual.imagePuzzle - 1;
        switch (randomImage)
        {
            case 1:
                palabraActual.image1 = palabraActual.image2;
                break;
            case 2:
                palabraActual.image1 = palabraActual.image3;
                break;
        }

        foreach (Image i in marcos)
        {
            PonerColorMarco(palabraActual.color, i);
        }


        /*if (palabraActual.user)
        {
            Rect newrect = new Rect(new Vector2(palabraActual.GetTexture2D(palabraActual.image1).width / 2 - Screen.height / 2, palabraActual.GetTexture2D(palabraActual.image1).height / 2 - Screen.height / 2), new Vector2(Screen.height, Screen.height));
            m_ImagePuzzle = Sprite.Create(palabraActual.GetTexture2D(palabraActual.image1), newrect, Vector2.zero).texture;
        }
        else*/
        m_ImagePuzzle = palabraActual.GetTexture2D(palabraActual.image1); //por ahora solo imagen 1

        WordInstantiation();
        m_TextAnim.text = palabraActual.palabraActual;
        m_TextAnim.GetComponent<ConvertFont>().Convert();
        if (m_TextAnim.text.Length > 5)
            m_TextAnim.transform.localScale -= m_TextAnim.transform.localScale * 0.2f;

        Sprite l_SpriteImage;

        if (palabraActual.user)
        {
            Rect newrect = new Rect(new Vector2(palabraActual.GetTexture2D(palabraActual.image1).width / 2 - Screen.height / 2, palabraActual.GetTexture2D(palabraActual.image1).height / 2 - Screen.height / 2), new Vector2(Screen.height, Screen.height));
            l_SpriteImage = Sprite.Create(palabraActual.GetTexture2D(palabraActual.image1), newrect, Vector2.zero);
        }
        else
            l_SpriteImage = palabraActual.GetSprite(palabraActual.image1);

        m_ImageAnim.sprite = l_SpriteImage;
        m_CollidersSpawns.GetComponent<Image>().sprite = l_SpriteImage;

        Sprite[] m_PiezasPuzzle = new Sprite[m_NumPieces];
        for (int i = m_NumPiecesY - 1; i >= 0; i--)
        {
            for (int j = 0; j < m_NumPiecesX; j++)
            {
                Sprite l_Sprite;
                Rect rect;
                if (palabraActual.user)
                {
                    rect = new Rect(new Vector2(j * l_Width + (m_ImagePuzzle.width / 2 - Screen.height / 2), i * l_Height + (m_ImagePuzzle.height / 2 - Screen.height / 2)), new Vector2(l_Width, l_Height));
                }
                else
                    rect = new Rect(new Vector2(j * l_Width, i * l_Height), new Vector2(l_Width, l_Height));

                l_Sprite = Sprite.Create(m_ImagePuzzle, rect, new Vector2(0, 0));
                m_PiezasPuzzle[k] = l_Sprite;
                k++;
            }
        }

        List<int> l_Numbers = new List<int>();
        int l_Number;

        for (int i = m_NumPiecesY - 1; i >= 0; i--)
        {
            sizeX = -l_Colliders.sizeDelta.x / (m_NumPiecesX);
            sizeY -= l_Colliders.sizeDelta.y / m_NumPiecesY;

            for (int j = 0; j < m_NumPiecesX; j++)
            {
                sizeX += l_Colliders.sizeDelta.x / m_NumPiecesX;

                #region ImageInstantiation
                GameObject local = Instantiate(m_ImageTemplate, m_ImagesSpawn.transform);
                m_Images.Add(local);
                m_Images[m_Images.Count - 1].GetComponent<MoveTouch>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();
                Random.InitState(Random.seed + Random.Range(-5, 5));
                l_Number = Random.Range(0, m_NumPieces);

                while (l_Numbers.Contains(l_Number))
                {
                    l_Number = Random.Range(0, m_NumPieces);
                }

                local.GetComponent<Image>().sprite = m_PiezasPuzzle[l_Number];
                l_Numbers.Add(l_Number);
                local.name = (l_Number).ToString();

                local.GetComponent<Image>().SetNativeSize();
                local.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
                local.GetComponent<RectTransform>().anchoredPosition = new Vector2(sizeX + 10 * j, sizeY + 10 * i);
                local.GetComponent<BoxCollider2D>().offset = new Vector2(l_Width / 2, -l_Height / 2);
                local.GetComponent<BoxCollider2D>().size = new Vector2(l_Width, l_Height);
                #endregion

                #region ColliderInstantiation
                GameObject local2 = Instantiate(m_ColliderTemplate, m_CollidersSpawns.transform);
                m_Colliders.Add(local2);
                local2.name = l_CurrentPiece.ToString();
                local2.GetComponent<RectTransform>().sizeDelta = new Vector2(l_Colliders.sizeDelta.x / m_NumPiecesX, l_Colliders.sizeDelta.y / m_NumPiecesY);
                local2.GetComponent<RectTransform>().anchoredPosition = new Vector2(sizeX, sizeY);
                local2.GetComponent<BoxCollider2D>().offset = new Vector2(l_Width / 2, -l_Height / 2);
                local2.GetComponent<BoxCollider2D>().size = new Vector2(l_Width / 8, l_Height / 8);
                #endregion

                l_CurrentPiece++;
            }
        }
    }

    public void ImagesCollsInstantiationRepeat()
    {
        m_Completed = false;
        m_ImagesSpawn.SetActive(true);
        m_CollidersSpawns.SetActive(true);
        m_ImageAnim.gameObject.SetActive(false);
        RectTransform l_Colliders = m_CollidersSpawns.GetComponent<RectTransform>();
        RectTransform l_Images = m_ImagesSpawn.GetComponent<RectTransform>();
        float sizeX = -l_Colliders.sizeDelta.x / (m_NumPiecesX);
        float sizeY = l_Colliders.sizeDelta.y / m_NumPiecesY;
        float l_Width = l_Colliders.sizeDelta.x / (m_NumPiecesX);
        float l_Height = l_Colliders.sizeDelta.y / m_NumPiecesY;
        int l_CurrentPiece = 0;
        int k = 0;

        WordInstantiation();

        Sprite l_SpriteImage;
        Rect rectImage = new Rect(new Vector2(0, 0), l_Colliders.sizeDelta);

        if (palabraActual.user)
        {
            Rect newrect = new Rect(new Vector2(palabraActual.GetTexture2D(palabraActual.image1).width / 2 - Screen.height / 2, palabraActual.GetTexture2D(palabraActual.image1).height / 2 - Screen.height / 2), new Vector2(Screen.height, Screen.height));
            l_SpriteImage = Sprite.Create(palabraActual.GetTexture2D(palabraActual.image1), newrect, Vector2.zero);
        }
        else
            l_SpriteImage = palabraActual.GetSprite(palabraActual.image1); m_ImageAnim.GetComponent<Image>().sprite = palabraActual.GetSprite(palabraActual.image1);

        m_CollidersSpawns.GetComponent<Image>().sprite = l_SpriteImage;

        Sprite[] m_PiezasPuzzle = new Sprite[m_NumPieces];
        for (int i = m_NumPiecesY - 1; i >= 0; i--)
        {
            for (int j = 0; j < m_NumPiecesX; j++)
            {
                Sprite l_Sprite;
                Rect rect;
                if (palabraActual.user)
                {
                    rect = new Rect(new Vector2(j * l_Width + (m_ImagePuzzle.width / 2 - Screen.height / 2), i * l_Height + (m_ImagePuzzle.height / 2 - Screen.height / 2)), new Vector2(l_Width, l_Height));
                }
                else
                    rect = new Rect(new Vector2(j * l_Width, i * l_Height), new Vector2(l_Width, l_Height));
                l_Sprite = Sprite.Create(m_ImagePuzzle, rect, new Vector2(0, 0));
                m_PiezasPuzzle[k] = l_Sprite;
                k++;
            }
        }

        List<int> l_Numbers = new List<int>();
        int l_Number;

        for (int i = m_NumPiecesY - 1; i >= 0; i--)
        {
            sizeX = -l_Colliders.sizeDelta.x / (m_NumPiecesX);
            sizeY -= l_Colliders.sizeDelta.y / m_NumPiecesY;

            for (int j = 0; j < m_NumPiecesX; j++)
            {
                sizeX += l_Colliders.sizeDelta.x / m_NumPiecesX;

                #region ImageInstantiation
                GameObject local = Instantiate(m_ImageTemplate, m_ImagesSpawn.transform);
                m_Images.Add(local);
                m_Images[m_Images.Count - 1].GetComponent<MoveTouch>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();
                l_Number = Random.Range(0, m_NumPieces);

                while (l_Numbers.Contains(l_Number))
                {
                    l_Number = Random.Range(0, m_NumPieces);
                }

                local.GetComponent<Image>().sprite = m_PiezasPuzzle[l_Number];
                l_Numbers.Add(l_Number);
                local.name = (l_Number).ToString();

                local.GetComponent<Image>().SetNativeSize();
                local.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
                local.GetComponent<RectTransform>().anchoredPosition = new Vector2(sizeX + 10 * j, sizeY + 10 * i);
                local.GetComponent<BoxCollider2D>().offset = new Vector2(l_Width / 2, -l_Height / 2);
                local.GetComponent<BoxCollider2D>().size = new Vector2(l_Width, l_Height);
                #endregion

                #region ColliderInstantiation
                GameObject local2 = Instantiate(m_ColliderTemplate, m_CollidersSpawns.transform);
                m_Colliders.Add(local2);
                local2.name = l_CurrentPiece.ToString();
                local2.GetComponent<RectTransform>().sizeDelta = new Vector2(l_Colliders.sizeDelta.x / m_NumPiecesX, l_Colliders.sizeDelta.y / m_NumPiecesY);
                local2.GetComponent<RectTransform>().anchoredPosition = new Vector2(sizeX, sizeY);
                local2.GetComponent<BoxCollider2D>().offset = new Vector2(l_Width / 2, -l_Height / 2);
                local2.GetComponent<BoxCollider2D>().size = new Vector2(l_Width / 8, l_Height / 8);
                #endregion

                l_CurrentPiece++;
            }
        }
    }

    public void PassPuzzle()
    {
        repeating = false;
        m_ImageAnim.gameObject.SetActive(false);
        m_Completed = false;
        m_TextAnim.transform.localScale = startSizeText;

        foreach (GameObject item in m_Images)
        {
            Destroy(item);
        }

        foreach (GameObject item in m_Colliders)
        {
            Destroy(item);
        }

        foreach (GameObject item in m_Words)
        {
            Destroy(item);
        }

        m_Images.Clear();
        m_Words.Clear();
        m_Colliders.Clear();
        m_Puntuacion = 0;
        m_Canvas.SetActive(false);


        m_CurrentNumRep = 1;

        if (GameManager.m_CurrentToMinigame[2] >= 3)
        {
            GameManager.ResetPointToMinigame(2);
            m_Canvas.SetActive(false);
            m_Scener.NextGame();
        }
        else
        {
            m_ImagesSpawn.SetActive(true);
            m_CollidersSpawns.SetActive(true);

            for (int i = 0; i <= GameManager.m_CurrentToMinigame[2]; i++)
            {
                if (i > 0 && m_Points.Length > i - 1)
                    m_Points[i - 1].GetComponent<Image>().sprite = m_CompletedPoint;
            }

            HowManyPieces(PuzzlePiecesPossibilities[Random.Range(0, 2)]);
            ImagesCollsInstantiation();

        }

    }

    public void RepeatPuzzle()
    {
        repeating = true;

        foreach (GameObject item in m_Images)
        {
            Destroy(item);
        }

        foreach (GameObject item in m_Colliders)
        {
            Destroy(item);
        }

        foreach (GameObject item in m_Words)
        {
            Destroy(item);
        }

        m_Images.Clear();
        m_Words.Clear();
        m_Colliders.Clear();
        m_Puntuacion = 0;
        m_Canvas.SetActive(false);
        m_CurrentNumRep++;
        ImagesCollsInstantiationRepeat();
    }

    public void ActivateButtons()
    {
        m_ActivitiesButton.color = new Color(255, 255, 255, 1);
        m_Siguiente.SetActive(true);
        if (m_CurrentNumRep < GameManager.configurartion.repetitionsOfExercise)
            m_Repetir.SetActive(true);
    }

    public void WordInstantiation()
    {
        GameObject l_Word = Instantiate(m_Word, m_WordTransform.transform);
        GameObject l_UnseenWord = Instantiate(m_UnseenWord, m_UnseenWordTransform.transform);
        l_Word.GetComponentInChildren<Text>().text = palabraActual.palabraActual;
        l_Word.GetComponentInChildren<ConvertFont>().Convert();
        if (palabraActual.palabraActual.Length > 5)
            l_Word.GetComponentInChildren<Text>().transform.localScale -= l_Word.GetComponentInChildren<Text>().transform.localScale * 0.2f;

        // l_Word.GetComponentInChildren<Text>().fontSize = SingletonLenguage.GetInstance().ConvertSizeDependWords(l_Word.GetComponentInChildren<Text>().text);
        l_Word.name = "Word";
        l_UnseenWord.GetComponentInChildren<Text>().text = palabraActual.palabraActual;
        l_UnseenWord.GetComponentInChildren<ConvertFont>().Convert();

        if (palabraActual.palabraActual.Length > 5)
            l_UnseenWord.GetComponentInChildren<Text>().transform.localScale -= l_UnseenWord.GetComponentInChildren<Text>().transform.localScale * 0.2f;

        // l_UnseenWord.GetComponentInChildren<Text>().fontSize = SingletonLenguage.GetInstance().ConvertSizeDependWords(l_Word.GetComponentInChildren<Text>().text);
        l_UnseenWord.name = "Word";
        PonerColorMarco(palabraActual.color, l_Word.GetComponent<MoveTouch>().marco);
        PonerColorMarco(palabraActual.color, l_UnseenWord.transform.GetChild(1).GetComponent<Image>());

        m_Words.Add(l_Word);
        m_Words[m_Words.Count - 1].GetComponent<MoveTouch>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();
        m_Words[m_Words.Count - 1].GetComponent<MoveTouch>().canMove = false;
        m_Words.Add(l_UnseenWord);
    }

    IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (GameManager.configurartion.refuerzoPositivo)
        {
            if (GameObject.Find("Dumi(Clone)") == null)
            {
                GameObject pinguino = Instantiate(dumi, dumi.transform.position, dumi.transform.rotation);
                pinguino.GetComponent<Dumi>().AudioPositivo();
            }
        }
        if (!repeating)
        {
            if (GameManager.m_CurrentToMinigame[2] < 3)
            {
                GameManager.SumPointToMinigame(2);
            }
            for (int i = 0; i <= GameManager.m_CurrentToMinigame[2]; i++)
            {
                if (i > 0 && m_Points.Length > i - 1)
                    m_Points[i - 1].GetComponent<Image>().sprite = m_CompletedPoint;
            }
            if (!PaquetePuzzle.GetInstance(lvl).acabado)
            {
                if (numRandom < PaquetePuzzle.GetInstance(lvl).currentPuzzlePaquet.Count)
                {
                    PaquetePuzzle.GetInstance(lvl).currentPuzzlePaquet.Remove(palabrasDisponibles[numRandom]);
                    if (PaquetePuzzle.GetInstance(lvl).currentPuzzlePaquet.Count == 0)
                    {
                        PaquetePuzzle.GetInstance(lvl).CrearNuevoPaquete();
                    }
                    PaquetePuzzle.GetInstance(lvl).CrearBinario();
                    InitBaseOfDates();
                }
            }
        }
        acabado = true;
    }

    public void InicioPuzzle()
    {
        m_ImagesSpawn.SetActive(true);
        m_CollidersSpawns.SetActive(true);
        m_ImageAnim.gameObject.SetActive(false);
        m_Completed = false;
        m_TextAnim.transform.localScale = startSizeText;

        for (int i = 0; i <= GameManager.m_CurrentToMinigame[2]; i++)
        {
            if (i > 0 && m_Points.Length > i - 1)
                m_Points[i - 1].GetComponent<Image>().sprite = m_CompletedPoint;
        }

        foreach (GameObject item in m_Images)
        {
            Destroy(item);
        }

        foreach (GameObject item in m_Colliders)
        {
            Destroy(item);
        }

        foreach (GameObject item in m_Words)
        {
            Destroy(item);
        }

        m_Images.Clear();
        m_Words.Clear();
        m_Colliders.Clear();
        m_Puntuacion = 0;
        m_Canvas.SetActive(false);
        ImagesCollsInstantiation();
        m_CurrentNumRep = 1;
    }

    public void HowManyPieces(int l_NumPieces)
    {
        m_NumPieces = l_NumPieces;
        if (Mathf.Sqrt(l_NumPieces) / (int)Mathf.Sqrt(l_NumPieces) == 1)
        {
            m_NumPiecesX = (int)Mathf.Sqrt(l_NumPieces);
            m_NumPiecesY = (int)Mathf.Sqrt(l_NumPieces);
        }
        else
        {
            m_NumPiecesX = (int)Mathf.Sqrt(l_NumPieces);
            m_NumPiecesY = (int)Mathf.Sqrt(l_NumPieces) + 1;
        }
    }

    public void ReturnColor()
    {
        m_ActivitiesButton.color = new Color(255, 255, 255, 0.5f);
    }
    public void PonerColorMarco(string _color, Image _marco)
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString(_color, out color);
        _marco.color = color;

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
    }

}
