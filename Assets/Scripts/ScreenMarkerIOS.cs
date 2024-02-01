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
    private static extern void _SetTextTileMode(string text, string font, string color, 
        int angle, int horizontalMargin, int verticalMargin);

    [DllImport("__Internal")]
    private static extern void _SetImageTileMode(Texture2D image, int angle, int horizontalMargin, int verticalMargin);

    void Start()
    {
        InitScreenMarker("12345");
    }

    public void InitScreenMarker(string userInfo)
    {
        _InitScreenMarker(userInfo);
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
}
