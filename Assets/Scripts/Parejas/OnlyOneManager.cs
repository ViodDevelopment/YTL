using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyOneManager : MonoBehaviour
{
    private bool oneIsCatch = false;
    public GameObject go;

    public void Catch(bool _catch, GameObject _go)
    {
        oneIsCatch = _catch;
        go = _go;
    }

    public bool GetCatch()
    {
        return oneIsCatch;
    }
}
