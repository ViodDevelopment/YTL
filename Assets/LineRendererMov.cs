using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererMov : MonoBehaviour
{
    

    LineRenderer lineRenderer ;
    Vector3[]  myPoints;
    public LayerMask layerMask;
    public GameObject arrayCube;
    public List<GameObject> cubes;
    public Image background;
    public Sprite endSprite;
    public float countdown;
    public SceneManagement mScener;
    public float maxCD;
    float currentCD;
    public float distance;
    public Transform positionIni;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        currentCD = maxCD;
        int nChild = arrayCube.transform.childCount;

        for (int i = 0; i < nChild; i++)
        {
            cubes.Add(arrayCube.transform.GetChild(i).gameObject);
        }

        lineRenderer.SetPosition(0, positionIni.position);

    }

    void Update()
    {
        currentCD += Time.deltaTime;
        countdown -= Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            /*if (Physics.Raycast(ray, out hit, layerMask))
            {
                lineRenderer.positionCount += 1;
                Vector3 position = Camera.main.ViewportToWorldPoint(Input.mousePosition);
                position.z = 0;
                lineRenderer.SetPosition(lineRenderer.positionCount-1, position);
            }*/
            if (Physics.Raycast(ray, out hit))
            {

                for (int k = 0; k < cubes.Count; k++)
                {

                    if (k == 0 && hit.collider.gameObject == cubes[k] && currentCD >= maxCD)
                    {

                        currentCD = 0;
                        lineRenderer.positionCount += 1;
                        //Vector3 position = Camera.main.ViewportToWorldPoint(Input.mousePosition);
                        //position.z = 0;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.collider.transform.position);
                        cubes.RemoveAt(0);
                        hit.collider.gameObject.SetActive(false);
                        if (cubes.Count == 0)
                        {
                            EndGame();

                        }
                    }
                }
                if (hit.collider.gameObject == gameObject && Vector2.Distance(hit.point, cubes[0].transform.position) < distance && ((cubes.Count > 52 && hit.point.x < cubes[0].transform.position.x) || (cubes.Count > 45 && hit.point.y > cubes[0].transform.position.y) || (cubes.Count > 23 && hit.point.x > cubes[0].transform.position.x) || (cubes.Count >= 17 && hit.point.y > cubes[0].transform.position.y) || ((cubes.Count < 16 && hit.point.x < cubes[0].transform.position.x))))
                {
                   /*lineRenderer.positionCount += 1;
                    Vector3 position = hit.point;
                    position.z = 0;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);*/
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
}
