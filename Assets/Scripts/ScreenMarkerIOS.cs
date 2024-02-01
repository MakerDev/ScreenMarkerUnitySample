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
    private static extern void _SetTextTileMode(
        string text, 
        string fontName, 
        float fontSize, 
        string colorString, 
        int angle, 
        int horizontalMargin, int verticalMargin);
    [DllImport("__Internal")]
    private static extern void _SetImageTileMode(
        byte[] imageBytes, 
        int length, 
        int angle, 
        int horizontalMargin, int verticalMargin);
    [DllImport("__Internal")]
    private static extern void _SetImageTileModeWithText(
        byte[] imageBytes, int length,
        string text,
        string fontName,
        float fontSize,
        string colorString,
        int angle, 
        int horizontalMargin, int verticalMargin);
    [DllImport("__Internal")]
    private static extern void _UnsetTextTileMode();
    [DllImport("__Internal")]
    private static extern void _UnsetImageTileMode();

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

    public void SetTextTileMode()
    {
        _SetTextTileMode("hello", null, 0.0f, "4c000000", 30, 50, 50);
    }

    public void SetImageTileMode()
    {
        var pngBytes = image.EncodeToPNG();
        _SetImageTileMode(pngBytes, pngBytes.Length, -30, 0, 20);
    }

    public void PrintTileTextAndImage()
    {
        var pngBytes = image.EncodeToPNG();
        _SetImageTileModeWithText(
            pngBytes, 
            pngBytes.Length, 
            "hello", 
            null, 
            0.0f,
            "4c000000", 30, 50, 50);
    }

    public void Reset()
    {
        _UnsetTextTileMode();
        _UnsetImageTileMode();
        _ShowScreenMarker();
    }
}
