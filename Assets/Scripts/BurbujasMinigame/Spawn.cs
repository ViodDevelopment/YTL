using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    private GameObject m_Spawn;
    private float m_Speed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        m_Spawn = GameObject.FindGameObjectWithTag("SpawnGlobo");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime * m_Speed;
    }
}
