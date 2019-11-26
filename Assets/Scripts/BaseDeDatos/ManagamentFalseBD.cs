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
    [SerializeField] private List<FraseBD> frasesPredeterminadas = new List<FraseBD>();
    private List<FraseBD> frasesGuardadas = new List<FraseBD>();
    private string nameRute, nameRuteFrase, nameRuteBolasMinijuegos, nameConfiguration;

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
                management = this;
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
                }


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
                    #endregion
                }
                if (!File.Exists(nameRute) || !existente)
                {
                    management.SaveDates();
                    management.LoadDates();
                }

                InitFrasesPredeterminadas();
                bool existenteFrase = true;

                if (File.Exists(nameRuteFrase))
                {
                    management.LoadDatesFrase();

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
                if (!File.Exists(nameRuteFrase) || !existenteFrase)
                {
                    management.SaveDatesFrase();
                    management.LoadDatesFrase();
                }

                if (File.Exists(nameRuteBolasMinijuegos))
                {
                    management.LoadBolasMinijuegos();
                }
                else if (!File.Exists(nameRuteBolasMinijuegos))
                {
                    management.SaveBolasMinijuegos();
                    management.LoadBolasMinijuegos();
                }
            }
        }
        else if (go != gameObject)
            Destroy(gameObject);
    }

    private void InitPalabrasPredeterminadas()
    {
        int count = 0;

        #region Agua
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "agua_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "agua_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "agua_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "agua";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Agua";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "A-gua";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Aigua";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "Ai-gua";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Boca
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
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
        #region Casa
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "casa_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "casa_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "casa_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "casa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Casa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "Ca-sa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Casa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "Ca-sa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Dedo
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "dedo_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "dedo_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "dedo_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "dedo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Dedo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "De-do";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Dit";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "Dit";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Gato
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "gato_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "gato_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "gato_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "gato";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Gato";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "Ga-to";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Gat";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "Gat";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Mano
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "mano_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "mano_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "mano_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "mano";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Mano";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "Ma-no";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Mà";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "Mà";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Mesa
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "mesa_2-01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "mesa_2-02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "mesa_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "mesa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Mesa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "Me-sa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Taula";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "Tau-la";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Moto
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
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
        #region Niña
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "nina_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "nina_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "nina_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "nina";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Niña";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "Ni-ña";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Nena";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "Ne-na";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Pan
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "pan_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "pan_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "pan_04";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "pan";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Pan";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "Pan";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Pa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "Pa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Pelota
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "pelota";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "pelota_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "pelota_04";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "pelota";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Pelota";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "Pe-lo-ta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Pilota";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "Pi-lo-ta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Pera
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
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
        #region Pie
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "pie_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "pie_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "pie_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "pie";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Pie";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "Pie";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Peu";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "Peu";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Sol
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "sol_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "sol_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "sol_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "sol";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Sol";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "Sol";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Sol";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "Sol";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Sopa
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "sopa_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "sopa_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "sopa_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "sopa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Sopa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "So-pa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Sopa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "So-pa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Vaso
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "Nombre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "vaso_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "vaso_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "vaso_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "vaso";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "Vaso";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "Va-so";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "Got";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "Got";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion

        foreach (PalabraBD p in palabrasPredeterminadass)
        {
            p.SeparateSilabas();
            p.SetPalabraActual();
        }
    }

    private void InitFrasesPredeterminadas()
    {
        int count = 0;
        #region Frase1
        frasesPredeterminadas.Add(new FraseBD());
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].id = count;
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].image = "moto_01";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].fraseCastellano = "Pera Moto Casa";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].fraseCatalan = "Pera Moto Casa";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].fraseIngles = "Por ahora no";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].frasesFrances = "Oh mamma";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].sound = "sopa";
        count++;
        #endregion
        #region Frase2
        frasesPredeterminadas.Add(new FraseBD());
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].id = count;
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].image = "moto_01";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].fraseCastellano = "Niña Pera Agua Pelota";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].fraseCatalan = "Nena Pera Aigua Pilota";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].fraseIngles = "Por ahora no";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].frasesFrances = "Oh mamma";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].sound = "sopa";
        count++;
        #endregion
        foreach (FraseBD f in frasesPredeterminadas)
        {
            f.SeparatePerPalabras();
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
            GameManager.m_CurrentToMinigame[i] = datos.bolasMinijuegos[i];
        }

        GameManager.currentMiniGame = datos.currentMiniGame;

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



    public void LoadDatesOfPlayer()
    {

    }

}


[Serializable]
class DatesToSave
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