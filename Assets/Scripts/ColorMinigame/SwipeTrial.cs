using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTrial : MonoBehaviour
{
    public GameObject ClonedSwipe;
    bool m_CanClick = true;
    void LateUpdate()
    {
        print(m_CanClick);
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)|| Input.GetMouseButtonDown(0))
        {

            if (m_CanClick)
            {
                Plane l_objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);

                Ray l_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float l_rayDistance;
                if (l_objPlane.Raycast(l_Ray, out l_rayDistance))
                    Instantiate(ClonedSwipe, l_Ray.GetPoint(l_rayDistance), ClonedSwipe.transform.rotation);
            }

           
        }

        if(!m_CanClick && (Input.GetMouseButtonUp(0) || Input.GetTouch(0).phase == TouchPhase.Ended))
        m_CanClick = true;

    }

    public void ClickTesting()
    {
        m_CanClick = false;
    }
    
}
