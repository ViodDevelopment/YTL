using System.Collections;
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
    private string imageAPoner = "";
    private List<PalabraBD> palabrasDisponibles = new List<PalabraBD>();
    public List<Image> marcos = new List<Image>();
    private int firstImage = 0;

    private List<string> m_PalabrasCastellano = new List<string>();
    private List<string> m_PalabrasCatalan = new List<string>();
    private List<AudioClip> m_AudioPoolCastellano = new List<AudioClip>();
    private List<AudioClip> m_AudioPoolCatalan = new List<AudioClip>();

    public static int m_Length;
    public Image m_Image;
    public Image m_ImageBehind;
    public Text m_Text;
    public List<Font> ourFonts = new List<Font>();
    public AudioSource m_AS;
    public int l_Number;
    private bool acabado = false;
    private bool AnimFinCaida = false;
    private PalabraBD lastPalabra;
    private PalabraBD currentPalabra;
    void Awake()
    {
       
        RecolectPalabrasBD();
        m_Length = palabrasDisponibles.Count;
        m_GMBit = GameObject.FindGameObjectWithTag("Bit").GetComponent<GameManagerBit>();
    }

    private void RecolectPalabrasBD()
    {
        if (PaqueteBit.GetInstance().acabado)
        {
            foreach (PalabraBD p in GameManager.palabrasDisponibles)
            {
                if (p.paquet == GameManager.configurartion.paquete)
                {
                    if (p.image1 != "")
                    {
                        palabrasDisponibles.Add(p);
                    }
                }
                else if (GameManager.configurartion.paquete == -1)
                {
                    if (p.image1 != "")
                    {
                        palabrasDisponibles.Add(p);
                    }
                }
            }
        }
        else
        {
            foreach (PalabraBD p in PaqueteBit.GetInstance().currentBitPaquet)
            {
                if (p.paquet == GameManager.configurartion.paquete)
                {
                    if (p.image1 != "")
                    {
                        palabrasDisponibles.Add(p);
                    }
                }
                else if (GameManager.configurartion.paquete == -1)
                {
                    if (p.image1 != "")
                    {
                        palabrasDisponibles.Add(p);
                    }
                }
            }
        }

        foreach (PalabraBD p in GameManager.palabrasUserDisponibles)
        {
            if (SingletonLenguage.GetInstance().GetLenguage() == SingletonLenguage.Lenguage.CASTELLANO)
            {
                if (p.nameSpanish != "" && (GameManager.configurartion.paquete == 0 || GameManager.configurartion.paquete == -1))
                    palabrasDisponibles.Add(p);
            }
            else if (SingletonLenguage.GetInstance().GetLenguage() == SingletonLenguage.Lenguage.CATALAN)
            {
                if (p.nameCatalan != "" && (GameManager.configurartion.paquete == 0 || GameManager.configurartion.paquete == -1))
                    palabrasDisponibles.Add(p);
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
            if(l_Number >= PaqueteBit.GetInstance().currentBitPaquet.Count)
            {
                if(m_GMBit.user)
                {
                    l_Number = Random.Range(0, PaqueteBit.GetInstance().currentBitPaquet.Count);
                    m_GMBit.user = false;
                }
                {
                    m_GMBit.user = true;
                }
            }
            else
            {
                m_GMBit.user = false;
            }
            m_GMBit.numLastImage = l_Number;

        }
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

        if(currentPalabra.user)
        {
            m_Image.sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(m_Image.sprite);
            m_ImageBehind.sprite = SiLoTienesBienSinoPaCasa.GetSpriteFromUser(m_ImageBehind.sprite);
        }


        m_Text.text = currentPalabra.palabraActual;
        SearchFont();
        if (m_Text.text.Length < 7)
        {

            m_Text.fontSize -= 2;
            if (m_Text.font == ourFonts[2])
                m_Text.fontSize -= 2;
        }
        else if (m_Text.text.Length < 8)
        {
            m_Text.fontSize -= 20;
            if (m_Text.font == ourFonts[2])
                m_Text.fontSize -= 10;
        }
        else if (m_Text.text.Length < 9)
        {
            m_Text.fontSize -= 30;
            if (m_Text.font == ourFonts[2])
                m_Text.fontSize -= 15;
        }
        else
        {
            m_Text.fontSize -= 40;
            if (m_Text.font == ourFonts[2])
                m_Text.fontSize -= 20;
        }
        //m_Text.fontSize = SingletonLenguage.GetInstance().ConvertSizeDependWords(m_Text.text);
        m_AS.clip = currentPalabra.GetAudioClip(currentPalabra.audio);


    }


    void Update()
    {
        if (AnimFinCaida && !m_Animation.isPlaying)
        {
            m_ImageBehind.transform.parent.transform.SetSiblingIndex(1);
            AnimFinCaida = true;
        }
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
                        m_Animation.clip = m_Slide;
                        m_Animation.Play();
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
                    m_0touch = false;
                    m_1touch = true;
                    AnimFinCaida = true;

                }
            }

        }

        else if (GameManager.GetInstance().InputRecieved() && m_1touch && !m_Animation.isPlaying && !m_AS.isPlaying)
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
                        m_1touch = false;

                        StartCoroutine(WaitSeconds(m_Animation.clip.length + m_AS.clip.length + 0.2f));
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
                    m_1touch = false;

                    StartCoroutine(WaitSeconds(m_Animation.clip.length + m_AS.clip.length + 0.2f));
                }
            }

        }
        else if (acabado)
        {
            if (!GameManager.configurartion.refuerzoPositivo)
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

        IEnumerator WaitSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            if (GameManager.configurartion.refuerzoPositivo)
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
                            if(p.paquet == GameManager.configurartion.paquete || GameManager.configurartion.paquete == -1)
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
                m_Text.text = m_Text.text.ToLower();
                m_Text.font = ourFonts[0];
                break;
            case SingletonLenguage.OurFont.MANUSCRITA:
                m_Text.text = m_Text.text.ToLower();
                m_Text.font = ourFonts[1];
                break;
            case SingletonLenguage.OurFont.MAYUSCULA:
                m_Text.text = m_Text.text.ToUpper();
                m_Text.font = ourFonts[2];
                break;
            default:
                m_Text.text = m_Text.text.ToLower();
                m_Text.font = ourFonts[0];
                break;
        }
    }

}
