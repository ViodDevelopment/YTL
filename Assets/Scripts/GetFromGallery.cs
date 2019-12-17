using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GetFromGallery : MonoBehaviour
{
    public GameObject placeHolder;
    Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = placeHolder.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                img.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

            }
        }, "Select a PNG image", "image/png");

        Debug.Log("Permission result: " + permission);
    }
}
