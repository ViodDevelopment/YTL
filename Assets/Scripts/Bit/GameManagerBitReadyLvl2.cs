using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerBitReadyLvl2 : MonoBehaviour
{
    public SceneManagement m_Scener;
    int m_CurrentNumRep = 1;
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
    static int l_NumReps = 7;//-1 aplicado
    GameObject[] m_Points = new GameObject[l_NumReps];

    public GameObject m_Siguiente;
    public GameObject m_Repetir;
    public bool repetir = false;
    public int numLastImage = 0;
    public bool repeating;

    public static int m_Alea = 0;
    public int levelBit = 2;

    private void Start()
    {
        //GameManager.Instance.m_CurrentToMinigame;
        Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Minute);
        m_Alea = Random.Range(0, BitLvl2.m_Length);

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
            if (i > 0 && m_Points.Length > i - 1)
                m_Points[i - 1].GetComponent<Image>().sprite = m_CompletedPoint;
        }
        repeating = false;
        InicioBit();
    }

    public void RepeatImage(bool _repetir)
    {
        if (m_CurrentBit != null && _repetir)
        {
            numLastImage = m_CurrentBit.GetComponent<BitLvl2>().l_Number;
            m_CurrentBit.GetComponent<BitLvl2>().DeletingAllBit();
            repetir = _repetir;
            repeating = true;
        }
        m_CurrentBit = Instantiate(m_NewBit, m_NewBitPosition);
        m_CurrentNumRep++;
    }

    public void NextBit()
    {
        repeating = false;
        if (m_Alea == 0)
        {
            m_Alea = Random.Range(0, BitLvl2.m_Length);
        }
        else
        {
            bool same = true;
            int count = 0;
            int rand = m_Alea;
            while (same)
            {
                count += System.DateTime.Now.Second + 1;
                Random.InitState(count);
                m_Alea = Random.Range(0, BitLvl2.m_Length);
                if (rand != m_Alea)
                    same = false;
            }
        }

        if (GameManager.m_CurrentToMinigame[1] >= 7)
        {
            m_CurrentBit.GetComponent<BitLvl2>().DeletingAllBit();
            GameManager.ResetPointToMinigame(1);
            m_Scener.NextGame();
        }
        else
        {
            m_CurrentBit.GetComponent<BitLvl2>().DeletingAllBit();
            if (GameManager.m_CurrentToMinigame[1] > 0 && m_Points.Length > GameManager.m_CurrentToMinigame[1] - 1)
                m_Points[GameManager.m_CurrentToMinigame[1] - 1].GetComponent<Image>().sprite = m_CompletedPoint;
            m_CurrentNumRep = 1;
            RepeatImage(false);
        }
    }

    public void AddCountMiniGameBit()
    {
        if (m_Points.Length > GameManager.m_CurrentToMinigame[1] - 1)
            GameManager.SumPointToMinigame(1);
        if (GameManager.m_CurrentToMinigame[1] > 0 && m_Points.Length > GameManager.m_CurrentToMinigame[1] - 1)
        {
            m_Points[GameManager.m_CurrentToMinigame[1] - 1].GetComponent<Image>().sprite = m_CompletedPoint;
        }
    }

    public void InicioBit()
    {
        if (m_Alea == 0)
        {
            m_Alea = Random.Range(0, BitLvl2.m_Length);
        }
        else
        {
            bool same = true;
            int count = 0;
            int rand = m_Alea;
            while (same)
            {
                count++;
                Random.InitState(count * System.DateTime.Now.Second);
                m_Alea = Random.Range(0, BitLvl2.m_Length);
                if (rand != m_Alea)
                    same = false;
            }
        }
        Destroy(m_CurrentBit);
        if (GameManager.m_CurrentToMinigame[1] > 0 && m_Points.Length > GameManager.m_CurrentToMinigame[1] - 1)
            m_Points[GameManager.m_CurrentToMinigame[1] - 1].GetComponent<Image>().sprite = m_CompletedPoint;
        m_CurrentNumRep = 1;
        RepeatImage(false);
    }

    public void ActivateButtons()
    {
        m_Siguiente.SetActive(true);
        if (m_CurrentNumRep <= GameManager.configurartion.repetitionsOfExercise)
            m_Repetir.SetActive(true);
    }
}
