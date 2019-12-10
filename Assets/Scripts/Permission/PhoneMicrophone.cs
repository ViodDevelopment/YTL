using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhoneMicrophone : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {

        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }

        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            audioSource.clip = Microphone.Start("Varios micrófonos (Realtek High Definition Audio)", true, 10, 44100);
            audioSource.Play();
        }


        if (Input.GetKeyUp(KeyCode.M))
        {
            Microphone.End("Varios micrófonos (Realtek High Definition Audio)");
        }

    }

}