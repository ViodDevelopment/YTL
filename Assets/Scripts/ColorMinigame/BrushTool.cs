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
    public float maxCD;
    float currentCD;
    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        currentCD = maxCD;

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
        currentCD += Time.deltaTime;
        countdown -= Time.deltaTime;
        if (countdown <= 0) changeScene();
        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {

                for (int k = 0; k < cubes.Count; k++)
                {
                    
                    if (k == 0 && hit.collider.gameObject == cubes[k]  && currentCD>=maxCD)
                    {
                       
                        currentCD = 0;
                        
                        cubes.RemoveAt(0);
                        hit.collider.gameObject.SetActive(false);
                        if (cubes.Count == 0)
                        {
                            EndGame();
                       
                        }
                    }
                }
                if (hit.collider.gameObject == gameObject && Vector2.Distance(hit.point, cubes[0].transform.position)<distance &&((cubes.Count>52 && hit.point.x<cubes[0].transform.position.x)||(cubes.Count > 45 && hit.point.y > cubes[0].transform.position.y)||(cubes.Count > 23 && hit.point.x > cubes[0].transform.position.x)||(cubes.Count >= 17 && hit.point.y > cubes[0].transform.position.y)||((cubes.Count <16 && hit.point.x < cubes[0].transform.position.x))))
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
