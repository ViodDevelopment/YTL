using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System;

public class Addword : MonoBehaviour
{

    public InputField word;
    public Image img;
    string sumSilabas;
    Sprite temp;

    public List<InputField> bloqueSilabas;
    public GameObject bloqueSilaba;

    public AudioSource audioSource;

    PalabraBD palabraBD;
    public CreateWord crateWord;
    public RemoveWord removeWord;
    public DropDownArticles articlesDet;
    public DropDownArticlesIndet articlesIndet;

    string imgLocation, audioLocation;

    private void Start()
    {
        for (int i = 0; i < bloqueSilaba.transform.childCount; i++)
        {
            Transform silaba = bloqueSilaba.transform.GetChild(i);
            if (silaba.name.Contains("LineaSilaba"))
            {
                bloqueSilabas.Add(silaba.gameObject.GetComponent<InputField>());
            }
        }

        gameObject.GetComponent<Button>().interactable = false;

    }

    Sprite MakeImgEven(Texture2D tex)
    {
        int maxSize, offset;

        if (tex.width == tex.height)
        {
            return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else if (tex.width < tex.height)
        {
            maxSize = tex.width;
            offset = (tex.height - maxSize) / 2;
            return Sprite.Create(tex, new Rect(0.0f, offset, tex.width, tex.width), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else
        {
            maxSize = tex.height;
            offset = (tex.width - maxSize) / 2;
            return Sprite.Create(tex, new Rect(offset, 0.0f, tex.height, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }


    }


    public void SaveWord()
    {
        if (GameManager.palabrasUserDisponibles.Count < 8)
        {
            sumSilabas = "";
            foreach (InputField inField in bloqueSilabas)
            {
                if (inField.text != "")
                    sumSilabas += inField.text + "-";
            }


            sumSilabas.Substring(0, sumSilabas.Length - 1);

           
            Texture2D textd = ToTexture2D(img.sprite.texture);
            

            File.WriteAllBytes(imgLocation = Application.persistentDataPath + "/UserWords/Images/img" + DateTime.Now.Year.ToString() + DateTime.Now.DayOfYear.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".png", textd.EncodeToPNG());

            FileStream file = File.Create(audioLocation = Application.persistentDataPath + "/UserWords/Sounds/audio" + DateTime.Now.Year.ToString() + DateTime.Now.DayOfYear.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".wav");

            ConvertAndWrite(file, audioSource.clip);
            WriteHeader(file, audioSource.clip);


            palabraBD = new PalabraBD();
            palabraBD.imagePuzzle = 1;
            palabraBD.image1 =
            palabraBD.image2 =
            palabraBD.image3 = imgLocation;


            palabraBD.piecesPuzzle.Add(4);
            palabraBD.id = -1;
            palabraBD.audio = audioLocation;

            if (SingletonLenguage.GetInstance().GetLenguage() == SingletonLenguage.Lenguage.CASTELLANO)
            {
                palabraBD.nameSpanish = word.text;
                palabraBD.silabasSpanish = sumSilabas;
                palabraBD.nameCatalan = "";
                palabraBD.silabasCatalan = "";
            }
            else if (SingletonLenguage.GetInstance().GetLenguage() == SingletonLenguage.Lenguage.CATALAN)
            {
                palabraBD.nameSpanish = "";
                palabraBD.silabasSpanish = "";
                palabraBD.nameCatalan = word.text;
                palabraBD.silabasCatalan = sumSilabas;
            }

            if (articlesDet != null)
                palabraBD.articulos.Add(articlesDet.GetArticuloDet());
            if(articlesIndet != null)
                palabraBD.articulos.Add(articlesIndet.GetArticuloIndet());

            palabraBD.user = true;
            palabraBD.color = "#eb6424";
            palabraBD.paquet = 0;
            palabraBD.SetPalabraActual();

            FindObjectOfType<ManagamentFalseBD>().SaveWordUser(palabraBD, true);

            Limpiar();
        }
    }

    private void Limpiar()
    {
        img.sprite = null;
        word.text = null;
        audioSource.clip = null;
        for (int i = 0; i < bloqueSilaba.transform.childCount; i++)
        {
            Transform silaba = bloqueSilaba.transform.GetChild(i);
            if (silaba.name.Contains("LineaSilaba"))
            {
                silaba.gameObject.GetComponent<InputField>().text = null;
            }
        }
        if (articlesDet != null)
            articlesDet.Unselected();
        if (articlesIndet != null)
            articlesIndet.Unselected();
    }

    public static Texture2D ToTexture2D(Texture2D texture)
    {
        /*
         * PRUEBA
         * Rect rect = new Rect(new Vector2(texture.width / 2 - 256, texture.height / 2 - 256), new Vector2(512, 512));
        Sprite l_Sprite = Sprite.Create(texture, rect, new Vector2(0, 0));
        return l_Sprite.texture;*/
        RenderTexture rendTexture = new RenderTexture(texture.width, texture.height, 0, RenderTextureFormat.ARGB32);
        Texture2D result = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false, false);

        Graphics.Blit(texture, rendTexture);
        result.ReadPixels(new Rect(0, 0, rendTexture.width, rendTexture.height), 0, 0);
        result.Apply();
        return result;

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

}
