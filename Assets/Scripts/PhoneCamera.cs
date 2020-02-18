﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PhoneCamera : MonoBehaviour
{
    bool camAvaliable;
    WebCamTexture backCam;
    Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    bool wasActive;

    public Button buttonMakePhoto, buttonCancel;

    GameManager gm;

    private void Awake()
    {
        ReloadCam();
    }

    // Start is called before the first frame update
    void Start()
    {
       

        buttonMakePhoto.onClick.AddListener(delegate { TakePhoto(); });
        buttonCancel.onClick.AddListener(delegate { CancelPhoto(); });

        ReloadCam();
 
    }


    // Update is called once per frame
    void Update()
    {
        if (!camAvaliable) return;

        float ratio = (float)backCam.width / (float)backCam.height;
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

    }

    void TakePhoto()
    {
        gm = GameManager.GetInstance();

        gm.PhotoFromCam = background.texture;

        //try
        //{
        //    backCam.Stop();
        //}
        //catch { Debug.Log("Couldn't stop the camera"); }
    }

    void CancelPhoto()
    {

        //try
        //{
        //    backCam.Stop();
        //}
        //catch { Debug.Log("Couldn't stop the camera"); }
    }

    void ReloadCam()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera detected");
            camAvaliable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);

            }

        }

        if (backCam == null)
        {
            Debug.Log("Unable to find back camera");
            return;
        }

        backCam.Play();

        background.texture = backCam;

        camAvaliable = true;
    }
}
