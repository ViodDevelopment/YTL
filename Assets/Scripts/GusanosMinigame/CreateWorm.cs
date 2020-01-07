using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWorm : MonoBehaviour
{
    public float m_MinTimeSpawn;
    public float m_MaxTimeSpawn;
    public GameObject m_Worm;
    float m_NextTime;
    float m_CurrentTime;

    private void Start()
    {
        m_NextTime = Random.Range(0, m_MinTimeSpawn);
    }

    void Update()
    {
        m_CurrentTime += Time.deltaTime;
        if(m_CurrentTime>=m_NextTime)
        {
            InstantiateWorm();
        }
    }

    void InstantiateWorm()
    {
        Instantiate(m_Worm, this.transform.position, m_Worm.transform.rotation);
        m_NextTime = Random.Range(m_MinTimeSpawn, m_MaxTimeSpawn);
        m_CurrentTime = 0;
    }
}
