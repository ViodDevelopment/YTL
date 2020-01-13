using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GetFromGallery : MonoBehaviour
{
    public GameObject placeHolder, txtPlaceholder;
    Image img;

    GameManager gm;
    
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
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
                Texture2D texture = NativeGallery.LoadImageAtPath(path);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                img.sprite = MakeImgEven(texture);
                txtPlaceholder.SetActive(false);

            }
        }, "Select a PNG image", "image/png");

        Debug.Log("Permission result: " + permission);
    }

    void SetPhotoFromCamera()
    {
        if (gm.PhotoFromCam)
        {
            img.sprite = MakeImgEven((Texture2D)gm.PhotoFromCam.texture);
            txtPlaceholder.SetActive(false);
        }
    }
    Sprite MakeImgEven(Texture2D tex)
    {
        int maxSize, offset;

        if(tex.width == tex.height)
        {
            return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else if(tex.width < tex.height)
        {
            maxSize = tex.width;
            offset = (tex.height - maxSize) / 2;
            return Sprite.Create(tex, new Rect(0.0f, offset, tex.width, tex.width), new Vector2(0.5f, 0.5f), 100.0f);
        }
        else
        {
            maxSize = tex.height;
            offset = (tex.width - maxSize) /2;
            return Sprite.Create(tex, new Rect(offset, 0.0f, tex.height, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        
    }

    
}
