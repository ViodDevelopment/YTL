using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class CrearBinarioAntesDeBuild : MonoBehaviour
{
    private List<PalabraBD> palabrasPredeterminadas = new List<PalabraBD>();
    private List<FraseBD> frasesPredeterminadas = new List<FraseBD>();
    public bool actualizacion = false;
    // Start is called before the first frame update
    void Start()
    {
        if (actualizacion)
        {
            if (!File.Exists(Application.streamingAssetsPath + "/Update.dat"))
            {
                FileStream file = File.Create(Application.streamingAssetsPath + "/Update.dat");
                file.Close();
            }
        }
            
        ReadCSV();
        ReadCSVFrases();
        //ReadCSVLite();
        ConvertBinnary();
    }
    private void ReadCSVLite()
    {
        if (File.Exists(Application.streamingAssetsPath + "/PalabrasLite.csv"))
        {
            StreamReader streamReader = new StreamReader(Application.streamingAssetsPath + "/PalabrasLite.csv");
            bool ended = false;
            int fila = 0;
            while (!ended)
            {
                string data = streamReader.ReadLine();
                fila++;
                if (data == null || streamReader == null || fila > 999999)
                {
                    ended = true;
                    break;
                }
                var valor = data.Split(',');

                if (valor[0] != null && fila >= 0 && valor[0] != "")
                {
                    palabrasPredeterminadas.Add(new PalabraBD());
                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].id = fila;
                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].color = valor[0];
                    if (valor[1] != null || valor[1].Length > 0)
                    {
                        var imagenes = valor[1].Split(' ');

                        if (imagenes != null)
                        {
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image1 = imagenes[0];
                            if (imagenes.Length > 1)
                            {
                                palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image2 = imagenes[1];
                                if (imagenes.Length > 2)
                                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image3 = imagenes[2];
                                else
                                {
                                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image3 = palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image1;
                                }
                            }
                            else
                            {
                                palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image2 = palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image1;
                                palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image3 = palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image1;
                            }
                        }
                        else
                        {
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image1 = "";
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image2 = "";
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image3 = "";
                        }

                    }
                    else
                    {
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image1 = "";
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image2 = "";
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image3 = "";
                    }

                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].audio = valor[2];

                    if (valor[3] != "" || valor[3].Length > 0)
                    {
                        var pieces = valor[3].Split(' ');
                        for (int i = 0; i < pieces.Length; i++)
                        {
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].piecesPuzzle.Add(int.Parse(pieces[i]));
                        }
                        if (palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].piecesPuzzle.Count == 0)
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].piecesPuzzle.Add(4);
                    }

                    if (valor[4] != "")
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].imagePuzzle = int.Parse(valor[4]);
                    else
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].imagePuzzle = 1;

                    if (valor[5] != "")
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].dificultSpanish = int.Parse(valor[5]);
                    else
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].dificultSpanish = 1;

                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].nameSpanish = valor[6];
                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].silabasSpanish = valor[7];

                    if (valor[8] != "")
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].dificultCatalan = int.Parse(valor[8]);
                    else
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].dificultCatalan = 1;


                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].nameCatalan = valor[9];
                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].silabasCatalan = valor[10];
                    switch (valor[11])
                    {
                        case "palabra_frase":
                        case "":
                        case "x":
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].paquet = 0;
                            break;
                        case "color":
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].paquet = 2;
                            break;
                        case "animales":
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].paquet = 3;
                            break;
                        case "escuela":
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].paquet = 1;
                            break;
                    }

                    if(valor[12] != null ||valor[12] == "")
                    {
                        var articuloCast = valor[12].Split(' ');
                        foreach (var item in articuloCast)
                        {
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].articulosSpanish.Add(item);
                        }
                    }

                    if (valor[13] != null || valor[13] == "")
                    {
                        var audioArtCast = valor[13].Split(' ');
                        foreach (var item in audioArtCast)
                        {
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].audiosArticulosSpanish.Add(item);
                        }
                    }

                    if (valor[14] != null || valor[14] == "")
                    {
                        var articuloCat = valor[14].Split(' ');
                        foreach (var item in articuloCat)
                        {
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].articulosCatalan.Add(item);
                        }
                    }

                    if (valor[15] != null || valor[15] == "")
                    {
                        var audioArtCat = valor[15].Split(' ');
                        foreach (var item in audioArtCat)
                        {
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].audiosArticulosCatalan.Add(item);
                        }
                    }


                }
                if (valor.Length == 0)
                    ended = true;
            }
        }
        else print("no existe");
    }

    private void ReadCSV()
    {
        if (File.Exists(Application.streamingAssetsPath + "/Actualizacion.csv"))
        {
            if (File.Exists(Application.streamingAssetsPath + "/Palabras.csv"))
            {
                File.Delete(Application.streamingAssetsPath + "/Palabras.csv");
                File.Move(Application.streamingAssetsPath + "/Actualizacion.csv", Application.streamingAssetsPath + "/Palabras.csv");
            }
            else
                File.Move(Application.streamingAssetsPath + "/Actualizacion.csv", Application.streamingAssetsPath + "/Palabras.csv");

        }

        if (File.Exists(Application.streamingAssetsPath + "/Palabras.csv"))
        {
            StreamReader streamReader = new StreamReader(Application.streamingAssetsPath + "/Palabras.csv");
            bool ended = false;
            int fila = 0;
            while (!ended)
            {
                string data = streamReader.ReadLine();
                fila++;
                
                if (data == null || streamReader == null || fila > 999999)
                {
                    ended = true;
                    break;
                }
                var valor = data.Split(',');
                if (valor[2] != null && fila > 2 && valor[2] != "")
                {
                    palabrasPredeterminadas.Add(new PalabraBD());
                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].id = palabrasPredeterminadas.Count - 1;
                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].color = valor[0];
                    if (valor[1] != null || valor[1].Length > 0)
                    {
                        var imagenes = valor[1].Split(' ');

                        if (imagenes != null)
                        {
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image1 = imagenes[0];
                            if (imagenes.Length > 1)
                            {
                                palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image2 = imagenes[1];
                                if (imagenes.Length > 2)
                                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image3 = imagenes[2];
                                else
                                {
                                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image3 = palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image1;
                                }
                            }
                            else
                            {
                                palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image2 = palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image1;
                                palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image3 = palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image1;
                            }
                        }
                        else
                        {
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image1 = "";
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image2 = "";
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image3 = "";
                        }

                    }
                    else
                    {
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image1 = "";
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image2 = "";
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].image3 = "";
                    }

                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].audio = valor[2];

                    if (valor[3] != "" || valor[3].Length > 0)
                    {
                        var pieces = valor[3].Split(' ');
                        for (int i = 0; i < pieces.Length; i++)
                        {
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].piecesPuzzle.Add(int.Parse(pieces[i]));
                        }
                        if (palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].piecesPuzzle.Count == 0)
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].piecesPuzzle.Add(0);
                    }

                    if (valor[4] != "")
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].imagePuzzle = int.Parse(valor[4]);
                    else
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].imagePuzzle = 0;

                    if (valor[5] != "")
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].dificultSpanish = int.Parse(valor[5]);
                    else
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].dificultSpanish = 1;

                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].nameSpanish = valor[6];
                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].silabasSpanish = valor[7];

                    if (valor[8] != "")
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].dificultCatalan = int.Parse(valor[8]);
                    else
                        palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].dificultCatalan = 1;


                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].nameCatalan = valor[9];
                    palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].silabasCatalan = valor[10];
                    switch (valor[11])
                    {
                        case "palabra_frase":
                        case "":
                        case "x":
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].paquet = 0;
                            break;
                        case "color":
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].paquet = 2;
                            break;
                        case "animales1":
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].paquet = 3;
                            break;
                        case "escuela":
                            palabrasPredeterminadas[palabrasPredeterminadas.Count - 1].paquet = 1;
                            break;
                    }


                }

                if (valor.Length == 0)
                    ended = true;
            }
        }
        else print("no existe");
    }

    private void ReadCSVFrases()
    {
        if (File.Exists(Application.streamingAssetsPath + "/Frases.csv"))
        {
            StreamReader streamReader = new StreamReader(Application.streamingAssetsPath + "/Frases.csv");
            bool ended = false;
            int fila = 0;
            while (!ended)
            {
                string data = streamReader.ReadLine();
                fila++;
                if (data == null || streamReader == null || fila > 999999)
                {
                    ended = true;
                    break;
                }
                var valor = data.Split(',');

                if (valor[0] != null && fila > 2 && valor[0] != "")
                {
                    frasesPredeterminadas.Add(new FraseBD());
                    frasesPredeterminadas[frasesPredeterminadas.Count - 1].id = frasesPredeterminadas.Count - 1;
                    var imagenes = valor[0].Split(' ');

                    if (imagenes != null)
                    {
                        frasesPredeterminadas[frasesPredeterminadas.Count - 1].image = imagenes[0];
                        if (imagenes.Length > 1)
                        {
                            frasesPredeterminadas[frasesPredeterminadas.Count - 1].image2 = imagenes[1];
                        }
                        else
                        {
                            frasesPredeterminadas[frasesPredeterminadas.Count - 1].image2 = frasesPredeterminadas[frasesPredeterminadas.Count - 1].image;
                        }
                    }
                    else
                    {
                        frasesPredeterminadas[frasesPredeterminadas.Count - 1].image = "";
                        frasesPredeterminadas[frasesPredeterminadas.Count - 1].image2 = "";
                    }


                    frasesPredeterminadas[frasesPredeterminadas.Count - 1].sound = valor[1];

                    if (valor[2] != "" || valor[2].Length > 0)
                    {
                        var pieces = valor[2].Split(' ');
                        for (int i = 0; i < pieces.Length; i++)
                        {
                            frasesPredeterminadas[frasesPredeterminadas.Count - 1].piecesPuzzle.Add(int.Parse(pieces[i]));
                        }
                        if (frasesPredeterminadas[frasesPredeterminadas.Count - 1].piecesPuzzle.Count == 0)
                            frasesPredeterminadas[frasesPredeterminadas.Count - 1].piecesPuzzle.Add(4);
                    }

                    if (valor[3] != "")
                        frasesPredeterminadas[frasesPredeterminadas.Count - 1].imagePuzzle = int.Parse(valor[3]);
                    else
                        frasesPredeterminadas[frasesPredeterminadas.Count - 1].imagePuzzle = 1;

                    if (valor[4] != "")
                        frasesPredeterminadas[frasesPredeterminadas.Count - 1].dificultadSpanish = int.Parse(valor[4]);
                    else
                        frasesPredeterminadas[frasesPredeterminadas.Count - 1].dificultadSpanish = 1;

                    frasesPredeterminadas[frasesPredeterminadas.Count - 1].fraseCastellano = valor[5];

                    if (valor[6] != "")
                        frasesPredeterminadas[frasesPredeterminadas.Count - 1].dificultadCatalan = int.Parse(valor[6]);
                    else
                        frasesPredeterminadas[frasesPredeterminadas.Count - 1].dificultadCatalan = 1;


                    frasesPredeterminadas[frasesPredeterminadas.Count - 1].fraseCatalan = valor[7];

                    switch (valor[8])
                    {
                        case "palabra_frase":
                        case "":
                        case "x":
                            frasesPredeterminadas[frasesPredeterminadas.Count - 1].paquet = 0;
                            break;
                        case "color":
                            frasesPredeterminadas[frasesPredeterminadas.Count - 1].paquet = 2;
                            break;
                        case "animales1":
                            frasesPredeterminadas[frasesPredeterminadas.Count - 1].paquet = 3;
                            break;
                        case "escuela":
                            frasesPredeterminadas[frasesPredeterminadas.Count - 1].paquet = 1;
                            break;
                    }
                    if (valor.Length == 0)
                        ended = true;
                }
            }

        }
        else print("no existe");


    }

    private void ConvertBinnary()
    {
        if (!File.Exists(Application.streamingAssetsPath + "/PalabrasBinario.dat"))
        {
            print("convertido en Binario");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.streamingAssetsPath + "/PalabrasBinario.dat");

            DatesToSave datos = new DatesToSave();
            datos.ChangeDates(palabrasPredeterminadas);

            bf.Serialize(file, datos);

            file.Close();
        }

        if (!File.Exists(Application.streamingAssetsPath + "/FrasesBinario.dat"))
        {
            print("convertido en Binario las Frases");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.streamingAssetsPath + "/FrasesBinario.dat");

            DatesFrasesToSave datos = new DatesFrasesToSave();
            datos.ChangeDates(frasesPredeterminadas);

            bf.Serialize(file, datos);

            file.Close();
        }
    }
}
