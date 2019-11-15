using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dumi : MonoBehaviour
{
    public List<AudioClip> audiosPositivosEsp;
    public List<AudioClip> audiosNegativosEsp;
    public List<AudioClip> audiosPositivosCat;
    public List<AudioClip> audiosNegativosCat;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if (audioSource.clip != null && !audioSource.isPlaying)
            Destroy(gameObject);
    }

    public void AudioPositivo()
    {
        switch(SingletonLenguage.GetInstance().GetLenguage())
        {
            case SingletonLenguage.Lenguage.CASTELLANO:
                int esp = Random.Range(0, audiosPositivosEsp.Count);

                audioSource.clip = audiosPositivosEsp[esp];
                audioSource.Play();
                break;
            case SingletonLenguage.Lenguage.CATALAN:
                int cat = Random.Range(0, audiosPositivosCat.Count);

                audioSource.clip = audiosPositivosCat[cat];
                audioSource.Play();
                break;
        }
    }

    public void AudioNegativo()
    {
        switch (SingletonLenguage.GetInstance().GetLenguage())
        {
            case SingletonLenguage.Lenguage.CASTELLANO:
                int esp = Random.Range(0, audiosNegativosEsp.Count);

                audioSource.clip = audiosNegativosEsp[esp];
                audioSource.Play();
                break;
            case SingletonLenguage.Lenguage.CATALAN:
                int cat = Random.Range(0, audiosNegativosCat.Count);

                audioSource.clip = audiosNegativosCat[cat];
                audioSource.Play();
                break;
        }
    }

}
