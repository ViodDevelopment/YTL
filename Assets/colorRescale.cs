using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorRescale : MonoBehaviour
{
    float proportionOriginal;
    float proportion;
    // Start is called before the first frame update
    void Start()
    {
        proportionOriginal =1.77f;
        proportion = Camera.main.aspect;
        Debug.Log(proportion);
        this.gameObject.GetComponent<RectTransform>().localScale *= new Vector2(1,(proportionOriginal/proportion));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
