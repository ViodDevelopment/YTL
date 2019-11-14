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

    public Image mainImage;
    public Text text;


    void Start()
    {
        myImage = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if (managerOnlyOne != null)
        {
            if (!m_PieceLocked && !m_PieceClicked && ((!Word) || (Word && canMove)))
            {

                if (Word)
                {
                    mainImage.color = mainImage.color + new Color(0, 0, 0, 255);
                    text.color = text.color + new Color(0, 0, 0, 255);
                }

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
            }

        }

    }

    IEnumerator WaitToFrame()
    {
        yield return new WaitForSeconds(5);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.name == this.gameObject.name) && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0)) && !m_PieceLocked)
        {
            this.transform.position = collision.gameObject.transform.position;
            m_PieceLocked = true;
            this.transform.SetParent(GameObject.FindGameObjectWithTag("GameManagerPuzzle").GetComponent<GameManagerPuzzle>().m_Saver.transform);
            if (SceneManager.GetActiveScene().name == "Puzzle")
                GameObject.FindGameObjectWithTag("GameManagerPuzzle").GetComponent<GameManagerPuzzle>().m_Puntuacion++;
        }
        else if ((collision.gameObject.name != this.gameObject.name) && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0)) && !m_PieceLocked)
        {
            GameObject pinguino = Instantiate(GameObject.FindGameObjectWithTag("GameManagerPuzzle").GetComponent<GameManagerPuzzle>().dumi, GameObject.FindGameObjectWithTag("GameManagerPuzzle").GetComponent<GameManagerPuzzle>().dumi.transform.position, GameObject.FindGameObjectWithTag("GameManagerPuzzle").GetComponent<GameManagerPuzzle>().dumi.transform.rotation);
            pinguino.GetComponent<Dumi>().AudioNegativo();
        }
    }


}
