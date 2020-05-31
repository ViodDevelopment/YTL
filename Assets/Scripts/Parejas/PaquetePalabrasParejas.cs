using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PaquetePalabrasParejas
{
    private static PaquetePalabrasParejas instance;

    public List<PalabraBD> currentParejasPaquet = new List<PalabraBD>();
    private List<PalabraBD> donePalabrasPaquet = new List<PalabraBD>();
    private List<PalabraBD> nextPalabrasPaquet = new List<PalabraBD>();
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
        currentParejasPaquet.Clear();
        donePalabrasPaquet.Clear();
        nextPalabrasPaquet.Clear();
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

        foreach (PalabraBD item in instance.nextPalabrasPaquet)
        {
            item.SetPalabraActual();
        }

        foreach (PalabraBD item in instance.currentParejasPaquet)
        {
            item.SetPalabraActual();
        }

        foreach (PalabraBD item in instance.donePalabrasPaquet)
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
                        List<PalabraBD> palabras = new List<PalabraBD>();
                        List<PalabraBD> palabrasanimales = new List<PalabraBD>();
                        for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                        {
                            if (GameManager.palabrasDisponibles[i].paquet == 0)
                            {
                                if (GameManager.palabrasDisponibles[i].dificultSpanish == dificultad && GameManager.palabrasDisponibles[i].image1 != "")
                                    palabras.Add(GameManager.palabrasDisponibles[i]);
                            }
                            else
                            {
                                if (GameManager.palabrasDisponibles[i].image1 != "") //poner dificultad animales
                                    palabrasanimales.Add(GameManager.palabrasDisponibles[i]);
                            }
                        }

                        int numEnter = 0;
                        for (int i = 0; i < palabras.Count; i++)
                        {
                            if (palabras[i].dificultSpanish == dificultad && palabras[i].image1 != "")
                            {
                                if (numEnter < palabras.Count / 3)
                                {
                                    numEnter++;
                                    currentParejasPaquet.Add(palabras[i]);
                                }
                                else
                                {
                                    nextPalabrasPaquet.Add(palabras[i]);
                                }
                            }

                        }


                        for (int i = 0; i < palabrasanimales.Count; i++)
                        {

                            if (i <= (palabrasanimales.Count) / 3)
                            {
                                currentParejasPaquet.Add(palabrasanimales[i]);
                            }
                            else
                            {
                                nextPalabrasPaquet.Add(palabrasanimales[i]);
                            }


                        }

                    }
                    else
                    {
                        if (fase == 2)
                        {
                            List<PalabraBD> palabras = new List<PalabraBD>();
                            List<PalabraBD> palabrasanimales = new List<PalabraBD>();
                            for (int i = 0; i < nextPalabrasPaquet.Count; i++)
                            {
                                if (nextPalabrasPaquet[i].paquet == 0)
                                {
                                    if (nextPalabrasPaquet[i].dificultSpanish == dificultad && nextPalabrasPaquet[i].image1 != "")
                                        palabras.Add(nextPalabrasPaquet[i]);
                                }
                                else
                                {
                                    if (nextPalabrasPaquet[i].image1 != "") //poner dificultad animales
                                        palabrasanimales.Add(nextPalabrasPaquet[i]);
                                }
                            }

                            for (int i = 0; i < palabras.Count / 2; i++)
                            {
                                currentParejasPaquet.Add(palabras[i]);
                            }
                            for (int i = 0; i < palabrasanimales.Count / 2; i++)
                            {
                                currentParejasPaquet.Add(palabrasanimales[i]);
                            }


                            foreach (PalabraBD item in currentParejasPaquet)
                            {
                                nextPalabrasPaquet.Remove(item);
                            }

                        }
                        else if (fase == 3)
                        {
                            for (int i = 0; i < nextPalabrasPaquet.Count; i++)
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
                        List<PalabraBD> palabras = new List<PalabraBD>();
                        List<PalabraBD> palabrasanimales = new List<PalabraBD>();
                        for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                        {
                            if (GameManager.palabrasDisponibles[i].paquet == 0)
                            {
                                if (GameManager.palabrasDisponibles[i].dificultCatalan == dificultad && GameManager.palabrasDisponibles[i].image1 != "")
                                    palabras.Add(GameManager.palabrasDisponibles[i]);
                            }
                            else
                            {
                                if (GameManager.palabrasDisponibles[i].image1 != "") //poner dificultad animales
                                    palabrasanimales.Add(GameManager.palabrasDisponibles[i]);
                            }
                        }

                        int numEnter = 0;
                        for (int i = 0; i < palabras.Count; i++)
                        {
                            if (palabras[i].dificultCatalan == dificultad && palabras[i].image1 != "")
                            {
                                if (numEnter < palabras.Count / 3)
                                {
                                    numEnter++;
                                    currentParejasPaquet.Add(palabras[i]);
                                }
                                else
                                {
                                    nextPalabrasPaquet.Add(palabras[i]);
                                }
                            }

                        }


                        for (int i = 0; i < palabrasanimales.Count; i++)
                        {

                            if (i <= (palabrasanimales.Count) / 3)
                            {
                                currentParejasPaquet.Add(palabrasanimales[i]);
                            }
                            else
                            {
                                nextPalabrasPaquet.Add(palabrasanimales[i]);
                            }


                        }

                    }
                    else
                    {
                        if (fase == 2)
                        {
                            List<PalabraBD> palabras = new List<PalabraBD>();
                            List<PalabraBD> palabrasanimales = new List<PalabraBD>();
                            for (int i = 0; i < nextPalabrasPaquet.Count; i++)
                            {
                                if (nextPalabrasPaquet[i].paquet == 0)
                                {
                                    if (nextPalabrasPaquet[i].dificultCatalan == dificultad && nextPalabrasPaquet[i].image1 != "")
                                        palabras.Add(nextPalabrasPaquet[i]);
                                }
                                else
                                {
                                    if (nextPalabrasPaquet[i].image1 != "") //poner dificultad animales
                                        palabrasanimales.Add(nextPalabrasPaquet[i]);
                                }
                            }

                            for (int i = 0; i < palabras.Count / 2; i++)
                            {
                                currentParejasPaquet.Add(palabras[i]);
                            }
                            for (int i = 0; i < palabrasanimales.Count / 2; i++)
                            {
                                currentParejasPaquet.Add(palabrasanimales[i]);
                            }


                            foreach (PalabraBD item in currentParejasPaquet)
                            {
                                nextPalabrasPaquet.Remove(item);
                            }

                        }
                        else if (fase == 3)
                        {
                            for (int i = 0; i < nextPalabrasPaquet.Count; i++)
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

            currentParejasPaquet.Clear();
            nextPalabrasPaquet.Clear();
            donePalabrasPaquet.Clear();

            if (fase == 1)
            {
                List<PalabraBD> palabras = new List<PalabraBD>();
                List<PalabraBD> palabrasanimales = new List<PalabraBD>();
                for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                {
                    if (GameManager.palabrasDisponibles[i].paquet == 0)
                    {
                        if (GameManager.palabrasDisponibles[i].dificultSpanish == dificultad && GameManager.palabrasDisponibles[i].image1 != "")
                            palabras.Add(GameManager.palabrasDisponibles[i]);
                    }
                    else
                    {
                        if (GameManager.palabrasDisponibles[i].image1 != "") //poner dificultad animales
                            palabrasanimales.Add(GameManager.palabrasDisponibles[i]);
                    }
                }

                int numEnter = 0;
                for (int i = 0; i < palabras.Count; i++)
                {
                    if (palabras[i].dificultSpanish == dificultad && palabras[i].image1 != "")
                    {
                        if (numEnter < palabras.Count / 3)
                        {
                            numEnter++;
                            currentParejasPaquet.Add(palabras[i]);
                        }
                        else
                        {
                            nextPalabrasPaquet.Add(palabras[i]);
                        }
                    }

                }


                for (int i = 0; i < palabrasanimales.Count; i++)
                {

                    if (i <= (palabrasanimales.Count) / 3)
                    {
                        currentParejasPaquet.Add(palabrasanimales[i]);
                    }
                    else
                    {
                        nextPalabrasPaquet.Add(palabrasanimales[i]);
                    }


                }

            }
            else
            {
                if (fase == 2)
                {
                    List<PalabraBD> palabras = new List<PalabraBD>();
                    List<PalabraBD> palabrasanimales = new List<PalabraBD>();
                    for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                    {
                        if (GameManager.palabrasDisponibles[i].paquet == 0)
                        {
                            if (GameManager.palabrasDisponibles[i].dificultSpanish == dificultad && GameManager.palabrasDisponibles[i].image1 != "")
                                palabras.Add(GameManager.palabrasDisponibles[i]);
                        }
                        else
                        {
                            if (GameManager.palabrasDisponibles[i].image1 != "") //poner dificultad animales
                                palabrasanimales.Add(GameManager.palabrasDisponibles[i]);
                        }
                    }

                    int numEnter = 0;
                    List<PalabraBD> palabras2 = new List<PalabraBD>();
                    List<PalabraBD> palabrasanimales2 = new List<PalabraBD>();

                    for (int i = 0; i < palabras.Count; i++)
                    {
                        if (palabras[i].dificultSpanish == dificultad && palabras[i].image1 != "")
                        {
                            if (numEnter < palabras.Count / 3)
                            {
                                numEnter++;
                                donePalabrasPaquet.Add(palabras[i]);
                            }
                            else
                            {
                                palabras2.Add(palabras[i]);
                            }
                        }

                    }


                    for (int i = 0; i < palabrasanimales.Count; i++)
                    {

                        if (i <= (palabrasanimales.Count) / 3)
                        {
                            donePalabrasPaquet.Add(palabrasanimales[i]);
                        }
                        else
                        {
                            palabrasanimales2.Add(palabrasanimales[i]);
                        }
                    }

                    for (int i = 0; i < palabras2.Count; i++)
                    {
                        if (i < palabras2.Count / 2)
                            currentParejasPaquet.Add(palabras2[i]);
                        else
                            nextPalabrasPaquet.Add(palabras2[i]);

                    }
                    for (int i = 0; i < palabrasanimales2.Count; i++)
                    {
                        if (i < palabrasanimales2.Count / 2)
                            currentParejasPaquet.Add(palabrasanimales2[i]);
                        else
                            nextPalabrasPaquet.Add(palabrasanimales2[i]);
                    }

                }
                else if (fase == 3)
                {
                    List<PalabraBD> palabras = new List<PalabraBD>();
                    List<PalabraBD> palabrasanimales = new List<PalabraBD>();
                    for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                    {
                        if (GameManager.palabrasDisponibles[i].paquet == 0)
                        {
                            if (GameManager.palabrasDisponibles[i].dificultSpanish == dificultad && GameManager.palabrasDisponibles[i].image1 != "")
                                palabras.Add(GameManager.palabrasDisponibles[i]);
                        }
                        else
                        {
                            if (GameManager.palabrasDisponibles[i].image1 != "") //poner dificultad animales
                                palabrasanimales.Add(GameManager.palabrasDisponibles[i]);
                        }
                    }

                    int numEnter = 0;
                    List<PalabraBD> palabras2 = new List<PalabraBD>();
                    List<PalabraBD> palabrasanimales2 = new List<PalabraBD>();

                    for (int i = 0; i < palabras.Count; i++)
                    {
                        if (palabras[i].dificultSpanish == dificultad && palabras[i].image1 != "")
                        {
                            if (numEnter < palabras.Count / 3)
                            {
                                numEnter++;
                                donePalabrasPaquet.Add(palabras[i]);
                            }
                            else
                            {
                                palabras2.Add(palabras[i]);
                            }
                        }

                    }


                    for (int i = 0; i < palabrasanimales.Count; i++)
                    {

                        if (i <= (palabrasanimales.Count) / 3)
                        {
                            donePalabrasPaquet.Add(palabrasanimales[i]);
                        }
                        else
                        {
                            palabrasanimales2.Add(palabrasanimales[i]);
                        }
                    }

                    for (int i = 0; i < palabras2.Count; i++)
                    {
                        if (i < palabras2.Count / 2)
                            donePalabrasPaquet.Add(palabras2[i]);
                        else
                            currentParejasPaquet.Add(palabras2[i]);

                    }
                    for (int i = 0; i < palabrasanimales2.Count; i++)
                    {
                        if (i < palabrasanimales2.Count / 2)
                            donePalabrasPaquet.Add(palabrasanimales2[i]);
                        else
                            currentParejasPaquet.Add(palabrasanimales2[i]);
                    }
                }
                else if (fase == 4)
                {
                    for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                    {
                        currentParejasPaquet.Add(GameManager.palabrasDisponibles[i]);
                        donePalabrasPaquet.Add(GameManager.palabrasDisponibles[i]);
                    }
                }

            }
        }
        else
        {
            nextPalabrasPaquet.Clear();
            currentParejasPaquet.Clear();
            donePalabrasPaquet.Clear();
            foreach (var item in GameManager.palabrasDisponibles)
            {
                currentParejasPaquet.Add(item);
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
            currentParejasPaquet.Clear();
            nextPalabrasPaquet.Clear();
            donePalabrasPaquet.Clear();

            if (fase == 1)
            {
                List<PalabraBD> palabras = new List<PalabraBD>();
                List<PalabraBD> palabrasanimales = new List<PalabraBD>();
                for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                {
                    if (GameManager.palabrasDisponibles[i].paquet == 0)
                    {
                        if (GameManager.palabrasDisponibles[i].dificultCatalan == dificultad && GameManager.palabrasDisponibles[i].image1 != "")
                            palabras.Add(GameManager.palabrasDisponibles[i]);
                    }
                    else
                    {
                        if (GameManager.palabrasDisponibles[i].image1 != "") //poner dificultad animales
                            palabrasanimales.Add(GameManager.palabrasDisponibles[i]);
                    }
                }

                int numEnter = 0;
                for (int i = 0; i < palabras.Count; i++)
                {
                    if (palabras[i].dificultCatalan == dificultad && palabras[i].image1 != "")
                    {
                        if (numEnter < palabras.Count / 3)
                        {
                            numEnter++;
                            currentParejasPaquet.Add(palabras[i]);
                        }
                        else
                        {
                            nextPalabrasPaquet.Add(palabras[i]);
                        }
                    }

                }


                for (int i = 0; i < palabrasanimales.Count; i++)
                {

                    if (i <= (palabrasanimales.Count) / 3)
                    {
                        currentParejasPaquet.Add(palabrasanimales[i]);
                    }
                    else
                    {
                        nextPalabrasPaquet.Add(palabrasanimales[i]);
                    }


                }

            }
            else
            {
                if (fase == 2)
                {
                    List<PalabraBD> palabras = new List<PalabraBD>();
                    List<PalabraBD> palabrasanimales = new List<PalabraBD>();
                    for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                    {
                        if (GameManager.palabrasDisponibles[i].paquet == 0)
                        {
                            if (GameManager.palabrasDisponibles[i].dificultCatalan == dificultad && GameManager.palabrasDisponibles[i].image1 != "")
                                palabras.Add(GameManager.palabrasDisponibles[i]);
                        }
                        else
                        {
                            if (GameManager.palabrasDisponibles[i].image1 != "") //poner dificultad animales
                                palabrasanimales.Add(GameManager.palabrasDisponibles[i]);
                        }
                    }

                    int numEnter = 0;
                    List<PalabraBD> palabras2 = new List<PalabraBD>();
                    List<PalabraBD> palabrasanimales2 = new List<PalabraBD>();

                    for (int i = 0; i < palabras.Count; i++)
                    {
                        if (palabras[i].dificultCatalan == dificultad && palabras[i].image1 != "")
                        {
                            if (numEnter < palabras.Count / 3)
                            {
                                numEnter++;
                                donePalabrasPaquet.Add(palabras[i]);
                            }
                            else
                            {
                                palabras2.Add(palabras[i]);
                            }
                        }

                    }


                    for (int i = 0; i < palabrasanimales.Count; i++)
                    {

                        if (i <= (palabrasanimales.Count) / 3)
                        {
                            donePalabrasPaquet.Add(palabrasanimales[i]);
                        }
                        else
                        {
                            palabrasanimales2.Add(palabrasanimales[i]);
                        }
                    }

                    for (int i = 0; i < palabras2.Count; i++)
                    {
                        if (i < palabras2.Count / 2)
                            currentParejasPaquet.Add(palabras2[i]);
                        else
                            nextPalabrasPaquet.Add(palabras2[i]);

                    }
                    for (int i = 0; i < palabrasanimales2.Count; i++)
                    {
                        if (i < palabrasanimales2.Count / 2)
                            currentParejasPaquet.Add(palabrasanimales2[i]);
                        else
                            nextPalabrasPaquet.Add(palabrasanimales2[i]);
                    }

                }
                else if (fase == 3)
                {
                    List<PalabraBD> palabras = new List<PalabraBD>();
                    List<PalabraBD> palabrasanimales = new List<PalabraBD>();
                    for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                    {
                        if (GameManager.palabrasDisponibles[i].paquet == 0)
                        {
                            if (GameManager.palabrasDisponibles[i].dificultCatalan == dificultad && GameManager.palabrasDisponibles[i].image1 != "")
                                palabras.Add(GameManager.palabrasDisponibles[i]);
                        }
                        else
                        {
                            if (GameManager.palabrasDisponibles[i].image1 != "") //poner dificultad animales
                                palabrasanimales.Add(GameManager.palabrasDisponibles[i]);
                        }
                    }

                    int numEnter = 0;
                    List<PalabraBD> palabras2 = new List<PalabraBD>();
                    List<PalabraBD> palabrasanimales2 = new List<PalabraBD>();

                    for (int i = 0; i < palabras.Count; i++)
                    {
                        if (palabras[i].dificultCatalan == dificultad && palabras[i].image1 != "")
                        {
                            if (numEnter < palabras.Count / 3)
                            {
                                numEnter++;
                                donePalabrasPaquet.Add(palabras[i]);
                            }
                            else
                            {
                                palabras2.Add(palabras[i]);
                            }
                        }

                    }


                    for (int i = 0; i < palabrasanimales.Count; i++)
                    {

                        if (i <= (palabrasanimales.Count) / 3)
                        {
                            donePalabrasPaquet.Add(palabrasanimales[i]);
                        }
                        else
                        {
                            palabrasanimales2.Add(palabrasanimales[i]);
                        }
                    }

                    for (int i = 0; i < palabras2.Count; i++)
                    {
                        if (i < palabras2.Count / 2)
                            donePalabrasPaquet.Add(palabras2[i]);
                        else
                            currentParejasPaquet.Add(palabras2[i]);

                    }
                    for (int i = 0; i < palabrasanimales2.Count; i++)
                    {
                        if (i < palabrasanimales2.Count / 2)
                            donePalabrasPaquet.Add(palabrasanimales2[i]);
                        else
                            currentParejasPaquet.Add(palabrasanimales2[i]);
                    }
                }
                else if (fase == 4)
                {
                    for (int i = 0; i < GameManager.palabrasDisponibles.Count; i++)
                    {
                        currentParejasPaquet.Add(GameManager.palabrasDisponibles[i]);
                        donePalabrasPaquet.Add(GameManager.palabrasDisponibles[i]);
                    }
                }

            }
        }
        else
        {
            nextPalabrasPaquet.Clear();
            currentParejasPaquet.Clear();
            donePalabrasPaquet.Clear();
            foreach (var item in GameManager.palabrasDisponibles)
            {
                currentParejasPaquet.Add(item);
            }
        }

        CrearBinario();


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
