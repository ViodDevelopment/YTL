using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MicroHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    AudioClip m_Recording;
    AudioSource m_AudioSource;
    private float startRecordingTime;


    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        print("END_RECORDING");
        Microphone.End("");

        AudioClip recordingNew = AudioClip.Create(m_Recording.name, (int)((Time.time - startRecordingTime) * m_Recording.frequency), m_Recording.channels, m_Recording.frequency, false);
        float[] data = new float[(int)((Time.time - startRecordingTime) * m_Recording.frequency)];
        m_Recording.GetData(data, 0);
        recordingNew.SetData(data, 0);
        this.m_Recording = recordingNew;

        m_AudioSource.clip = m_Recording;
        m_AudioSource.Play();

    }



    public void OnPointerDown(PointerEventData eventData)
    {

        print("START_RECORDING");
        int minFreq;
        int maxFreq;
        int freq = 44100;
        Microphone.GetDeviceCaps("", out minFreq, out maxFreq);

        if (maxFreq < 44100)
            freq = maxFreq;

        m_Recording = Microphone.Start("", false, 300, 44100);
        startRecordingTime = Time.time;

    }

    

}