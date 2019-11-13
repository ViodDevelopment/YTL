using UnityEngine;
using System.Collections;
using UnityEditor;

public class OtherOptions : MonoBehaviour {
    
    [MenuItem("Window/DDP Creator/Feedback")]
    static void GotoComments()
    {
        Application.OpenURL(@"https://docs.google.com/forms/d/1PGQGMRniAr69FgcohDa548DlZbdTPUgnNVDAUjXcLiM/viewform");
    }

    
    [MenuItem("Window/DDP Creator/WebPreviews")]
    static void GotoWebPreviews()
    {
        Application.OpenURL(@"http://mumaireh.oracleapexservices.com");
    }

    [MenuItem("Window/DDP Creator/Documentation")]
    static void GotoDocumentation()
    {
        Application.OpenURL(@"https://docs.google.com/document/d/1t9M4GTlk3SI_9KBTDetu8y6ZnnGqBTvlfmQ5amvlEog/edit?usp=sharing");
    }

}
