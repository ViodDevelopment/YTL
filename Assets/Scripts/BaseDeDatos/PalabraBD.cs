using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class PalabraBD
{
    public int id;
    public string color;
    public string image1;
    public string image2;
    public string image3;
    public string audio;
    public List<int> piecesPuzzle = new List<int>();
    public int imagePuzzle;
    public int dificultSpanish;
    public string nameSpanish;
    public string silabasSpanish;
    public int dificultCatalan;
    public string nameCatalan;
    public string silabasCatalan;
    public int paquet; //Paquete 0 = determinado, 1 = escuela, paquete 2 = color, paquete 3 = animales
    public List<string> silabasActuales = new List<string>();
    public string palabraActual;

    //eliminar en un futuro y cambiarlo por el original
    //

    public void SeparateSilabas()//separa la linea de string de silabas segun el idioma a un lista en orden de las silabas.
    {
        silabasActuales.Clear();
        string actualWord = "";
        switch (SingletonLenguage.GetInstance().GetLenguage())
        {
            case SingletonLenguage.Lenguage.CASTELLANO:
                actualWord = silabasSpanish;
                break;
            case SingletonLenguage.Lenguage.CATALAN:
                actualWord = silabasCatalan;
                break;
            case SingletonLenguage.Lenguage.INGLES:
                break;
            case SingletonLenguage.Lenguage.FRANCES:
                break;
        }
        if (actualWord != "")
        {
            string currentSilaba = "";
            for (int i = 0; i < actualWord.Length; i++)
            {
                if (actualWord[i] != '-')
                {
                    currentSilaba += actualWord[i];
                }
                else
                {
                    silabasActuales.Add(currentSilaba);
                    //Debug.Log(silabasActuales[silabasActuales.Count - 1]);
                    currentSilaba = "";
                }
            }
            if (currentSilaba != "")
            {
                silabasActuales.Add(currentSilaba);
                //Debug.Log(silabasActuales[silabasActuales.Count - 1]);
                currentSilaba = "";
            }
        }
    }

    public void SetPalabraActual()
    {
        switch (SingletonLenguage.GetInstance().GetLenguage())
        {
            case SingletonLenguage.Lenguage.CASTELLANO:
                palabraActual = nameSpanish;
                break;
            case SingletonLenguage.Lenguage.CATALAN:
                palabraActual = nameCatalan;
                break;
            case SingletonLenguage.Lenguage.INGLES:
                break;
            case SingletonLenguage.Lenguage.FRANCES:
                break;
        }
    }

    public Sprite GetSprite(string _name)
    {
        return Resources.Load<Sprite>("images/Version1.0/Palabra/" + _name); //CAMBIAR RUTA DE IMAGEN CUANDO NO SEA LITE
    }

    public Texture2D GetTexture2D(string _name)
    {
        return Resources.Load<Texture2D>("images/Version1.0/Palabra/" + _name);
    }

    public AudioClip GetAudioClip(string _audio)
    {
        string completeRute = "";
        switch (SingletonLenguage.GetInstance().GetLenguage())
        {
            case SingletonLenguage.Lenguage.CASTELLANO:
                completeRute = "Audios/Castellano/Version1.0/Palabra/" + _audio + "_esp";  //CAMBIAR EN UN FUTURO LA RUTA
                break;
            case SingletonLenguage.Lenguage.CATALAN:
                completeRute = "Audios/Catalan/Version1.0/Palabra/" + _audio + "_cat"; //LOMISMO
                break;
            case SingletonLenguage.Lenguage.INGLES:
                break;
            case SingletonLenguage.Lenguage.FRANCES:
                break;
        }

        return Resources.Load<AudioClip>(completeRute);
    }

}
