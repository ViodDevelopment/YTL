using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class FraseBD
{
    public int id;
    public string actualFrase;
    public string fraseCastellano;
    public string fraseCatalan;
    public string fraseIngles;
    public string frasesFrances;
    public string image;
    public string sound;
    public int dificultad;
    public List<PalabraBD> palabras = new List<PalabraBD>();

    public void SeparatePerPalabras()
    {
        SetPalabraLenguaje();
        palabras.Clear();
        string palabra = "";
        List<string> palabrasABuscar = new List<string>();
        foreach (char c in actualFrase)
        {
            if (c != ' ')
            {
                palabra += c;
            }
            else
            {
                palabrasABuscar.Add(palabra);
                palabra = "";
            }
        }

        if (palabra != "")
        {
            palabrasABuscar.Add(palabra);
            palabra = "";
        }

        bool existe = false;
        foreach (string p in palabrasABuscar)
        {
            existe = false;
            foreach (PalabraBD pal in GameManager.palabrasDisponibles)
            {
                if (p.ToLower() == pal.palabraActual.ToLower())
                {
                    existe = true;
                    palabras.Add(pal);
                    break;
                }
            }
            if (!existe)
            {
                Debug.LogError("Esa palabra de la frase no existe: " + p);
            }
        }
        if (palabras.Count <= 4)
            dificultad = 0;
        else
            dificultad = 1;

    }

    private void SetPalabraLenguaje()
    {
        switch (SingletonLenguage.GetInstance().GetLenguage())
        {
            case SingletonLenguage.Lenguage.CASTELLANO:
                actualFrase = fraseCastellano;
                break;
            case SingletonLenguage.Lenguage.CATALAN:
                actualFrase = fraseCatalan;
                break;
            case SingletonLenguage.Lenguage.INGLES:
                actualFrase = fraseIngles;
                break;
            case SingletonLenguage.Lenguage.FRANCES:
                actualFrase = frasesFrances;
                break;
        }
    }

    public AudioClip GetAudioClip(string _audio)
    {
        string completeRute = "";
        switch (SingletonLenguage.GetInstance().GetLenguage())
        {
            case SingletonLenguage.Lenguage.CASTELLANO:
                completeRute = "Audios/Castellano/Version1.0/Frase/" + _audio + "_esp";  //CAMBIAR EN UN FUTURO LA RUTA
                break;
            case SingletonLenguage.Lenguage.CATALAN:
                completeRute = "Audios/Catalan/Version1.0/Frase/" + _audio + "_cat"; //LOMISMO
                break;
            case SingletonLenguage.Lenguage.INGLES:
                break;
            case SingletonLenguage.Lenguage.FRANCES:
                break;
        }

        return Resources.Load<AudioClip>(completeRute);
    }
}
