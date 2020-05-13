﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PairsLvl2 : MonoBehaviour
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

    public GameManagerParejasLvl2 m_GameManagerParejas;
    private AudioSource audioSource;
    private float timer = 0;
    public int numImage;
    public Text texto;

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
        m_GameManagerParejas = GameObject.FindGameObjectWithTag("GMParejas").GetComponent<GameManagerParejasLvl2>();
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

        if (SingletonLenguage.GetInstance().GetFont() == SingletonLenguage.OurFont.MANUSCRITA)
            texto.gameObject.transform.localScale *= 1.3f;
    }

    private void Update()
    {
        if (managerOnlyOne != null)
        {

            #region animación
            if (GameManager.configuration.ayudaVisual)
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
                            gameObject.transform.position += new Vector3(-0.25f * Time.deltaTime, -0.25f * Time.deltaTime, 0);
                            rectTransform.localScale += new Vector3(0.25f * Time.deltaTime, 0.25f * Time.deltaTime, 0);
                        }
                        else if (currentTimerAnim < 1f)
                        {
                            gameObject.transform.position += new Vector3(0.25f * Time.deltaTime, 0.25f * Time.deltaTime, 0);
                            rectTransform.localScale += new Vector3(-0.25f * Time.deltaTime, -0.25f * Time.deltaTime, 0);
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
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        Touch touch = Input.GetTouch(i);
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
                                //this.gameObject.transform.parent.transform.parent.transform.SetAsLastSibling();
                                Transform grandpa = this.gameObject.transform.parent;
                                grandpa.SetAsLastSibling();

                                managerOnlyOne.Catch(true, gameObject);
                                break;
                            }
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
                if (Input.GetMouseButton(0) && Input.touchCount == 0)
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    touchPosition.z = 0f;
                    this.transform.position = touchPosition - new Vector3(myImage.rectTransform.rect.width / 200, myImage.rectTransform.rect.height / 200);
                }
                else
                {
                    if (Input.touchCount > 0)
                    {
                        int position = 0;
                        float min = 999;
                        bool tiene = false;
                        for (int i = 0; i < Input.touchCount; i++)
                        {
                            Touch touch = Input.GetTouch(i);
                            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                            touchPosition.z = 0f;

                            RaycastHit2D l_RaycastHit = Physics2D.Raycast(touchPosition, Camera.main.transform.forward);
                            if (l_RaycastHit)
                            {
                                if (l_RaycastHit.collider.gameObject == this.gameObject)
                                {
                                    tiene = true;
                                    this.transform.position = touchPosition - new Vector3(myImage.rectTransform.rect.width / 200, myImage.rectTransform.rect.height / 200);
                                    break;
                                }
                            }
                            else
                            {
                                if ((new Vector2(touchPosition.x, touchPosition.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).magnitude < min)
                                {
                                    min = (new Vector2(touchPosition.x, touchPosition.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).magnitude;
                                    position = i;
                                }
                            }
                        }
                        if (min <= 3f && !tiene)
                        {
                            Touch touch = Input.GetTouch(position);
                            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                            touchPosition.z = 0f;

                            this.transform.position = touchPosition - new Vector3(myImage.rectTransform.rect.width / 200, myImage.rectTransform.rect.height / 200);
                        }
                    }
                }
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (GameManager.configuration.refuerzoPositivo)
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

            if (m_PieceClicked && Input.GetMouseButtonUp(0) && timer == 0 && Input.touchCount == 0)
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
                        m_GameManagerParejas.m_TextZoomed.gameObject.transform.localScale = Vector3.one * 0.4f; m_GameManagerParejas.m_TextZoomed.GetComponent<ConvertFont>().Convert();
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
            else if (Input.touchCount > 0 && timer == 0 && m_PieceClicked)
            {
                float min = 999;
                int position = 0;
                bool tocando = false;
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    touchPosition.z = 0f;

                    if ((new Vector2(touchPosition.x, touchPosition.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).magnitude < min)
                    {
                        if (touch.phase != TouchPhase.Ended)
                        {
                            min = (new Vector2(touchPosition.x, touchPosition.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).magnitude;
                            position = i;
                        }
                    }
                }
                if (min <= 3f)
                {
                    tocando = true;
                    Touch touch = Input.GetTouch(position);
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    touchPosition.z = 0f;

                    this.transform.position = touchPosition - new Vector3(myImage.rectTransform.rect.width / 200, myImage.rectTransform.rect.height / 200);

                }

                if (!tocando)
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
                            m_GameManagerParejas.m_TextZoomed.gameObject.transform.localScale = Vector3.one * 0.4f; m_GameManagerParejas.m_TextZoomed.GetComponent<ConvertFont>().Convert();
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
    }





    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.name == this.gameObject.name)
        {
            colision = _collision.gameObject;
            dentro = true;
        }
        if (colision == null && _collision.gameObject.GetComponent<PairsLvl2>() == null)
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
