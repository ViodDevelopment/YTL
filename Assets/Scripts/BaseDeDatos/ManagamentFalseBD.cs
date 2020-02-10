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
    private List<FraseBD> frasesGuardadas = new List<FraseBD>();
    private List<FraseBD> frasesUserGuardadas = new List<FraseBD>();
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
                    if (GameManager.configurartion == null)
                        GameManager.configurartion = new Configurartion();

                    management.SaveConfig();
                    management.LoadConfig();
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
        bool existente = true;
        if (File.Exists(nameRute))
        {
            management.LoadDates();
            #region si no tiene los datos minimos se los creamos
            List<bool> existe = new List<bool>();
            if (palabrasPredeterminadass.Count == GameManager.palabrasDisponibles.Count)
            {
                foreach (PalabraBD p in palabrasPredeterminadass)
                {
                    existe.Add(false);
                    foreach (PalabraBD w in GameManager.palabrasDisponibles)
                    {
                        if (p.palabraActual == w.palabraActual && p.color == w.color && p.image1 == w.image1)
                        {
                            existe[existe.Count - 1] = true;
                            break;
                        }
                    }
                }


                foreach (bool b in existe)
                {
                    if (!b)
                    {
                        existente = false;
                        break;
                    }
                }
            }
            else
                existente = false;
            #endregion
        }
        if (!File.Exists(nameRute) || !existente)
        {
            management.SaveDates();
            management.LoadDates();
        }

        StartCoroutine(CopyFrasesBinaryToPersistentPath("FrasesBinario.dat"));
    }

    IEnumerator CopyPalabrasBinaryToPersistentPath(string fileName)
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
        Debug.Log("Copied palabras predeterminadas file");
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
        InitPalabrasPredeterminadas();
    }

    IEnumerator CopyFrasesBinaryToPersistentPath(string fileName)
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
        Debug.Log("Copied frases predeterminadas file");
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
        InitFrasesPredeterminadas();
    }



    private void InitFrasesPredeterminadas()
    {
        bool existenteFrase = true;

        if (File.Exists(nameRuteFrase))
        {
            management.LoadDatesFrase();

            if (frasesGuardadas.Count == frasesPredeterminadas.Count)
            {
                #region si no tiene los datos minimos se los creamos
                List<bool> existeFrase = new List<bool>();
                foreach (FraseBD p in frasesPredeterminadas)
                {
                    existeFrase.Add(false);
                    foreach (FraseBD w in frasesGuardadas)
                    {
                        if (p.actualFrase == w.actualFrase && p.sound == w.sound && p.image == w.image && p.fraseCatalan == w.fraseCatalan)
                        {
                            existeFrase[existeFrase.Count - 1] = true;
                            break;
                        }
                    }
                }



                foreach (bool b in existeFrase)
                {
                    if (!b)
                    {
                        existenteFrase = false;
                        break;
                    }
                }
                #endregion
            }
            else
                existenteFrase = false;
        }
        if (!File.Exists(nameRuteFrase) || !existenteFrase)
        {
            management.SaveDatesFrase();
            management.LoadDatesFrase();
        }

        PaquetePalabrasParejas.GetInstance();
    }

    public void SaveDates()
    {
        print("guarda");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(nameRute);

        DatesToSave datos = new DatesToSave();
        datos.ChangeDates(palabrasPredeterminadass);

        bf.Serialize(file, datos);

        file.Close();
    }

    public void LoadDates()
    {
        print("carga");
        List<PalabraBD> palabras = new List<PalabraBD>();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(nameRute, FileMode.Open);

        DatesToSave datos = (DatesToSave)bf.Deserialize(file);
        palabras = datos.GetListOfPalabras();

        file.Close();


        GameManager.palabrasDisponibles.Clear();
        foreach (PalabraBD p in palabras)
        {
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
        frasesGuardadas.Clear();
        foreach (FraseBD p in frases)
        {
            GameManager.frasesDisponibles.Add(p);
            frasesGuardadas.Add(p);
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

        datos.config = GameManager.configurartion;

        bf.Serialize(file, datos);

        file.Close();
    }

    public void LoadConfig()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(nameConfiguration, FileMode.Open);

        PlayerConfiguration datos = (PlayerConfiguration)bf.Deserialize(file);

        GameManager.configurartion = datos.config;

        file.Close();

        GameManager.Instance.ChangeConfig();
    }

    public void SaveWordUser(PalabraBD _pal, bool _add)
    {
        if (_pal != null)
        {
            if (_add)
                palabrasUserGuardadas.Add(_pal);
            else
                palabrasUserGuardadas.Remove(_pal);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(nameRuteUser);

        DatesOfPlayer datos = new DatesOfPlayer();

        datos.ChangeDates(palabrasUserGuardadas);

        bf.Serialize(file, datos);

        file.Close();

        GameManager.palabrasUserDisponibles = palabrasUserGuardadas;
    }

    public void LoadDatesOfPlayer()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(nameRuteUser, FileMode.Open);

        DatesOfPlayer datos = (DatesOfPlayer)bf.Deserialize(file);

        GameManager.palabrasUserDisponibles = datos.GetListOfPalabras();

        file.Close();

        GameManager.Instance.ChangeConfig();
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
    private List<PalabraBD> palabrasUser = new List<PalabraBD>();

    public void ChangeDates(List<PalabraBD> _palabras)
    {
        palabrasUser.Clear();
        foreach (PalabraBD p in _palabras)
        {
            palabrasUser.Add(p);
        }
    }

    public void AddPalabras(List<PalabraBD> _palabras)
    {
        foreach (PalabraBD p in _palabras)
        {
            palabrasUser.Add(p);
        }
    }

    public List<PalabraBD> GetListOfPalabras()
    {
        return palabrasUser;
    }
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
    public Configurartion config;
}