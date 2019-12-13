using System.Collections;
using UnityEngine;
using System.IO;

public class SingletonLoadThing : MonoBehaviour
{
    private static SingletonLoadThing instance;
    private AudioClip audioClip;

    public static SingletonLoadThing GetInstance()
    {
        if (instance == null)
        {
            instance = new SingletonLoadThing();
        }
        return instance;
    }

    public Texture2D GetTexture2D(string _path)
    {
        if (string.IsNullOrEmpty(_path))
            Debug.Log("no hay path");


        Texture2D texture = new Texture2D(512, 512, TextureFormat.RGB24, false);

        if (File.Exists(_path))
        {
            byte[] bytes = File.ReadAllBytes(_path);
            texture.filterMode = FilterMode.Trilinear;
            texture.LoadImage(bytes);
        }

        return texture;
    }

    public Sprite GetSprite(string _path)
    {
        if (string.IsNullOrEmpty(_path))
            Debug.Log("no hay path");


        Sprite sprite = null;

        if (File.Exists(_path))
        {
            //POSIBLIDAD DE CAMBIAR
            sprite = Sprite.Create(SingletonLoadThing.GetInstance().GetTexture2D(_path), new Rect(0, 0, 8, 8), new Vector2(0.5f, 0.0f), 1.0f);
        }


        return sprite;
    }

    public AudioClip GetAudio(string _path)
    {
        audioClip = null;

        if (string.IsNullOrEmpty(_path))
            Debug.Log("no hay path");

        if (File.Exists(_path))
        {
            WWW www = new WWW(_path);
            float num = 0;
            while (!www.isDone && num < 5)
            {
                num = Time.timeSinceLevelLoad;
                LoadAudio(www);
            }
            audioClip = www.GetAudioClip();
        }

        return audioClip;
    }

    public IEnumerator LoadAudio(WWW _www)
    {
        WWW request = _www;
        yield return request;
        
        AudioClip audio = request.GetAudioClip();
        audioClip = audio;
    }




}
