using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTrialCloned : MonoBehaviour
{
    public TrailRenderer m_Trail;

    private void Start()
    {
        m_Trail = GetComponent<TrailRenderer>();
    
    }

    void Update()
    {
        
            Plane l_objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);

            Ray l_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float l_rayDistance;
            if (l_objPlane.Raycast(l_Ray, out l_rayDistance))
                this.transform.position = l_Ray.GetPoint(l_rayDistance);

        if (Input.GetMouseButtonUp(0) && !(Input.touchCount>0) )
        {
            print("detected");
            Destroy(this.gameObject.GetComponent<SwipeTrialCloned>());
        }
    }

}
