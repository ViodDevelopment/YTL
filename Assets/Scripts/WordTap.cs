using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTap : MonoBehaviour
{
    AudioSource m_AS;
    Animation m_AN;

    private void Start()
    {
        m_AS = GetComponent<AudioSource>();
        m_AN = GetComponent<Animation>();
    }

    void Update()
    {
        if (GameManager.InputRecieved())
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0f;
                RaycastHit2D l_RaycastHit = Physics2D.Raycast(touchPosition, Camera.main.transform.forward);

                if (l_RaycastHit)
                {
                    if (l_RaycastHit.collider.gameObject == this.gameObject)
                    {
                        //m_AS.clip =;
                        m_AS.Play();
                        m_AN.Play();
                    }
                }
            }
            else
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                touchPosition.z = 0f;
                RaycastHit2D l_RaycastHit = Physics2D.Raycast(touchPosition, Camera.main.transform.forward);

                if (l_RaycastHit)
                {
                    if (l_RaycastHit.collider.gameObject == this.gameObject)
                    {
                        //m_AS.clip =;
                        m_AS.Play();
                        m_AN.Play();
                    }
                }
            }
           
        }
    }
}
