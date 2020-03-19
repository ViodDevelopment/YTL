using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConvertFont : MonoBehaviour
{
    public List<Font> ourFonts = new List<Font>();
    public Text myText;

    public void Convert()
    {
        switch (SingletonLenguage.GetInstance().GetFont())
        {
            case SingletonLenguage.OurFont.IMPRENTA:
                myText.font =  ourFonts[0];
                break;
            case SingletonLenguage.OurFont.MANUSCRITA:
                myText.font = ourFonts[1];
                break;
            case SingletonLenguage.OurFont.MAYUSCULA:
                myText.text = myText.text.ToUpper();
                myText.font = ourFonts[2];
                break;
            default:
                myText.font = ourFonts[0];
                break;
        }
    }
}
