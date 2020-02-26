using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reescale3DObj : MonoBehaviour
{

    public List<GameObject> objToRescale;
    public GameObject path;
    public float repositionFactor, quatrotercios;


    void GetObjects()
    {
        GameObject l_instance = GameObject.Find("CubeArray");
        objToRescale.Add(l_instance);
        for(int i = 0; i < l_instance.transform.childCount; i++)
        {
            objToRescale.Add(l_instance.transform.GetChild(i).gameObject);
        }

    }

    private void Start()
    {
        GetObjects();
        quatrotercios = (float)4/3;
        repositionFactor  = (float)16/9;

        foreach (GameObject go in objToRescale)
        {
            go.transform.position = new Vector3(go.transform.position.x / repositionFactor, go.transform.position.y / repositionFactor, go.transform.position.z);
            go.transform.localScale = new Vector3(go.transform.localScale.x / repositionFactor, go.transform.localScale.y / repositionFactor, go.transform.localScale.z);
        }

        path.transform.position = new Vector3(path.transform.position.x / repositionFactor, path.transform.position.y / repositionFactor, path.transform.position.z);
        path.transform.localScale = new Vector3(path.transform.localScale.x / repositionFactor, path.transform.localScale.y, path.transform.localScale.z / repositionFactor);

        repositionFactor = GetComponent<Camera>().aspect;

        if ((int)(repositionFactor*100) == (int)(quatrotercios*100))
        {

            repositionFactor = 1.54f;
            foreach (GameObject go in objToRescale)
            {
                go.transform.position = new Vector3(go.transform.position.x * repositionFactor, go.transform.position.y * repositionFactor, go.transform.position.z);
                go.transform.localScale = new Vector3(go.transform.localScale.x * repositionFactor, go.transform.localScale.y * repositionFactor, go.transform.localScale.z);
            }

            repositionFactor = 1.38f;

            path.transform.position = new Vector3(path.transform.position.x * repositionFactor, path.transform.position.y * repositionFactor, path.transform.position.z);
            path.transform.localScale = new Vector3(path.transform.localScale.x * repositionFactor, path.transform.localScale.y, path.transform.localScale.z * repositionFactor);

        }
        else
        {
           
            foreach (GameObject go in objToRescale)
            {
                go.transform.position = new Vector3(go.transform.position.x * repositionFactor, go.transform.position.y * repositionFactor, go.transform.position.z);
                go.transform.localScale = new Vector3(go.transform.localScale.x * repositionFactor, go.transform.localScale.y * repositionFactor, go.transform.localScale.z);
            }

            path.transform.position = new Vector3(path.transform.position.x * repositionFactor, path.transform.position.y * repositionFactor, path.transform.position.z);
            path.transform.localScale = new Vector3(path.transform.localScale.x * repositionFactor, path.transform.localScale.y, path.transform.localScale.z * repositionFactor);

        }
    }

    
}
