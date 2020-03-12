using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PaquetePuzzle
{
    private static PaquetePuzzle instance;

    public List<PalabraBD> currentPuzzlePaquet = new List<PalabraBD>();
    public List<PalabraBD> nextPuzzlePaquets = new List<PalabraBD>();

    public int dificultad = 0;
    public int fase = 0;
    private int lastLvl = 0;
    public bool acabado = false;
    private string nameRute = "";
    private string ruteOriginal = "PaquetePuzzle.dat";
    private SingletonLenguage.Lenguage lastLenguaje = SingletonLenguage.Lenguage.INGLES;


    public static PaquetePuzzle GetInstance(int _lvl)
    {
        if (instance == null)
        {
            instance = new PaquetePuzzle();
        }
        if (_lvl != instance.lastLvl || instance.lastLenguaje != SingletonLenguage.GetInstance().GetLenguage())
        {
            if (instance.dificultad != 0)
            {
                instance.CrearBinario();
            }
            instance.Reset();
            instance.InitPaquet(_lvl);
        }
        return instance;
    }

    private void Reset()
    {
        instance.dificultad = 0;
        instance.acabado = false;
        currentPuzzlePaquet.Clear();
    }

    public void InitPaquet(int _lvl)
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

        foreach (PalabraBD item in instance.currentPuzzlePaquet)
        {
            item.SeparateSilabas();
            item.SetPalabraActual();
        }

        foreach (PalabraBD item in instance.nextPuzzlePaquets)
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
        if (currentPuzzlePaquet.Count == 0)
        {
            if (!acabado)
                fase++;

            if (dificultad < 3 && (fase == 1 || fase == 4))
            {
                dificultad++;
                fase = 1;
            }
            else if (dificultad == 3 && fase >= 4)
            {
                acabado = true;
            }

            if (!acabado)
            {
                if (fase == 1)
                {
                    currentPuzzlePaquet.Clear();

                    List<PalabraBD> palabrasTotales = new List<PalabraBD>();
                    List<PalabraBD> palabrasanimales = new List<PalabraBD>();

                    for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                    {
                        if (GameManager.palabrasDisponibles[i].paquet == 0)
                        {
                            if (GameManager.palabrasDisponibles[i].dificultSpanish == dificultad && GameManager.palabrasDisponibles[i].image1 != "")
                            {
                                for (int j = 0; j < GameManager.palabrasDisponibles[i].piecesPuzzle.Count; j++)
                                {
                                    if (GameManager.palabrasDisponibles[i].piecesPuzzle[j] >= 4)
                                    {
                                        palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                        break;

                                    }
                                }
                            }
                        }
                        else
                        {

                            for (int j = 0; j < GameManager.palabrasDisponibles[i].piecesPuzzle.Count; j++)
                            {
                                if (GameManager.palabrasDisponibles[i].piecesPuzzle[j] >= 4)
                                {
                                    palabrasanimales.Add(GameManager.palabrasDisponibles[i]);
                                    break;

                                }
                            }

                        }
                    }

                    for (int i = 0; i < palabrasTotales.Count; i++)
                    {

                        if (i < palabrasTotales.Count / 3)
                        {
                            currentPuzzlePaquet.Add(palabrasTotales[i]);
                            currentPuzzlePaquet.Add(palabrasTotales[i]);
                        }
                        else
                        {
                            nextPuzzlePaquets.Add(palabrasTotales[i]);
                        }
                    }

                    for (int i = 0; i < palabrasanimales.Count; i++)
                    {

                        if (i < palabrasanimales.Count / 3)
                        {
                            currentPuzzlePaquet.Add(palabrasanimales[i]);
                            currentPuzzlePaquet.Add(palabrasanimales[i]);
                        }
                        else
                        {
                            nextPuzzlePaquets.Add(palabrasanimales[i]);
                        }
                    }


                }
                else if (fase == 2)
                {
                    currentPuzzlePaquet.Clear();
                    List<PalabraBD> palabras = new List<PalabraBD>();
                    List<PalabraBD> palabrasanimales = new List<PalabraBD>();

                    for (int i = 0; i < nextPuzzlePaquets.Count; i++)
                    {
                        if (nextPuzzlePaquets[i].paquet == 0)
                        {
                            if (nextPuzzlePaquets[i].dificultSpanish == dificultad && nextPuzzlePaquets[i].image1 != "")
                                palabras.Add(nextPuzzlePaquets[i]);
                        }
                        else
                        {
                            if (nextPuzzlePaquets[i].image1 != "") //poner dificultad animales
                                palabrasanimales.Add(nextPuzzlePaquets[i]);
                        }
                    }

                    for (int i = 0; i < palabras.Count; i++)
                    {
                        if (palabras.Count / 2 > i)
                        {
                            currentPuzzlePaquet.Add(palabras[i]);
                            currentPuzzlePaquet.Add(palabras[i]);
                        }
                    }

                    for (int i = 0; i < palabrasanimales.Count; i++)
                    {
                        if (palabrasanimales.Count / 2 > i)
                        {
                            currentPuzzlePaquet.Add(palabrasanimales[i]);
                            currentPuzzlePaquet.Add(palabrasanimales[i]);
                        }
                    }

                    for (int i = 0; i < currentPuzzlePaquet.Count; i += 2)
                    {
                        nextPuzzlePaquets.Remove(currentPuzzlePaquet[i]);
                    }
                }
                else
                {
                    currentPuzzlePaquet.Clear();
                    foreach (var item in nextPuzzlePaquets)
                    {
                        currentPuzzlePaquet.Add(item);
                        currentPuzzlePaquet.Add(item);

                    }
                    nextPuzzlePaquets.Clear();
                }
            }
            else
            {
                foreach (var item in GameManager.palabrasDisponibles)
                {
                    if (item.image1 != "")
                    {
                        currentPuzzlePaquet.Add(item);
                        currentPuzzlePaquet.Add(item);
                    }
                }
            }

        }
    }

    public void CrearNuevoPaqueteCat()
    {
        if (currentPuzzlePaquet.Count == 0)
        {
            if (!acabado)
                fase++;

            if (dificultad < 3 && (fase == 1 || fase == 4))
            {
                dificultad++;
                fase = 1;
            }
            else if (dificultad == 3 && fase >= 4)
            {
                acabado = true;
            }

            if (!acabado)
            {
                if (fase == 1)
                {
                    currentPuzzlePaquet.Clear();

                    List<PalabraBD> palabrasTotales = new List<PalabraBD>();
                    List<PalabraBD> palabrasanimales = new List<PalabraBD>();

                    for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                    {
                        if (GameManager.palabrasDisponibles[i].paquet == 0)
                        {
                            if (GameManager.palabrasDisponibles[i].dificultCatalan == dificultad && GameManager.palabrasDisponibles[i].image1 != "")
                            {
                                for (int j = 0; j < GameManager.palabrasDisponibles[i].piecesPuzzle.Count; j++)
                                {
                                    if (GameManager.palabrasDisponibles[i].piecesPuzzle[j] >= 4)
                                    {
                                        palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                        break;

                                    }
                                }
                            }
                        }
                        else
                        {

                            for (int j = 0; j < GameManager.palabrasDisponibles[i].piecesPuzzle.Count; j++)
                            {
                                if (GameManager.palabrasDisponibles[i].piecesPuzzle[j] >= 4)
                                {
                                    palabrasanimales.Add(GameManager.palabrasDisponibles[i]);
                                    break;

                                }
                            }

                        }
                    }

                    for (int i = 0; i < palabrasTotales.Count; i++)
                    {

                        if (i < palabrasTotales.Count / 3)
                        {
                            currentPuzzlePaquet.Add(palabrasTotales[i]);
                            currentPuzzlePaquet.Add(palabrasTotales[i]);
                        }
                        else
                        {
                            nextPuzzlePaquets.Add(palabrasTotales[i]);
                        }
                    }

                    for (int i = 0; i < palabrasanimales.Count; i++)
                    {

                        if (i < palabrasanimales.Count / 3)
                        {
                            currentPuzzlePaquet.Add(palabrasanimales[i]);
                            currentPuzzlePaquet.Add(palabrasanimales[i]);
                        }
                        else
                        {
                            nextPuzzlePaquets.Add(palabrasanimales[i]);
                        }
                    }


                }
                else if (fase == 2)
                {
                    currentPuzzlePaquet.Clear();
                    List<PalabraBD> palabras = new List<PalabraBD>();
                    List<PalabraBD> palabrasanimales = new List<PalabraBD>();

                    for (int i = 0; i < nextPuzzlePaquets.Count; i++)
                    {
                        if (nextPuzzlePaquets[i].paquet == 0)
                        {
                            if (nextPuzzlePaquets[i].dificultCatalan == dificultad && nextPuzzlePaquets[i].image1 != "")
                                palabras.Add(nextPuzzlePaquets[i]);
                        }
                        else
                        {
                            if (nextPuzzlePaquets[i].image1 != "") //poner dificultad animales
                                palabrasanimales.Add(nextPuzzlePaquets[i]);
                        }
                    }

                    for (int i = 0; i < palabras.Count; i++)
                    {
                        if (palabras.Count / 2 > i)
                        {
                            currentPuzzlePaquet.Add(palabras[i]);
                            currentPuzzlePaquet.Add(palabras[i]);
                        }
                    }

                    for (int i = 0; i < palabrasanimales.Count; i++)
                    {
                        if (palabrasanimales.Count / 2 > i)
                        {
                            currentPuzzlePaquet.Add(palabrasanimales[i]);
                            currentPuzzlePaquet.Add(palabrasanimales[i]);
                        }
                    }

                    for (int i = 0; i < currentPuzzlePaquet.Count; i += 2)
                    {
                        nextPuzzlePaquets.Remove(currentPuzzlePaquet[i]);
                    }
                }
                else
                {
                    currentPuzzlePaquet.Clear();
                    foreach (var item in nextPuzzlePaquets)
                    {
                        currentPuzzlePaquet.Add(item);
                        currentPuzzlePaquet.Add(item);

                    }
                    nextPuzzlePaquets.Clear();
                }
            }
            else
            {
                foreach (var item in GameManager.palabrasDisponibles)
                {
                    if (item.image1 != "")
                    {
                        currentPuzzlePaquet.Add(item);
                        currentPuzzlePaquet.Add(item);
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
        datos.currentBitPaquet = currentPuzzlePaquet;
        datos.dificultad = dificultad;
        datos.acabado = acabado;
        datos.nextBitPaquet = nextPuzzlePaquets;
        datos.fase = fase;

        bf.Serialize(file, datos);

        file.Close();

    }

    public void CargarBinario()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + nameRute, FileMode.Open);

        PaquetesPalabrasBit datos = (PaquetesPalabrasBit)bf.Deserialize(file);

        currentPuzzlePaquet = datos.currentBitPaquet;
        nextPuzzlePaquets = datos.nextBitPaquet;
        dificultad = datos.dificultad;
        acabado = datos.acabado;
        fase = datos.fase;

        file.Close();


    }
}

[Serializable]
public class PaquetesPalabrasPuzzle
{
    public List<PalabraBD> currentPuzzlePaquet = new List<PalabraBD>();
    public List<PalabraBD> nextPuzzlePaquets = new List<PalabraBD>();
    public int dificultad;
    public bool acabado;
    public int fase;
}
