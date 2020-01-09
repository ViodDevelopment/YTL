using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumPad : MonoBehaviour
{

    public void OpenNumPad()
    {
        TouchScreenKeyboard.Open("",TouchScreenKeyboardType.NumberPad);
    }

   
}
