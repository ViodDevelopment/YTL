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
    private Vector3 startSize;
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
    private string lvl = "2";

    private void Start()
    {
        startSize = m_TextAnim.transform.localScale;
        lvl = "2";
        InitBaseOfDates();

        if (GameManager.configuration.listosPuzzleCompletado)
            m_Scener.InicioScene(true);

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
        m_NumPieces = 4;
        HowManyPieces(m_NumPieces);

        for (int i = 0; i <= GameManager.m_CurrentToMinigame[5]; i++)
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
        palabrasDisponibles.Clear();

        foreach (PalabraBD p in GameManager.palabrasDisponibles)
        {
            if (p.paquet == 5)
            {
                if (p.image1 != "")
                {
                    p.SetPalabraActual();
                    palabrasDisponibles.Add(p);

                    if (palabrasDisponibles.Count >= 19)
                        break;
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

        /*if (Input.GetKey(KeyCode.P))
            PassPuzzle();

        if (Input.GetKey(KeyCode.R))
            RepeatPuzzle();*/

    }

    public void PuzzleComplete()
    {

        if (m_Puntuacion >= m_NumPieces && m_Puntuacion <= m_NumPieces + (palabraActual.silabasActuales.Count - 1))
        {
            if (currentSilaba < m_Words.Count)
                m_Words[currentSilaba].GetComponent<MoveTouchLvl2>().canMove = true;

            foreach (GameObject go in m_Words)
            {

                go.GetComponent<MoveTouchLvl2>().mainImage.color = go.GetComponent<MoveTouchLvl2>().mainImage.color + new Color(0, 0, 0, 255);
                go.GetComponent<MoveTouchLvl2>().text.color = go.GetComponent<MoveTouchLvl2>().text.color + new Color(0, 0, 0, 255);

            }
        }
        else if (m_Puntuacion == m_NumPieces + (palabraActual.silabasActuales.Count) && !m_Completed)
        {
            m_Completed = true;

            StartCoroutine(FirstWaitSeconds(0.5f));


        }
    }

    IEnumerator FirstWaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        AudioSource l_AS = GetComponent<AudioSource>();
        if (!GameManager.configuration.palabrasConArticulo || palabraActual.actualArticulo == null)
        {
            l_AS.clip = palabraActual.GetAudioClip(palabraActual.audio);
            l_AS.Play();
        }
        else
        {

            l_AS.clip = palabraActual.GetAudioArticulo();
            l_AS.Play();
            if (!palabraActual.onlyArticulo)
                StartCoroutine(WaitForArticle());

        }

        currentSilaba = 0;
        m_ImageAnim.gameObject.SetActive(true);
        m_AnimationCenter.Play();
        m_ImagesSpawn.SetActive(false);
        m_CollidersSpawns.SetActive(false);
        foreach (Transform child in m_Saver.transform)
        {
            Destroy(child.gameObject);
        }
        if (!palabraActual.onlyArticulo)
            StartCoroutine(WaitSeconds(palabraActual.GetAudioClip(palabraActual.audio).length + 0.2f + (palabraActual.actualArticulo != null ? 1f : 0)));
        else
            StartCoroutine(WaitSeconds(0.2f + (palabraActual.actualArticulo != null ? 2f : 0)));
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



        if (palabraActual.user)
        {
            m_ImagePuzzle = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(palabraActual.GetSprite(palabraActual.image1), true).texture;
        }
        else
            m_ImagePuzzle = palabraActual.GetTexture2D(palabraActual.image1); //por ahora solo imagen 1

        WordInstantiation();
        Color color = new Color();
        ColorUtility.TryParseHtmlString(palabraActual.color, out color);
        m_TextAnim.transform.parent.GetChild(1).GetComponent<Image>().color = color;
        m_TextAnim.text = palabraActual.palabraActual;
        if (GameManager.configuration.palabrasConArticulo)
        {
            if (palabraActual != null)
            {
                m_TextAnim.text = palabraActual.actualArticulo + m_TextAnim.text;

            }
        }
        m_TextAnim.GetComponent<ConvertFont>().Convert();
        m_TextAnim.transform.localScale = startSize;
        ModifyTextPair(m_TextAnim);
        Sprite l_sprite;

        if (palabraActual.user)
        {
            l_sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(palabraActual.GetSprite(palabraActual.image1), true);
        }
        else
            l_sprite = palabraActual.GetSprite(palabraActual.image1);

        m_ImageAnim.sprite = l_sprite;
        m_ImageAnim.transform.GetChild(0).GetComponent<Image>().color = color;
        m_CollidersSpawns.GetComponent<Image>().sprite = l_sprite;

        Sprite[] m_PiezasPuzzle = new Sprite[m_NumPieces];
        bool ancho;
        float l_tamanoPiezas = SiLoTienesBienSinoPaCasa.GetSizePuzzle(l_sprite, out ancho);
        for (int i = m_NumPiecesY - 1; i >= 0; i--)
        {
            for (int j = 0; j < m_NumPiecesX; j++)
            {
                Sprite l_Sprite;
                Rect rect;
                if (palabraActual.user)
                {
                    if ((float)l_sprite.texture.width / (float)l_sprite.texture.height < 2)
                    {
                        if (ancho)
                        {
                            rect = new Rect(new Vector2(j * l_tamanoPiezas + l_sprite.texture.width / 8, i * l_tamanoPiezas), new Vector2(l_tamanoPiezas, l_tamanoPiezas));

                        }
                        else
                            rect = new Rect(new Vector2(j * l_tamanoPiezas, i * l_tamanoPiezas + l_sprite.texture.height / 8), new Vector2(l_tamanoPiezas, l_tamanoPiezas));

                    }
                    else
                    {
                        if (ancho)
                            rect = new Rect(new Vector2(j * l_tamanoPiezas + l_tamanoPiezas, i * l_tamanoPiezas), new Vector2(l_tamanoPiezas, l_tamanoPiezas));
                        else
                            rect = new Rect(new Vector2(j * l_tamanoPiezas, i * l_tamanoPiezas + l_tamanoPiezas), new Vector2(l_tamanoPiezas, l_tamanoPiezas));
                    }
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
                m_Images[m_Images.Count - 1].GetComponent<MoveTouchLvl2>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();
                l_Number = Random.Range(0, m_NumPieces);

                float multiplier = 1;
                if (palabraActual.user)
                {
                    m_Images[m_Images.Count - 1].GetComponent<MoveTouchLvl2>().user = true;
                    if (ancho)
                        multiplier = l_tamanoPiezas / l_Width;
                    else
                        multiplier = l_tamanoPiezas / l_Height;
                    m_Images[m_Images.Count - 1].GetComponent<MoveTouchLvl2>().multiplier = multiplier;

                }

                if (i == m_NumPiecesY - 1 && j == m_NumPiecesX - 1)
                {
                    m_Images[m_Images.Count - 1].GetComponent<MoveTouchLvl2>().thispiece = true;
                }

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

                if (palabraActual.user)
                {
                    local.transform.localScale /= multiplier;
                    local.GetComponent<BoxCollider2D>().size *= multiplier;
                    local.GetComponent<BoxCollider2D>().offset *= multiplier;

                }
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

                if (palabraActual.user)
                {
                    local2.transform.localScale /= multiplier;
                    local2.GetComponent<BoxCollider2D>().size *= multiplier;
                    local2.GetComponent<BoxCollider2D>().offset *= multiplier;

                }
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

        if (palabraActual.user)
        {
            m_ImagePuzzle = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(palabraActual.GetSprite(palabraActual.image1), true).texture;
        }
        else
            m_ImagePuzzle = palabraActual.GetTexture2D(palabraActual.image1); //por ahora solo imagen 1

        WordInstantiation();
        Color color = new Color();
        ColorUtility.TryParseHtmlString(palabraActual.color, out color);
        m_TextAnim.transform.parent.GetChild(1).GetComponent<Image>().color = color;
        m_TextAnim.text = palabraActual.palabraActual;
        if (GameManager.configuration.palabrasConArticulo)
        {
            if (palabraActual != null)
            {
                m_TextAnim.text = palabraActual.actualArticulo + m_TextAnim.text;

            }
        }
        m_TextAnim.GetComponent<ConvertFont>().Convert();
        m_TextAnim.transform.localScale = startSize;
        ModifyTextPair(m_TextAnim);
        Sprite l_sprite;

        if (palabraActual.user)
        {
            l_sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(palabraActual.GetSprite(palabraActual.image1), true);
        }
        else
            l_sprite = palabraActual.GetSprite(palabraActual.image1);

        m_ImageAnim.sprite = l_sprite;
        m_ImageAnim.transform.GetChild(0).GetComponent<Image>().color = color;
        m_CollidersSpawns.GetComponent<Image>().sprite = l_sprite;

        Sprite[] m_PiezasPuzzle = new Sprite[m_NumPieces];
        bool ancho;
        float l_tamanoPiezas = SiLoTienesBienSinoPaCasa.GetSizePuzzle(l_sprite, out ancho);
        for (int i = m_NumPiecesY - 1; i >= 0; i--)
        {
            for (int j = 0; j < m_NumPiecesX; j++)
            {
                Sprite l_Sprite;
                Rect rect;

                if (palabraActual.user)
                {
                    if ((float)l_sprite.texture.width / (float)l_sprite.texture.height < 2)
                    {
                        if (ancho)
                        {
                            rect = new Rect(new Vector2(j * l_tamanoPiezas + l_sprite.texture.width / 8, i * l_tamanoPiezas), new Vector2(l_tamanoPiezas, l_tamanoPiezas));

                        }
                        else
                            rect = new Rect(new Vector2(j * l_tamanoPiezas, i * l_tamanoPiezas + l_sprite.texture.height / 8), new Vector2(l_tamanoPiezas, l_tamanoPiezas));

                    }
                    else
                    {
                        if (ancho)
                            rect = new Rect(new Vector2(j * l_tamanoPiezas + l_tamanoPiezas, i * l_tamanoPiezas), new Vector2(l_tamanoPiezas, l_tamanoPiezas));
                        else
                            rect = new Rect(new Vector2(j * l_tamanoPiezas, i * l_tamanoPiezas + l_tamanoPiezas), new Vector2(l_tamanoPiezas, l_tamanoPiezas));
                    }
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
                m_Images[m_Images.Count - 1].GetComponent<MoveTouchLvl2>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();
                l_Number = Random.Range(0, m_NumPieces);

                float multiplier = 1;
                if (palabraActual.user)
                {
                    m_Images[m_Images.Count - 1].GetComponent<MoveTouchLvl2>().user = true;
                    if (ancho)
                        multiplier = l_tamanoPiezas / l_Width;
                    else
                        multiplier = l_tamanoPiezas / l_Height;
                    m_Images[m_Images.Count - 1].GetComponent<MoveTouchLvl2>().multiplier = multiplier;

                }

                if (i == m_NumPiecesY - 1 && j == m_NumPiecesX - 1)
                {
                    m_Images[m_Images.Count - 1].GetComponent<MoveTouchLvl2>().thispiece = true;
                }

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

                if (palabraActual.user)
                {
                    local.transform.localScale /= multiplier;
                    local.GetComponent<BoxCollider2D>().size *= multiplier;
                    local.GetComponent<BoxCollider2D>().offset *= multiplier;

                }
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

                if (palabraActual.user)
                {
                    local2.transform.localScale /= multiplier;
                    local2.GetComponent<BoxCollider2D>().size *= multiplier;
                    local2.GetComponent<BoxCollider2D>().offset *= multiplier;

                }
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

        if (GameManager.m_CurrentToMinigame[5] >= 3)
        {
            GameManager.ResetPointToMinigame(5);
            m_Canvas.SetActive(false);
            GameManager.configuration.listosPuzzleCompletado = true;
            ManagamentFalseBD.management.SaveConfig();

            /*
             -SaveConfig
             -LoadConfig (en Start)
             -
             -
             -*/
            m_Scener.NextGame();
        }
        else
        {
            m_ImagesSpawn.SetActive(true);
            m_CollidersSpawns.SetActive(true);

            for (int i = 0; i <= GameManager.m_CurrentToMinigame[5]; i++)
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
        if (m_CurrentNumRep < GameManager.configuration.repetitionsOfExercise)
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
            GameObject l_Word;
            if (palabraActual.silabasActuales.Count > 1)
            {
                int randomNumToPos = posiciones[Random.Range(0, posiciones.Count)];
                posiciones.Remove(randomNumToPos);
                l_Word = Instantiate(silabaPrefab, m_WordTransform[randomNumToPos]);
            }
            else
            {
                l_Word = Instantiate(silabaPrefab, m_WordTransform[1]);

            }
            l_Word.GetComponentInChildren<Text>().text = palabraActual.silabasActuales[i];
            l_Word.GetComponentInChildren<ConvertFont>().Convert();
            l_Word.name = palabraActual.silabasActuales[i] + i;
            l_Word.GetComponent<MoveTouchLvl2>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();
            l_Word.GetComponent<MoveTouchLvl2>().canMove = false;
            Color color = new Color();
            ColorUtility.TryParseHtmlString(palabraActual.color, out color);
            l_Word.GetComponent<MoveTouchLvl2>().mainImage.color = color;
            ConvertMarco(l_Word.GetComponent<MoveTouchLvl2>().mainImage, l_Word.transform.GetChild(0).GetComponent<Image>(), palabraActual.silabasActuales[i]);
            if (i == 0 && palabraActual.silabasActuales.Count > 1)
                l_Word.GetComponent<MoveTouchLvl2>().silaba = -1;
            else if (i == palabraActual.silabasActuales.Count - 1 && palabraActual.silabasActuales.Count > 1)
                l_Word.GetComponent<MoveTouchLvl2>().silaba = 1;
            else if (palabraActual.silabasActuales.Count > 1)
                l_Word.GetComponent<MoveTouchLvl2>().silaba = 0;
            else
                l_Word.GetComponent<MoveTouchLvl2>().silaba = 2;

            m_Words.Add(l_Word);

            switch (SingletonLenguage.GetInstance().GetFont())
            {
                case SingletonLenguage.OurFont.MAYUSCULA:
                    l_Word.GetComponentInChildren<Text>().fontSize -= 50;
                    break;

            }

            Vector3 position = SearchPosition(m_UnseenWordTransform, i);
            GameObject l_UnseenWord = Instantiate(m_UnseenWord, m_UnseenWordTransform.transform);
            ColorUtility.TryParseHtmlString(palabraActual.color, out color);
            l_UnseenWord.GetComponent<SilabaUnseedColocarMarco>().imagen.color = color;
            l_UnseenWord.transform.position += position;

            l_UnseenWord.GetComponentInChildren<Text>().text = palabraActual.silabasActuales[i];
            if (i == 0 && palabraActual.silabasActuales.Count > 1)
                l_UnseenWord.GetComponentInChildren<Text>().transform.position += new Vector3(0.17f, 0, 0);
            else if (i == palabraActual.silabasActuales.Count - 1 && palabraActual.silabasActuales.Count > 1)
                l_UnseenWord.GetComponentInChildren<Text>().transform.position -= new Vector3(0.17f, 0, 0);
            ConvertMarco(l_UnseenWord.GetComponent<SilabaUnseedColocarMarco>().imagen, l_UnseenWord.transform.GetChild(0).GetComponent<Image>(), palabraActual.silabasActuales[i]);

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
            l_UnseenWord.name = palabraActual.silabasActuales[i] + i;
            if (i != palabraActual.silabasActuales.Count - 1)
                l_UnseenWord.transform.position += new Vector3(-0.04f, 0);
            if (palabraActual.silabasActuales[i].Length > 3)
            {
                l_UnseenWord.transform.position += new Vector3(-0.25f, 0);

            }
            switch (SingletonLenguage.GetInstance().GetFont())
            {
                case SingletonLenguage.OurFont.MAYUSCULA:
                    l_UnseenWord.GetComponentInChildren<Text>().fontSize -= 50;
                    break;

            }
            //m_Words.Add(l_UnseenWord);
        }

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
            if (GameManager.m_CurrentToMinigame[5] < 3)
            {
                GameManager.SumPointToMinigame(5);
            }
            for (int i = 0; i <= GameManager.m_CurrentToMinigame[5]; i++)
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
        float distanceMax = (palabraActual.silabasActuales.Count - 1) * distancePerSilaba;
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
        for (int i = 0; i <= GameManager.m_CurrentToMinigame[5]; i++)
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
        l_NumPieces = 4;
        m_NumPieces = 4;
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
        palabra.color = toCopy.color;
        palabra.user = toCopy.user;
        palabra.nameSpanish = toCopy.nameSpanish;
        palabra.nameCatalan = toCopy.nameCatalan;
        palabra.articulos.Clear();
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


    private void ModifyTextPair(Text _text)
    {
        Text texto = _text;
        if (texto.text.Length < 9 && texto.text.Length > 7)
            texto.gameObject.transform.localScale = texto.gameObject.transform.localScale * 0.73f;
        else if (texto.text.Length < 12 && texto.text.Length > 7)
        {
            texto.gameObject.transform.localScale = texto.gameObject.transform.localScale * 0.68f;
        }
        else if (texto.text.Length > 7)
        {
            texto.gameObject.transform.localScale = texto.gameObject.transform.localScale * 0.57f;

        }

    }

    private void ConvertMarco(Image _imagen, Image _fondo, string _silaba)
    {
        switch (_silaba.Length)
        {
            case 1:
                _imagen.rectTransform.localScale += new Vector3((_imagen.rectTransform.localScale.x / 5f - 0.07f), 0, 0);
                _fondo.rectTransform.localScale += new Vector3((_fondo.rectTransform.localScale.x / 5f - 0.07f), 0, 0);
                break;
            case 3:
                _imagen.rectTransform.localScale += new Vector3((_imagen.rectTransform.localScale.x / 4.5f - 0.06f), 0, 0);
                _fondo.rectTransform.localScale += new Vector3((_fondo.rectTransform.localScale.x / 4.5f - 0.06f), 0, 0);
                break;
            case 4:
                _imagen.rectTransform.localScale += new Vector3((_imagen.rectTransform.localScale.x / 4f - 0.08f), 0, 0);
                _fondo.rectTransform.localScale += new Vector3((_fondo.rectTransform.localScale.x / 4f - 0.08f), 0, 0);
                break;
            case 5:
                _imagen.rectTransform.localScale += new Vector3((_imagen.rectTransform.localScale.x / 3f - 0.04f), 0, 0);
                _fondo.rectTransform.localScale += new Vector3((_fondo.rectTransform.localScale.x / 3f - 0.04f), 0, 0);
                break;

        }

    }

    IEnumerator WaitForArticle()
    {
        AudioSource l_AS = GetComponent<AudioSource>();
        yield return new WaitForSeconds(l_AS.clip.length);
        l_AS.clip = palabraActual.GetAudioClip(palabraActual.audio);
        l_AS.Play();
    }
}
