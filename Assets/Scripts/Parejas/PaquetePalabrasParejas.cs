using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PaquetePalabrasParejas : MonoBehaviour
{
    public static PaquetePalabrasParejas instance;

    public List<PalabraBD> currentParejasPaquet = new List<PalabraBD>();
    public List<PalabraBD> donePalabrasPaquet = new List<PalabraBD>();
    public List<PalabraBD> nextPalabrasPaquet = new List<PalabraBD>();
    public List<bool> pantallasHorizontal = new List<bool>();
    public int parejas = 0;
    public int dificultad = 0;
    public int fase = 0;
    public bool acabado = false;
    private string nameRute = "/PaqueteParejas.dat";

    private void Start()
    {

        if (instance == null)
        {
            instance = this;
            {
                if (File.Exists(Application.persistentDataPath + nameRute))
                    CargarBinario();
                else
                {
                    CrearNuevoPaquete();
                    CrearBinario();
                }
            }
        }
        else
        {
            DestroyImmediate(this);
        }
    }



    public void CrearNuevoPaquete()
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
                donePalabrasPaquet.Clear();
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
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                }
                else if (parejas == 3)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                }
                else if (parejas == 4)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        pantallasHorizontal.Add(true);
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        pantallasHorizontal.Add(false);
                    }
                }

                if (fase != l_firstFase)
                {
                    if (l_firstDif == dificultad)
                    {
                        foreach (var item in currentParejasPaquet)
                        {
                            donePalabrasPaquet.Add(item);
                        }
                    }
                    currentParejasPaquet.Clear();


                    if (nextPalabrasPaquet.Count == 0 && currentParejasPaquet.Count == 0 && fase == 1)
                    {
                        for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                        {
                            if (GameManager.palabrasDisponibles[i].dificultCatalan == dificultad || GameManager.palabrasDisponibles[i].dificultSpanish == dificultad)
                            {
                                if (i < (GameManager.palabrasDisponibles.Count - 1) / 3)
                                {
                                    currentParejasPaquet.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else
                                {
                                    nextPalabrasPaquet.Add(GameManager.palabrasDisponibles[i]);
                                }
                            }

                        }
                    }
                    else
                    {
                        if (fase == 2)
                        {
                            for (int i = (nextPalabrasPaquet.Count - 1) / 2; i >= 0; i--)
                            {
                                currentParejasPaquet.Add(nextPalabrasPaquet[i]);
                                nextPalabrasPaquet.RemoveAt(i);
                            }
                        }
                        else if (fase == 3)
                        {
                            for (int i = nextPalabrasPaquet.Count - 1; i >= 0; i--)
                            {
                                currentParejasPaquet.Add(nextPalabrasPaquet[i]);
                            }
                            nextPalabrasPaquet.Clear();
                        }
                        else if (fase == 4)
                        {
                            foreach (var item in donePalabrasPaquet)
                            {
                                currentParejasPaquet.Add(item);
                            }
                        }
                    }
                }
            }
            else
            {
                nextPalabrasPaquet.Clear();
                foreach (var item in GameManager.palabrasDisponibles)
                {
                    currentParejasPaquet.Add(item);
                }
            }

        }
    }

    public void CrearBinario()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + nameRute);

        PaquetesPalabras datos = new PaquetesPalabras();
        datos.currentParejasPaquet = currentParejasPaquet;
        datos.donePalabrasPaquet = donePalabrasPaquet;
        datos.nextPalabrasPaquet = nextPalabrasPaquet;
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

        currentParejasPaquet = datos.currentParejasPaquet;
        donePalabrasPaquet = datos.donePalabrasPaquet;
        nextPalabrasPaquet = datos.nextPalabrasPaquet;
        pantallasHorizontal = datos.pantallasHorizontal;
        parejas = datos.parejas;
        dificultad = datos.dificultad;
        fase = datos.fase;
        acabado = datos.acabado;

        file.Close();

    }
}

[Serializable]
public class PaquetesPalabras
{
    public List<PalabraBD> currentParejasPaquet = new List<PalabraBD>();
    public List<PalabraBD> donePalabrasPaquet = new List<PalabraBD>();
    public List<PalabraBD> nextPalabrasPaquet = new List<PalabraBD>();
    public List<bool> pantallasHorizontal = new List<bool>();
    public int parejas;
    public int dificultad;
    public int fase;
    public bool acabado;
}
