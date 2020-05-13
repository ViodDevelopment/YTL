using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ManagamentFalseBD : MonoBehaviour
{

    public static ManagamentFalseBD management;

    [SerializeField] private List<PalabraBD> palabrasPredeterminadass = new List<PalabraBD>();
    private List<PalabraBD> palabrasUserGuardadas = new List<PalabraBD>();
    [SerializeField] private List<FraseBD> frasesPredeterminadas = new List<FraseBD>();
    private string nameRute, nameRuteFrase, nameRuteBolasMinijuegos, nameConfiguration, nameRutePassword, nameRuteUser, nameRuteUserFrase;
    private void Awake()
    {
        GameObject go = GameObject.Find("ManagementFalsaBD");
        if (go == null || go == gameObject)
        {
            if (management == null)
            {
                nameRute = Application.persistentDataPath + "/datos.dat";
                nameRuteFrase = Application.persistentDataPath + "/datosFrases.dat";
                nameRuteBolasMinijuegos = Application.persistentDataPath + "/BolasMinijuegos.dat";
                nameConfiguration = Application.persistentDataPath + "/Configuration.dat";
                nameRutePassword = Application.persistentDataPath + "/password.dat";
                management = this;
                ComprobarCarpetaUsuario("UserWords");
                nameRuteUser = Application.persistentDataPath + "/UserWords/datosUsuario.dat";
                nameRuteUserFrase = Application.persistentDataPath + "/UserWords/datosFrasesUsuario.dat";
                DontDestroyOnLoad(gameObject);

                if (File.Exists(nameConfiguration))
                {
                    management.LoadConfig();
                }
                else if (!File.Exists(nameConfiguration))
                {
                    if (GameManager.configuration == null)
                        GameManager.configuration = new Configuration();

                    management.SaveConfig();
                    management.LoadConfig();
                }

                if (File.Exists(Application.streamingAssetsPath + "/Update.dat"))
                {
                    File.Delete(Application.streamingAssetsPath + "/Update.dat");
                    GameManager.actualizacion = true;
                }
                StartCoroutine(CopyPalabrasBinaryToPersistentPath("PalabrasBinario.dat"));

                if (File.Exists(nameRuteBolasMinijuegos))
                {
                    management.LoadBolasMinijuegos();
                }
                else if (!File.Exists(nameRuteBolasMinijuegos))
                {
                    management.SaveBolasMinijuegos();
                    management.LoadBolasMinijuegos();
                }

                if (File.Exists(nameRutePassword))
                {
                    management.LoadPassword();
                }
                else if (!File.Exists(nameRutePassword))
                {
                    management.SavePassword();
                    management.LoadPassword();
                }

                if (!Directory.Exists(Path.GetDirectoryName(Application.persistentDataPath + "/UserWords/datosUsuario")))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Application.persistentDataPath + "/UserWords/datosUsuario"));
                }

                if (File.Exists(nameRuteUser))
                {
                    management.LoadDatesOfPlayer();
                }
                else if (!File.Exists(nameRuteUser))
                {
                    management.SaveWordUser(null, false);
                    management.LoadDatesOfPlayer();
                }
            }
        }
        else if (go != gameObject)
            Destroy(gameObject);
    }



    private void InitPalabrasPredeterminadas()
    {

        if (File.Exists(nameRute) && !GameManager.actualizacion)
        {
            management.LoadDates();
        }
        if (!File.Exists(nameRute) || GameManager.actualizacion)
        {
            management.SaveDates();
            management.LoadDates();
        }

        if (GameManager.actualizacion)
        {
            ActualizarPaquetesAlCompleto();
            Debug.LogWarning("cambiar esto cuando esté todo hecho");
            StartCoroutine(CopyFrasesBinaryToPersistentPath("FrasesBinario.dat"));

            GameManager.actualizacion = false;
        }
        else
            StartCoroutine(CopyFrasesBinaryToPersistentPath("FrasesBinario.dat"));



    }

    IEnumerator CopyPalabrasBinaryToPersistentPath(string fileName)
    {
        if (!File.Exists(nameRute) || GameManager.actualizacion)
        {
            //Where to copy the db to
            string dbDestination = Path.Combine(Application.persistentDataPath, "palabrasPredeterminadas.dat");

            //Check if the File do not exist then copy it

            //Where the db file is at
            string dbStreamingAsset = Path.Combine(Application.streamingAssetsPath, fileName);

            byte[] result;

            //Read the File from streamingAssets. Use WWW for Android
            if (dbStreamingAsset.Contains("://") || dbStreamingAsset.Contains(":///"))
            {
                WWW www = new WWW(dbStreamingAsset);
                yield return www;
                result = www.bytes;
            }
            else
            {
                result = File.ReadAllBytes(dbStreamingAsset);
            }

            //Create Directory if it does not exist
            if (!Directory.Exists(Path.GetDirectoryName(dbDestination)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dbDestination));
            }

            //Copy the data to the persistentDataPath where the database API can freely access the file
            File.WriteAllBytes(dbDestination, result);
            if (File.Exists(Path.Combine(Application.persistentDataPath, "palabrasPredeterminadas.dat")))
            {
                List<PalabraBD> palabras = new List<PalabraBD>();
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Path.Combine(Application.persistentDataPath, "palabrasPredeterminadas.dat"), FileMode.Open);

                DatesToSave datos = (DatesToSave)bf.Deserialize(file);
                palabras = datos.GetListOfPalabras();

                file.Close();


                palabrasPredeterminadass.Clear();
                foreach (PalabraBD p in palabras)
                {
                    palabrasPredeterminadass.Add(p);
                }

            }


            foreach (PalabraBD p in palabrasPredeterminadass)
            {
                p.SeparateSilabas();
                p.SetPalabraActual();
            }
        }
        InitPalabrasPredeterminadas();
    }

    IEnumerator CopyFrasesBinaryToPersistentPath(string fileName)
    {
        if (!File.Exists(nameRuteFrase) || GameManager.actualizacion)
        {
            //Where to copy the db to
            string dbDestination = Path.Combine(Application.persistentDataPath, "frasesPredeterminadas.dat");

            //Check if the File do not exist then copy it

            //Where the db file is at
            string dbStreamingAsset = Path.Combine(Application.streamingAssetsPath, fileName);

            byte[] result;

            //Read the File from streamingAssets. Use WWW for Android
            if (dbStreamingAsset.Contains("://") || dbStreamingAsset.Contains(":///"))
            {
                WWW www = new WWW(dbStreamingAsset);
                yield return www;
                result = www.bytes;
            }
            else
            {
                result = File.ReadAllBytes(dbStreamingAsset);
            }

            //Create Directory if it does not exist
            if (!Directory.Exists(Path.GetDirectoryName(dbDestination)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dbDestination));
            }

            //Copy the data to the persistentDataPath where the database API can freely access the file
            File.WriteAllBytes(dbDestination, result);
            if (File.Exists(Path.Combine(Application.persistentDataPath, "frasesPredeterminadas.dat")))
            {
                List<FraseBD> frases = new List<FraseBD>();
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Path.Combine(Application.persistentDataPath, "frasesPredeterminadas.dat"), FileMode.Open);

                DatesFrasesToSave datos = (DatesFrasesToSave)bf.Deserialize(file);
                frases = datos.GetListOfFrases();

                file.Close();


                frasesPredeterminadas.Clear();
                foreach (FraseBD p in frases)
                {
                    frasesPredeterminadas.Add(p);
                }

            }


            foreach (FraseBD p in frasesPredeterminadas)
            {
                p.SeparatePerPalabras();
            }
        }

        InitFrasesPredeterminadas();
    }



    private void InitFrasesPredeterminadas()
    {

        if (File.Exists(nameRuteFrase) && !GameManager.actualizacion)
        {
            management.LoadDatesFrase();


        }
        if (!File.Exists(nameRuteFrase) || GameManager.actualizacion)
        {
            management.SaveDatesFrase();
            management.LoadDatesFrase();
        }
    }

    public void SaveDates()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(nameRute);

        DatesToSave datos = new DatesToSave();
        datos.ChangeDates(palabrasPredeterminadass);

        bf.Serialize(file, datos);

        file.Close();
    }

    public void LoadDates()
    {
        List<PalabraBD> palabras = new List<PalabraBD>();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(nameRute, FileMode.Open);

        DatesToSave datos = (DatesToSave)bf.Deserialize(file);
        palabras = datos.GetListOfPalabras();

        file.Close();


        GameManager.palabrasDisponibles.Clear();
        palabrasPredeterminadass.Clear();
        foreach (PalabraBD p in palabras)
        {
            p.SeparateSilabas();
            p.SetPalabraActual();
            palabrasPredeterminadass.Add(p);
            GameManager.palabrasDisponibles.Add(p);
        }

    }

    public void SaveDatesFrase()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileFrase = File.Create(nameRuteFrase);

        DatesFrasesToSave datos = new DatesFrasesToSave();
        datos.ChangeDates(frasesPredeterminadas);

        bf.Serialize(fileFrase, datos);

        fileFrase.Close();
    }

    public void LoadDatesFrase()
    {
        List<FraseBD> frases = new List<FraseBD>();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileFrase = File.Open(nameRuteFrase, FileMode.Open);

        DatesFrasesToSave datos = (DatesFrasesToSave)bf.Deserialize(fileFrase);
        frases = datos.GetListOfFrases();

        fileFrase.Close();


        GameManager.frasesDisponibles.Clear();
        foreach (FraseBD p in frases)
        {
            p.SeparatePerPalabras();
            GameManager.frasesDisponibles.Add(p);
        }
    }

    public void SaveBolasMinijuegos()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(nameRuteBolasMinijuegos);

        PointsOfMinigames datos = new PointsOfMinigames();
        datos.bolasMinijuegos.Clear();
        for (int i = 0; i < GameManager.m_CurrentToMinigame.Count; i++)
            datos.bolasMinijuegos.Add(GameManager.m_CurrentToMinigame[i]);

        datos.currentMiniGame = GameManager.currentMiniGame;

        bf.Serialize(file, datos);

        file.Close();
    }

    public void LoadBolasMinijuegos()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(nameRuteBolasMinijuegos, FileMode.Open);

        PointsOfMinigames datos = (PointsOfMinigames)bf.Deserialize(file);
        for (int i = 0; i < GameManager.m_CurrentToMinigame.Count; i++)
        {
            if (datos.bolasMinijuegos.Count - 1 < i)
                GameManager.m_CurrentToMinigame[i] = 0;
            else
                GameManager.m_CurrentToMinigame[i] = datos.bolasMinijuegos[i];
        }

        GameManager.currentMiniGame = datos.currentMiniGame;

        file.Close();
    }

    public void SavePassword()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (!File.Exists(nameRutePassword))
            file = File.Create(nameRutePassword);
        else
        {
            file = File.Open(nameRutePassword, FileMode.Open);

        }
        string password = "";//get the password
        bf.Serialize(file, password);
        file.Close();
    }

    public void LoadPassword()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(nameRutePassword, FileMode.Open);
        try
        {
            string password = (string)bf.Deserialize(file);
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
        file.Close();
    }


    public void SaveConfig()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(nameConfiguration);

        PlayerConfiguration datos = new PlayerConfiguration();

        datos.config = GameManager.configuration;

        bf.Serialize(file, datos);

        file.Close();
    }

    public void LoadConfig()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(nameConfiguration, FileMode.Open);

        PlayerConfiguration datos = (PlayerConfiguration)bf.Deserialize(file);

        GameManager.configuration = datos.config;

        file.Close();

        GameManager.GetInstance().ChangeConfig();
    }

    public void SaveWordUser(PalabraBD _pal, bool _add)
    {
        if (_pal != null)
        {
            if (_add)
                palabrasUserGuardadas.Add(_pal);
            else
            {
                if (File.Exists(_pal.audio))
                    File.Delete(_pal.audio);
                if (File.Exists(_pal.image1))
                    File.Delete(_pal.image1);
                palabrasUserGuardadas.Remove(_pal);
            }
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(nameRuteUser);

        DatesOfPlayer datos = new DatesOfPlayer();

        datos.palabrasUser = palabrasUserGuardadas;

        bf.Serialize(file, datos);

        file.Close();
        foreach (PalabraBD item in palabrasUserGuardadas)
        {
            item.SeparateSilabas();
            item.SetPalabraActual();
        }

        GameManager.palabrasUserDisponibles = palabrasUserGuardadas;
    }

    public void LoadDatesOfPlayer()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(nameRuteUser, FileMode.Open);

        DatesOfPlayer datos = (DatesOfPlayer)bf.Deserialize(file);

        palabrasUserGuardadas = datos.palabrasUser;

        file.Close();

        foreach (PalabraBD item in palabrasUserGuardadas)
        {
            item.SeparateSilabas();
            item.SetPalabraActual();
        }

        GameManager.palabrasUserDisponibles = palabrasUserGuardadas;
        GameManager.GetInstance().ChangeConfig();
    }

    private void ComprobarCarpetaUsuario(string fileName)
    {
        //Where to copy the db to
        string destination = Path.Combine(Application.persistentDataPath, fileName);

        //Create Directory if it does not exist
        if (!Directory.Exists(destination))
        {
            Directory.CreateDirectory(destination);
        }


        string otherDestination = destination;
        otherDestination = Path.Combine(destination, "Images");

        if (!Directory.Exists(otherDestination))
        {
            Directory.CreateDirectory(otherDestination);
        }


        otherDestination = destination;
        otherDestination = Path.Combine(destination, "Sounds");

        if (!Directory.Exists(otherDestination))
        {
            Directory.CreateDirectory(otherDestination);
        }

    }

    private void ActualizarPaquetesAlCompleto()
    {
        PaquetePalabrasParejas.GetInstance("1").ReiniciarPaquetes();//da igual el numero que se le ponga
        PaqueteBit.GetInstance().ReiniciarPaquetes();
        PaquetePuzzle.GetInstance("1").ReiniciarPaquetes();//da igual el numero que se le ponga
    }
}



