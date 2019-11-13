using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globo : MonoBehaviour
{

    private float m_Speed = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * m_Speed;

        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray l_Ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit l_Hit;
            if (Physics.Raycast(l_Ray, out l_Hit))
            {
                if (l_Hit.collider.tag == "Globo")
                {
                    Debug.Log("TAPPED");
                    Destroy(l_Hit.collider.gameObject);
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            Ray l_Ray;
            RaycastHit l_Hit;
            l_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(l_Ray, out l_Hit))
            {
                if(l_Hit.collider.tag == "Globo")
                {
                    Debug.Log("PUM");
                    Destroy(l_Hit.collider.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DestroyGlobo")
        {
            Destroy(gameObject);
        }
    }
}
