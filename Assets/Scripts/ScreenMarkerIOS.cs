using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMarkerIOS : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void _InitScreenMarker(string userInfo);
    [DllImport("__Internal")]
    private static extern void _ShowScreenMarker();
    [DllImport("__Internal")]
    private static extern void _HideScreenMarker();

    [DllImport("__Internal")]
    private static extern void _SetImageSource(byte[] imageBytes, int length);

    [DllImport("__Internal")]
    private static extern void _SetTextTileMode(string text, string fontName, 
        float fontSize, string colorString, int angle, int horizontalMargin, int verticalMargin)


    [DllImport("__Internal")]
    private static extern void _SetImageTileMode(string imageFilePath, int angle, int horizontalMargin, int verticalMargin);


    public Texture2D image;

    void Start()
    {
        InitScreenMarker("12345");
    }

    public void InitScreenMarker(string userInfo)
    {
        _InitScreenMarker(userInfo);

        if (image != null)
        {
            var pngBytes = image.EncodeToPNG();
            _SetImageSource(pngBytes, pngBytes.Length);
        }
    }


    public void ShowScreenMarker()
    {
        _ShowScreenMarker();
    }

    public void HideScreenMarker()
    {
        _HideScreenMarker();
    }

    public void SetTextTileMode(string text, string font, float fontSize, string color, int angle, int horizontalMargin, int verticalMargin)
    {
        _SetTextTileMode(text, font, 0, color, angle, horizontalMargin, verticalMargin);
    }

    public void SetImageTileMode(string image, int angle, int horizontalMargin, int verticalMargin)
    {
        _SetImageTileMode(image, angle, horizontalMargin, verticalMargin);
    }

    public void PrintTileTextAndImage()
    {
        SetTextTileMode("hello", null, 0.0f, "4c000000", 30, 50, 50);
        // SetImageTileMode(imageSource, -30, 0, 10);
    }
}
