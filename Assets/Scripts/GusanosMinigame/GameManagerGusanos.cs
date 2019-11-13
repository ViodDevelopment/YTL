using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerGusanos : MonoBehaviour
{
    public float m_MaxTime = 30;
    private float currentTime = 0;
    public SceneManagement m_Scener;

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= m_MaxTime)
        {
            int mayor = -1;
            int count = 0;
            for (int i = 0; i < GameManager.m_CurrentToMinigame.Count; i++)
            {
                if (GameManager.m_CurrentToMinigame[i] > mayor)
                {
                    count = i;
                    mayor = GameManager.m_CurrentToMinigame[i];
                }
            }
            GameManager.m_CurrentToMinigame[count] = 0;
            m_Scener.InicioScene();
        }
    }

}
