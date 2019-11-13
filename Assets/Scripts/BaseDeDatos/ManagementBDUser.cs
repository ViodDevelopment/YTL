using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class ManagementBDUser : MonoBehaviour
{
    private Texture2D texture;
    public enum NumOfSearch {NONE, PALABRA, FRASE };
    private NumOfSearch currentSearch = NumOfSearch.NONE;
    public Image imagen;
    public AudioSource audioSource;
    public string ruteFolderImage;
    private string ruteFolderImageInsert;
    private string ruteFolderAudio;


    // Start is called before the first frame update
    void Awake()
    {
        ruteFolderImage = "file://" + Application.persistentDataPath + "/";
        ruteFolderImageInsert = Application.dataPath + "/com.ViOD.YoTambienLeo/files/";//cambiar la dirección cuando se tenga la definitiva

        ruteFolderAudio = "file://" + Application.dataPath + "/files/";//SI NO FUNCIONA PONER DESPUES DE DATA PATH com.ViOD.YoTambienLeo
        //InsertPalabra("fs","sdv ","avs","asv", 0);
       //ReadSQlitePalabra();
    }

    public List<PalabraFraseUsuarioBD> ReadSQlitePalabra()
    {
        string conection = "URI=file:" + Application.dataPath + "/Plugins/SQLite/BaseDeDatosYoTambienLeo.db";
        IDbConnection dbConection = (IDbConnection)new SqliteConnection(conection);
        dbConection.Open();
        IDbCommand dbcommand = dbConection.CreateCommand();
        string sqlQuery = SearchInBDUsuario("Usuario");
        //string sqlQuery = "SELECT id, nombre, imagen, imagen2 FROM Contenido";  //en el from tiene que poner el nombre de la tabla y antes lo que se tiene que seleccionar en sql. Al buscar algun string hay que ponerle las comillas estas '  ' osino no lo reconoce
        dbcommand.CommandText = sqlQuery;
        IDataReader reader = dbcommand.ExecuteReader();

        List<PalabraFraseUsuarioBD> currentPalabrasBD = new List<PalabraFraseUsuarioBD>();
        while (reader.Read())
        {
            currentPalabrasBD.Add(new PalabraFraseUsuarioBD());
            currentPalabrasBD[currentPalabrasBD.Count - 1].id = reader.GetInt32(0);
            currentPalabrasBD[currentPalabrasBD.Count - 1].palabra = reader.GetString(1);
            currentPalabrasBD[currentPalabrasBD.Count - 1].silabas = reader.GetString(2);
            currentPalabrasBD[currentPalabrasBD.Count - 1].image = reader.GetString(3);
            currentPalabrasBD[currentPalabrasBD.Count - 1].sound = reader.GetString(4);
            currentPalabrasBD[currentPalabrasBD.Count - 1].frase = reader.GetInt32(5);
            // Debug.Log("Id = " + id + "  Nombre 1 =" + nombre1 + "  imagen 1 =" + imagen1 + " imagen 2 =" + imagen2);

        }
        if (currentPalabrasBD.Count > 0)
        {
            foreach (PalabraFraseUsuarioBD f in currentPalabrasBD)
            {
                f.SeparateSilabas();
            }
        }
        else Debug.Log("No existe");


        reader.Close();
        reader = null;

        dbcommand.Dispose();
        dbcommand = null;

        dbConection.Close();
        dbConection = null;

        return currentPalabrasBD;
    }
    private string SearchInBDUsuario(string _table)
    {
        string mySQL = "";
        switch (currentSearch)
        {
            case NumOfSearch.NONE:
                mySQL = ("SELECT* FROM " + _table);
                break;
            case NumOfSearch.PALABRA:
                mySQL = ("SELECT* FROM " + _table + "WHERE frase = 0");
                break;
            case NumOfSearch.FRASE:
                mySQL = ("SELECT* FROM " + _table + "WHERE frase = 1");
                break;
        }
       
        return mySQL;
    }

    #region Busquedas

    public void ChangeSearchToPalabra()
    {
        currentSearch = NumOfSearch.PALABRA;
    }
    public void ChangeSearchToFrase()
    {
        currentSearch = NumOfSearch.FRASE;
    }
    public void ChangeSearchToNone()
    {
        currentSearch = NumOfSearch.NONE;
    }

    #endregion

    #region Inserts

    public void InsertPalabra(string _palabra, string _silabas, string _image, string _audio, int _frase, ref Text _text)//quitar el texto
    {
        _text.text = "entra0";
        string conection = "URI=file:" + Application.dataPath + "/Plugins/SQLite/BaseDeDatosYoTambienLeo.db";
        IDbConnection dbConection = (IDbConnection)new SqliteConnection(conection);
        dbConection.Open();
        IDbCommand dbcommand = dbConection.CreateCommand();
        _text.text = "entra1";
        string sqlQuery = "INSERT INTO Usuario (nombre, silabas, imagen, audio, frase) VALUES ('" + _palabra + "', '" + _silabas + "', '" + _image + "', '" + _audio + "', '" + _frase + "')";
        _text.text = "entra2";
        //string sqlQuery = "SELECT id, nombre, imagen, imagen2 FROM Contenido";  //en el from tiene que poner el nombre de la tabla y antes lo que se tiene que seleccionar en sql. Al buscar algun string hay que ponerle las comillas estas '  ' osino no lo reconoce
        dbcommand.CommandText = sqlQuery;
        dbcommand.ExecuteNonQuery();

        dbcommand.Dispose();
        dbcommand = null;

        dbConection.Close();
        dbConection = null;
        _text.text = "entra3";

    }

    #endregion

    #region Encontrar archivos
    public void SearchSpriteInRuteFolders(string _rute, Image _image)
    {
        imagen = _image;
        string completeRute = ruteFolderImage + _rute;
        imagen.color = Color.white;
        StartCoroutine(ConvertURLToTexture(completeRute));
    }


    //PARA PASAR DE UNA IMAGEN WEB A UN SPRITE, LLAMANDO CON UNA CORUTINE A ESTO

    IEnumerator ConvertURLToTexture(string _rute)
    {
        WWW www = new WWW(_rute); //Cargando la imagen
        yield return www;

        texture = www.texture; //una vez cargada 
        if(texture != null)
            PassTexture2DToSprite();
    }

    private void PassTexture2DToSprite()
    {
        Rect rect = new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height));
        imagen.sprite = Sprite.Create(texture, rect, Vector2.down);
    }

    public void SearchAudioClip(string _audio, AudioSource _audioSource)
    {
        audioSource = _audioSource;
        string completeRute = ruteFolderAudio + _audio;

        WWW www = new WWW(completeRute);
        StartCoroutine(LoadAudio(www));
    }

    private IEnumerator LoadAudio(WWW _www)
    {
        WWW request = _www;
        yield return request;

        AudioClip audio = request.GetAudioClip();
        audioSource.clip = audio;
        audioSource.Play();
    }
    #endregion
}
