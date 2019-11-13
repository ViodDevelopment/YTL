using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burbuja : MonoBehaviour
{

    private float m_Speed = 2;
    private float m_GrowSpeed = 0.3f;
    private bool l_SpeedChanged = false;
    public bool explotada = false;
    private float l_StopGrowing;
    private Vector3 l_Direction;

    public GameObject m_BubblePS;
    public AudioSource m_AS;
    // Start is called before the first frame update
    void Start()
    {
        l_StopGrowing = Vector3.one.magnitude + Random.Range(-0.5f, 0.5f);
        l_Direction = new Vector3(Random.Range(-0.6f, -0.2f), 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!explotada)
        {
            transform.position += l_Direction * Time.deltaTime * m_Speed;

            if (!l_SpeedChanged)
                ChangeSpeed();

            if (l_SpeedChanged && m_Speed > 0.5f)
                m_Speed -= Time.deltaTime;

            if (transform.localScale.magnitude < l_StopGrowing)
                transform.localScale += Vector3.one * Time.deltaTime * m_GrowSpeed;

            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
            {
                Vector3 l_Ray = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                RaycastHit2D l_Hit = Physics2D.Raycast(l_Ray, Vector2.zero);
                if (l_Hit.collider != null)
                {
                    if (l_Hit.collider.tag == "Burbuja")
                    {
                        Debug.Log("TAPPED");
                        //Vector3 actualPos = gameObject.transform.position;
                        GameObject l_Ball = Instantiate(m_BubblePS, l_Hit.collider.transform.position, m_BubblePS.transform.rotation);
                        //l_Ball.transform.position = actualPos;
                        m_AS.Play();
                        GetComponent<SpriteRenderer>().enabled = false;
                        Destroy(l_Hit.collider.gameObject, 1f);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 l_Ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D l_Hit = Physics2D.Raycast(l_Ray, Vector2.zero);
                if (l_Hit.collider != null)
                {
                    if (l_Hit.collider.tag == "Burbuja")
                    {
                        Debug.Log("PUM");
                        //Vector3 actualPos = gameObject.transform.position;
                        GameObject l_Ball = Instantiate(m_BubblePS, l_Hit.collider.transform.position, m_BubblePS.transform.rotation);
                        l_Ball.transform.localScale = l_Hit.collider.gameObject.transform.localScale;
                        //l_Ball.transform.position = actualPos;
                        m_AS.Play();
                        l_Hit.collider.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        l_Hit.collider.gameObject.GetComponent<Burbuja>().explotada = true;
                        l_Hit.collider.tag = "Untagged";
                        l_Hit.collider.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                        Destroy(l_Hit.collider.gameObject, 1f);
                    }
                }
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DestroyBurbuja")
        {
            Destroy(gameObject);
        }
    }

    private void ChangeSpeed()
    {
        int l_rand = Random.Range(0, 1000);
        if (l_rand > 995)
        {
            l_SpeedChanged = true;
        }
    }
}