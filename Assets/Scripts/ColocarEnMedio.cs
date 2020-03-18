using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColocarEnMedio : MonoBehaviour
{
    private void Awake()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height / 2)).y, gameObject.transform.position.z);
    }
}
