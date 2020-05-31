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
    private string lastLvl = "0";
    public bool acabado = false;
    private string nameRute = "";
    private string ruteOriginal = "PaquetePuzzle.dat";
    private SingletonLenguage.Lenguage lastLenguaje = SingletonLenguage.Lenguage.INGLES;


    public static PaquetePuzzle GetInstance(string _lvl)
    {
        if (instance == null)
        {
            instance = new PaquetePuzzle();
        }
        if (!GameManager.actualizacion)
        {
            if (_lvl != instance.lastLvl || instance.lastLenguaje != SingletonLenguage.GetInstance().GetLenguage())
            {
                if (instance.dificultad != 0)
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
        instance.dificultad = 0;
        instance.fase = 0;
        instance.acabado = false;
        instance.currentPuzzlePaquet.Clear();
        instance.nextPuzzlePaquets.Clear();
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

        foreach (PalabraBD item in instance.currentPuzzlePaquet)
        {
            item.SetPalabraActual();
        }

        foreach (PalabraBD item in instance.nextPuzzlePaquets)
        {
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

                for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                {
                    if (GameManager.palabrasDisponibles[i].paquet == 0)
                    {
                        if (GameManager.palabrasDisponibles[i].dificultSpanish == dificultad && GameManager.palabrasDisponibles[i].image1 != "" && GameManager.palabrasDisponibles[i].piecesPuzzle.Count > 0)
                        {
                            if (GameManager.palabrasDisponibles[i].piecesPuzzle[0] >= 4)
                            {
                                if (palabrasTotales.Count < 10 && dificultad == 1)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 6 && dificultad == 2)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 4 && dificultad == 3)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count == 20)
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


            }
            else if (fase == 2)
            {
                currentPuzzlePaquet.Clear();
                List<PalabraBD> palabras = new List<PalabraBD>();

                for (int i = 0; i < nextPuzzlePaquets.Count; i++)
                {
                    if (nextPuzzlePaquets[i].paquet == 0)
                    {
                        if (nextPuzzlePaquets[i].dificultSpanish == dificultad && nextPuzzlePaquets[i].image1 != "")
                            palabras.Add(nextPuzzlePaquets[i]);
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

        CrearBinario();
    }

    public void CrearNuevoPaqueteCat()
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

                for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                {
                    if (GameManager.palabrasDisponibles[i].paquet == 0)
                    {
                        if (GameManager.palabrasDisponibles[i].dificultCatalan == dificultad && GameManager.palabrasDisponibles[i].image1 != "" && GameManager.palabrasDisponibles[i].piecesPuzzle.Count > 0)
                        {
                            if (GameManager.palabrasDisponibles[i].piecesPuzzle[0] >= 4)
                            {
                                if (palabrasTotales.Count < 10 && dificultad == 1)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 6 && dificultad == 2)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 4 && dificultad == 3)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count == 20)
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


            }
            else if (fase == 2)
            {
                currentPuzzlePaquet.Clear();
                List<PalabraBD> palabras = new List<PalabraBD>();

                for (int i = 0; i < nextPuzzlePaquets.Count; i++)
                {
                    if (nextPuzzlePaquets[i].paquet == 0)
                    {
                        if (nextPuzzlePaquets[i].dificultCatalan == dificultad && nextPuzzlePaquets[i].image1 != "")
                            palabras.Add(nextPuzzlePaquets[i]);
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
        CrearBinario();

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

    public void ReiniciarPaquetes()
    {
        for (int i = 0; i < 2; i++)
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
        currentPuzzlePaquet.Clear();
        nextPuzzlePaquets.Clear();

        if (!acabado)
        {
            if (fase == 0)
                fase = 1;

            if (dificultad == 0)
                dificultad = 1;

            if (fase == 1)
            {
                List<PalabraBD> palabrasTotales = new List<PalabraBD>();

                for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                {
                    if (GameManager.palabrasDisponibles[i].paquet == 0)
                    {
                        if (GameManager.palabrasDisponibles[i].dificultSpanish == dificultad && GameManager.palabrasDisponibles[i].image1 != "" && GameManager.palabrasDisponibles[i].piecesPuzzle.Count > 0)
                        {
                            if (GameManager.palabrasDisponibles[i].piecesPuzzle[0] >= 4)
                            {
                                if (palabrasTotales.Count < 10 && dificultad == 1)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 6 && dificultad == 2)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 4 && dificultad == 3)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count == 20)
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



            }
            else if (fase == 2)
            {
                List<PalabraBD> palabrasTotales = new List<PalabraBD>();

                for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                {
                    if (GameManager.palabrasDisponibles[i].paquet == 0)
                    {
                        if (GameManager.palabrasDisponibles[i].dificultSpanish == dificultad && GameManager.palabrasDisponibles[i].image1 != "" && GameManager.palabrasDisponibles[i].piecesPuzzle.Count > 0)
                        {
                            if (GameManager.palabrasDisponibles[i].piecesPuzzle[0] >= 4)
                            {
                                if (palabrasTotales.Count < 10 && dificultad == 1)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 6 && dificultad == 2)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 4 && dificultad == 3)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count == 20)
                                    break;
                            }
                        }
                    }

                }
                List<PalabraBD> palabrasTotales2 = new List<PalabraBD>();

                for (int i = 0; i < palabrasTotales.Count; i++)
                {

                    if (i >= palabrasTotales.Count / 3)
                        palabrasTotales2.Add(palabrasTotales[i]);

                }


                for (int i = 0; i < palabrasTotales2.Count; i++)
                {
                    if (palabrasTotales2.Count / 2 > i)
                    {
                        currentPuzzlePaquet.Add(palabrasTotales2[i]);
                        currentPuzzlePaquet.Add(palabrasTotales2[i]);
                    }
                    else
                        nextPuzzlePaquets.Add(palabrasTotales2[i]);
                }
            }
            else
            {
                List<PalabraBD> palabrasTotales = new List<PalabraBD>();

                for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                {
                    if (GameManager.palabrasDisponibles[i].paquet == 0)
                    {
                        if (GameManager.palabrasDisponibles[i].dificultSpanish == dificultad && GameManager.palabrasDisponibles[i].image1 != "" && GameManager.palabrasDisponibles[i].piecesPuzzle.Count > 0)
                        {
                            if (GameManager.palabrasDisponibles[i].piecesPuzzle[0] >= 4)
                            {
                                if (palabrasTotales.Count < 10 && dificultad == 1)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 6 && dificultad == 2)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 4 && dificultad == 3)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count == 20)
                                    break;
                            }
                        }
                    }

                }
                List<PalabraBD> palabrasTotales2 = new List<PalabraBD>();

                for (int i = 0; i < palabrasTotales.Count; i++)
                {

                    if (i >= palabrasTotales.Count / 3)
                        palabrasTotales2.Add(palabrasTotales[i]);

                }


                for (int i = 0; i < palabrasTotales2.Count; i++)
                {
                    if (palabrasTotales2.Count / 2 >= i)
                    {
                        currentPuzzlePaquet.Add(palabrasTotales2[i]);
                        currentPuzzlePaquet.Add(palabrasTotales2[i]);
                    }
                }

            }
        }
        else
        {
            foreach (var item in GameManager.palabrasDisponibles)
            {
                if (item.image1 != "")
                {
                    for (int j = 0; j < item.piecesPuzzle.Count; j++)
                    {
                        if (item.piecesPuzzle[j] >= 4)
                        {
                            currentPuzzlePaquet.Add(item);
                            currentPuzzlePaquet.Add(item);
                            break;
                        }
                    }
                }
            }
        }



        CrearBinario();
    }

    private void ReiniciarPaqueteCat()
    {
        currentPuzzlePaquet.Clear();
        nextPuzzlePaquets.Clear();

        if (!acabado)
        {
            if (fase == 0)
                fase = 1;

            if (dificultad == 0)
                dificultad = 1;

            if (fase == 1)
            {
                List<PalabraBD> palabrasTotales = new List<PalabraBD>();

                for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                {
                    if (GameManager.palabrasDisponibles[i].paquet == 0)
                    {
                        if (GameManager.palabrasDisponibles[i].dificultCatalan == dificultad && GameManager.palabrasDisponibles[i].image1 != "" && GameManager.palabrasDisponibles[i].piecesPuzzle.Count > 0)
                        {
                            if (GameManager.palabrasDisponibles[i].piecesPuzzle[0] >= 4)
                            {
                                if (palabrasTotales.Count < 10 && dificultad == 1)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 6 && dificultad == 2)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 4 && dificultad == 3)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count == 20)
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



            }
            else if (fase == 2)
            {
                List<PalabraBD> palabrasTotales = new List<PalabraBD>();

                for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                {
                    if (GameManager.palabrasDisponibles[i].paquet == 0)
                    {
                        if (GameManager.palabrasDisponibles[i].dificultCatalan == dificultad && GameManager.palabrasDisponibles[i].image1 != "" && GameManager.palabrasDisponibles[i].piecesPuzzle.Count > 0)
                        {
                            if (GameManager.palabrasDisponibles[i].piecesPuzzle[0] >= 4)
                            {
                                if (palabrasTotales.Count < 10 && dificultad == 1)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 6 && dificultad == 2)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 4 && dificultad == 3)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count == 20)
                                    break;
                            }
                        }
                    }

                }
                List<PalabraBD> palabrasTotales2 = new List<PalabraBD>();

                for (int i = 0; i < palabrasTotales.Count; i++)
                {

                    if (i >= palabrasTotales.Count / 3)
                        palabrasTotales2.Add(palabrasTotales[i]);

                }


                for (int i = 0; i < palabrasTotales2.Count; i++)
                {
                    if (palabrasTotales2.Count / 2 > i)
                    {
                        currentPuzzlePaquet.Add(palabrasTotales2[i]);
                        currentPuzzlePaquet.Add(palabrasTotales2[i]);
                    }
                    else
                        nextPuzzlePaquets.Add(palabrasTotales2[i]);
                }
            }
            else
            {
                List<PalabraBD> palabrasTotales = new List<PalabraBD>();

                for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                {
                    if (GameManager.palabrasDisponibles[i].paquet == 0)
                    {
                        if (GameManager.palabrasDisponibles[i].dificultCatalan == dificultad && GameManager.palabrasDisponibles[i].image1 != "" && GameManager.palabrasDisponibles[i].piecesPuzzle.Count > 0)
                        {
                            if (GameManager.palabrasDisponibles[i].piecesPuzzle[0] >= 4)
                            {
                                if (palabrasTotales.Count < 10 && dificultad == 1)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 6 && dificultad == 2)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count < 4 && dificultad == 3)
                                {
                                    palabrasTotales.Add(GameManager.palabrasDisponibles[i]);
                                }
                                else if (palabrasTotales.Count == 20)
                                    break;
                            }
                        }
                    }

                }
                List<PalabraBD> palabrasTotales2 = new List<PalabraBD>();

                for (int i = 0; i < palabrasTotales.Count; i++)
                {

                    if (i >= palabrasTotales.Count / 3)
                        palabrasTotales2.Add(palabrasTotales[i]);

                }


                for (int i = 0; i < palabrasTotales2.Count; i++)
                {
                    if (palabrasTotales2.Count / 2 >= i)
                    {
                        currentPuzzlePaquet.Add(palabrasTotales2[i]);
                        currentPuzzlePaquet.Add(palabrasTotales2[i]);
                    }
                }

            }
        }
        else
        {
            foreach (var item in GameManager.palabrasDisponibles)
            {
                if (item.image1 != "")
                {
                    for (int j = 0; j < item.piecesPuzzle.Count; j++)
                    {
                        if (item.piecesPuzzle[j] >= 4)
                        {
                            currentPuzzlePaquet.Add(item);
                            currentPuzzlePaquet.Add(item);
                            break;
                        }
                    }
                }
            }
        }



        CrearBinario();
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
