using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    private GameObject m_Spawn;
    private float m_Speed;
    public Transform m_Vuelta;
    bool m_Giro =false;

    // Start is called before the first frame update
    void Start()
    {
        m_Speed = (m_Vuelta.position.x - this.transform.position.x )/ 30 * 2;    
        m_Spawn = GameObject.FindGameObjectWithTag("SpawnGlobo");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, m_Vuelta.position) < 0.15f)
        {
            m_Giro = true;
            this.GetComponent<SpriteRenderer>().flipX=true;
        }
        if (!m_Giro)
        transform.position += Vector3.right * Time.deltaTime * m_Speed;
        else
        transform.position -= Vector3.right * Time.deltaTime * m_Speed;

       
    }
}
