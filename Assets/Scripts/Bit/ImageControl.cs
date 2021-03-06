﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageControl : MonoBehaviour
{
    public GameObject dumi;
    bool m_0touch = true;
    bool m_1touch = false;
    Animation m_Animation;
    GameManagerBit m_GMBit;
    public AnimationClip m_Spin;
    public AnimationClip m_Slide;
    private List<Texture2D> m_ImagesPool = new List<Texture2D>();
    private List<Texture2D> m_ImagesPool2 = new List<Texture2D>();
    private List<Texture2D> m_ImagesPool3 = new List<Texture2D>();
    private List<PalabraBD> palabrasDisponibles = new List<PalabraBD>();
    public List<Image> marcos = new List<Image>();
    private int firstImage = 0;

    private List<string> m_PalabrasCastellano = new List<string>();
    private List<string> m_PalabrasCatalan = new List<string>();
    private List<AudioClip> m_AudioPoolCastellano = new List<AudioClip>();
    private List<AudioClip> m_AudioPoolCatalan = new List<AudioClip>();

    public static int m_Length;
    private int lenghtPaquet = 0;
    public Image m_Image;
    public Image m_ImageBehind;
    public Text m_Text;
    public List<Font> ourFonts = new List<Font>();
    public AudioSource m_AS;
    public AudioSource m_ASArticle;
    public int l_Number;
    private bool acabado = false;
    private bool AnimFinCaida = false;
    private bool firstAudio = true;
    private bool finished = false;
    private PalabraBD lastPalabra;
    private PalabraBD currentPalabra;
    void Awake()
    {
        RecolectPalabrasBD();
        m_Length = palabrasDisponibles.Count;
        m_GMBit = GameObject.FindGameObjectWithTag("Bit").GetComponent<GameManagerBit>();
    }

    private void RecolectPalabrasBD(bool enter = false)
    {
        palabrasDisponibles.Clear();
        if (!PaqueteBit.GetInstance().acabado)
        {
            int num = 0;
            foreach (PalabraBD p in PaqueteBit.GetInstance().currentBitPaquet)
            {
                if (p.paquet == GameManager.configuration.paquete || GameManager.configuration.paquete == -1)
                {
                    num++;
                }
            }
            if (num == 0)
            {
                PaqueteBit.GetInstance().CrearNuevoPaquete();
                if (PaqueteBit.GetInstance().currentBitPaquet.Count == 0)
                    PaqueteBit.GetInstance().acabado = true;
                PaqueteBit.GetInstance().CrearBinario();
            }
        }
        if (PaqueteBit.GetInstance().acabado)
        {
            foreach (PalabraBD p in GameManager.palabrasDisponibles)
            {
                if (p.paquet == GameManager.configuration.paquete)
                {
                    if (p.image1 != "")
                    {
                        p.SetPalabraActual();
                        palabrasDisponibles.Add(p);
                    }
                }
                else if (GameManager.configuration.paquete == -1)
                {
                    if (p.image1 != "")
                    {
                        p.SetPalabraActual();
                        palabrasDisponibles.Add(p);
                    }
                }
            }
        }
        else
        {
            foreach (PalabraBD p in PaqueteBit.GetInstance().currentBitPaquet)
            {
                if (p.paquet == GameManager.configuration.paquete)
                {
                    if (p.image1 != "")
                    {
                        p.SetPalabraActual();
                        palabrasDisponibles.Add(p);
                    }
                }
                else if (GameManager.configuration.paquete == -1)
                {
                    if (p.image1 != "")
                    {
                        p.SetPalabraActual();
                        palabrasDisponibles.Add(p);
                    }
                }
            }
        }

        lenghtPaquet = palabrasDisponibles.Count;

        foreach (PalabraBD p in GameManager.palabrasUserDisponibles)
        {
            if (SingletonLenguage.GetInstance().GetLenguage() == SingletonLenguage.Lenguage.CASTELLANO)
            {
                if (p.nameSpanish != "" && (GameManager.configuration.paquete == 0 || GameManager.configuration.paquete == -1))
                {
                    p.SetPalabraActual();
                    palabrasDisponibles.Add(p);
                }
            }
            else if (SingletonLenguage.GetInstance().GetLenguage() == SingletonLenguage.Lenguage.CATALAN)
            {
                if (p.nameCatalan != "" && (GameManager.configuration.paquete == 0 || GameManager.configuration.paquete == -1))
                {
                    p.SetPalabraActual();
                    palabrasDisponibles.Add(p);
                }
            }
        }

    }

    void Start()
    {
        Random.InitState(Random.seed + 1);

        if (m_GMBit.repetir)
        {
            l_Number = m_GMBit.numLastImage;
            m_GMBit.repetir = false;
            if (!PaqueteBit.GetInstance().acabado)
                lastPalabra = m_GMBit.lastPalabra;
        }
        else if (PaqueteBit.GetInstance().acabado)
        {
            bool same = true;
            while (same && m_Length > 1)
            {
                int random = Random.Range(0, m_Length);

                if (random != m_GMBit.numLastImage)
                {
                    l_Number = random;
                    same = false;
                    m_GMBit.numLastImage = l_Number;
                }
                else
                    Random.InitState(Random.seed + 1);
            }
        }
        else
        {
            int random = Random.Range(0, m_Length);
            l_Number = random;
            if (l_Number >= lenghtPaquet)
            {
                if (GameManagerBit.user)
                {
                    l_Number = Random.Range(0, lenghtPaquet);
                    GameManagerBit.user = false;
                }
                else
                {
                    GameManagerBit.user = true;
                }
            }
            else
            {
                GameManagerBit.user = false;
            }
            m_GMBit.numLastImage = l_Number;

        }
        if (palabrasDisponibles.Count > l_Number)
            currentPalabra = palabrasDisponibles[l_Number];
        if (lastPalabra != null)
            currentPalabra = lastPalabra;
        m_GMBit.lastPalabra = currentPalabra;

        Color color = new Color();
        foreach (Image i in marcos)
        {
            ColorUtility.TryParseHtmlString(currentPalabra.color, out color);
            i.color = color;
        }

        m_Animation = GetComponent<Animation>();

        Random.InitState(Random.seed + Random.Range(-5, 5));
        firstImage = Random.Range(0, 3);

        switch (firstImage)
        {
            case 0:
                m_Image.sprite = currentPalabra.GetSprite(currentPalabra.image1);
                break;
            case 1:
                m_Image.sprite = currentPalabra.GetSprite(currentPalabra.image2);
                break;
            case 2:
                m_Image.sprite = currentPalabra.GetSprite(currentPalabra.image3);
                break;
        }


        Random.InitState(Random.seed + Random.Range(-5, 5));
        int otherImage = Random.Range(0, 3);

        int contador = 0;
        while (otherImage == firstImage && contador <= 99999)
        {
            contador++;
            Random.InitState(Random.seed + Random.Range(-5, 5));
            otherImage = Random.Range(0, 3);
        }
        /*if (otherImage == firstImage)
        {
            if (otherImage >= 2)
                otherImage--;
            else if (otherImage <= 0)
                otherImage++;
            else otherImage++;
        }*/
        /*while (otherImage == firstImage)
         {
             Random.InitState(Random.seed + 1);
             otherImage = Random.Range(0, 3);
         }*/

        switch (otherImage)
        {
            case 0:
                m_ImageBehind.sprite = currentPalabra.GetSprite(currentPalabra.image1);
                break;
            case 1:
                m_ImageBehind.sprite = currentPalabra.GetSprite(currentPalabra.image2);
                break;
            case 2:
                m_ImageBehind.sprite = currentPalabra.GetSprite(currentPalabra.image3);
                break;
        }

        if (currentPalabra.user)
        {
            m_Image.sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(m_Image.sprite);
            m_ImageBehind.sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(m_ImageBehind.sprite);
        }

        if (!GameManager.configuration.palabrasConArticulo)
            m_Text.text = currentPalabra.palabraActual;
        else
        {
            if (currentPalabra.actualArticulo != null && currentPalabra.actualArticulo != "")
            {
                m_Text.text = currentPalabra.actualArticulo + currentPalabra.palabraActual;
            }
            else
                m_Text.text = currentPalabra.palabraActual;

        }

        SearchFont();
        if (m_Text.text.Length < 7)
        {

            m_Text.fontSize -= 2;
            if (m_Text.font == ourFonts[2])
                m_Text.fontSize -= 2;
        }
        else if (m_Text.text.Length < 8)
        {
            m_Text.fontSize -= 25;
            if (m_Text.font == ourFonts[2])
                m_Text.fontSize -= 10;
        }
        else if (m_Text.text.Length < 9)
        {
            m_Text.fontSize -= 35;
            if (m_Text.font == ourFonts[2])
                m_Text.fontSize -= 15;
        }
        else if (m_Text.text.Length < 10)
        {
            m_Text.fontSize -= 40;
            if (m_Text.font == ourFonts[2])
                m_Text.fontSize -= 20;
        }
        else if (m_Text.text.Length < 11)
        {
            m_Text.fontSize -= 50;
            if (m_Text.font == ourFonts[2])
                m_Text.fontSize -= 25;
        }
        else if (m_Text.text.Length < 12)
        {
            m_Text.fontSize -= 55;
            if (m_Text.font == ourFonts[2])
                m_Text.fontSize -= 30;
        }
        else
        {
            m_Text.fontSize -= 65;
            if (m_Text.font == ourFonts[2])
                m_Text.fontSize -= 40;
        }
        //m_Text.fontSize = SingletonLenguage.GetInstance().ConvertSizeDependWords(m_Text.text);
        m_AS.clip = currentPalabra.GetAudioClip(currentPalabra.audio);
        finished = true;
        if (GameManager.configuration.palabrasConArticulo)
        {
            if (currentPalabra.actualAudioArticulo != null)
            {
                m_ASArticle.clip = currentPalabra.GetAudioArticulo();
                firstAudio = false;
                finished = false;

            }
        }

    }


    void Update()
    {
        if (firstAudio && !finished)
        {
            if (!m_ASArticle.isPlaying)
            {
                if (m_1touch)
                {
                    firstAudio = false;
                    finished = false;
                }
                else
                {
                    finished = true;
                }
                if (!currentPalabra.onlyArticulo)
                    m_AS.Play();
            }
        }

        if (AnimFinCaida && !m_Animation.isPlaying)
        {
            m_ImageBehind.transform.parent.transform.SetSiblingIndex(1);
            AnimFinCaida = true;
        }
        if (GameManager.InputRecieved() && m_0touch)
        {
            Vector3 positionInput;
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    positionInput = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    if ((new Vector2(positionInput.x, positionInput.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).magnitude < 3f)
                    {
                        m_Animation.clip = m_Slide;
                        m_Animation.Play();

                        if (m_ASArticle.clip != null)
                        {
                            StartCoroutine(WaitForPlayArt(0.2f));

                        }
                        else
                        {
                            StartCoroutine(WaitForOnlyPlay(0.2f));
                        }

                        m_0touch = false;
                        m_1touch = true;
                        AnimFinCaida = true;
                        break;
                    }
                }
            }
            else
            {
                positionInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if ((new Vector2(positionInput.x, positionInput.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).magnitude < 3f)
                {
                    m_Animation.clip = m_Slide;
                    m_Animation.Play();
                    if (m_ASArticle.clip != null)
                    {
                        StartCoroutine(WaitForPlayArt(0.2f));

                    }
                    else
                    {
                        StartCoroutine(WaitForOnlyPlay(0.2f));
                    }
                    m_0touch = false;
                    m_1touch = true;
                    AnimFinCaida = true;

                }
            }

        }

        else if (GameManager.InputRecieved() && m_1touch && !m_Animation.isPlaying && !m_AS.isPlaying && !m_ASArticle.isPlaying)
        {
            Vector3 positionInput;
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    positionInput = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    if ((new Vector2(positionInput.x, positionInput.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).magnitude <= 3f)
                    {
                        m_Animation.clip = m_Spin;
                        m_Animation.Play();
                        if (m_ASArticle.clip != null)
                        {
                            StartCoroutine(WaitForPlayArt(1f));

                        }
                        else
                        {
                            StartCoroutine(WaitForOnlyPlay(1f));
                        }
                        m_1touch = false;

                        if (!currentPalabra.onlyArticulo)
                            StartCoroutine(WaitSeconds(m_Animation.clip.length + m_AS.clip.length + 0.2f + (m_ASArticle.clip == null ? 0 : m_ASArticle.clip.length)));
                        else
                            StartCoroutine(WaitSeconds(0.2f + (m_ASArticle.clip == null ? 0 : m_ASArticle.clip.length)));
                        break;
                    }
                }
            }
            else
            {
                positionInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if ((new Vector2(positionInput.x, positionInput.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).magnitude <= 3f)
                {
                    m_Animation.clip = m_Spin;
                    m_Animation.Play();
                    if (m_ASArticle.clip != null)
                    {
                        StartCoroutine(WaitForPlayArt(1f));
                    }
                    else
                    {
                        StartCoroutine(WaitForOnlyPlay(1f));
                    }
                    m_1touch = false;

                    if (!currentPalabra.onlyArticulo)
                        StartCoroutine(WaitSeconds(m_Animation.clip.length + m_AS.clip.length + 0.2f + (m_ASArticle.clip == null ? 0 : m_ASArticle.clip.length)));
                    else
                        StartCoroutine(WaitSeconds(0.2f + (m_ASArticle.clip == null ? 0 : m_ASArticle.clip.length)));
                }
            }

        }
        else if (acabado)
        {
            if (!GameManager.configuration.refuerzoPositivo)
            {
                m_GMBit.ActivateButtons();
                acabado = false;
            }
            else if (GameObject.Find("Dumi(Clone)") == null)
            {
                m_GMBit.ActivateButtons();
                acabado = false;
            }
        }

        IEnumerator WaitForOnlyPlay(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            m_AS.Play();
        }


        IEnumerator WaitForPlayArt(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            m_ASArticle.Play();
            firstAudio = true;
            finished = false;
        }

        IEnumerator WaitSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            if (GameManager.configuration.refuerzoPositivo)
            {
                GameObject pinguino = Instantiate(dumi, dumi.transform.position, dumi.transform.rotation);
                pinguino.GetComponent<Dumi>().AudioPositivo();
            }
            if (!m_GMBit.repeating)
            {
                m_GMBit.AddCountMiniGameBit();
                if (!PaqueteBit.GetInstance().acabado)
                {
                    if (PaqueteBit.GetInstance().currentBitPaquet.Count > l_Number)
                    {
                        PaqueteBit.GetInstance().currentBitPaquet.Remove(currentPalabra);
                        int num = 0;
                        foreach (PalabraBD p in PaqueteBit.GetInstance().currentBitPaquet)
                        {
                            if (p.paquet == GameManager.configuration.paquete || GameManager.configuration.paquete == -1)
                            {
                                num++;
                            }
                        }
                        if (num == 0)
                        {
                            PaqueteBit.GetInstance().CrearNuevoPaquete();
                        }
                        PaqueteBit.GetInstance().CrearBinario();

                    }
                }
            }
            acabado = true;


        }
    }

    private string PutName(int _alea)
    {
        switch (SingletonLenguage.GetInstance().GetLenguage())
        {
            case SingletonLenguage.Lenguage.CASTELLANO:
                return m_PalabrasCastellano[_alea];
            case SingletonLenguage.Lenguage.CATALAN:
                return m_PalabrasCatalan[_alea];
            default:
                return m_PalabrasCastellano[_alea];
        }
    }

    private AudioClip PutAudio(int _alea)
    {
        switch (SingletonLenguage.GetInstance().GetLenguage())
        {
            case SingletonLenguage.Lenguage.CASTELLANO:
                return m_AudioPoolCastellano[_alea];
            case SingletonLenguage.Lenguage.CATALAN:
                return m_AudioPoolCatalan[_alea];
            default:
                return m_AudioPoolCastellano[_alea];

        }
    }


    private void SearchFont()
    {
        switch (SingletonLenguage.GetInstance().GetFont())
        {
            case SingletonLenguage.OurFont.IMPRENTA:
                m_Text.font = ourFonts[0];
                break;
            case SingletonLenguage.OurFont.MANUSCRITA:
                m_Text.font = ourFonts[1];
                break;
            case SingletonLenguage.OurFont.MAYUSCULA:
                m_Text.text = m_Text.text.ToUpper();
                m_Text.font = ourFonts[2];
                break;
            default:
                m_Text.font = ourFonts[0];
                break;
        }
    }

}
