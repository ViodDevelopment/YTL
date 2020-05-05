using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using System.IO;
#if UNITY_EDITOR
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
#endif

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
    static string DescriptionMicrophone = "Microphone is used to register the user words";

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
        gm = GameManager.Instance;

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

#if UNITY_EDITOR
    [PostProcessBuildAttribute(1)]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget != BuildTarget.iOS)
            return;
        // Get plist
        string plistPath = pathToBuiltProject + "/Info.plist";
        PlistDocument plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));
        // Get root
        PlistElementDict rootDict = plist.root;
        // Change value of NSMicrophoneUsageDescription in Xcode plist
        var buildKey = "NSMicrophoneUsageDescription";
        rootDict.SetString(buildKey, DescriptionMicrophone);
        // Write to file
        File.WriteAllText(plistPath, plist.WriteToString());
    }
#endif
}
