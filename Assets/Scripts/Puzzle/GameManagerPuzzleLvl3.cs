using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManagerPuzzleLvl3 : MonoBehaviour
{
    public GameObject dumi;
    public Animation m_AnimationCenter;
    public Image m_ImageAnim;
    public Text m_TextAnim;
    private List<FraseBD> frasesDisponibles = new List<FraseBD>();

    List<GameObject> m_Words = new List<GameObject>();
    public int currentPalabra = 0;
    public SceneManagement m_Scener;
    int m_CurrentNumRep = 1;
    public GameObject m_ImageTemplate;
    public GameObject m_ColliderTemplate;
    public GameObject m_CollidersSpawns;
    public GameObject m_ImagesSpawn;
    public GameObject m_Canvas;

    private FraseBD fraseActual = new FraseBD();
    Texture2D m_ImagePuzzle;
    public GameObject palabraPrefab;
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

    [HideInInspector]
    public List<GameObject> rectanglesInScene = new List<GameObject>();

    private bool acabado = false;
    public GameObject m_Saver;
    public Sprite marcoMasDe5;

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

        for (int i = 0; i <= GameManager.m_CurrentToMinigame[8]; i++)
        {
            if (i > 0 && m_Points.Length > i - 1)
                m_Points[i - 1].GetComponent<Image>().sprite = m_CompletedPoint;
        }
        repeating = false;
        m_Completed = false;
        currentPalabra = 0;
        InicioPuzzle();
    }

    private void InitBaseOfDates()
    {
        foreach (FraseBD f in GameManager.frasesDisponibles)
        {
            if(f.paquet == GameManager.configurartion.paquete)
            {
                if(f.image != "")
                    frasesDisponibles.Add(f);
            }else if(GameManager.configurartion.paquete == -1)
            {
                if (f.image != "")
                    frasesDisponibles.Add(f);
            }
        }
    }

    private void Update()
    {
        if (!m_Completed)
        {
            PuzzleComplete();
        }

        /*if (m_Canvas.activeSelf && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
            PassPuzzle();*/

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

    }

    public void PuzzleComplete()
    {

        if (m_Puntuacion >= m_NumPieces && m_Puntuacion <= m_NumPieces + (frasesDisponibles[numRandom].palabras.Count - 1))
        {
            if (!m_Words[0].active)
            {
                foreach (GameObject go in m_Words)
                {
                    go.SetActive(true);
                }
            }

            if (currentPalabra < m_Words.Count)
                m_Words[currentPalabra].GetComponent<MoveTouchLvl3>().canMove = true;
        }
        else if (m_Puntuacion == m_NumPieces + (frasesDisponibles[numRandom].palabras.Count) && !m_Completed)
        {
            AudioSource l_AS = GetComponent<AudioSource>();
            l_AS.clip = frasesDisponibles[numRandom].GetAudioClip(frasesDisponibles[numRandom].sound);
            l_AS.Play();

            m_Completed = true;
            currentPalabra = 0;
            m_ImageAnim.gameObject.SetActive(true);
            m_AnimationCenter.Play();
            m_ImagesSpawn.SetActive(false);
            m_CollidersSpawns.SetActive(false);

            for(int i = 0; i < m_NumPieces; i++)
            {
               DestroyImmediate(m_Saver.transform.GetChild(0).gameObject);
            }

            StartCoroutine(WaitSeconds(l_AS.clip.length + m_AnimationCenter.clip.length * 0.5f));

        }
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
            numRandom = Random.Range(0, frasesDisponibles.Count);

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
                numRandom = Random.Range(0, frasesDisponibles.Count);
                if (rand != numRandom)
                    same = false;
            }
        }
        CopyFrase(frasesDisponibles[numRandom], fraseActual);


        m_ImagePuzzle = fraseActual.GetTexture2D(fraseActual.image); //por ahora solo imagen 1
        WordInstantiation();
        m_TextAnim.text = fraseActual.palabras[0].palabraActual; //HACERLO CON TODAS LAS PALABRAS
        m_TextAnim.GetComponent<ConvertFont>().Convert();

        Sprite l_SpriteImage;
        l_SpriteImage = fraseActual.GetSprite(fraseActual.image);
        m_ImageAnim.sprite = fraseActual.GetSprite(fraseActual.image);
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
                m_PiezasPuzzle[k].name = "imagen";

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
                m_Images[m_Images.Count - 1].GetComponent<MoveTouchLvl3>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();
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

        CopyFrase(frasesDisponibles[numRandom], fraseActual);


        m_ImagePuzzle = fraseActual.GetTexture2D(fraseActual.image); //por ahora solo imagen 1
        WordInstantiation();
        m_TextAnim.text = fraseActual.palabras[0].palabraActual; //HACERLO CON TODAS LAS PALABRAS
        m_TextAnim.GetComponent<ConvertFont>().Convert();

        Sprite l_SpriteImage;
        l_SpriteImage = fraseActual.GetSprite(fraseActual.image);
        m_ImageAnim.sprite = fraseActual.GetSprite(fraseActual.image);
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
                m_Images[m_Images.Count - 1].GetComponent<MoveTouchLvl3>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();
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

        foreach(GameObject item in rectanglesInScene)
        {
            Destroy(item);
        }

        m_Images.Clear();
        m_Words.Clear();
        m_Colliders.Clear();
        rectanglesInScene.Clear();
        currentPalabra = 0;
        m_Puntuacion = 0;
        m_Canvas.SetActive(false);


        m_CurrentNumRep = 1;

        if (GameManager.m_CurrentToMinigame[8] >= 3)
        {
            GameManager.ResetPointToMinigame(8);
            m_Canvas.SetActive(false);
            m_Scener.NextGame();
        }
        else
        {
            m_ImagesSpawn.SetActive(true);
            m_CollidersSpawns.SetActive(true);

            for (int i = 0; i <= GameManager.m_CurrentToMinigame[8]; i++)
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

        foreach(GameObject item in rectanglesInScene)
        {
            Destroy(item);
        }

        m_Images.Clear();
        m_Words.Clear();
        m_Colliders.Clear();
        rectanglesInScene.Clear();
        currentPalabra = 0;
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
        InstanciacionDeRectangulos();

        List<int> posiciones = new List<int>();
        for (int i = 0; i < m_WordTransform.Count; i++)
        {
            posiciones.Add(i);
        }
        for (int i = 0; i < fraseActual.palabras.Count; i++)
        {
            int randomNumToPos = posiciones[Random.Range(0, posiciones.Count - 1)];
            posiciones.Remove(randomNumToPos);
            GameObject l_Word = Instantiate(palabraPrefab, m_WordTransform[randomNumToPos]);
            l_Word.GetComponentInChildren<Text>().text = frasesDisponibles[numRandom].palabras[i].palabraActual;
            l_Word.GetComponentInChildren<ConvertFont>().Convert();
            if(i == 0 && SingletonLenguage.GetInstance().GetFont() != SingletonLenguage.OurFont.MAYUSCULA)
            {
                string mayus = frasesDisponibles[numRandom].palabras[i].palabraActual.ToUpper();
                string tex = "";

                for (int j = 0; j < frasesDisponibles[numRandom].palabras[i].palabraActual.Length; j++)
                {
                    if (j == 0)
                        tex += mayus[j];
                    else
                        tex += frasesDisponibles[numRandom].palabras[i].palabraActual[j];
                }
                l_Word.GetComponentInChildren<Text>().text = tex;
            }
            
            l_Word.name = fraseActual.palabras[i].palabraActual;
            l_Word.GetComponent<MoveTouchLvl3>().managerOnlyOne = gameObject.GetComponent<OnlyOneManager>();
            l_Word.GetComponent<MoveTouchLvl3>().canMove = false;
            l_Word.SetActive(false);
            
            foreach(GameObject go in rectanglesInScene)
            {
                if(go.name == l_Word.name)
                {
                    l_Word.gameObject.transform.localScale = go.transform.localScale;
                    l_Word.gameObject.transform.GetChild(0).GetComponent<Image>().gameObject.transform.localScale = go.GetComponentInChildren<Image>().gameObject.transform.localScale - new Vector3(0.02f,0,0);
                    l_Word.gameObject.transform.GetChild(1).GetComponent<Image>().gameObject.transform.localScale = go.GetComponentInChildren<Image>().gameObject.transform.localScale;
                    if(l_Word.name.Length >= 5)
                        l_Word.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = marcoMasDe5;
                    CambiarRecuadroDependiendoDePalabra(l_Word.gameObject.transform.GetChild(1).GetComponent<Image>(), fraseActual.palabras[i].color);
                }
            }
            m_Words.Add(l_Word);
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
            if (GameManager.m_CurrentToMinigame[8] < 3)
            {
                GameManager.SumPointToMinigame(8);
            }
            for (int i = 0; i <= GameManager.m_CurrentToMinigame[8]; i++)
            {
                if (i > 0 && m_Points.Length > i - 1)
                    m_Points[i - 1].GetComponent<Image>().sprite = m_CompletedPoint;
            }
        }
        acabado = true;
    }

    private Vector3 SearchPosition(Transform _trans, float _position)
    {
        _position += 1;
        Vector3 pos = Vector3.zero;
        float multiplier = (fraseActual.palabras.Count) / 2 * 1.5f;
        if (frasesDisponibles[numRandom].palabras.Count > 1)
            pos = new Vector3((_position / (frasesDisponibles[numRandom].palabras.Count) * multiplier * 2) - multiplier, 0, 0);

        return pos;
    }

    public void InicioPuzzle()
    {
        m_ImagesSpawn.SetActive(true);
        m_CollidersSpawns.SetActive(true);
        m_ImageAnim.gameObject.SetActive(false);
        m_Completed = false;
        for (int i = 0; i <= GameManager.m_CurrentToMinigame[8]; i++)
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

    private void InstanciacionDeRectangulos()
    {
        Vector3 position = m_UnseenWordTransform.position;
        float anchototal = 0;
        float scale = 0;
        float distance = 5.7f;
        int count = 0;

        foreach (PalabraBD p in fraseActual.palabras)
        {
            if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MAYUSCULA)
            {
                scale = p.palabraActual.Length * 0.095f;
                anchototal += scale * distance + 1.25f;
                if (fraseActual.palabras[0] != p)
                {
                    anchototal += fraseActual.palabras[count - 1].palabraActual.Length * 0.095f * distance / 2;
                }
            }
            else if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
            {
                scale = p.palabraActual.Length * 0.08f;
                anchototal += scale * distance + 1.25f;
                if (fraseActual.palabras[0] != p)
                {
                    anchototal += fraseActual.palabras[count - 1].palabraActual.Length * 0.08f * distance / 2;
                }
            }
            else
            {
                scale = p.palabraActual.Length * 0.075f;
                anchototal += scale * distance + 1f;
                if (fraseActual.palabras[0] != p)
                {
                    anchototal += fraseActual.palabras[count - 1].palabraActual.Length * 0.075f * distance / 2;
                }
            }

            count++;
        }
        anchototal /= 1.5f;
        position = new Vector3(position.x - anchototal / 2, position.y, position.z);
        Text texto;
        Image imagen;
        int coun = 0;
        foreach (PalabraBD p in fraseActual.palabras)
        {
            if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MAYUSCULA)
            {
                scale = p.palabraActual.Length * 0.08f;
                if (rectanglesInScene.Count > 0)
                {
                    position = new Vector3(position.x + 1 + scale * distance / 2 + fraseActual.palabras[rectanglesInScene.Count - 1].palabraActual.Length * 0.095f * distance / 2, position.y, position.z);
                }
                else
                    position = new Vector3(position.x + scale * distance / 2, position.y, position.z);
            }
            else if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
            {
                scale = p.palabraActual.Length * 0.08f;
                if (rectanglesInScene.Count > 0)
                {
                    position = new Vector3(position.x + 1f + scale * distance / 2 + fraseActual.palabras[rectanglesInScene.Count - 1].palabraActual.Length * 0.08f * distance / 2, position.y, position.z);
                }
                else
                    position = new Vector3(position.x + scale * distance / 2, position.y, position.z);
            }
            else
            {
                scale = p.palabraActual.Length * 0.08f;
                if (rectanglesInScene.Count > 0)
                {
                    position = new Vector3(position.x + 1f + scale * distance / 2 + fraseActual.palabras[rectanglesInScene.Count - 1].palabraActual.Length * 0.075f * distance / 2, position.y, position.z);
                }
                else
                    position = new Vector3(position.x + scale * distance / 2, position.y, position.z);
            }
            rectanglesInScene.Add(Instantiate(m_UnseenWord, position, m_UnseenWord.transform.rotation));

            rectanglesInScene[rectanglesInScene.Count - 1].transform.parent = m_UnseenWordTransform.transform;
            rectanglesInScene[rectanglesInScene.Count - 1].transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            rectanglesInScene[rectanglesInScene.Count - 1].name = p.palabraActual;
            texto = rectanglesInScene[rectanglesInScene.Count - 1].GetComponentInChildren<Text>();
            imagen = rectanglesInScene[rectanglesInScene.Count - 1].transform.GetChild(1).GetComponent<Image>();
            Image fondo = rectanglesInScene[rectanglesInScene.Count - 1].transform.GetChild(0).GetComponent<Image>();
            texto.text = p.palabraActual;
            rectanglesInScene[rectanglesInScene.Count - 1].GetComponentInChildren<ConvertFont>().Convert();
            if(coun == 0 && SingletonLenguage.GetInstance().GetFont() != SingletonLenguage.OurFont.MAYUSCULA)
            {
                string mayus = p.palabraActual.ToUpper();
                string tex = "";

                for (int i = 0; i < p.palabraActual.Length; i++)
                {
                    if (i == 0)
                        tex += mayus[i];
                    else
                        tex += p.palabraActual[i];
                }
                texto.text = tex;
            }
            imagen.gameObject.transform.localScale = new Vector3(0.25f, imagen.gameObject.transform.localScale.y, imagen.gameObject.transform.localScale.z);
            fondo.gameObject.transform.localScale = new Vector3(0.23f, fondo.gameObject.transform.localScale.y, fondo.gameObject.transform.localScale.z);
            if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MAYUSCULA)
            {
                imagen.gameObject.transform.localScale += new Vector3(p.palabraActual.Length * 0.065f, 0, 0);
                fondo.gameObject.transform.localScale += new Vector3(p.palabraActual.Length * 0.065f, 0, 0);
            }
            else if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
            {
                imagen.gameObject.transform.localScale += new Vector3(p.palabraActual.Length * 0.055f, 0, 0);
                fondo.gameObject.transform.localScale += new Vector3(p.palabraActual.Length * 0.055f, 0, 0);
            }
            else
            {
                imagen.gameObject.transform.localScale += new Vector3(p.palabraActual.Length * 0.055f, 0, 0);
                fondo.gameObject.transform.localScale += new Vector3(p.palabraActual.Length * 0.055f, 0, 0);
            }

            if (p.palabraActual.Length >= 5)
                imagen.sprite = marcoMasDe5;

            rectanglesInScene[rectanglesInScene.Count - 1].transform.localScale += Vector3.one * 70;
            CambiarRecuadroDependiendoDePalabra(imagen, p.color);

            //rectanglesInScene[rectanglesInScene.Count - 1].SetActive(false);
            texto = null;
            imagen = null;
            scale = 0;
            coun++;
        }

    }

    private void CambiarRecuadroDependiendoDePalabra(Image _imagen, string _color)
    {
            Color color = new Color();
            ColorUtility.TryParseHtmlString(_color, out color);
            _imagen.color = color;
    }

    private void CopyFrase(FraseBD _toCopy, FraseBD _frase)
    {
        _frase.actualFrase = _toCopy.actualFrase;
        _frase.palabras = _toCopy.palabras;
        _frase.image = _toCopy.image;
        _frase.image2 = _toCopy.image2;
        _frase.sound = _toCopy.sound;
        _frase.actualDificultad = _toCopy.actualDificultad;
    }
}
