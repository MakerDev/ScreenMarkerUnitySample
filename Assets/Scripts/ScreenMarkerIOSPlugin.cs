using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMarkerIOSPlugin : MonoBehaviour, IScreenMarker
{
    [DllImport("__Internal")]
    private static extern void _InitScreenMarker(string userInfo);
    [DllImport("__Internal")]
    private static extern void _ShowScreenMarker();
    [DllImport("__Internal")]
    private static extern void _HideScreenMarker();
    [DllImport("__Internal")]
    private static extern void _SetScreenMarkerAlpha(float alpha);
    [DllImport("__Internal")]
    private static extern void _AddTextWithRect(
        int x, int y, int width, int height,
        string text, string fontName, float fontSize,
        string colorString,
        int angle,
        bool useSizeToFit);
    [DllImport("__Internal")]
    private static extern void _AddTextWithCenter(
        int x, int y,
        string text, string fontName, float fontSize,
        string colorString,
        int angle);
    [DllImport("__Internal")]
    private static extern void _ClearTextAll();
    [DllImport("__Internal")]
    private static extern void _SetTextAll(string text);
    [DllImport("__Internal")]
    private static extern void _SetTextRotationAll(int angle);
    [DllImport("__Internal")]
    private static extern void _SetTextColorAll(string colorString);
    [DllImport("__Internal")]
    private static extern void _SetTextFontAll(string fontName, float fontSize);
    [DllImport("__Internal")]
    private static extern void _SetTextTileMode(
        string text,
        string fontName,
        float fontSize,
        string colorString,
        int angle,
        int horizontalMargin, int verticalMargin);
    [DllImport("__Internal")]
    private static extern void _UnsetTextTileMode();

    [DllImport("__Internal")]
    private static extern void _SetImage(byte[] imageBytes, int length);
    [DllImport("__Internal")]
    private static extern void _SetImagePosition(int x, int y);
    [DllImport("__Internal")]
    private static extern void _SetImageRotation(int angle);
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
    private static extern void _UnsetImageTileMode();


    private const string DEFAULT_COLOR = "a0000000";


    public void InitScreenMarker(string userInfo, Texture2D image)
    {
        _InitScreenMarker(userInfo);

        if (image != null)
        {
            var pngBytes = image.EncodeToPNG();
            _SetImage(pngBytes, pngBytes.Length);
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

    public void SetScreenMarkerAlpha(float alpha)
    {
        _SetScreenMarkerAlpha(alpha);
    }

    public void AddTextWithRect(
        int x, int y, int width, int height,
        string text,
        string fontName,
        float fontSize,
        string colorString,
        float angle, int align, bool useSizeToFit)
    {
        if (string.IsNullOrEmpty(colorString))
        {
            colorString = DEFAULT_COLOR;
        }

        _AddTextWithRect(x, y, width, height, text, fontName, fontSize, colorString, (int)angle, useSizeToFit);
    }
    public void AddTextWithRect(
        int x, int y, int width, int height,
        string text)
    {

        _AddTextWithRect(x, y, width, height, text, null, 20, DEFAULT_COLOR, 0, true);
    }

    public void ClearTextAll()
    {
        _ClearTextAll();
    }

    public void SetTextAll(string text)
    {
        _SetTextAll(text);
    }

    public void SetTextRotateAll(float angle)
    {
        _SetTextRotationAll((int)angle);
    }

    public void SetTextColorAll(string colorString)
    {
        if (string.IsNullOrEmpty(colorString))
        {
            colorString = DEFAULT_COLOR;
        }

        _SetTextColorAll(colorString);
    }

    public void SetTextFontAll(string fontName, float fontSize)
    {
        _SetTextFontAll(fontName, fontSize);
    }

    public void SetTextTileMode(string text, string fontName, float fontSize, string colorString, float angle, int horizontalMargin = 50, int verticalMargin = 50)
    {
        if (string.IsNullOrEmpty(colorString))
        {
            colorString = DEFAULT_COLOR;
        }

        _SetTextTileMode(text, fontName, fontSize, colorString, (int)angle, horizontalMargin, verticalMargin);
    }

    public void UnsetTextTileMode()
    {
        _UnsetTextTileMode();
    }

    public void SetImage(Texture2D image)
    {
        var pngBytes = image.EncodeToPNG();
        _SetImage(pngBytes, pngBytes.Length);
    }

    public void SetImagePosition(int x, int y)
    {
        _SetImagePosition(x, y);
    }

    public void SetImageRotation(float angle)
    {
        _SetImageRotation((int)angle);
    }

    public void SetImageTileMode(Texture2D image, float angle, int horizontalMargin = 0, int verticalMargin = 10)
    {
        var pngBytes = image.EncodeToPNG();
        _SetImageTileMode(pngBytes, pngBytes.Length, (int)angle, horizontalMargin, verticalMargin);
    }

    public void SetImageTileModeWithText(
        Texture2D image,
        string text, string fontName, float fontSize,
        string colorString,
        float angle,
        int horizontalMargin = 0, int verticalMargin = 20)
    {
        var pngBytes = image.EncodeToPNG();

        if (colorString == null)
        {
            colorString = DEFAULT_COLOR;
        }

        _SetImageTileModeWithText(pngBytes, pngBytes.Length, text, fontName, fontSize, colorString, (int)angle, horizontalMargin, verticalMargin);
    }

    public void UnsetImageTileMode()
    {
        _UnsetImageTileMode();
    }
}
