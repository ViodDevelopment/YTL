using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Configuration
{
    public SingletonLenguage.Lenguage currentLenguaje = SingletonLenguage.Lenguage.CASTELLANO;
    public SingletonLenguage.OurFont currentFont = SingletonLenguage.OurFont.IMPRENTA;
    public int repetitionsOfExercise = 2;
    public bool ayudaVisual = true;
    public bool refuerzoPositivo = true;
    public int difficult = 1;
    public int paquete = 0;
    public bool registrado = false;
    public bool palabrasConArticulo = false;
    public bool determinados = true;
    public bool listosParejasCompletado = false;
    public bool listosPuzzleCompletado = false;
    public bool listosBitCompletado = false;
    public bool yaParejasCompletado = false;
    public bool yaPuzzleCompletado = false;
    public bool yaBitCompletado = false;

}
