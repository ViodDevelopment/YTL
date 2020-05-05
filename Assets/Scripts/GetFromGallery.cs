using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GetFromGallery : MonoBehaviour
{
    public GameObject placeHolder, txtPlaceholder;
    Image img;

    GameManager gm;

    public bool photoAvaliable;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
        img = placeHolder.GetComponent<Image>();

        NativeGallery.GetImageProperties("");
        photoAvaliable = true;
        if(NativeGallery.CheckPermission()!=NativeGallery.Permission.Granted)
        {
            NativeGallery.RequestPermission();
          
        }


    }

    // Update is called once per frame
    void Update()
    {
        SetPhotoFromCamera();
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
        if (gm.PhotoFromCam && photoAvaliable)
        {
            img.sprite = MakeImgEven(TextureToTexture2D(gm.PhotoFromCam));
            txtPlaceholder.SetActive(false);
            gm.PhotoFromCam = null;
            photoAvaliable = false;
        }
    }

    Sprite MakeImgEven(Texture2D tex)
    {
        int maxSize, offset;

        #if UNITY_IOS
         tex = RotateTexture(tex,true);
         tex = RotateTexture(tex, true);
         tex = FlipTexture(tex);
        #endif

        if (tex.width == tex.height)
        {
            return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new
           Vector2(0.5f, 0.5f), 100.0f);
        }
        else if (tex.width < tex.height)
        {
            maxSize = tex.width;
            offset = (tex.height - maxSize) / 2;
            return Sprite.Create(tex, new Rect(0.0f, offset, tex.width, tex.width), new
           Vector2(0.5f, 0.5f), 100.0f);
        }
        else
        {
            maxSize = tex.height;
            offset = (tex.width - maxSize) / 2;
            return Sprite.Create(tex, new Rect(offset, 0.0f, tex.height, tex.height), new
           Vector2(0.5f, 0.5f), 100.0f);
        }
    }

    private Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height,
       TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width,
       texture.height, 32);
        Graphics.Blit(texture, renderTexture);
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0,
       0);
        texture2D.Apply();
        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);
        return texture2D;
    }

    public void SetPhotoAvaliable(bool var)
    {
        photoAvaliable = var;
    }

    Texture2D RotateTexture(Texture2D origTexture, bool clockwise)
    {
        // Take the original Texture Color
        Color32[] orig = origTexture.GetPixels32();
        Color32[] rotated = new Color32[orig.Length];
        int w = origTexture.width;
        int h = origTexture.height;
        int iRotated, iOriginal;
        for (int j = 0; j < origTexture.height; ++j)
        {
            for (int i = 0; i < origTexture.width; ++i)
            {
                iRotated = (i + 1) * origTexture.height - j - 1;
                iOriginal = clockwise ? orig.Length - 1 - (j * origTexture.width + i) : j *
               origTexture.width + i;
                rotated[iRotated] = orig[iOriginal];
            }
        }
        // Create the new texture and add the new rotated pixels
        Texture2D rotatedTexture = new Texture2D(origTexture.height, origTexture.width);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }

    Texture2D FlipTexture(Texture2D original)
    {
        Texture2D flipped = new Texture2D(original.width, original.height);
        int xN = original.width;
        int yN = original.height;
        for (int i = 0; i < xN; i++)
        {
            for (int j = 0; j < yN; j++)
            {
                flipped.SetPixel(xN - i - 1, j, original.GetPixel(i, j));
            }
        }
        flipped.Apply();
        return flipped;
    }
}
