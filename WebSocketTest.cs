using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebSocketTest : MonoBehaviour
{
    public GameObject cube;
    public RawImage rawImage;
    private Texture2D tex;
    private ImageMa imim;
    private class ImageMa
    {
        public int width;
        public int height;
        public byte[] imagebytes;
    }
    void Start()
    {
        imim = new ImageMa();
        tex = new Texture2D(2, 2);
        var sub = new Subscriber<CompressedImage>("/cv_camera/image_raw/compressed", Callback);
        //Subscriber<StdString> strsub = new Subscriber<StdString>("/foobar", CallbackString);

    }

    void Update()
    {
        var imcopy = imim;
        if (imcopy.imagebytes != null)
        {
            if (tex.height != imcopy.height || tex.width != imcopy.width)
            {
                tex = new Texture2D(imcopy.width, imcopy.height, TextureFormat.RGB24, false);
            }
            tex.LoadImage(imcopy.imagebytes);
            tex.Apply();
            cube.GetComponent<Renderer>().material.mainTexture = tex;
            rawImage.texture = tex;
            rawImage.SetNativeSize();
        }
    }

    void Callback(CompressedImage msg)
    {
        Debug.Log("callback = ");
        byte[] imageBytes = Convert.FromBase64String(msg.data);
        if (msg.format.IndexOf("jpeg") >= 0)
        {
            int i = 4;
            for (; i < imageBytes.Length; i++)
            {
                if (imageBytes[i] == 192)
                {
                    break;
                }
            }
            i += 4;
            imim.height = (imageBytes[i++] << 8) | imageBytes[i++];
            imim.width = (imageBytes[i++] << 8) | imageBytes[i++];
            imim.imagebytes = imageBytes;
        }
    }

    void CallbackString(StdString msg)
    {
        Debug.Log("string data: " + msg.data.ToString());
    }

    private void OnDestroy()
    {
    }
}
