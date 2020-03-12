using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveTouch : MonoBehaviour
{
    [HideInInspector]
    public OnlyOneManager managerOnlyOne;
    public bool m_PieceLocked = false;
    bool m_PieceClicked = false;
    private Vector3 m_ClickedPiecePosition;
    private Image myImage;
    public bool Word = false;
    public bool canMove = false;
    private float timer = 0;
    private bool dentro = false;
    private GameObject colision;
    private GameObject otherObject;

    public Image marco;
    public Image fondoImage;
    public Text text;

    private float currentTime = 0;
    private float maxTime = 0;
    public bool thispiece = false;
    private Vector3 startPos = Vector3.zero;

    void Start()
    {
        myImage = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if (managerOnlyOne != null)
        {
            if(startPos.x == 0 && startPos.y == 0)
            {
                startPos = gameObject.transform.position;
                m_ClickedPiecePosition = startPos;
            }
            if (!m_PieceLocked && !m_PieceClicked && ((!Word) || (Word && canMove)) && managerOnlyOne.go == null)
            {

                if (Word)
                {
                    marco.color = marco.color + new Color(0, 0, 0, 255);
                    fondoImage.color = fondoImage.color + new Color(0, 0, 0, 255);
                    text.color = text.color + new Color(0, 0, 0, 255);
                }
                else if(thispiece)
                {
                    if(GameManager.configurartion.ayudaVisual)
                    {
                        currentTime += Time.deltaTime;
                        if (maxTime == 0)
                        {
                            if (currentTime < 0.7f && currentTime > 0.2f)
                            {
                                gameObject.transform.position += new Vector3(Time.deltaTime * 1.5f, 0, 0);
                            }
                            else if (currentTime < 1.2f && currentTime > 0.7f)
                                gameObject.transform.position -= new Vector3(Time.deltaTime * 1.5f, 0, 0);
                            else if (currentTime > 1.2f)
                            {
                                gameObject.transform.position = startPos;
                                maxTime = Random.Range(1.5f, 3f);
                                currentTime = 0;
                            }
                        }
                        else if (currentTime >= maxTime)
                        {
                            maxTime = 0;
                            currentTime = 0;
                        }
                    }
                }


                if (Input.GetMouseButtonDown(0) && Input.touchCount == 0)
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    touchPosition.z = 0f;

                    RaycastHit2D l_RaycastHit = Physics2D.Raycast(touchPosition, Camera.main.transform.forward);
                    if (l_RaycastHit)
                    {
                        if (l_RaycastHit.collider.gameObject == this.gameObject)
                        {
                            m_PieceClicked = true;
                            this.gameObject.transform.SetAsLastSibling();
                            managerOnlyOne.Catch(true, gameObject);
                            currentTime = 0;
                            maxTime = Random.Range(1.5f, 3f);

                        }
                    }
                }
                else
                {

                    if (Input.touchCount > 0)
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
                                    m_PieceClicked = true;
                                    this.gameObject.transform.SetAsLastSibling();
                                    managerOnlyOne.Catch(true, gameObject);
                                    currentTime = 0;
                                    maxTime = Random.Range(1.5f, 3f);
                                    break;
                                }
                            }
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

                    if (!Word)
                        this.transform.position = touchPosition - new Vector3(myImage.rectTransform.rect.width / 256, -myImage.rectTransform.rect.height / 256);
                    else
                        this.transform.position = touchPosition;
                }

                else if (Input.touchCount > 0)
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

                                if (!Word)
                                    this.transform.position = touchPosition - new Vector3(myImage.rectTransform.rect.width / 256, -myImage.rectTransform.rect.height / 256);
                                else
                                    this.transform.position = touchPosition;
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

                        if (!Word)
                            this.transform.position = touchPosition - new Vector3(myImage.rectTransform.rect.width / 256, -myImage.rectTransform.rect.height / 256);
                        else
                            this.transform.position = touchPosition;
                    }
                }
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;

                if (GameManager.configurartion.refuerzoPositivo)
                {
                    if (otherObject != null && otherObject.name != this.gameObject.name && !dentro)
                    {
                        GameManager.fallosPuzzle++;
                        if (GameObject.Find("Dumi(Clone)") == null && GameManager.fallosPuzzle >= 2)
                        {
                            GameObject pinguino = Instantiate(GameObject.FindGameObjectWithTag("GameManagerPuzzle").GetComponent<GameManagerPuzzle>().dumi, GameObject.FindGameObjectWithTag("GameManagerPuzzle").GetComponent<GameManagerPuzzle>().dumi.transform.position, GameObject.FindGameObjectWithTag("GameManagerPuzzle").GetComponent<GameManagerPuzzle>().dumi.transform.rotation);
                            pinguino.GetComponent<Dumi>().AudioNegativo();
                            GameManager.fallosPuzzle = 0;
                        }
                    }
                }

                if (timer <= 0)
                {
                    timer = 0;
                    if (!m_PieceLocked)
                        this.transform.position = m_ClickedPiecePosition;

                    m_PieceClicked = false;
                    managerOnlyOne.Catch(false, null);
                }

            }

            if (m_PieceClicked && Input.GetMouseButtonUp(0) && timer == 0 && Input.touchCount == 0)
            {
                timer = 0.01f;

                if (dentro)
                {
                    this.transform.position = colision.gameObject.transform.position;
                    m_PieceLocked = true;
                    this.transform.SetParent(GameObject.FindGameObjectWithTag("GameManagerPuzzle").GetComponent<GameManagerPuzzle>().m_Saver.transform);
                    if (SceneManager.GetActiveScene().name == "Puzzle")
                        GameObject.FindGameObjectWithTag("GameManagerPuzzle").GetComponent<GameManagerPuzzle>().m_Puntuacion++;
                    dentro = false;
                    colision = null;
                    otherObject = null;
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

                    if (!Word)
                        this.transform.position = touchPosition - new Vector3(myImage.rectTransform.rect.width / 256, -myImage.rectTransform.rect.height / 256);
                    else
                        this.transform.position = touchPosition;
                }

                if (!tocando)
                {
                    timer = 0.01f;

                    if (dentro)
                    {
                        this.transform.position = colision.gameObject.transform.position;
                        m_PieceLocked = true;
                        this.transform.SetParent(GameObject.FindGameObjectWithTag("GameManagerPuzzle").GetComponent<GameManagerPuzzle>().m_Saver.transform);
                        if (SceneManager.GetActiveScene().name == "Puzzle")
                            GameObject.FindGameObjectWithTag("GameManagerPuzzle").GetComponent<GameManagerPuzzle>().m_Puntuacion++;
                        dentro = false;
                        colision = null;
                        otherObject = null;
                    }
                }

            }
        }
    }

    IEnumerator WaitToFrame()
    {
        yield return new WaitForSeconds(5);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.name == this.gameObject.name)
        {
            colision = _collision.gameObject;
            dentro = true;
        }
        if (colision == null && _collision.gameObject.GetComponent<MoveTouch>() == null)
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
