using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FraseBD : MonoBehaviour
{
    public int id;
    public string frase;
    public string image;
    public string sound;
    public int idioma; //singletone.lenguage
    public int dificultad;
    public List<PalabraBD> palabras = new List<PalabraBD>();
}
