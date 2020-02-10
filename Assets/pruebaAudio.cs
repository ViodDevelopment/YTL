using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pruebaAudio : MonoBehaviour
{
    private void Start()
    {
        Cargar();
        gameObject.GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
    }

    private void Cargar()
    {
        WWW www = new WWW("file://" + Application.persistentDataPath + "/UserWords/Sounds/" + "eres_el_mejor_dumi_esp.ogg");


        while (!www.isDone)
        {
            print("Processing File" + Application.persistentDataPath + "/UserWords/Sounds/" + "eres_el_mejor_dumi_esp.ogg");
        }

        gameObject.GetComponent<AudioSource>().clip = www.GetAudioClip();
    }
}
