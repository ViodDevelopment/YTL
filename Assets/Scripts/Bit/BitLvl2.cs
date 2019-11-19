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
    GameManagerBit m_GMBit;
    public AnimationClip m_Spin;
    public AnimationClip m_Slide;
    private List<FraseBD> frasesDisponibles = new List<FraseBD>();
    private int firstImage = 0;

    public static int m_Length;
    public Image m_Image;
    public GameObject rectanglePrefab;
    private List<GameObject> rectanglesInScene = new List<GameObject>();
    public List<Sprite> listOfRectangles = new List<Sprite>(); //Ordenarlos para saber el orden para ponerlos abajo
    public List<Font> ourFonts = new List<Font>();
    public AudioSource m_AS;
    public int l_Number;
    void Awake()
    {
        RecolectFrasesBD();
        m_Length = frasesDisponibles.Count;
        m_GMBit = GameObject.FindGameObjectWithTag("Bit").GetComponent<GameManagerBit>();
        GameManagerBit.m_Alea = Random.Range(0, m_Length);
    }

    private void RecolectFrasesBD()
    {
        frasesDisponibles.Clear();
        foreach (FraseBD f in GameManager.frasesDisponibles)
        {
            frasesDisponibles.Add(f);
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
                    GameManagerBit.m_Alea = random;
                    l_Number = GameManagerBit.m_Alea;
                    same = false;
                    m_GMBit.numLastImage = l_Number;
                }
                else
                    Random.InitState(Random.seed + 1);
            }
        }

        m_Animation = GetComponent<Animation>();
        firstImage = Random.Range(0, 3);

        m_Image.sprite = Resources.Load<Sprite>("Images/Lite/" + frasesDisponibles[l_Number].image);

        //crear imagenes de los rectangulos
        InstanciacionDeRectangulos();
    }

    private void InstanciacionDeRectangulos()
    {

    }


    void Update()
    {
        if (GameManager.Instance.InputRecieved() && m_0touch)
        {
            Vector3 positionInput;
            if (Input.touchCount > 0)
                positionInput = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            else
                positionInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if ((new Vector2(positionInput.x, positionInput.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).magnitude < 3f)
            {
               /* m_Animation.clip = m_Slide;
                m_Animation.Play();*/
                m_0touch = false;
                m_1touch = true;
            }

        }

        else if (GameManager.Instance.InputRecieved() && m_1touch && !m_Animation.isPlaying && !m_AS.isPlaying)
        {
            Vector3 positionInput;
            if (Input.touchCount > 0)
                positionInput = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            else
                positionInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if ((new Vector2(positionInput.x, positionInput.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).magnitude <= 3f)
            {
                m_Animation.clip = m_Spin;
                m_Animation.Play();
                m_1touch = false;

                StartCoroutine(WaitSeconds(3f));
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
                _text.text = _text.text.ToLower();
                _text.font = ourFonts[0];
                break;
            case SingletonLenguage.OurFont.MANUSCRITA:
                _text.text = _text.text.ToLower();
                _text.font = ourFonts[1];
                break;
            case SingletonLenguage.OurFont.MAYUSCULA:
                _text.text = _text.text.ToUpper();
                _text.font = ourFonts[2];
                break;
            default:
                _text.text = _text.text.ToLower();
                _text.font = ourFonts[0];
                break;
        }
    }

}
