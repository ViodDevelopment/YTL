using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frases : MonoBehaviour
{
    public int dificultad = 1;
    public ManagementBD management;
    private List<FraseBD> frases = new List<FraseBD>();
    
    private void Start()
    {
        /*Init();
        foreach (PalabraBD p in GetRandomFrase().palabras)
        {
            print(p.nameSpanish);
        }*/
    }

    private void Init()
    {
        //management.ChangeDificultFrase(dificultad);
        frases = management.ReadSQliteFrase();
    }

    public FraseBD GetRandomFrase()
    {
        int random = Random.Range(0, frases.Count - 1);
        return frases[random];
    }
}
