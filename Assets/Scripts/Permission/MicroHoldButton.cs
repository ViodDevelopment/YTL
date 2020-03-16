using System.Collections;
using System.IO;
using UnityEngine;
using System;
using UnityEngine.EventSystems;


public class MicroHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    AudioClip m_Recording;
    AudioSource m_AudioSource;
    private float startRecordingTime;
    private bool apretado = false;
    public AudioClip sonidoDefault;

    void Start()
    {
        Microphone.GetDeviceCaps("", out minFreq, out maxFreq);
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void AceptarPalabra()
    {
        //FileStream file = File.Create(Application.persistentDataPath + "/UserWords/Sounds/audio" + DateTime.Now.Year.ToString() + DateTime.Now.DayOfYear.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".wav");

        //ConvertAndWrite(file, m_Recording);
        //WriteHeader(file, m_Recording);
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

        AceptarPalabra();//Quitar y pone cuando se acepte palabra
        m_AudioSource.clip = m_Recording;
        apretado = false;
    }



    public IEnumerator OnPointerDown()
    {
        float l_timer = 0;
        if (!apretado)
        {
            apretado = true;
            m_AudioSource.clip = sonidoDefault;
            m_AudioSource.Play();
            l_timer = m_AudioSource.clip.length + 0.4f;
        }



        yield return new WaitForSeconds(l_timer);
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

    static void ConvertAndWrite(FileStream fileStream, AudioClip clip)
    {



        var samples = new float[clip.samples];



        clip.GetData(samples, 0);



        Int16[] intData = new Int16[samples.Length];

        //converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]



        Byte[] bytesData = new Byte[samples.Length * 2];

        //bytesData array is twice the size of

        //dataSource array because a float converted in Int16 is 2 bytes.



        int rescaleFactor = 32767; //to convert float to Int16



        for (int i = 0; i < samples.Length; i++)
        {

            intData[i] = (short)(samples[i] * rescaleFactor);

            Byte[] byteArr = new Byte[2];

            byteArr = BitConverter.GetBytes(intData[i]);

            byteArr.CopyTo(bytesData, i * 2);

        }



        fileStream.Write(bytesData, 0, bytesData.Length);

    }

    static void WriteHeader(FileStream fileStream, AudioClip clip)
    {



        var hz = clip.frequency;

        var channels = clip.channels;

        var samples = clip.samples;



        fileStream.Seek(0, SeekOrigin.Begin);



        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");

        fileStream.Write(riff, 0, 4);



        Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);

        fileStream.Write(chunkSize, 0, 4);



        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");

        fileStream.Write(wave, 0, 4);



        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");

        fileStream.Write(fmt, 0, 4);



        Byte[] subChunk1 = BitConverter.GetBytes(16);

        fileStream.Write(subChunk1, 0, 4);



        UInt16 two = 2;

        UInt16 one = 1;



        Byte[] audioFormat = BitConverter.GetBytes(one);

        fileStream.Write(audioFormat, 0, 2);



        Byte[] numChannels = BitConverter.GetBytes(channels);

        fileStream.Write(numChannels, 0, 2);



        Byte[] sampleRate = BitConverter.GetBytes(hz);

        fileStream.Write(sampleRate, 0, 4);



        Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2

        fileStream.Write(byteRate, 0, 4);



        UInt16 blockAlign = (ushort)(channels * 2);

        fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);



        UInt16 bps = 16;

        Byte[] bitsPerSample = BitConverter.GetBytes(bps);

        fileStream.Write(bitsPerSample, 0, 2);



        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");

        fileStream.Write(datastring, 0, 4);



        Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);

        fileStream.Write(subChunk2, 0, 4);



        fileStream.Close();

    }

    void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(OnPointerDown());
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(OnPointerDown());
    }
}