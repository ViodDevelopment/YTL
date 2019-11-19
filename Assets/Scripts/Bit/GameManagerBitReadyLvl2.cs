using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerBitReadyLvl2 : MonoBehaviour
{
    public SceneManagement m_Scener;
    int m_CurrentNumRep = 0;
    public GameObject m_NewBit;
    public Transform m_NewBitPosition;
    public Transform m_NewFrasePosition;
    public Sprite m_CompletedPoint;
    public Sprite m_IncompletedPoint;
    [HideInInspector]
    public GameObject m_CurrentBit;

    public Transform m_SpawnImpar;
    public Transform m_SpawnPar;
    Transform m_CurrentSpawn;
    public GameObject m_Point;
    static int l_NumReps = GameManager.Instance.m_NeededToMinigame;
    GameObject[] m_Points = new GameObject[l_NumReps];

    public GameObject m_Siguiente;
    public GameObject m_Repetir;

    private void Start()
    {
        //GameManager.Instance.m_CurrentToMinigame;

        print(m_Points.Length);
        if (l_NumReps % 2 == 0)
        {
            m_CurrentSpawn = m_SpawnPar;
            m_CurrentSpawn.GetComponent<RectTransform>().anchoredPosition -= new Vector2((75 * (l_NumReps / 2 - 1)), 0);
        }
        else
        {
            m_CurrentSpawn = m_SpawnImpar;
            m_CurrentSpawn.GetComponent<RectTransform>().anchoredPosition -= new Vector2((75 * (l_NumReps / 2)), 0);
        }

        for (int i = 0; i < l_NumReps; i++)
        {
                m_Points[i] = Instantiate(m_Point, m_CurrentSpawn.transform);
                m_Points[i].GetComponent<RectTransform>().anchoredPosition += new Vector2(m_Points[i].transform.position.x + (i * 75), 0);
        }

        for (int i = 0; i <= GameManager.m_CurrentToMinigame[1]; i++)
        {
            m_Points[i].GetComponent<Image>().sprite = m_CompletedPoint;
        }
       
        RepeatImage();
    }

    public void RepeatImage()
    {
            Destroy(m_CurrentBit);
            m_CurrentBit = Instantiate(m_NewBit, m_NewBitPosition);
            m_CurrentNumRep++;
    }

    public void NextBit()
    {
        GameManager.m_CurrentToMinigame[1]++;

        if (GameManager.m_CurrentToMinigame[1] >= GameManager.Instance.m_NeededToMinigame)
            m_Scener.NextGame();

        else
        {
            Destroy(m_CurrentBit);
            m_Points[GameManager.m_CurrentToMinigame[1]].GetComponent<Image>().sprite = m_CompletedPoint;
            m_CurrentNumRep = 0;
            RepeatImage();
        }
    }

    public void ActivateButtons()
    {
        m_Siguiente.SetActive(true);
        if(m_CurrentNumRep<=GameManager.configurartion.repetitionsOfExercise)
        m_Repetir.SetActive(true);
    }
}
