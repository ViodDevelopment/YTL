using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushTool : MonoBehaviour
{
    Texture2D tex;
    public int brushRight, brushLeft, brushUp, brushDown;
    public GameObject arrayCube;
    public List<GameObject> cubes;
    public Image background;
    public Sprite endSprite;
    public float countdown;
    public SceneManagement mScener;
    // Start is called before the first frame update
    void Start()
    {

        tex = gameObject.GetComponent<Renderer>().material.mainTexture as Texture2D;
        RaycastHit hit;
        Transform child;
        int nChild = arrayCube.transform.childCount;

        for (int i = 0; i < nChild; i++)
        {
            cubes.Add(arrayCube.transform.GetChild(i).gameObject);
        }

         
           for(int i = 0; i < tex.width; i++)
            {
                for(int j = 0; j < tex.height; j++)
                {
                    tex.SetPixel(i, j, Color.white);
                }
            }
        tex.Apply();
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0) EndGame();
        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                for (int k = 0; k < cubes.Count; k++)
                {
                    if (k == 0 && hit.collider.gameObject == cubes[k])
                    {
                        hit.collider.gameObject.SetActive(false);
                        cubes.RemoveAt(0);
                        if(cubes.Count == 0)
                        {
                            EndGame();
                        }
                    }
                      
                }
                if (hit.collider.gameObject == gameObject)
                {
                    for (int i = -brushLeft; i < brushRight; i++)
                        for (int j = -brushDown; j < brushUp; j++)
                        {
                            {
                                tex.SetPixel((int)(hit.textureCoord.x * tex.width) + i, (int)(hit.textureCoord.y * tex.height) + j, Color.red);
                            }
                        }
                }

                tex.Apply();

                //Debug.Log((int)(hit.textureCoord.x * tex.width) + " " + (int)(hit.textureCoord.y * tex.height));
            }

        }

        
    }

    void EndGame()
    {
        background.sprite = endSprite;
        StartCoroutine(changeScene());
    }

    IEnumerator changeScene()
    {
        yield return new WaitForSeconds(3);
        mScener.InicioScene(true);
    }
}
