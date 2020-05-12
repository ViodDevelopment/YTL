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
                    if (GameManager.configuration == null)
                        GameManager.configuration = new Configuration();

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

        if (GameManager.configuration.actualizacion)
        {
            QuitarPalabrasQueYaNoExisten();
            Debug.LogWarning("cambiar esto cuando esté todo hecho");
            GameManager.configuration.actualizacion = false;
            SaveConfig();
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

    private void QuitarPalabrasQueYaNoExisten()
    {
        SingletonLenguage.Lenguage leng = GameManager.configuration.currentLenguaje;
        GameManager.configuration.currentLenguaje = SingletonLenguage.Lenguage.CASTELLANO;
        ActualizarPaqueteBit();
        ActualizarPaqeuteParejas("1");
        ActualizarPaqeuteParejas("2");
        ActualizarPaqeuteParejas("3");
        ActualizarPaquetePuzzle("1");
        ActualizarPaquetePuzzle("2");
        GameManager.configuration.currentLenguaje = SingletonLenguage.Lenguage.CATALAN;
        ActualizarPaqueteBit();
        ActualizarPaqeuteParejas("1");
        ActualizarPaqeuteParejas("2");
        ActualizarPaqeuteParejas("3");
        ActualizarPaquetePuzzle("1");
        ActualizarPaquetePuzzle("2");
        GameManager.configuration.currentLenguaje = leng;
    }

    private void ActualizarPaqueteBit()
    {
        List<int> paraEliminar = new List<int>();//current
        for (int i = 0; i < PaqueteBit.GetInstance().currentBitPaquet.Count; i++)
        {
            if (PaqueteBit.GetInstance().currentBitPaquet[i].nameSpanish == "culo")
            {
                paraEliminar.Add(i);
            }
        }
        paraEliminar.Reverse();
        for (int i = 0; i < paraEliminar.Count; i++)
        {
            PaqueteBit.GetInstance().currentBitPaquet.RemoveAt(i);
        }
        paraEliminar.Clear();
        //next
        for (int i = 0; i < PaqueteBit.GetInstance().nextBitPaquet.Count; i++)
        {
            if (PaqueteBit.GetInstance().nextBitPaquet[i].nameSpanish == "culo")
            {
                paraEliminar.Add(i);
            }
        }
        paraEliminar.Reverse();
        for (int i = 0; i < paraEliminar.Count; i++)
        {
            PaqueteBit.GetInstance().nextBitPaquet.RemoveAt(i);
        }
        int num = 0;
        foreach (PalabraBD p in PaqueteBit.GetInstance().currentBitPaquet)
        {
            if (p.paquet == GameManager.configuration.paquete || GameManager.configuration.paquete == -1)
            {
                num++;
            }
        }
        if (num == 0)
        {
            PaqueteBit.GetInstance().CrearNuevoPaquete();
        }
        PaqueteBit.GetInstance().CrearBinario();
        paraEliminar.Clear();
    }

    private void ActualizarPaqeuteParejas(string _lvl)
    {
        PaquetePalabrasParejas.GetInstance(_lvl).QuitarPalabras("culo");
        int num = 0;
        foreach (PalabraBD p in PaquetePalabrasParejas.GetInstance(_lvl).currentParejasPaquet)
        {
            if (p.paquet == GameManager.configuration.paquete || GameManager.configuration.paquete == -1)
            {
                num++;
            }
        }
        if (num == 0)
        {
            PaquetePalabrasParejas.GetInstance(_lvl).CrearNuevoPaquete();
        }
        PaquetePalabrasParejas.GetInstance(_lvl).CrearBinario();
    }

    private void ActualizarPaquetePuzzle(string _lvl)
    {
        List<int> paraEliminar = new List<int>();//current
        for (int i = 0; i < PaquetePuzzle.GetInstance(_lvl).currentPuzzlePaquet.Count; i++)
        {
            if (PaquetePuzzle.GetInstance(_lvl).currentPuzzlePaquet[i].nameSpanish == "culo")
            {
                paraEliminar.Add(i);
            }
        }
        paraEliminar.Reverse();
        for (int i = 0; i < paraEliminar.Count; i++)
        {
            PaquetePuzzle.GetInstance(_lvl).currentPuzzlePaquet.RemoveAt(i);
        }
        paraEliminar.Clear();
        //next
        for (int i = 0; i < PaquetePuzzle.GetInstance(_lvl).nextPuzzlePaquets.Count; i++)
        {
            if (PaquetePuzzle.GetInstance(_lvl).nextPuzzlePaquets[i].nameSpanish == "culo")
            {
                paraEliminar.Add(i);
            }
        }
        paraEliminar.Reverse();
        for (int i = 0; i < paraEliminar.Count; i++)
        {
            PaquetePuzzle.GetInstance(_lvl).nextPuzzlePaquets.RemoveAt(i);
        }
        int num = 0;
        foreach (PalabraBD p in PaquetePuzzle.GetInstance(_lvl).currentPuzzlePaquet)
        {
            if (p.paquet == GameManager.configuration.paquete || GameManager.configuration.paquete == -1)
            {
                num++;
            }
        }
        if (num == 0)
        {
            PaquetePuzzle.GetInstance(_lvl).CrearNuevoPaquete();
        }
        PaquetePuzzle.GetInstance(_lvl).CrearBinario();
        paraEliminar.Clear();

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