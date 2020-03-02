using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PaqueteBit
{
    private static PaqueteBit instance;

    public List<PalabraBD> currentBitPaquet = new List<PalabraBD>();
    public List<PalabraBD> nextBitPaquet = new List<PalabraBD>();

    public int dificultad = 0;
    public int fase = 0;
    public bool acabado = false;
    private string nameRute = "";
    private string ruteOriginal = "PaqueteBit.dat";
    private SingletonLenguage.Lenguage lastLenguaje = SingletonLenguage.Lenguage.INGLES;


    public static PaqueteBit GetInstance()
    {
        if (instance == null)
        {
            instance = new PaqueteBit();
        }
        if (instance.lastLenguaje != SingletonLenguage.GetInstance().GetLenguage())
        {
            if (instance.dificultad != 0)
            {
                instance.CrearBinario();
            }
            instance.Reset();
            instance.InitPaquet();
        }
        return instance;
    }

    private void Reset()
    {
        instance.dificultad = 0;
        instance.acabado = false;
        currentBitPaquet.Clear();
    }

    public void InitPaquet()
    {
        instance.nameRute = "/" + SingletonLenguage.GetInstance().GetLenguage().ToString() + instance.ruteOriginal;
        if (File.Exists(Application.persistentDataPath + instance.nameRute))
            instance.CargarBinario();
        else
        {
            instance.CrearNuevoPaquete();
            instance.CrearBinario();
        }
        instance.lastLenguaje = SingletonLenguage.GetInstance().GetLenguage();

        foreach (PalabraBD item in instance.currentBitPaquet)
        {
            item.SeparateSilabas();
            item.SetPalabraActual();
        }

        foreach (PalabraBD item in instance.nextBitPaquet)
        {
            item.SeparateSilabas();
            item.SetPalabraActual();
        }
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
        if (currentBitPaquet.Count == 0)
        {
            if (!acabado)
                fase++;

            if (dificultad < 3 && (fase == 1 || fase == 4))
            {
                dificultad++;
                fase = 1;
            }
            else if (dificultad == 3)
            {
                acabado = true;
            }

            if (!acabado)
            {
                if (fase == 1)
                {
                    currentBitPaquet.Clear();

                    List<PalabraBD> palabrasTotales = new List<PalabraBD>();
                    for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                    {
                        if (GameManager.palabrasDisponibles[i].paquet == GameManager.configurartion.paquete)
                        {
                            if (GameManager.palabrasDisponibles[i].dificultSpanish == dificultad && GameManager.palabrasDisponibles[i].image1 != "")
                            {
                                palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                            }
                        }
                    }

                    for (int i = 0; i < palabrasTotales.Count; i++)
                    {

                        if (i < palabrasTotales.Count / 3)
                        {
                            currentBitPaquet.Add(palabrasTotales[i]);
                            currentBitPaquet.Add(palabrasTotales[i]);
                            currentBitPaquet.Add(palabrasTotales[i]);
                        }
                        else
                        {
                            nextBitPaquet.Add(palabrasTotales[i]);
                        }
                    }


                }
                else if (fase == 2)
                {
                    currentBitPaquet.Clear();
                    for (int i = 0; i < nextBitPaquet.Count; i++)
                    {
                        if(nextBitPaquet.Count / 2 > i)
                        {
                            currentBitPaquet.Add(nextBitPaquet[i]);
                            currentBitPaquet.Add(nextBitPaquet[i]);
                            currentBitPaquet.Add(nextBitPaquet[i]);
                        }
                    }

                    for (int i = 0; i < currentBitPaquet.Count; i+=3)
                    {
                        nextBitPaquet.Remove(currentBitPaquet[i]);
                    }
                }
                else
                {
                    currentBitPaquet.Clear();
                    foreach (var item in nextBitPaquet)
                    {
                        currentBitPaquet.Add(item);
                        currentBitPaquet.Add(item);
                        currentBitPaquet.Add(item);

                    }
                    nextBitPaquet.Clear();
                }
            }
            else
            {
                foreach (var item in GameManager.palabrasDisponibles)
                {
                    if (item.image1 != "")
                    {
                        currentBitPaquet.Add(item);
                        currentBitPaquet.Add(item);
                        currentBitPaquet.Add(item);
                    }
                }
            }

        }
    }

    public void CrearNuevoPaqueteCat()
    {
        if (currentBitPaquet.Count == 0)
        {
            if (!acabado)
                fase++;

            if ((dificultad == 0) && dificultad < 3 && (fase == 1 || fase == 4))
            {
                dificultad++;
                fase = 1;
            }
            else if (dificultad == 3)
            {
                acabado = true;
            }
            if (!acabado)
            {
                if (fase == 1)
                {
                    currentBitPaquet.Clear();

                    List<PalabraBD> palabrasTotales = new List<PalabraBD>();
                    for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                    {
                        if (GameManager.palabrasDisponibles[i].paquet == GameManager.configurartion.paquete)
                        {
                            if (GameManager.palabrasDisponibles[i].dificultCatalan == dificultad && GameManager.palabrasDisponibles[i].image1 != "")
                            {
                                palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                            }
                        }
                    }

                    for (int i = 0; i < palabrasTotales.Count; i++)
                    {

                        if (i < palabrasTotales.Count / 3)
                        {
                            currentBitPaquet.Add(palabrasTotales[i]);
                            currentBitPaquet.Add(palabrasTotales[i]);
                            currentBitPaquet.Add(palabrasTotales[i]);
                        }
                        else
                        {
                            nextBitPaquet.Add(palabrasTotales[i]);
                        }
                    }


                }
                else if (fase == 2)
                {
                    currentBitPaquet.Clear();
                    for (int i = 0; i < nextBitPaquet.Count; i++)
                    {
                        if (nextBitPaquet.Count / 2 > i)
                        {
                            currentBitPaquet.Add(nextBitPaquet[i]);
                            currentBitPaquet.Add(nextBitPaquet[i]);
                            currentBitPaquet.Add(nextBitPaquet[i]);
                        }
                    }

                    for (int i = 0; i < currentBitPaquet.Count; i += 3)
                    {
                        nextBitPaquet.Remove(currentBitPaquet[i]);
                    }
                }
                else
                {
                    currentBitPaquet.Clear();
                    foreach (var item in nextBitPaquet)
                    {
                        currentBitPaquet.Add(item);
                        currentBitPaquet.Add(item);
                        currentBitPaquet.Add(item);

                    }
                    nextBitPaquet.Clear();
                }
            }
            else
            {
                foreach (var item in GameManager.palabrasDisponibles)
                {
                    if (item.image1 != "")
                    {
                        currentBitPaquet.Add(item);
                        currentBitPaquet.Add(item);
                        currentBitPaquet.Add(item);
                    }
                }
            }

        }
    }

    public void CrearBinario()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + nameRute);

        PaquetesPalabrasBit datos = new PaquetesPalabrasBit();
        datos.currentBitPaquet = currentBitPaquet;
        datos.dificultad = dificultad;
        datos.acabado = acabado;
        datos.nextBitPaquet = nextBitPaquet;
        datos.fase = fase;

        bf.Serialize(file, datos);

        file.Close();

    }

    public void CargarBinario()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + nameRute, FileMode.Open);

        PaquetesPalabrasBit datos = (PaquetesPalabrasBit)bf.Deserialize(file);

        currentBitPaquet = datos.currentBitPaquet;
        nextBitPaquet = datos.nextBitPaquet;
        dificultad = datos.dificultad;
        acabado = datos.acabado;
        fase = datos.fase;

        file.Close();


    }
}

[Serializable]
public class PaquetesPalabrasBit
{
    public List<PalabraBD> currentBitPaquet = new List<PalabraBD>();
    public List<PalabraBD> nextBitPaquet = new List<PalabraBD>();
    public int dificultad;
    public bool acabado;
    public int fase;
}
