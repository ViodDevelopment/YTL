using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BitLvl2 : MonoBehaviour
{
    public GameObject dumi;
    bool m_0touch = true;
    bool m_1touch = false;
    Animation m_Animation;
    GameManagerBitReadyLvl2 m_GMBit;
    public AnimationClip m_Spin;
    public AnimationClip m_Slide;
    private List<FraseBD> frasesDisponibles = new List<FraseBD>();
    private int firstImage = 0;

    public static int m_Length;
    public Image m_Image;
    public Image otherImage;
    public GameObject rectanglePrefab;

    [HideInInspector]
    public List<GameObject> rectanglesInScene = new List<GameObject>();

    public List<Sprite> listOfRectangles = new List<Sprite>(); //Ordenarlos para saber el orden para ponerlos abajo
    public List<Font> ourFonts = new List<Font>();
    public AudioSource m_AS;
    public int l_Number;
    public int currentWord = 0;
    private int levelBit = 2;
    public Sprite marcoMasDe5;

    void Awake()
    {
        m_GMBit = GameObject.FindGameObjectWithTag("Bit").GetComponent<GameManagerBitReadyLvl2>();
        levelBit = m_GMBit.levelBit;
        RecolectFrasesBD();
        m_Length = frasesDisponibles.Count;
        if (m_Length == 0)
            m_Length = 1;
        GameManagerBitReadyLvl2.m_Alea = Random.Range(0, m_Length);
    }

    private void RecolectFrasesBD()
    {
        frasesDisponibles.Clear();
        foreach (FraseBD f in GameManager.frasesDisponibles)
        {
            if (levelBit == 2)
            {
                if (f.actualDificultad == 1)
                {
                    /*if (f.paquet == GameManager.configurartion.paquete)
                        frasesDisponibles.Add(f);
                    else if (GameManager.configurartion.paquete == -1)*/
                        frasesDisponibles.Add(f);

                }
            }
            else
            {
                if (f.actualDificultad > 1)
                {
                    /* (f.paquet == GameManager.configurartion.paquete)
                        frasesDisponibles.Add(f);
                    else if (GameManager.configurartion.paquete == -1)*/
                        frasesDisponibles.Add(f);
                }
            }
        }

    }

    void Start()
    {

        if (m_GMBit.repetir)
        {
            l_Number = m_GMBit.numLastImage;
            m_GMBit.repetir = false;
        }
        else
        {
            bool same = true;
            while (same)
            {
                int random = Random.Range(0, m_Length);

                if (random != m_GMBit.numLastImage)
                {
                    GameManagerBitReadyLvl2.m_Alea = random;
                    l_Number = GameManagerBitReadyLvl2.m_Alea;
                    same = false;
                    m_GMBit.numLastImage = l_Number;
                }
                else
                    Random.InitState(Random.seed + 1);
            }
        }

        m_Animation = GetComponent<Animation>();


        m_Image.sprite = frasesDisponibles[l_Number].GetSprite(frasesDisponibles[l_Number].image);
        otherImage.sprite = frasesDisponibles[l_Number].GetSprite(frasesDisponibles[l_Number].image2);
        //crear imagenes de los rectangulos
        if (frasesDisponibles[l_Number].palabras.Count <= 2)
        {
            m_GMBit.m_NewFrasePosition.position -= new Vector3(0f, 0, 0);
        }
        else if (frasesDisponibles[l_Number].palabras.Count == 3)
        {
            m_GMBit.m_NewFrasePosition.position -= new Vector3(0.12f, 0, 0);
        }
        else if (frasesDisponibles[l_Number].palabras.Count == 4)
        {
            m_GMBit.m_NewFrasePosition.position -= new Vector3(0.25f, 0, 0);

        }
        else if (frasesDisponibles[l_Number].palabras.Count == 5)
        {
            m_GMBit.m_NewFrasePosition.position -= new Vector3(0.6f, 0, 0);
        }
        else if (frasesDisponibles[l_Number].palabras.Count >= 6)
        {
            m_GMBit.m_NewFrasePosition.position -= new Vector3(0.7f, 0, 0);
        }

        InstanciacionDeRectangulos();
    }

    private void InstanciacionDeRectangulos()
    {
        Vector3 position = m_GMBit.m_NewFrasePosition.position;
        float anchototal = 0;
        float scale = 0;
        float distance = 5.7f;
        int count = 0;
        foreach (PalabraBD p in frasesDisponibles[l_Number].palabras)
        {
            if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MAYUSCULA)
            {
                scale = p.palabraActual.Length * 0.073f;
                anchototal += scale * distance + 1.25f;
                if (frasesDisponibles[l_Number].palabras[0] != p)
                {
                    anchototal += frasesDisponibles[l_Number].palabras[count - 1].palabraActual.Length * 0.095f * distance / 2;
                }
            }
            else if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
            {
                scale = p.palabraActual.Length * 0.08f;
                anchototal += scale * distance + 1.25f;
                if (frasesDisponibles[l_Number].palabras[0] != p)
                {
                    anchototal += frasesDisponibles[l_Number].palabras[count - 1].palabraActual.Length * 0.08f * distance / 2;
                }
            }
            else
            {
                scale = p.palabraActual.Length * 0.075f;
                anchototal += scale * distance + 1f;
                if (frasesDisponibles[l_Number].palabras[0] != p)
                {
                    anchototal += frasesDisponibles[l_Number].palabras[count - 1].palabraActual.Length * 0.075f * distance / 2;
                }
            }

            count++;
        }
        anchototal /= 1.5f;
        position = new Vector3(position.x - anchototal / 2, position.y, position.z);
        Text texto;
        Image imagen;
        PalabraFraseBit2 palabraBit;
        int coun = 0;
        foreach (PalabraBD p in frasesDisponibles[l_Number].palabras)
        {
            if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MAYUSCULA)
            {
                scale = p.palabraActual.Length * 0.073f;
                if (rectanglesInScene.Count > 0)
                {
                    position = new Vector3(position.x + 1 + scale * distance / 2 + frasesDisponibles[l_Number].palabras[rectanglesInScene.Count - 1].palabraActual.Length * 0.095f * distance / 2, position.y, position.z);
                }
                else
                    position = new Vector3(position.x + scale * distance / 2, position.y, position.z);
            }
            else if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
            {
                scale = p.palabraActual.Length * 0.08f;
                if (rectanglesInScene.Count > 0)
                {
                    position = new Vector3(position.x + 1f + scale * distance / 2 + frasesDisponibles[l_Number].palabras[rectanglesInScene.Count - 1].palabraActual.Length * 0.08f * distance / 2, position.y, position.z);
                }
                else
                    position = new Vector3(position.x + scale * distance / 2, position.y, position.z);
            }
            else
            {
                scale = p.palabraActual.Length * 0.075f;
                if (rectanglesInScene.Count > 0)
                {
                    position = new Vector3(position.x + 1f + scale * distance / 2 + frasesDisponibles[l_Number].palabras[rectanglesInScene.Count - 1].palabraActual.Length * 0.075f * distance / 2, position.y, position.z);
                }
                else
                    position = new Vector3(position.x + scale * distance / 2, position.y, position.z);
            }
            rectanglesInScene.Add(Instantiate(rectanglePrefab, position, rectanglePrefab.transform.rotation));

            palabraBit = rectanglesInScene[rectanglesInScene.Count - 1].GetComponent<PalabraFraseBit2>();
            palabraBit.distance = scale * distance / 1.9f;
            palabraBit.numImage = rectanglesInScene.Count - 1;
            palabraBit.bit = this;
            palabraBit.audioSource.clip = p.GetAudioClip(p.audio);

            rectanglesInScene[rectanglesInScene.Count - 1].transform.parent = m_GMBit.m_NewFrasePosition.transform;
            texto = rectanglesInScene[rectanglesInScene.Count - 1].GetComponentInChildren<Text>();
            imagen = rectanglesInScene[rectanglesInScene.Count - 1].transform.GetChild(1).GetComponent<Image>();
            Image fondo = rectanglesInScene[rectanglesInScene.Count - 1].transform.GetChild(0).GetComponent<Image>();
            if (coun > 0)
                texto.text = p.palabraActual;
            else if (SingletonLenguage.GetInstance().GetFont() != SingletonLenguage.OurFont.MAYUSCULA)
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
            else
                texto.text = p.palabraActual;

            SearchFont(texto);
            if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MAYUSCULA)
            {
                imagen.rectTransform.sizeDelta = new Vector2(p.palabraActual.Length * 1.15f, imagen.rectTransform.sizeDelta.y);
                imagen.transform.localScale = new Vector2(imagen.transform.localScale.y, imagen.transform.localScale.y);

                if (p.palabraActual.Length <= 2)
                {
                    fondo.gameObject.transform.localScale += new Vector3(p.palabraActual.Length * 0.11f, 0, 0);

                    imagen.rectTransform.sizeDelta += new Vector2(p.palabraActual.Length * 0.4f, 0);
                    imagen.transform.localScale = new Vector2(imagen.transform.localScale.y, imagen.transform.localScale.y);
                    fondo.gameObject.transform.localScale += new Vector3(p.palabraActual.Length * 0.025f, 0, 0);

                }
                else
                {
                    fondo.gameObject.transform.localScale += new Vector3(p.palabraActual.Length * 0.1f, 0, 0);

                }
            }
            else if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
            {
                imagen.rectTransform.sizeDelta = new Vector2(p.palabraActual.Length * 1.135f, imagen.rectTransform.sizeDelta.y);
                imagen.transform.localScale = new Vector2(imagen.transform.localScale.y, imagen.transform.localScale.y);

                if (p.palabraActual.Length <= 2)
                {
                    fondo.gameObject.transform.localScale += new Vector3(p.palabraActual.Length * 0.11f, 0, 0);
                    imagen.rectTransform.sizeDelta += new Vector2(p.palabraActual.Length * 0.4f, 0);
                    imagen.transform.localScale = new Vector2(imagen.transform.localScale.y, imagen.transform.localScale.y);
                    fondo.gameObject.transform.localScale += new Vector3(p.palabraActual.Length * 0.025f, 0, 0);

                }
                else
                {
                    fondo.gameObject.transform.localScale += new Vector3(p.palabraActual.Length * 0.1f, 0, 0);

                }

            }
            else
            {
                imagen.rectTransform.sizeDelta = new Vector2(p.palabraActual.Length * 1f, imagen.rectTransform.sizeDelta.y);
                imagen.transform.localScale = new Vector2(imagen.transform.localScale.y, imagen.transform.localScale.y);

                if (p.palabraActual.Length <= 2)
                {
                    fondo.gameObject.transform.localScale += new Vector3(p.palabraActual.Length * 0.09f, 0, 0);

                    imagen.rectTransform.sizeDelta += new Vector2(p.palabraActual.Length * 0.4f, 0);
                    imagen.transform.localScale = new Vector2(imagen.transform.localScale.y, imagen.transform.localScale.y);
                    fondo.gameObject.transform.localScale += new Vector3(p.palabraActual.Length * 0.025f, 0, 0);

                }
                else
                {
                    fondo.gameObject.transform.localScale += new Vector3(p.palabraActual.Length * 0.087f, 0, 0);
                }

            }

            if (p.palabraActual.Length >= 5)
                imagen.sprite = marcoMasDe5;

            CambiarRecuadroDependiendoDePalabra(imagen, p.color);

            rectanglesInScene[rectanglesInScene.Count - 1].SetActive(false);
            texto = null;
            palabraBit = null;
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


    void Update()
    {
        if (GameManager.GetInstance().InputRecieved() && m_0touch)
        {
            Vector3 positionInput;
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    positionInput = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    if ((new Vector2(positionInput.x, positionInput.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).magnitude < 3f)
                    {
                        /* m_Animation.clip = m_Slide;
                         m_Animation.Play();*/
                        foreach (GameObject go in rectanglesInScene)
                        {
                            go.SetActive(true);
                        }
                        m_AS.clip = frasesDisponibles[l_Number].GetAudioClip(frasesDisponibles[l_Number].sound);
                        m_AS.Play();

                        m_0touch = false;
                        m_1touch = true;
                        break;
                    }
                }
            }
            else
            {
                positionInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if ((new Vector2(positionInput.x, positionInput.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).magnitude < 3f)
                {
                    /* m_Animation.clip = m_Slide;
                     m_Animation.Play();*/
                    foreach (GameObject go in rectanglesInScene)
                    {
                        go.SetActive(true);
                    }
                    m_AS.clip = frasesDisponibles[l_Number].GetAudioClip(frasesDisponibles[l_Number].sound);
                    m_AS.Play();

                    m_0touch = false;
                    m_1touch = true;
                }
            }

        }
        else if (rectanglesInScene.Count > 0)
        {
            if (m_1touch && currentWord == rectanglesInScene.Count && !rectanglesInScene[rectanglesInScene.Count - 1].GetComponent<PalabraFraseBit2>().audioSource.isPlaying)
            {
                m_Animation.clip = m_Spin;
                m_Animation.Play();
                m_1touch = false;


                StartCoroutine(WaitSeconds(m_AS.clip.length + m_Animation.clip.length + 0.2f));

            }
        }

        IEnumerator WaitSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            if (GameManager.configurartion.refuerzoPositivo)
            {
                GameObject pinguino = Instantiate(dumi, dumi.transform.position, dumi.transform.rotation);
                pinguino.GetComponent<Dumi>().AudioPositivo();
            }
            m_GMBit.ActivateButtons();
            if (!m_GMBit.repeating)
                m_GMBit.AddCountMiniGameBit();
        }
    }


    private void SearchFont(Text _text)
    {
        switch (SingletonLenguage.GetInstance().GetFont())
        {
            case SingletonLenguage.OurFont.IMPRENTA:
                _text.font = ourFonts[0];
                break;
            case SingletonLenguage.OurFont.MANUSCRITA:
                _text.font = ourFonts[1];
                break;
            case SingletonLenguage.OurFont.MAYUSCULA:
                _text.text = _text.text.ToUpper();
                _text.font = ourFonts[2];
                break;
            default:
                _text.font = ourFonts[0];
                break;
        }
    }

    public void DeletingAllBit()
    {
        foreach (GameObject go in rectanglesInScene)
        {
            Destroy(go);
        }
        Destroy(gameObject);
    }

}
