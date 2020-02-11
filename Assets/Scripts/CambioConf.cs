using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CambioConf : MonoBehaviour
{
    SingletonLenguage.Lenguage last;
    public List<Text> textos = new List<Text>();

    private void Start()
    {
        last = SingletonLenguage.GetInstance().GetLenguage();
        if (last == SingletonLenguage.Lenguage.CATALAN)
            DoCatalan();
        else if (last == SingletonLenguage.Lenguage.CASTELLANO)
            DoCastellano();
    }

    private void Update()
    {
        if(SingletonLenguage.GetInstance().GetLenguage() != last)
        {
            last = SingletonLenguage.GetInstance().GetLenguage();
            if (last == SingletonLenguage.Lenguage.CATALAN)
                DoCatalan();
            else if (last == SingletonLenguage.Lenguage.CASTELLANO)
                DoCastellano();

        }
    }

    private void DoCatalan()
    {
        textos[0].text = "Tipus de lletra:";
    }

    private void DoCastellano()
    {
        textos[0].text = "Tipo de letra:";

    }
}