[Serializable]
public class DatesToSave
{
    private List<PalabraBD> palabrasPredeterminadas = new List<PalabraBD>();

    public void ChangeDates(List<PalabraBD> _palabras)
    {
        palabrasPredeterminadas.Clear();
        foreach (PalabraBD p in _palabras)
        {
            palabrasPredeterminadas.Add(p);
        }
    }

    public void AddPalabras(List<PalabraBD> _palabras)
    {
        foreach (PalabraBD p in _palabras)
        {
            palabrasPredeterminadas.Add(p);
        }
    }

    public List<PalabraBD> GetListOfPalabras()
    {
        return palabrasPredeterminadas;
    }

}

[Serializable]
class DatesFrasesToSave
{
    private List<FraseBD> frasesPredeterminadas = new List<FraseBD>();

    public void ChangeDates(List<FraseBD> _frases)
    {
        frasesPredeterminadas.Clear();
        foreach (FraseBD f in _frases)
        {
            frasesPredeterminadas.Add(f);
        }
    }

    public void AddFrases(List<FraseBD> _frases)
    {
        foreach (FraseBD f in _frases)
        {
            frasesPredeterminadas.Add(f);
        }
    }

    public List<FraseBD> GetListOfFrases()
    {
        return frasesPredeterminadas;
    }
}

[Serializable]
class DatesOfPlayer
{
    public List<PalabraBD> palabrasUser = new List<PalabraBD>();
}


[Serializable]
class FrasesUsers
{
    private List<FraseBD> frasesUser = new List<FraseBD>();

    public void ChangeDates(List<FraseBD> _frases)
    {
        frasesUser.Clear();
        foreach (FraseBD f in _frases)
        {
            frasesUser.Add(f);
        }
    }

    public void AddFrases(List<FraseBD> _frases)
    {
        foreach (FraseBD f in _frases)
        {
            frasesUser.Add(f);
        }
    }

    public List<FraseBD> GetListOfFrases()
    {
        return frasesUser;
    }
}

[Serializable]
class PointsOfMinigames
{
    public List<int> bolasMinijuegos = new List<int>();
    public int currentMiniGame;
}

[Serializable]
class PlayerConfiguration
{
    public Configuration config;
}