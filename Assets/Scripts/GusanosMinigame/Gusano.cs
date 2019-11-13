using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gusano : MonoBehaviour
{

    public GameObject m_Mariposa;

    private float m_Speed = 0;

    public Sprite m_Gusano_01;
    public Sprite m_Gusano_02;
    public Sprite m_Gusano_03;

    private bool m_SpriteGusano01;
    private bool m_SpriteGusano02;
    private bool m_SpriteGusano03;
    private bool desactivado;
    private float m_TimePassed;

    // Start is called before the first frame update
    void Start()
    {
        m_SpriteGusano01 = true;
        m_SpriteGusano02 = false;
        m_SpriteGusano03 = false;
        desactivado = false;
        m_TimePassed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!desactivado)
        {
            transform.position += Vector3.right * Time.deltaTime * m_Speed;

            if (transform.position.x - 5 > Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x)
                desactivado = true;


            if (m_SpriteGusano01 && m_TimePassed > 0.25f)
            {
                GetComponent<SpriteRenderer>().sprite = m_Gusano_01;
                // this.gameObject.transform.localScale = new Vector3(1, 1, 1);
                m_TimePassed = 0;
                m_SpriteGusano01 = false;
                m_SpriteGusano02 = true;
                // transform.position += new Vector3(0, 0.2f, 0);
                m_Speed = 0f;
            }

            else if (m_SpriteGusano02 && m_TimePassed > 0.2f)
            {
                GetComponent<SpriteRenderer>().sprite = m_Gusano_02;
                //  this.gameObject.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                m_TimePassed = 0;
                m_SpriteGusano02 = false;
                m_SpriteGusano03 = true;
                //transform.position -= new Vector3(0, 0.2f, 0);
                m_Speed = 2f;
            }

            else if (m_SpriteGusano03 && m_TimePassed > 0.15f)
            {
                GetComponent<SpriteRenderer>().sprite = m_Gusano_03;
                // this.gameObject.transform.localScale = new Vector3(1, 1, 1);
                m_TimePassed = 0;
                m_SpriteGusano03 = false;
                // transform.position -= new Vector3(0, 0.2f, 0);
                m_Speed = 3f;
            }

            else if (m_TimePassed > 0.25f)
            {
                GetComponent<SpriteRenderer>().sprite = m_Gusano_02;
                // this.gameObject.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                m_TimePassed = 0;
                m_SpriteGusano01 = true;
                //  transform.position += new Vector3(0, 0.2f, 0);
                m_Speed = 2f;
            }

            m_TimePassed += Time.deltaTime;

            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
            {
                Vector3 l_Ray = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                RaycastHit2D l_Hit = Physics2D.Raycast(l_Ray, Vector2.zero);
                if (l_Hit.collider != null)
                {
                    if (l_Hit.collider.tag == "Gusano")
                    {
                        Debug.Log("TAPPED");
                        //mariposa
                        Instantiate(m_Mariposa, l_Hit.collider.gameObject.transform.position, Quaternion.identity);
                        Destroy(l_Hit.collider.gameObject);
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 l_Ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D l_Hit = Physics2D.Raycast(l_Ray, Vector2.zero);
                if (l_Hit.collider != null)
                {
                    if (l_Hit.collider.tag == "Gusano")
                    {
                        Debug.Log("ECLOSION");
                        //mariposa
                        Instantiate(m_Mariposa, l_Hit.collider.gameObject.transform.position, Quaternion.identity);
                        Destroy(l_Hit.collider.gameObject);
                    }
                }
            }
        }
    }
}
