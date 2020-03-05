using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveTouchLvl2 : MonoBehaviour
{
    [HideInInspector]
    public OnlyOneManager managerOnlyOne;
    private GameManagerPuzzleLvl2 gameManagerPuzzle2;
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

    public List<Sprite> sprites = new List<Sprite>();
    public List<Sprite> spritesFondo = new List<Sprite>();
    public int silaba = -5; // -1 inicio, 0 medio, 1 final
    private bool done = false;
    public Image mainImage;
    public Text text;
    public Image fondo;


    void Start()
    {
        myImage = gameObject.GetComponent<Image>();
        gameManagerPuzzle2 = GameObject.FindGameObjectWithTag("GameManagerPuzzle").GetComponent<GameManagerPuzzleLvl2>();
    }

    void Update()
    {
        if (managerOnlyOne != null)
        {
            if (Word)
            {
                if (!done && silaba != -5)
                {
                    switch (silaba)
                    {
                        case -1:
                            mainImage.sprite = sprites[0];
                            fondo.sprite = spritesFondo[0];
                            fondo.rectTransform.anchoredPosition = new Vector3(4, 0, 0);

                            text.transform.position += new Vector3(-0.13f, 0, 0);
                            if (text.text.Length > 3)
                            {
                                mainImage.rectTransform.sizeDelta = new Vector2(fondo.rectTransform.sizeDelta.x * 1.2f, 319);
                                fondo.rectTransform.sizeDelta = new Vector2(fondo.rectTransform.sizeDelta.x*1.2f, 319);
                                text.transform.position += new Vector3(-0.010f, 0, 0);
                            }
                            break;
                        case 0:
                            mainImage.sprite = sprites[1];
                            fondo.sprite = spritesFondo[1];
                            fondo.rectTransform.sizeDelta = new Vector2(315, 287.5f);
                            mainImage.rectTransform.sizeDelta = new Vector2(315, 319);
                            fondo.rectTransform.anchoredPosition = new Vector3(0, 0, 0);

                            break;
                        case 1:
                            mainImage.sprite = sprites[2];
                            fondo.sprite = spritesFondo[2];
                            fondo.rectTransform.anchoredPosition = new Vector3(-4, 0, 0);
                            text.transform.position += new Vector3(-0.13f, 0, 0);
                            if(text.text.Length > 3)
                            {
                                text.transform.position += new Vector3(-0.18f, 0, 0);
                            }
                            break;
                        case 2:
                            mainImage.sprite = sprites[3];
                            fondo.sprite = spritesFondo[3];
                            fondo.rectTransform.sizeDelta = new Vector3(315, 308, 0);
                            break;
                    }
                    done = true;
                }
            }
            if (!m_PieceLocked && !m_PieceClicked && ((!Word) || (Word && canMove)))
            {

                if (Input.touchCount > 0 && managerOnlyOne.go == null)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    touchPosition.z = 0f;

                    RaycastHit2D l_RaycastHit = Physics2D.Raycast(touchPosition, Camera.main.transform.forward);
                    if (l_RaycastHit)
                    {
                        if (l_RaycastHit.collider.gameObject == this.gameObject)
                        {
                            m_PieceClicked = true;
                            this.gameObject.transform.SetAsLastSibling();
                            m_ClickedPiecePosition = this.gameObject.transform.position;
                            managerOnlyOne.Catch(true, gameObject);
                        }
                    }

                }

                if (Input.GetMouseButtonDown(0) && managerOnlyOne.go == null)
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
                            m_ClickedPiecePosition = this.gameObject.transform.position;
                            m_ClickedPiecePosition = this.gameObject.transform.position;
                            managerOnlyOne.Catch(true, gameObject);
                        }
                    }
                }
            }

            if (m_PieceClicked && timer == 0)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    touchPosition.z = 0f;
                    if (!Word)
                        this.transform.position = touchPosition - new Vector3(myImage.rectTransform.rect.width / 256, -myImage.rectTransform.rect.height / 256);
                    else
                        this.transform.position = touchPosition;
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
                        GameManager.fallosPuzzle++;
                        if (GameObject.Find("Dumi(Clone)") == null && GameManager.fallosPuzzle >= 2)
                        {
                            GameObject pinguino = Instantiate(gameManagerPuzzle2.dumi, gameManagerPuzzle2.dumi.transform.position, gameManagerPuzzle2.dumi.transform.rotation);
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

            if (m_PieceClicked && (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) && timer == 0)
            {
                timer = 0.01f;

                if (dentro)
                {
                    this.transform.position = colision.gameObject.transform.position;
                    m_PieceLocked = true;
                    this.transform.SetParent(gameManagerPuzzle2.m_Saver.transform);
                    gameManagerPuzzle2.m_Puntuacion++;
                    if (Word)
                    {
                        Destroy(colision.gameObject);
                        gameManagerPuzzle2.currentSilaba++;
                    }
                    dentro = false;
                    colision = null;
                    otherObject = null;
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
