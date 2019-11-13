using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskController : MonoBehaviour
{
    public GameObject[] Masks;
    public float[] MaxMaskScale;
    public Color[] m_Colors;
    int currentMask =0;

    public SpriteRenderer m_Background;
    public SpriteRenderer m_Path;
    public Sprite m_Completed;

    private void Start()
    {
        m_Path.color = m_Colors[RandomColor()];
    }
    void Update()
    {
        if ((Input.touchCount > 0 ) || Input.GetMouseButton(0))
        {
            
            RaycastHit l_RaycastHit;
            if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Camera.main.transform.forward, out l_RaycastHit, 1000))
            {
                GameObject l_HitObj = l_RaycastHit.collider.gameObject;
                if (l_HitObj.CompareTag("Path") && l_HitObj == Masks[currentMask])
                {
                    Masks[currentMask].transform.localScale = new Vector3(Masks[currentMask].transform.localScale.x + Time.deltaTime, Masks[currentMask].transform.localScale.y, Masks[currentMask].transform.localScale.z);
                }
            }

        }
        if (Masks[currentMask].transform.localScale.x >= MaxMaskScale[currentMask])
            currentMask++;
        if (currentMask >= 3)
            m_Background.sprite = m_Completed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward*1000);
    }

    int RandomColor()
    {
        return Random.Range(0, m_Colors.Length);

    }


}
