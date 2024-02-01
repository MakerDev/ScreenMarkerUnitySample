using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScreenMarkerIOS : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void _InitScreenMarker(string userInfo);
    [DllImport("__Internal")]
    private static extern void _ShowScreenMarker();
    [DllImport("__Internal")]
    private static extern void _HideScreenMarker();

    [DllImport("__Internal")]
    private static extern void _SetImageSource(string imageFilePath);

    [DllImport("__Internal")]
    private static extern void _SetTextTileMode(string text, string font, string color, 
        int angle, int horizontalMargin, int verticalMargin);

    [DllImport("__Internal")]
    private static extern void _SetImageTileMode(string imageFilePath, int angle, int horizontalMargin, int verticalMargin);


    public string imageSource = "Assets/Sprites/hana_logo.png";

    void Start()
    {
        InitScreenMarker("12345");
    }

    public void InitScreenMarker(string userInfo)
    {
        _InitScreenMarker(userInfo);
        _SetImageSource(imageSource);
    }


    public void ShowScreenMarker()
    {
        _ShowScreenMarker();
    }

    public void HideScreenMarker()
    {
        _HideScreenMarker();
    }

    public void SetTextTileMode(string text, string font, string color, int angle, int horizontalMargin, int verticalMargin)
    {
        _SetTextTileMode(text, font, color, angle, horizontalMargin, verticalMargin);
    }

    public void SetImageTileMode(string image, int angle, int horizontalMargin, int verticalMargin)
    {
        _SetImageTileMode(image, angle, horizontalMargin, verticalMargin);
    }

    public void PrintTileTextAndImage()
    {
        SetTextTileMode("hello", null, "4c000000", 30, 0, 20);
        SetImageTileMode(imageSource, -30, 0, 10);
    }
}
