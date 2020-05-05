using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Configurartion
{
    public SingletonLenguage.Lenguage currentLenguaje = SingletonLenguage.Lenguage.CASTELLANO;
    public SingletonLenguage.OurFont currentFont = SingletonLenguage.OurFont.IMPRENTA;
    public int repetitionsOfExercise = 2;
    public bool palabrasConArticulo = false;
    public bool ayudaVisual = true;
    public bool refuerzoPositivo = true;
    public bool registrado = false;
    public int difficult = 1;
    public int paquete = 0;
}
