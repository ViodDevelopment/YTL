using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mariposa : MonoBehaviour
{
    private float m_Speed = 5;

    public Sprite m_Mariposa_01;
    public Sprite m_Mariposa_02;
    public Sprite m_Mariposa_03;

    private bool m_SpriteMariposa01;
    private bool m_SpriteMariposa02;
    private bool m_SpriteMariposa03;
    private float m_TimePassed;

    // Start is called before the first frame update
    void Start()
    {
        m_SpriteMariposa01 = true;
        m_SpriteMariposa02 = false;
        m_SpriteMariposa03 = false;

        m_TimePassed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0.5f, 0.5f, 0) * Time.deltaTime * m_Speed;

        if (m_SpriteMariposa01 && m_TimePassed > 0.1f)
        {
            GetComponent<SpriteRenderer>().sprite = m_Mariposa_01;
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            m_TimePassed = 0;
            m_SpriteMariposa01 = false;
            m_SpriteMariposa02 = true;
            transform.position += new Vector3(0, 0.2f, 0);
        }

        else if (m_SpriteMariposa02 && m_TimePassed > 0.1f)
        {
            GetComponent<SpriteRenderer>().sprite = m_Mariposa_02;
            this.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            m_TimePassed = 0;
            m_SpriteMariposa02 = false;
            m_SpriteMariposa03 = true;
            transform.position -= new Vector3(0, 0.2f, 0);
        }

        else if (m_SpriteMariposa03 && m_TimePassed > 0.1f)
        {
            GetComponent<SpriteRenderer>().sprite = m_Mariposa_03;
            m_TimePassed = 0;
            m_SpriteMariposa03 = false;
            transform.position -= new Vector3(0, 0.2f, 0);
        }

        else if (m_TimePassed > 0.1f)
        {
            GetComponent<SpriteRenderer>().sprite = m_Mariposa_02;
            m_TimePassed = 0;
            m_SpriteMariposa01 = true;
            transform.position += new Vector3(0, 0.2f, 0);
        }

        m_TimePassed += Time.deltaTime;
    }
}
