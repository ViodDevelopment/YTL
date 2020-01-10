using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleProportion : MonoBehaviour
{
    float Height;
    float Width;
    public List<GameObject> Objetos = new List<GameObject>();
    void Start()
    {
        Height = Camera.main.scaledPixelHeight;
        Width = Camera.main.pixelWidth;
        print(Mathf.Round(Width / Height * 100));
        if (Mathf.Round(Width / Height * 100) < Mathf.Round(1.5f * 100))
        {
            for (int i = 0; i < Objetos.Count; i++)
            {
                Objetos[i].transform.localScale *= 0.9f;

                if(i < 3)
                {
                    Objetos[i].transform.position += (Vector3.zero - Objetos[i].transform.position + Vector3.down) / 6;
                }
                else
                {
                    Objetos[i].transform.position += (Vector3.zero - new Vector3(Objetos[i].transform.position.x,0, Objetos[i].transform.position.z) + Vector3.down * 2.5f) / 6;
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
