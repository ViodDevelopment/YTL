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
    public bool user = false;
    public List<string> articulosSpanish = new List<string>();
    public List<string> articulosCatalan = new List<string>();
    public List<string> audiosArticulosSpanish = new List<string>();
    public List<string> audiosArticulosCatalan = new List<string>();
    public string actualArticulo;
    public string actualAudioArticulo;

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

        SetActualArticulo();
    }

    public Sprite GetSprite(string _name)
    {
        if (!user)
            return Resources.Load<Sprite>("images/Version1.0/Palabra/" + _name); //CAMBIAR RUTA DE IMAGEN CUANDO NO SEA LITE
        else
            return GetSpriteUserWord(_name);

    }

    public Texture2D GetTexture2D(string _name)
    {
        if (!user)
            return Resources.Load<Texture2D>("images/Version1.0/Palabra/" + _name);
        else
            return GetTexture2DUserWord(_name);
    }

    public AudioClip GetAudioClip(string _audio)
    {
        if (!user)
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
        else
            return GetAudioClipUserWord();
    }

    public Sprite GetSpriteUserWord(string _name)
    {
        WWW www = new WWW("file://" + _name); //Cargando la imagen

        while (!www.isDone)
        {
            Debug.Log("Cargando imagen");
        }
        Texture2D texture = www.texture; //una vez cargada 

        Rect rect = new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height));
        return Sprite.Create(texture, rect, Vector2.down);

    }

    public Texture2D GetTexture2DUserWord(string _name)
    {
        WWW www = new WWW("file://" + _name); //Cargando la imagen

        while (!www.isDone)
        {
            Debug.Log("Cargando Texture");
        }
        Texture2D texture = www.texture;
        return texture; //una vez cargada    
    }

    public AudioClip GetAudioClipUserWord()
    {
        WWW www = new WWW("file://" + audio);


        while (!www.isDone)
        {
            Debug.Log("downloading");
        }

        return www.GetAudioClip();
    }

    private void SetActualArticulo()
    {
        if(GameManager.configuration.palabrasConArticulo)
        {
            switch (SingletonLenguage.GetInstance().GetLenguage())
            {
                case SingletonLenguage.Lenguage.CASTELLANO:
                    if (articulosSpanish != null)//no enrta si los usuarios tienen palabras ya creadas, tienen que crearlas de nuevo
                    {
                        if (GameManager.configuration.determinados && articulosSpanish.Count > 0)
                        {
                            actualArticulo = articulosSpanish[0];
                            if (audiosArticulosSpanish.Count > 0)
                                actualAudioArticulo = audiosArticulosSpanish[0];
                        }
                        else if (!GameManager.configuration.determinados && articulosSpanish.Count > 1)
                        {
                            actualArticulo = articulosSpanish[1];
                            if (audiosArticulosSpanish.Count > 1)
                                actualAudioArticulo = audiosArticulosSpanish[1];
                        }
                    }
                   

                    break;
                case SingletonLenguage.Lenguage.CATALAN:
                    if (articulosCatalan != null)
                    {
                        if (GameManager.configuration.determinados && articulosCatalan.Count > 0)
                        {
                            actualArticulo = articulosCatalan[0];
                            if (audiosArticulosCatalan.Count > 0)
                                actualAudioArticulo = audiosArticulosCatalan[0];
                        }
                        else if (!GameManager.configuration.determinados && articulosCatalan.Count > 1)
                        {
                            actualArticulo = articulosCatalan[1];
                            if (audiosArticulosCatalan.Count > 1)
                                actualAudioArticulo = audiosArticulosCatalan[1];
                        }
                    }
                    break;
                case SingletonLenguage.Lenguage.INGLES:
                    break;
                case SingletonLenguage.Lenguage.FRANCES:
                    break;
            }
        }
    }

    public AudioClip GetAudioArticulo()
    {
        string completeRute = "";
        switch (SingletonLenguage.GetInstance().GetLenguage())
        {
            case SingletonLenguage.Lenguage.CASTELLANO:
                completeRute = "Audios/Castellano/Version1.0/Art/" + actualAudioArticulo + "_esp";  //CAMBIAR EN UN FUTURO LA RUTA
                break;
            case SingletonLenguage.Lenguage.CATALAN:
                completeRute = "Audios/Catalan/Version1.0/Art/" + actualAudioArticulo + "_cat"; //LOMISMO
                break;
            case SingletonLenguage.Lenguage.INGLES:
                break;
            case SingletonLenguage.Lenguage.FRANCES:
                break;
        }

        return Resources.Load<AudioClip>(completeRute);
    }

}
