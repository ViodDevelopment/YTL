using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinijuegoBurbujas : MonoBehaviour
{

    private GameObject m_Spawn;
    private GameObject m_Destroy;

    public GameObject m_Burbuja;
    public GameObject m_EndGame;

    private float m_TimePassed;
    private float m_TimeToSpawn;
    private float m_TimeToNext;

    public SceneManagement m_Scener;

   

    // Start is called before the first frame update
    void Start()
    {
        m_Spawn = GameObject.FindGameObjectWithTag("SpawnBurbuja");
        m_Destroy = GameObject.FindGameObjectWithTag("DestroyBurbuja");

        m_TimePassed = 0;
        m_TimeToSpawn = 0;
        m_TimeToNext = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_TimePassed < 30)
        {
            m_TimePassed += Time.deltaTime;

            if (m_TimeToSpawn > m_TimeToNext)
            {
                //Instantiate(m_Globo, m_Spawn.transform.position + new Vector3(Random.Range(-7, 7), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity);
                Instantiate(m_Burbuja, m_Spawn.transform.position, Quaternion.identity);
                m_TimeToSpawn = 0;
                m_TimeToNext = Random.Range(0.75f, 1f);
            }
            else
                m_TimeToSpawn += Time.deltaTime;
        }
        else 
        {
            Debug.Log("End Minigame");
            int mayor = -1;
            int count = 0;
            for (int i = 0; i < GameManager.m_CurrentToMinigame.Count; i++)
            {
                if(GameManager.m_CurrentToMinigame[i] > mayor)
                {
                    count = i;
                    mayor = GameManager.m_CurrentToMinigame[i];
                }
            }
            GameManager.m_CurrentToMinigame[count] = 0;
           m_Scener.InicioScene(true);
            //endgame
        }
    }
}
