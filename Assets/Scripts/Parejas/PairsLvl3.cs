using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PairsLvl3 : MonoBehaviour
{
    [HideInInspector]
    bool m_PieceClicked = false;
    public OnlyOneManager managerOnlyOne;
    public string nombre = "";
    public string color = "";
    public AudioClip audioClip;
    private Image myImage;
    private bool dentro = false;
    private int lastFallos = 0;
    private GameObject colision;
    private GameObject otherObject;

    public GameManagerParejasLvl3 m_GameManagerParejas;
    private AudioSource audioSource;
    private float timer = 0;
    public int numImage;
    private float originalSizeText;

    private RectTransform rectTransform;
    private float currentTimerAnim = 0;
    private float maxTimerAnim = 1;
    private bool firstTime = true;
    private bool animIsplaying;
    private Vector3 lastSize;
    private Vector3 lastPosition;
    private bool lastPair;

    private void Start()
    {
        m_GameManagerParejas = GameObject.FindGameObjectWithTag("GMParejas").GetComponent<GameManagerParejasLvl3>();
        audioSource = m_GameManagerParejas.GetComponent<AudioSource>();
        myImage = gameObject.GetComponent<Image>();

        rectTransform = GetComponent<RectTransform>();
        lastPosition = gameObject.transform.position;
        lastSize = rectTransform.localScale;
        Random.InitState(Random.seed + 1);
        maxTimerAnim = Random.Range(1.5f, 3f);
        if (numImage == 0)
            maxTimerAnim = 1;

        if (numImage < m_GameManagerParejas.m_NumPairs - 1)
            lastPair = true;
        else lastPair = false;
    }
    private void Update()
    {
        if (managerOnlyOne != null)
        {

            #region animación
            if (GameManager.configurartion.ayudaVisual)
            {
                if (numImage == 0 && firstTime)
                {
                    firstTime = false;
                    maxTimerAnim = 1;
                }

                if (m_GameManagerParejas.m_CurrentPairs == numImage && !m_PieceClicked)
                {
                    currentTimerAnim += Time.deltaTime;
                    if (currentTimerAnim >= maxTimerAnim && !m_PieceClicked && !animIsplaying)
                    {
                        currentTimerAnim = 0;
                        animIsplaying = true;
                        maxTimerAnim = Random.Range(1.5f, 3);
                    }

                    if (animIsplaying && !m_PieceClicked)
                    {
                        if (currentTimerAnim < 0.5f)
                        {
                            gameObject.transform.position += new Vector3(-0.5f * Time.deltaTime, -0.5f * Time.deltaTime, 0);
                            rectTransform.localScale += new Vector3(0.5f * Time.deltaTime, 0.5f * Time.deltaTime, 0);
                        }
                        else if (currentTimerAnim < 1f)
                        {
                            gameObject.transform.position += new Vector3(0.5f * Time.deltaTime, 0.5f * Time.deltaTime, 0);
                            rectTransform.localScale += new Vector3(-0.5f * Time.deltaTime, -0.5f * Time.deltaTime, 0);
                        }
                        else
                        {
                            currentTimerAnim = 0;
                            animIsplaying = false;
                            rectTransform.localScale = lastSize;
                            gameObject.transform.position = lastPosition;
                        }
                    }

                }
            }
            #endregion


            if (!m_PieceClicked && m_GameManagerParejas.m_CurrentPairs == numImage)
            {
                if (Input.touchCount > 0 && managerOnlyOne.go == null && !m_GameManagerParejas.m_Animation.isPlaying)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    touchPosition.z = 0f;

                    RaycastHit2D l_RaycastHit = Physics2D.Raycast(touchPosition, Camera.main.transform.forward);
                    if (l_RaycastHit)
                    {
                        if (l_RaycastHit.collider.gameObject == this.gameObject)
                        {
                            currentTimerAnim = 0;
                            animIsplaying = false;
                            rectTransform.localScale = lastSize;

                            m_PieceClicked = true;
                            this.gameObject.transform.parent.transform.parent.transform.SetAsLastSibling();
                            managerOnlyOne.Catch(true, gameObject);
                        }
                    }

                }

                if (Input.GetMouseButtonDown(0) && managerOnlyOne.go == null && !m_GameManagerParejas.m_Animation.isPlaying)
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    touchPosition.z = 0f;

                    RaycastHit2D l_RaycastHit = Physics2D.Raycast(touchPosition, Camera.main.transform.forward);
                    if (l_RaycastHit)
                    {
                        if (l_RaycastHit.collider.gameObject == this.gameObject)
                        {
                            currentTimerAnim = 0;
                            animIsplaying = false;
                            rectTransform.localScale = lastSize;

                            m_PieceClicked = true;
                            Transform grandpa = this.gameObject.transform.parent;
                            grandpa.SetAsLastSibling();
                            managerOnlyOne.Catch(true, gameObject);

                        }
                    }
                }
            }

            if (m_PieceClicked)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    touchPosition.z = 0f;
                    this.transform.position = touchPosition - new Vector3(myImage.rectTransform.rect.width / 200, myImage.rectTransform.rect.height / 200);
                }

                else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
                {
                    Touch touch = Input.GetTouch(0);
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    touchPosition.z = 0f;
                    this.transform.position = touchPosition;
                }
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (GameManager.configurartion.refuerzoPositivo)
                {
                    if (otherObject != null && otherObject.name != this.gameObject.name && !dentro)
                    {
                        if (lastFallos == GameManager.fallosParejas)
                            GameManager.fallosParejas++;
                        if (GameObject.Find("Dumi(Clone)") == null && GameManager.fallosParejas >= 2)
                        {
                            GameObject pinguino = Instantiate(m_GameManagerParejas.dumi, m_GameManagerParejas.dumi.transform.position, m_GameManagerParejas.dumi.transform.rotation);
                            pinguino.GetComponent<Dumi>().AudioNegativo();
                            GameManager.fallosParejas = 0;
                        }
                    }
                }

                if (timer <= 0)
                {
                    timer = 0;
                    this.transform.position = lastPosition;
                    currentTimerAnim = 0;
                    animIsplaying = false;
                    rectTransform.localScale = lastSize;
                    m_PieceClicked = false;
                    managerOnlyOne.Catch(false, null);
                    colision = null;
                    otherObject = null;
                    dentro = false;

                }



            }

            if (m_PieceClicked && (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) && timer == 0)
            {
                timer = 0.02f;
                lastFallos = GameManager.fallosParejas;
                if (dentro)
                {
                    this.transform.position = colision.gameObject.transform.position;
                    m_GameManagerParejas.m_ImageZoomed.sprite = this.gameObject.GetComponent<Image>().sprite;
                    m_GameManagerParejas.m_TextZoomed.text = nombre;

                    foreach (Image i in m_GameManagerParejas.marcos)
                    {
                        m_GameManagerParejas.PonerColorMarco(color, i);
                    }

                    if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
                        m_GameManagerParejas.m_TextZoomed.gameObject.transform.localScale = Vector3.one * 0.35f;

                    m_GameManagerParejas.m_TextZoomed.GetComponent<ConvertFont>().Convert();

                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = audioClip;
                        audioSource.Play();
                    }
                    m_GameManagerParejas.PairDone();

                    if (lastPair)
                        m_GameManagerParejas.planeImageWhenPair.gameObject.SetActive(true);
                    else
                        m_GameManagerParejas.planeImageWhenPair.gameObject.SetActive(false);

                    if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
                        colision.gameObject.GetComponentInChildren<Text>().gameObject.transform.localScale /= 1.5f;
                    colision.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                    gameObject.transform.position = lastPosition;
                    rectTransform.localScale = lastSize;
                    currentTimerAnim = 0;
                    firstTime = true;
                    animIsplaying = false;
                    m_PieceClicked = false;
                    managerOnlyOne.Catch(false, null);
                    colision = null;
                    otherObject = null;
                    dentro = false;
                    timer = 0;
                }

            }
        }

    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.name == this.gameObject.name)
        {
            colision = _collision.gameObject;
            originalSizeText = colision.gameObject.GetComponentInChildren<Text>().gameObject.transform.localScale.x;
            dentro = true;
        }
        if (colision == null && _collision.gameObject.GetComponent<PairsLvl3>() == null)
            otherObject = _collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.gameObject.name == this.gameObject.name)
        {
            colision = null;
            dentro = false;
        }

        if (otherObject == _collision.gameObject)
            otherObject = null;
    }
}
