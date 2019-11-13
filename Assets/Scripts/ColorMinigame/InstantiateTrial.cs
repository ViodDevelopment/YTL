using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateTrial : MonoBehaviour
{
    public GameObject m_SwipeTrial;
    GameObject m_CurrentTrial = null;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(m_CurrentTrial==null)
          m_CurrentTrial = Instantiate(m_SwipeTrial, Camera.main.ScreenToWorldPoint(Input.mousePosition), m_SwipeTrial.transform.rotation);

            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)|| Input.GetMouseButton(0))
            {
                Plane l_objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);

                Ray l_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float l_rayDistance;
                if (l_objPlane.Raycast(l_Ray, out l_rayDistance))
                    m_CurrentTrial.transform.position = l_Ray.GetPoint(l_rayDistance);
            }
        }

        else if (m_CurrentTrial != null)
        {
            //Destroy(m_CurrentTrial.GetComponent<SwipeTrial>());
            m_CurrentTrial = null;
        }

    }

}
