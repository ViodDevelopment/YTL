﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerBit : MonoBehaviour
{
    public SceneManagement m_Scener;
    int m_CurrentNumRep = 1;
    public GameObject m_NewBit;
    public Transform m_NewBitPosition;
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
    public Image m_ActivitiesButton;
    public bool repetir = false;
    public PalabraBD lastPalabra;
    public int numLastImage = 0;
    public bool repeating;
    public static bool user = false;

    private void Start()
    {
        //GameManager.Instance.m_CurrentToMinigame;
        Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Minute);

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
            numLastImage = m_CurrentBit.GetComponent<ImageControl>().l_Number;
            repetir = _repetir;
            repeating = true;
        }
        Destroy(m_CurrentBit);
        m_CurrentBit = Instantiate(m_NewBit, m_NewBitPosition);
        m_CurrentNumRep++;
    }

    public void NextBit()
    {
        repeating = false;


        if (GameManager.m_CurrentToMinigame[1] >= 7)
        {
            GameManager.ResetPointToMinigame(1);
            m_Scener.NextGame();
        }
        else
        {
            Destroy(m_CurrentBit);
            if (GameManager.m_CurrentToMinigame[1] > 0 && m_Points.Length > GameManager.m_CurrentToMinigame[1] - 1)
                m_Points[GameManager.m_CurrentToMinigame[1] - 1].GetComponent<Image>().sprite = m_CompletedPoint;
            m_CurrentNumRep = 1;
            RepeatImage(false);
        }
    }

    public void AddCountMiniGameBit()
    {
        if(m_Points.Length > GameManager.m_CurrentToMinigame[1] - 1)
            GameManager.SumPointToMinigame(1);
        if (GameManager.m_CurrentToMinigame[1] > 0 && m_Points.Length > GameManager.m_CurrentToMinigame[1] - 1)
        {
            m_Points[GameManager.m_CurrentToMinigame[1] - 1].GetComponent<Image>().sprite = m_CompletedPoint;
        }
    }

    public void InicioBit()
    {
        Destroy(m_CurrentBit);
        if (GameManager.m_CurrentToMinigame[1] > 0 && m_Points.Length > GameManager.m_CurrentToMinigame[1] - 1)
            m_Points[GameManager.m_CurrentToMinigame[1] - 1].GetComponent<Image>().sprite = m_CompletedPoint;
        m_CurrentNumRep = 1;
        RepeatImage(false);
    }

    public void ActivateButtons()
    {
        m_ActivitiesButton.color = new Color(255, 255, 255, 1);
        m_Siguiente.SetActive(true);
        if (m_CurrentNumRep <= GameManager.configuration.repetitionsOfExercise)
            m_Repetir.SetActive(true);
    }

    public void ReturnColor()
    {
        m_ActivitiesButton.color = new Color(255, 255, 255, 0.5f);
    }
}
