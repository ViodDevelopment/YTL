using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManagerPuzzleLvl2 : MonoBehaviour
{
    public GameObject dumi;
    public Animation m_AnimationCenter;
    public Image m_ImageAnim;
    public Text m_TextAnim;
    private List<PalabraBD> palabrasDisponibles = new List<PalabraBD>();

    List<GameObject> m_Words = new List<GameObject>();
    public int currentSilaba = 0;
    public SceneManagement m_Scener;
    int m_CurrentNumRep = 1;
    public GameObject m_ImageTemplate;
    public GameObject m_ColliderTemplate;
    public GameObject m_CollidersSpawns;
    public GameObject m_ImagesSpawn;
    public GameObject m_Canvas;

    private PalabraBD palabraActual = new PalabraBD();
    Texture2D m_ImagePuzzle;
    public GameObject silabaPrefab;
    public List<Transform> m_WordTransform = new List<Transform>();
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

    public int[] PuzzlePiecesPossibilities;

    private bool acabado = false;
    public GameObject m_Saver;

    private void Start()
    {
        InitBaseOfDates();

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
        HowManyPieces(m_NumPieces);

        for (int i = 0; i <= GameManager.m_CurrentToMinigame[2]; i++)
        {
            if (i > 0 && m_Points.Length > i - 1)
                m_Points[i - 1].GetComponent<Image>().sprite = m_CompletedPoint;
        }
        repeating = false;
        m_Completed = false;
        currentSilaba = 0;
        InicioPuzzle();
    }

    private void InitBaseOfDates()
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

        if (m_Puntuacion >= m_NumPieces && m_Puntuacion <= m_NumPieces + (palabrasDisponibles[numRandom].silabasActuales.Count - 1))
        {
            if (currentSilaba < m_Words.Count)
                m_Words[currentSilaba].GetComponent<MoveTouchLvl2>().canMove = true;

            foreach (GameObject go in m_Words)
            {

                go.GetComponent<MoveTouchLvl2>().mainImage.color = go.GetComponent<MoveTouchLvl2>().mainImage.color + new Color(0, 0, 0, 255);
                go.GetComponent<MoveTouchLvl2>().text.color = go.GetComponent<MoveTouchLvl2>().text.color + new Color(0, 0, 0, 255);

            }
        }
        else if (m_Puntuacion == m_NumPieces + (palabrasDisponibles[numRandom].silabasActuales.Count) && !m_Completed)
        {
            m_Completed = true;

            StartCoroutine(FirstWaitSeconds(0.5f));


        }
    }

    IEnumerator FirstWaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        AudioSource l_AS = GetComponent<AudioSource>();
        l_AS.clip = palabrasDisponibles[numRandom].GetAudioClip(palabrasDisponibles[numRandom].audio);
        l_AS.Play();

        currentSilaba = 0;
        m_ImageAnim.gameObject.SetActive(true);
        m_AnimationCenter.Play();
        m_ImagesSpawn.SetActive(false);
        m_CollidersSpawns.SetActive(false);
        foreach (Transform child in m_Saver.transform)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(WaitSeconds(l_AS.clip.length + m_AnimationCenter.clip.length));

    }

    public void ImagesCollsInstantiation()
    {
        RectTransform l_Colliders = m_CollidersSpawns.GetComponent<RectTransform>();
        RectTransform l_Images = m_ImagesSpawn.GetComponent<RectTransform>();
        float sizeX = -l_Colliders.sizeDelta.x / (m_NumPiecesX);
        float sizeY = l_Colliders.sizeDelta.y / m_NumPiecesY;
        float l_Width = l_Colliders.sizeDelta.x / (m_NumPiecesX);
        float l_Height = l_Colliders.sizeDelta.y / m_NumPiecesY;
        int l_CurrentPiece = 0;
        int k = 0;

        if (m_ImagePuzzle == null)
        {
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
                Random.InitState(count * System.DateTime.Now.Second);
                numRandom = Random.Range(0, palabrasDisponibles.Count);
                if (rand != numRandom)
                    same = false;
            }
        }
        //palabraActual = palabrasDisponibles[numRandom];
        CopyWords(palabrasDisponibles[numRandom], ref palabraActual);
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



        m_ImagePuzzle = palabraActual.GetTexture2D(palabraActual.image1); //por ahora solo imagen 1
        WordInstantiation();
        Color color = new Color();
        ColorUtility.TryParseHtmlString(palabraActual.color, out color);
        m_TextAnim.transform.parent.GetChild(1).GetComponent<Image>().color = color;
        m_TextAnim.text = palabraActual.palabraActual;
        m_TextAnim.GetComponent<ConvertFont>().Convert();

        Sprite l_SpriteImage;
        l_SpriteImage = palabraActual.GetSprite(palabraActual.image1);
        m_ImageAnim.sprite = palabraActual.GetSprite(palabraActual.image1);
        m_ImageAnim.transform.GetChild(0).GetComponent<Image>().color = color;
        m_CollidersSpawns.GetComponent<Image>().sprite = l_SpriteImage;

        Sprite[] m_PiezasPuzzle = new Sprite[m_NumPieces];
        for (int i = m_NumPiecesY - 1; i >= 0; i--)
        {
            for (int j = 0; j < m_NumPiecesX; j++)
            {
                Sprite l_Sprite;
                Rect rect = new Rect(new Vector2(j * l_Width, i * l_Height), new Vector2(l_Width, l_Height));
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
                m_Images[m_Images.Count - 1].GetComponent<MoveTouchLvl2>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();
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
                local2.transform.parent.GetChild(0).GetComponent<Image>().color = color;
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
        l_SpriteImage = palabraActual.GetSprite(palabraActual.image1);
        m_ImageAnim.GetComponent<Image>().sprite = palabraActual.GetSprite(palabraActual.image1);
        m_CollidersSpawns.GetComponent<Image>().sprite = l_SpriteImage;

        Sprite[] m_PiezasPuzzle = new Sprite[m_NumPieces];
        for (int i = m_NumPiecesY - 1; i >= 0; i--)
        {
            for (int j = 0; j < m_NumPiecesX; j++)
            {
                Sprite l_Sprite;
                Rect rect = new Rect(new Vector2(j * l_Width, i * l_Height), new Vector2(l_Width, l_Height));
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
                m_Images[m_Images.Count - 1].GetComponent<MoveTouchLvl2>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();
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
        currentSilaba = 0;
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
        currentSilaba = 0;
        m_Puntuacion = 0;
        m_Canvas.SetActive(false);
        m_CurrentNumRep++;
        ImagesCollsInstantiationRepeat();
    }

    public void ActivateButtons()
    {
        m_Siguiente.SetActive(true);
        if (m_CurrentNumRep < GameManager.configurartion.repetitionsOfExercise)
            m_Repetir.SetActive(true);
    }

    public void WordInstantiation()
    {
        List<int> posiciones = new List<int>();
        for (int i = 0; i < palabraActual.silabasActuales.Count; i++)
        {
            posiciones.Add(i);
        }
        for (int i = 0; i < palabraActual.silabasActuales.Count; i++)
        {
            int randomNumToPos = posiciones[Random.Range(0, posiciones.Count)];
            posiciones.Remove(randomNumToPos);
            GameObject l_Word = Instantiate(silabaPrefab, m_WordTransform[randomNumToPos]);
            l_Word.GetComponentInChildren<Text>().text = palabraActual.silabasActuales[i];
            l_Word.GetComponentInChildren<ConvertFont>().Convert();
            l_Word.name = palabraActual.silabasActuales[i];
            l_Word.GetComponent<MoveTouchLvl2>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();
            l_Word.GetComponent<MoveTouchLvl2>().canMove = false;
            Color color = new Color();
            ColorUtility.TryParseHtmlString(palabrasDisponibles[numRandom].color, out color);
            l_Word.GetComponent<MoveTouchLvl2>().mainImage.color = color;
            ConvertMarco(l_Word.GetComponent<MoveTouchLvl2>().mainImage, palabraActual.silabasActuales[i]);
            if (i == 0 && palabraActual.silabasActuales.Count > 1)
                l_Word.GetComponent<MoveTouchLvl2>().silaba = -1;
            else if (i == palabraActual.silabasActuales.Count - 1 && palabraActual.silabasActuales.Count > 1)
                l_Word.GetComponent<MoveTouchLvl2>().silaba = 1;
            else if (palabraActual.silabasActuales.Count > 1)
                l_Word.GetComponent<MoveTouchLvl2>().silaba = 0;
            else
                l_Word.GetComponent<MoveTouchLvl2>().silaba = 2;

            m_Words.Add(l_Word);

            Vector3 position = SearchPosition(m_UnseenWordTransform, i);
            GameObject l_UnseenWord = Instantiate(m_UnseenWord, m_UnseenWordTransform.transform);
            ColorUtility.TryParseHtmlString(palabrasDisponibles[numRandom].color, out color);
            l_UnseenWord.GetComponentInChildren<Image>().color = color;
            l_UnseenWord.transform.position += position;
            l_UnseenWord.GetComponentInChildren<Text>().text = palabrasDisponibles[numRandom].silabasActuales[i];
            if (i == 0 && palabrasDisponibles[numRandom].silabasActuales.Count > 1)
                l_UnseenWord.GetComponentInChildren<Text>().transform.position += new Vector3(0.17f, 0, 0);
            else if (i == palabrasDisponibles[numRandom].silabasActuales.Count - 1 && palabrasDisponibles[numRandom].silabasActuales.Count > 1)
                l_UnseenWord.GetComponentInChildren<Text>().transform.position -= new Vector3(0.17f, 0, 0);
            ConvertMarco(l_UnseenWord.GetComponentInChildren<Image>(), palabraActual.silabasActuales[i]);

            if (i == 0 && palabraActual.silabasActuales.Count > 1)
                l_UnseenWord.GetComponent<SilabaUnseedColocarMarco>().SetMarco(-1);
            else if (i == palabraActual.silabasActuales.Count - 1 && palabraActual.silabasActuales.Count > 1)
                l_UnseenWord.GetComponent<SilabaUnseedColocarMarco>().SetMarco(1);
            else if (palabraActual.silabasActuales.Count > 1)
                l_UnseenWord.GetComponent<SilabaUnseedColocarMarco>().SetMarco(0);
            else
                l_UnseenWord.GetComponent<SilabaUnseedColocarMarco>().SetMarco(2);


            /*else
                l_Word.GetComponent<MoveTouchLvl2>().silaba = 0;*/

            l_UnseenWord.GetComponentInChildren<ConvertFont>().Convert();
            l_UnseenWord.name = palabrasDisponibles[numRandom].silabasActuales[i];
            //m_Words.Add(l_UnseenWord);
        }

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
        }
        acabado = true;
    }

    private Vector3 SearchPosition(Transform _trans, float _position)
    {
        Vector3 pos = Vector3.zero;
        float distancePerSilaba = 1.75f;
        float distanceMax = (palabrasDisponibles[numRandom].silabasActuales.Count - 1) * distancePerSilaba;
        if (palabraActual.silabasActuales.Count > 1)
        {
            float distanceInit = -distanceMax / 2;
            float distanceForMySilaba = _position * distancePerSilaba + distanceInit;
            pos = new Vector3(distanceForMySilaba, 0, 0);
        }

        return pos;
    }

    public void InicioPuzzle()
    {
        m_ImagesSpawn.SetActive(true);
        m_CollidersSpawns.SetActive(true);
        m_ImageAnim.gameObject.SetActive(false);
        m_Completed = false;
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

    private void CopyWords(PalabraBD toCopy, ref PalabraBD palabra)
    {
        palabra.image1 = toCopy.image1;
        palabra.image2 = toCopy.image2;
        palabra.image3 = toCopy.image3;
        palabra.audio = toCopy.audio;
        palabra.imagePuzzle = toCopy.imagePuzzle;
        palabra.piecesPuzzle = toCopy.piecesPuzzle;
        palabra.palabraActual = toCopy.palabraActual;
        palabra.silabasActuales = toCopy.silabasActuales;
        palabra.color = toCopy.color;
    }

    private void ConvertMarco(Image _imagen, string _silaba)
    {
        switch (_silaba.Length)
        {
            case 1:
                _imagen.rectTransform.localScale -= new Vector3(_imagen.rectTransform.localScale.x / 5, 0, 0);
                break;
            case 3:
                _imagen.rectTransform.localScale += new Vector3(_imagen.rectTransform.localScale.x / 5, 0, 0);
                break;
            case 4:
                _imagen.rectTransform.localScale += new Vector3(_imagen.rectTransform.localScale.x / 4, 0, 0);
                break;
            case 5:
                _imagen.rectTransform.localScale += new Vector3(_imagen.rectTransform.localScale.x / 3, 0, 0);
                break;

        }

    }
}
