using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonLenguage
{
    private static SingletonLenguage instance;
    public enum Lenguage { CASTELLANO, CATALAN, INGLES, FRANCES }
    private Lenguage currentLenguage;
    public enum OurFont { IMPRENTA, MAYUSCULA, MANUSCRITA }
    private OurFont currentFont;

    public static SingletonLenguage GetInstance()
    {
        if (instance == null)
        {
            instance = new SingletonLenguage();
            instance.SetLenguage(Lenguage.CASTELLANO);//cambiar en un futuro
            instance.SetFont(OurFont.MAYUSCULA);//cambiar en un futuro
        }
        return instance;
    }

    public void SetLenguage(Lenguage _leng)
    {
        if (_leng != currentLenguage)
        {
            currentLenguage = _leng;
            InitalizePalabras();
        }
    }

    public void InitalizePalabras()
    {
        foreach (PalabraBD p in GameManager.palabrasDisponibles)
        {
            p.SetPalabraActual();
        }

        foreach (FraseBD f in GameManager.frasesDisponibles)
        {
            f.SeparatePerPalabras();
        }

        foreach (PalabraBD p in GameManager.palabrasUserDisponibles)
        {
            p.SetPalabraActual();
        }
    }

    public Lenguage GetLenguage()
    {
        return currentLenguage;
    }

    public void SetFont(OurFont _font)
    {
        currentFont = _font;
    }

    public OurFont GetFont()
    {
        return currentFont;
    }

    public int ConvertSizeDependWords(string name)
    {
        int size = 0;
        int num = 0;

        foreach (char c in name)
        {
            num++;
        }
        switch (currentFont)
        {
            case OurFont.IMPRENTA:
                size = 20;
                if (num > 10)
                {
                    size = 20 - (num - 10)/2;
                }
                break;
            case OurFont.MANUSCRITA:
                size = 15;

                if (num > 15)
                {
                    size = 20 - (num - 15)/2;
                }
                break;
            case OurFont.MAYUSCULA:
                size = 7;
                if (num > 7)
                {
                    size = 20 - (num - 10)/2;
                }
                break;

        }


        return size;
    }


}
