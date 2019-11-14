using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pairs : MonoBehaviour
{
    [HideInInspector]
    bool m_PieceClicked = false;
    public OnlyOneManager managerOnlyOne;
    public string nombre = "";
    public AudioClip audioClip;
    private Image myImage;

    public GameManagerParejas m_GameManagerParejas;
    private AudioSource audioSource;
    private float timer = 0;
    public int numImage;

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
        m_GameManagerParejas = GameObject.FindGameObjectWithTag("GMParejas").GetComponent<GameManagerParejas>();
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
                if (timer <= 0)
                {
                    timer = 0;
                    this.transform.position = lastPosition;
                    currentTimerAnim = 0;
                    animIsplaying = false;
                    rectTransform.localScale = lastSize;
                    m_PieceClicked = false;
                    managerOnlyOne.Catch(false, null);
                }

            }

            if (m_PieceClicked && (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) && timer == 0)
            {
                timer = 0.01f;
            }
        }

    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.name == this.gameObject.name) && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0)))
        {
            this.transform.position = collision.gameObject.transform.position;
            m_GameManagerParejas.m_ImageZoomed.sprite = this.gameObject.GetComponent<Image>().sprite;
            m_GameManagerParejas.m_TextZoomed.text = nombre;
            //m_GameManagerParejas.m_TextZoomed.fontSize = SingletonLenguage.GetInstance().ConvertSizeDependWords(m_GameManagerParejas.m_TextZoomed.text);
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


            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
            gameObject.transform.position = lastPosition;
            rectTransform.localScale = lastSize;
            currentTimerAnim = 0;
            firstTime = true;
            animIsplaying = false;
            m_PieceClicked = false;
            managerOnlyOne.Catch(false, null);

        }
        else if((collision.gameObject.name != this.gameObject.name) && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0)))
        {
            if (!m_GameManagerParejas.dumiActivo)
            {
                GameObject pinguino = Instantiate(m_GameManagerParejas.dumi, m_GameManagerParejas.dumi.transform.position, m_GameManagerParejas.dumi.transform.rotation);
                pinguino.GetComponent<Dumi>().AudioNegativo();
                m_GameManagerParejas.dumiActivo = true;
                m_GameManagerParejas.timerDumi = 1.5f;
            }
        }
    }
}
