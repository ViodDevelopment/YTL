using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System.IO;

public class PhoneCamera : MonoBehaviour
{

    private bool m_CamAvaible;
    private WebCamTexture m_BackCam;
    private Texture m_DefaultBackground;
    public RawImage m_Background;
    public AspectRatioFitter fit;
    Texture newImage;


    void Start()
    {
    
        m_DefaultBackground = m_Background.texture;
        WebCamDevice[] m_Devices = WebCamTexture.devices;

        if (m_Devices.Length == 0)
        {
            Debug.Log("No camera detected");
            m_CamAvaible = false;
            return;
        }


        for (int i = 0; i < m_Devices.Length; i++)
        {
   
            if (!m_Devices[i].isFrontFacing)
            {
                m_BackCam = new WebCamTexture(m_Devices[i].name, Screen.width, Screen.height);
            }
        }

        if (m_BackCam == null)
        {
            Debug.Log("Unable to find back camera");
            return;
        }

        m_BackCam.Play();
        m_Background.texture = m_BackCam;
        m_CamAvaible = true;
    }



    void Update()
    {

        if (!m_CamAvaible)
            return;

        float l_Ratio = (float)m_BackCam.width / (float)m_BackCam.height;
        //fit.aspectRatio = l_Ratio;

        float l_ScaleY = m_BackCam.videoVerticallyMirrored ? -1f : 1f;
        m_Background.rectTransform.localScale = new Vector3(1f, l_ScaleY, 1f);

        int l_Orientation = -m_BackCam.videoRotationAngle;
        m_Background.rectTransform.localEulerAngles = new Vector3(0, 0, l_Orientation);


        if (Input.GetKeyDown(KeyCode.P) || ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            TakeTexture();
        }

    }


    public void TakeAShot()
    {
        StartCoroutine("TakePicture");
    }

    IEnumerator TakePicture()
    {
        ScreenCapture.CaptureScreenshot("Photo.png");
        yield return new WaitForEndOfFrame();
    }

    void TakeTexture()
    {
        newImage = m_BackCam;
    }

}