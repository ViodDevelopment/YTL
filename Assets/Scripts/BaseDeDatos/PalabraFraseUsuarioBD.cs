using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalabraFraseUsuarioBD : MonoBehaviour
{
    public int id;
    public string palabra;
    public string silabas;
    public string image;
    public string sound;
    public int frase;//0 si no es frase y 1 si lo es
    public List<string> silabasSeparadas = new List<string>(); //palabras en el caso de la frase

    public void SeparateSilabas()//separa la linea de string de silabas segun el idioma a un lista en orden de las silabas.
    {
        string actualWord = "";
        actualWord = silabas;
        if (actualWord != "")
        {
            string currentSilaba = "";
            for (int i = 0; i < actualWord.Length; i++)
            {
                if (actualWord[i] != '-')
                {
                    currentSilaba += actualWord[i];
                }
                else
                {
                    silabasSeparadas.Add(currentSilaba);
                    currentSilaba = "";
                }
            }
            if (currentSilaba != "")
            {
                silabasSeparadas.Add(currentSilaba);
                currentSilaba = "";
            }
        }
    }

}
