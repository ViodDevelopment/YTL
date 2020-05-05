using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
#endif
using UnityEngine;


public class PhoneMicrophone : MonoBehaviour
{
    AudioSource audioSource;
    static string DescriptionMicrophone = "The microphone is used to register the user own words";

    void Start()
    {

        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }

        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            audioSource.clip = Microphone.Start("Varios micrófonos (Realtek High Definition Audio)", true, 10, 44100);
            audioSource.Play();
        }


        if (Input.GetKeyUp(KeyCode.M))
        {
            Microphone.End("Varios micrófonos (Realtek High Definition Audio)");
        }

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