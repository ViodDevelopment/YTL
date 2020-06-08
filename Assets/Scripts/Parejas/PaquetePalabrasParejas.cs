using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PaquetePalabrasParejas
{
    private static PaquetePalabrasParejas instance;
    public List<bool> pantallasHorizontal = new List<bool>();
    public int parejas = 0;
    public int dificultad = 0;
    public int fase = 0;
    public bool acabado = false;
    private string nameRute = "";
    private string ruteOriginal = "PaqueteParejas.dat";
    private string lastLvl = "0";
    private SingletonLenguage.Lenguage lastLenguaje = SingletonLenguage.Lenguage.INGLES;


    public static PaquetePalabrasParejas GetInstance(string _lvl)
    {
        if (instance == null)
        {
            instance = new PaquetePalabrasParejas();
        }
        if (!GameManager.actualizacion)
        {
            if (_lvl != instance.lastLvl || instance.lastLenguaje != SingletonLenguage.GetInstance().GetLenguage())
            {

                if (instance.parejas != 0)
                {
                    instance.CrearBinario();
                }
                instance.Reset();
                instance.InitPaquet(_lvl);
            }
        }
        return instance;
    }

    private void Reset()
    {
        instance.parejas = 0;
        instance.dificultad = 0;
        instance.fase = 0;
        instance.acabado = false;
        pantallasHorizontal.Clear();
    }

    public void InitPaquet(string _lvl)
    {
        instance.nameRute = "/" + SingletonLenguage.GetInstance().GetLenguage().ToString() + _lvl + instance.ruteOriginal;

        if (File.Exists(Application.persistentDataPath + instance.nameRute))
            instance.CargarBinario();
        else
        {
            instance.CrearNuevoPaquete();
            instance.CrearBinario();
        }

        instance.lastLvl = _lvl;
        instance.lastLenguaje = SingletonLenguage.GetInstance().GetLenguage();
    }

    public void CrearNuevoPaquete()
    {
        if (SingletonLenguage.GetInstance().GetLenguage() == SingletonLenguage.Lenguage.CASTELLANO)
            instance.CrearNuevoPaqueteEsp();
        else if (SingletonLenguage.GetInstance().GetLenguage() == SingletonLenguage.Lenguage.CATALAN)
            instance.CrearNuevoPaqueteCat();
    }



    public void CrearNuevoPaqueteEsp()
    {
        if (pantallasHorizontal.Count == 0)
        {
            int l_firstFase = fase;
            int l_firstDif = dificultad;
            if (fase == 0)
                fase = 1;
            else if (fase <= 4 && parejas >= 3)
            {
                fase++;
                parejas = 2;
            }
            else if (parejas == 2)
                parejas++;

            if (parejas == 0)
                parejas = 2;

            if (fase == 4)
                parejas = 4;


            if ((dificultad == 0 || fase == 5) && dificultad < 3)
            {
                dificultad++;
                fase = 1;
                parejas = 2;
            }
            else if ((dificultad == 0 || fase == 5) && dificultad == 3)
            {
                acabado = true;
            }

            if (!acabado)
            {
                if (parejas == 2)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                }
                else if (parejas == 3)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                }
                else if (parejas == 4)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                }
            }

        }
        CrearBinario();

    }

    public void CrearNuevoPaqueteCat()
    {
        if (pantallasHorizontal.Count == 0)
        {
            int l_firstFase = fase;
            int l_firstDif = dificultad;
            if (fase == 0)
                fase = 1;
            else if (fase <= 4 && parejas >= 3)
            {
                fase++;
                parejas = 2;
            }
            else if (parejas == 2)
                parejas++;

            if (parejas == 0)
                parejas = 2;

            if (fase == 4)
                parejas = 4;


            if ((dificultad == 0 || fase == 5) && dificultad < 3)
            {
                dificultad++;
                fase = 1;
                parejas = 2;
            }
            else if ((dificultad == 0 || fase == 5) && dificultad == 3)
            {
                acabado = true;
            }

            if (!acabado)
            {
                if (parejas == 2)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                }
                else if (parejas == 3)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                }
                else if (parejas == 4)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                }


            }

        }
        CrearBinario();
    }

    public void CrearBinario()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + nameRute);

        PaquetesPalabras datos = new PaquetesPalabras(); ;
        datos.pantallasHorizontal = pantallasHorizontal;
        datos.parejas = parejas;
        datos.dificultad = dificultad;
        datos.fase = fase;
        datos.acabado = acabado;

        bf.Serialize(file, datos);

        file.Close();

    }

    public void CargarBinario()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + nameRute, FileMode.Open);

        PaquetesPalabras datos = (PaquetesPalabras)bf.Deserialize(file);

        pantallasHorizontal = datos.pantallasHorizontal;
        parejas = datos.parejas;
        dificultad = datos.dificultad;
        fase = datos.fase;
        acabado = datos.acabado;

        file.Close();


    }

    public void ReiniciarPaquetes()
    {
        for (int i = 0; i < 3; i++)
        {
            Reset();
            instance.nameRute = "/" + SingletonLenguage.Lenguage.CASTELLANO + (i + 1).ToString() + instance.ruteOriginal;
            if (File.Exists(Application.persistentDataPath + instance.nameRute))
                instance.CargarBinario();
            ReiniciarPaqueteCast();
            Reset();
            instance.nameRute = "/" + SingletonLenguage.Lenguage.CATALAN + (i + 1).ToString() + instance.ruteOriginal;
            if (File.Exists(Application.persistentDataPath + instance.nameRute))
                instance.CargarBinario();
            ReiniciarPaqueteCat();
        }
        Reset();
    }

    private void ReiniciarPaqueteCast()
    {
        if (fase == 0)
            fase = 1;

        if (parejas == 0)
            parejas = 2;

        if (fase == 4)
            parejas = 4;

        if (dificultad == 0)
            dificultad = 1;

        if (!acabado)
        {
            if (pantallasHorizontal.Count == 0)
            {
                if (parejas == 2)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                }
                else if (parejas == 3)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                }
                else if (parejas == 4)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                }
            }
        }
        CrearBinario();


    }

    private void ReiniciarPaqueteCat()
    {
        if (fase == 0)
            fase = 1;

        if (parejas == 0)
            parejas = 2;

        if (fase == 4)
            parejas = 4;

        if (dificultad == 0)
            dificultad = 1;

        if (!acabado)
        {
            if (pantallasHorizontal.Count == 0)
            {
                if (parejas == 2)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                }
                else if (parejas == 3)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                }
                else if (parejas == 4)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                }
            }


            CrearBinario();


        }

    }
}



[Serializable]
public class PaquetesPalabras
{

    public List<bool> pantallasHorizontal = new List<bool>();
    public int parejas;
    public int dificultad;
    public int fase;
    public bool acabado;
}
