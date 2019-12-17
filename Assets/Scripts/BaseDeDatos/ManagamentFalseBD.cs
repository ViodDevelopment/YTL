using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ManagamentFalseBD : MonoBehaviour
{

    public static ManagamentFalseBD management;
    public AudioSource audioSource;

    [SerializeField] private List<PalabraBD> palabrasPredeterminadass = new List<PalabraBD>();
    private List<PalabraBD> palabrasGuardadas = new List<PalabraBD>();
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
                    if (GameManager.configurartion == null)
                        GameManager.configurartion = new Configurartion();

                    management.SaveConfig();
                    management.LoadConfig();
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

                if (File.Exists(nameRutePassword))
                {
                    management.LoadPassword();
                }
                else if (!File.Exists(nameRutePassword))
                {
                    management.SavePassword();
                    management.LoadPassword();
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
        int count = 0;

        #region palabras
        #region Agenda
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "agenda";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "agenda";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "a-gen-da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "agenda";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "a-gen-da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 1;
        count++;
        #endregion
        #region Agua
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "agua_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "agua_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "agua_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "agua";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "agua";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "a-gua";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "aigua";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ai-gua";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Almuerzo
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "almuerzo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "almuerzo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "al-muer-zo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "esmorzar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "es-mor-zar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Amarilla
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "amarillo_f";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "amarilla";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "a-ma-ri-lla";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "groga";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "gro-ga";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 2;
        count++;
        #endregion
        #region Amarillo
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "amarillo_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "amarillo_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "amarillo_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "amarillo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "amarillo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "a-ma-ri-llo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "groc";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "groc";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 2;
        count++;
        #endregion
        #region Asustada
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "asustada_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "asustada_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "asustada_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "asustada_f";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "asustada";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "a-sus-ta-da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "espantada";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "es-pan-ta-da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Asustado
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "asustado_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "asustado_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "asustado_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "asustado";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 9;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "asustado";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "a-sus-ta-do";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "espantat";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "es-pan-tat";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Azul
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "azul_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "azul_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "azul_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "azul";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "azul";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "a-zul";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "blau";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "blau";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 2;
        count++;
        #endregion
        #region Azul-a
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "azul_f";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "azul";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "a-zul";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "blava";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "bla-va";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 2;
        count++;
        #endregion
        #region Baila
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#5bb030";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "baila";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "baila";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "bai-la";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "balla";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ba-lla";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Bailar
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#5bb030";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "bailar_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "bailar_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "bailar_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "bailar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "bailar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "bai-lar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "ballar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ba-llar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Blanca
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "blanco_f";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "blanca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "blan-ca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "blanca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "blan-ca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 2;
        count++;
        #endregion
        #region Blanco
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "blanco_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "blanco_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "blanco_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "blanco";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "blanco";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "blan-co";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "blanc";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "blanc";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 2;
        count++;
        #endregion
        #region Boca
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "boca_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "boca_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "boca_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "boca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "boca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "bo-ca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "boca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "bo-ca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Caballo
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "caballo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "caballo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ca-ba-llo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "cavall";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ca-vall";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 3;
        count++;
        #endregion
        #region Cabeza
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "cabeza_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "cabeza_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "cabeza_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "cabeza";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "cabeza";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ca-be-za";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "cap";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "cap";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Cama
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "cama_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "cama_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "cama_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "cama";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "cama";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ca-ma";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "llit";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "llit";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Camiseta
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "camiseta_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "camiseta_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "camiseta_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "camiseta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "camiseta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ca-mi-se-ta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "samarreta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "sa-mar-reta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Canta
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#5bb030";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "canta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "canta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "can-ta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "canta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "can-ta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 3;
        count++;
        #endregion
        #region Cantar
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#5bb030";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "cantar_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "cantar_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "cantar_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "cantar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "cantar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "can-tar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "cantar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "can-tar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Casa
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "casa_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "casa_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "casa_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "casa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "casa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ca-sa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "casa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ca-sa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Cena
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "cena";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "cena";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ce-na";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "sopar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "so-par";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Cerdo
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "cerdo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "cerdo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "cer-do";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "porc";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "porc";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 3;
        count++;
        #endregion
        #region Clase
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "clase";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "clase";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "cla-se";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "classe";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "clas-se";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 1;
        count++;
        #endregion
        #region Coche
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "coche_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "coche_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "coche_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "coche";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "coche";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "co-che";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "cotxe";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "cot-xe";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Cola
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "cola";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "cola";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "co-la";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "cola";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "co-la";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Color
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "color";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "color";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "co-lor";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "color";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "co-lor";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Come
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#5bb030";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "come";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "come";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "co-me";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "menja";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "men-ja";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Comedor
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "comedor";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "comedor";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "co-me-dor";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "menjador";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "men-ja-dor";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Con
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#988DAC";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "con";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "con";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "con";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "amb";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "amb";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Contenta
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "contenta_f";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "contenta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "con-ten-ta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "contenta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "con-ten-ta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Contento
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "contento_m";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "contento";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "con-ten-to";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "content";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "con-tent";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Corre
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#5bb030";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "corre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "corre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "co-rre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "corre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "co-rre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Correr
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#5bb030";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "correr_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "correr_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "correr_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "correr";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "correr";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "co-rrer";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "córrer";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "có-rrer";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Culo
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "culo_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "culo_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "culo_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "culo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "culo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "cu-lo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "cul";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "cul";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region De
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#988DAC";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "de";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "de";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "de";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "de";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "de";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Dedo
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
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
        #region Delfin
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "delfin";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "delfín";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "del-fín";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "dufí";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "du-fí";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 3;
        count++;
        #endregion
        #region Dientes
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "dientes";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "dientes";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "dien-tes";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "dents";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "dents";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region El
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#FF8BCE";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "el";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "el";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "el";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "el";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "el";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Elefante
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "delfin";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "elefante";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "e-le-fan-te";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "elefant";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "e-le-fant";//cambiar idk
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 3;
        count++;
        #endregion
        #region Enfadada
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "enfadado";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "enfadada";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "en-fa-da-da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "enfadada";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "en-fa-da-da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Enfadado
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "enfadado_m";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "enfadado";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "en-fa-da-do";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "enfadat";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "en-fa-dat";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Es
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#5bb030";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "es";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "es";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "es";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "és";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "és";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Escuela
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "escuela_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "escuela_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "escuela_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "escuela";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 16;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "escuela";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "es-cue-la";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "escola";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "es-co-la";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 1;
        count++;
        #endregion
        #region Está
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#5bb030";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "esta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "está";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "es-tá";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "està";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "es-tà";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Estuche
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "estuche";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "estuche";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "es-tu-che";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "estoig";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "es-toig";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 1;
        count++;
        #endregion
        #region Fresas
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "fresas_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "fresas_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "fresas_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "fresas";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "fresas";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "fre-sas";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "maduixes";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ma-dui-xes";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Gafas
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "gafas_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "gafas_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "gafas_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "gafas";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "gafas";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ga-fas";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "ulleres";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "u-lle-res";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Gallina
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "gallina";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "gallina";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ga-lli-na";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "gallina";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ga-lli-na";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 3;
        count++;
        #endregion
        #region Gallo
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "gallo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "gallo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ga-llo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "gall";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "gall";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 3;
        count++;
        #endregion
        #region Gato
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "gato_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "gato_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "gato_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "gato";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "gato";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ga-to";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "gat";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "gat";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Gimnasio
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "gimnasio";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "gimnasio";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "gim-na-sio";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "gimnàs";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "gim-nàs";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Goma
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "goma_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "goma_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "goma_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "goma";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "goma";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "go-ma";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "goma";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "go-ma";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Hormiga
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "hormiga";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "hormiga";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "hor-mi-ga";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "formiga";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "for-mi-ga";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 3;
        count++;
        #endregion
        #region Huevo
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "huevo_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "huevo_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "huevo_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "huevo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "huevo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "hue-vo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "ou";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ou";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Juega
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#5bb030";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "juega";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "juega";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "jue-ga";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "juga";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ju-ga";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Jugar
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#5bb030";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "jugar_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "jugar_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "jugar_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "jugar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "jugar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ju-gar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "jugar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ju-gar";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region La
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#FF8BCE";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "la";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "la";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "la";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "la";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "la";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Lápiz
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "lapiz_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "lapiz_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "lapiz_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "lapiz";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "lápiz";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "lá-piz";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "llapis";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "lla-pis";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Las
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#FF8BCE";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "las";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "las";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "las";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "las";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "las";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Lava
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#5bb030";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "lava";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "lava";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "la-va";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "renta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ren-ta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Leche
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "leche_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "leche_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "leche_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "leche";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "leche";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "le-che";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "llet";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "llet";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region León
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "leon";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "león";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "le-ón";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "lleó";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "lleó";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 3;
        count++;
        #endregion
        #region Libreta
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "libreta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "libreta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "li-bre-ta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "llibreta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "lli-bre-ta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 1;
        count++;
        #endregion
        #region Libro
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "libro";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "libro";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "li-bro";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "llibre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "lli-bre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 1;
        count++;
        #endregion
        #region Limón
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "limon_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "limon_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "limon_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "limon";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "limón";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "li-món";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "llimona";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "lli-mo-na";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Lleva
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#5bb030";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "lleva";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "lleva";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "lle-va";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "porta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "por-ta";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Lobo
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "lobo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "lobo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "lo-bo";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "llop";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "llop";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 3;
        count++;
        #endregion
        #region Los
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#FF8BCE";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "los";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "los";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "los";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "los";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "los";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Luna
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "luna_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "luna_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "luna_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "luna";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "luna";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "lu-na";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "lluna";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "llu-na";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Mamá
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "mama";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "mamá";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ma-má";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "mama";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ma-ma";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Mano
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "mano_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "mano_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "mano_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "mano";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "mano";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ma-no";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "mà";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "mà";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Manzana
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "manzana";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "manzana";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "man-za-na";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "poma";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "po-ma";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Mariposa
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "mariposa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "mariposa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ma-ri-po-sa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "papallona";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "pa-pa-llo-na";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 3;
        count++;
        #endregion
        #region Mesa
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "mesa_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "mesa_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "mesa_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "mesa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "mesa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "me-sa";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "taula";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "tau-la";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Mochila
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "mochila";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "mochila";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "mo-chi-la";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "motxilla";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "mot-xi-la"; //idk
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Mosca
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "mosca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "mosca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "mos-ca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "mosca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "mos-ca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 3;
        count++;
        #endregion
        #region Moto
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "moto_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "moto_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "moto_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "moto";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "moto";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "mo-to";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "moto";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "mo-to";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Muñeca
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "muneca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "muñeca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "mu-ñe-ca";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "nina";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ni-na";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region NaranjaFruta
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "naranja_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "naranja_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "naranja_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "naranja";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "naranja";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "na-ran-ja";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "taronja";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ta-ron-ja";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region NaranjaColor
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "naranja_06";//si peta es pq no estan las fotos
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "naranja_07";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "naranja_08";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "naranja";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "naranja";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "na-ran-ja";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "taronja";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ta-ron-ja";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 2;
        count++;
        #endregion
        #region Nariz
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "nariz_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "nariz_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "nariz_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "nariz";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 3;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "nariz";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "na-riz";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "nas";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "nas";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Negra
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "negro_f";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "negra";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ne-gra";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "negra";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ne-gra";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Negro
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#29a3da";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "negro_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "negro_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "negro_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "negro";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "negro";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ne-gro";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "ne-gre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ne-gre";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion
        #region Niña
        palabrasPredeterminadass.Add(new PalabraBD());
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].id = count;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].color = "#eb6424";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image1 = "nina_01";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image2 = "nina_02";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].image3 = "nina_03";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].audio = "nina";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].piecesPuzzle = 4;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].imagePuzzle = 0;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultSpanish = 2;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameSpanish = "niña";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasSpanish = "ni-ña";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].dificultCatalan = 1;
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].nameCatalan = "nena";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].silabasCatalan = "ne-na";
        palabrasPredeterminadass[palabrasPredeterminadass.Count - 1].paquet = 0;
        count++;
        #endregion




        #endregion
        //borrar esto de la lite



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
        //borrar frases lite
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
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].image = "moto_02";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].fraseCastellano = "Niña Pera Agua Pelota";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].fraseCatalan = "Nena Pera Aigua Pilota";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].fraseIngles = "Por ahora no";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].frasesFrances = "Oh mamma";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].sound = "sopa";
        count++;
        #endregion
        #region Frase3
        frasesPredeterminadas.Add(new FraseBD());
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].id = count;
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].image = "moto_01";
        frasesPredeterminadas[frasesPredeterminadas.Count - 1].image = "moto_02";
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

    public void SaveWordUser(PalabraBD _pal, bool _add)
    {
        if (_pal != null)
        {
            if (_add)
                palabrasUserGuardadas.Add(_pal);
            else
                palabrasUserGuardadas.Remove(_pal);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(nameRuteUser);

        DatesOfPlayer datos = new DatesOfPlayer();

        datos.AddPalabras(palabrasUserGuardadas);

        bf.Serialize(file, datos);

        file.Close();
    }

    public void LoadDatesOfPlayer()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(nameRuteUser, FileMode.Open);

        DatesOfPlayer datos = (DatesOfPlayer)bf.Deserialize(file);

        GameManager.palabrasUserDisponibles = datos.GetListOfPalabras();

        file.Close();

        GameManager.Instance.ChangeConfig();
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
    private List<PalabraBD> palabrasUser = new List<PalabraBD>();

    public void ChangeDates(List<PalabraBD> _palabras)
    {
        palabrasUser.Clear();
        foreach (PalabraBD p in _palabras)
        {
            palabrasUser.Add(p);
        }
    }

    public void AddPalabras(List<PalabraBD> _palabras)
    {
        foreach (PalabraBD p in _palabras)
        {
            palabrasUser.Add(p);
        }
    }

    public List<PalabraBD> GetListOfPalabras()
    {
        return palabrasUser;
    }
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
    public Configurartion config;
}