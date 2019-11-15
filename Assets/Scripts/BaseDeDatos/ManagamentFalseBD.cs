using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ManagamentFalseBD : MonoBehaviour
{

    public static ManagamentFalseBD management;

    [SerializeField] private List<PalabraBD> palabrasPredeterminadass = new List<PalabraBD>();
    private List<PalabraBD> palabrasGuardadas = new List<PalabraBD>();
    private string nameRute;
    private string nameRuteBolasMinijuegos;

    private void Awake()
    {
        GameObject go = GameObject.Find("ManagementFalsaBD");
        if (go == null || go == gameObject)
        {
            if (management == null)
            {
                nameRute = Application.persistentDataPath + "/datos.dat";
                nameRuteBolasMinijuegos = Application.persistentDataPath + "/BolasMinijuegos.dat";
                management = this;
                DontDestroyOnLoad(gameObject);
                InitPalabrasPredeterminadas();
                bool existente = true;
                if (File.Exists(nameRute))
                {
                    management.LoadDates();

                    #region si no tiene los datos minimos se los creamos
                    List<bool> existe = new List<bool>();
                    foreach (PalabraBD p in palabrasPredeterminadass)
                    {
                        existe.Add(false);
                        foreach (PalabraBD w in palabrasGuardadas)
                        {
                            if (p == w)
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
                    #endregion
                }
                if (!File.Exists(nameRute) || !existente)
                {
                    management.SaveDates();
                }
                if (File.Exists(nameRuteBolasMinijuegos))
                {
                    management.LoadBolasMinijuegos();
                }
                if (!File.Exists(nameRuteBolasMinijuegos))
                {
                    management.SaveBolasMinijuegos();
                }



            }
        }
        else if (go != gameObject)
            Destroy(gameObject);
    }

    private void InitPalabrasPredeterminadas()
    {
        int count = 0;
        #region Pera
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Naranja";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "pera_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "pera_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "pera_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "pera";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Pera";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "Pe-ra";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Pera";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "Pe-ra";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Moto
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Naranja";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "moto_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "moto_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "moto_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "moto";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Moto";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "Mo-to";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Moto";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "Mo-to";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Boca
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Naranja";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "boca_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "boca_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "boca_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "boca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Boca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "Bo-ca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Boca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "Bo-ca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        foreach (PalabraBD p in palabrasPredeterminadass)
        {
            p.SeparateSilabas(SingletonLenguage.GetInstance().GetLenguage());
            p.SetPalabraActual(SingletonLenguage.GetInstance().GetLenguage());
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
        palabrasGuardadas.Clear();
        foreach (PalabraBD p in palabras)
        {
            GameManager.palabrasDisponibles.Add(p);
            palabrasGuardadas.Add(p);
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
            GameManager.m_CurrentToMinigame[i] = datos.bolasMinijuegos[i];

        GameManager.currentMiniGame = datos.currentMiniGame;

        file.Close();
    }

    public void LoadDatesOfPlayer()
    {

    }

}


[Serializable] class DatesToSave
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

[Serializable] class DatesOfPlayer
{

}

[Serializable] class PointsOfMinigames
{
    public List<int> bolasMinijuegos = new List<int>();
    public int currentMiniGame;
}