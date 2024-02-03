using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{

    public class ScreenMarkerAndroidPlugin : MonoBehaviour, IScreenMarker
    {
        public enum FontType
        {
            NORMAL = 0,
            BOLD,
            ITALIC,
            BOLD_ITALIC,
        }

        public enum Gravity
        {
            Center = 0,
        }

        private AndroidJavaClass _unityClass;
        private AndroidJavaObject _unityActivity;
        private AndroidJavaClass _pluginClass;
        private AndroidJavaObject _pluginInstance;

        private const string PLUGIN_NAME = "com.eis.plugin.ScreenMarker"; // Change this to your package name


        public void InitScreenMarker(string userInfo, Texture2D defaultImage)
        {
            _unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _unityActivity = _unityClass.GetStatic<AndroidJavaObject>("currentActivity");

            if (_unityActivity == null)
            {
                Debug.LogError("No Unity Activity");
            }

            _pluginClass = new AndroidJavaClass(PLUGIN_NAME);

            _pluginClass.CallStatic("implementationTest");
            _pluginClass.CallStatic("setIsUnity", true); // This must be called to notify this app is working with unity.
            _pluginInstance = new AndroidJavaObject(PLUGIN_NAME, _unityActivity, userInfo);

            if (defaultImage != null)
            {
                var bitmap = ToAndroidBitmap(defaultImage);
                _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    _pluginInstance.Call("setImageSource", bitmap);
                }));
            }
        }

        public void ShowScreenMarker()
        {
            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call("showScreenMarker");
            }));
        }

        public void HideScreenMarker()
        {
            if (_pluginInstance == null)
            {
                Debug.LogError("Plugin not initilized");
            }

            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call("hideScreenMarker");
            }));
        }

        public void SetScreenMarkerAlpha(float alpha)
        {
            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call("setScreenMarkerAlpha", alpha);
            }));
        }

        public void AddTextWithRect(int x, int y, int width, int height, string text, string fontName, float fontSize, string colorString, float angle, int align, bool useSizeToFit)
        {
            var rect = new AndroidJavaObject("android.graphics.Rect", x, y, width, height);
            var font = ToAndroidTypeface(fontName, FontType.NORMAL);
            var colorInt = ColorStringToInt(colorString);
            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call("addTextWithRect", rect, text, font, (int)fontSize, angle, colorInt, align);
            }));
        }

        public void AddTextWithRectAndroid(Rect rect, string text)
        {
            var rectAndroid = new AndroidJavaObject("android.graphics.Rect", rect.x, rect.y, rect.width, rect.height);
            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call("addTextWithRect", rect, text);
            }));
        }

        public void AddTextWithCenterIOS(int x, int y, string text, string fontName, float fontSize, string colorString, float angle)
        {
            throw new NotSupportedException("This function is not supported on Android.");
        }

        public void ClearTextAll()
        {
            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call("clearTextAll");
            }));
        }

        public void SetTextAll(string text)
        {
            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call("setTextAll", text);
            }));
        }

        public void SetTextRotateAll(float angle)
        {
            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call("setTextRotationAll", angle);
            }));
        }

        public void SetTextColorAll(string colorString)
        {
            int colorInt = ColorStringToInt(colorString);

            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call("setTextColorAll", colorInt);
            }));
        }

        public void SetTextTileMode(
            string text, string fontName, float fontSize, string colorString,
            float angle,
            int horizontalMargin=50, int verticalMargin=50)
        {
            var font = ToAndroidTypeface(fontName, (FontType)(int)fontSize);
            int colorInt = ColorStringToInt(colorString);

            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call(
                    "setTextTileMode",
                    horizontalMargin, verticalMargin,
                    text, font, colorInt,
                    angle);
            }));
        }

        public void UnsetTextTileMode()
        {
            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call("unsetTextTileMode");
            }));
        }

        public void SetImage(Texture2D image)
        {
            var bitmap = ToAndroidBitmap(image);

            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call("setImage", bitmap);
            }));
        }

        public void SetImagePosition(int x, int y)
        {
            var point = new AndroidJavaObject("android.graphics.Point", x, y);
            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call(
                    "setImagePosition",
                    point);
            }));
        }

        public void SetImageRotation(float angle)
        {
            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call(
                    "setImageRotation",
                    (float)angle);
            }));
        }

        public void SetImageTileMode(Texture2D image, float angle, int horizontalMargin=0, int verticalMargin=20)
        {
            var bitmap = ToAndroidBitmap(image);

            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call(
                    "setImageTileMode",
                    bitmap,
                    angle);
            }));
        }

        public void SetImageTileModeWithText(
            Texture2D image,
            string text,
            string fontName, float fontSize, string colorString,
            float angle,
            int horizontalMargin=0, int verticalMargin=10)
        {
            var bitmap = ToAndroidBitmap(image);
            int colorInt = ColorStringToInt(colorString);
            var font = ToAndroidTypeface(fontName, FontType.NORMAL);
            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call(
                    "setImageTileModeWithText",
                    bitmap,
                    text, font, (int)fontSize, colorInt,
                    angle);
            }));
        }

        public void UnsetImageTileMode()
        {
            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _pluginInstance.Call("unsetImageTileMode");
            }));
        }

        private static int ColorStringToInt(string colorString)
        {
            return int.Parse(colorString, System.Globalization.NumberStyles.HexNumber);
        }

        private static AndroidJavaObject ToAndroidTypeface(string fontName, FontType fontType)
        {
            var assets = CallStaticOnce("android.content.res.Resources", "getAssets");
            return CallStaticOnce("android.graphics.Typeface", "create", fontName, (int)fontType);
        }

        private static AndroidJavaObject ToAndroidBitmap(Texture2D texture2D)
        {
            byte[] pngBytes = texture2D.EncodeToPNG();
            return CallStaticOnce("android.graphics.BitmapFactory", "decodeByteArray", pngBytes, 0, pngBytes.Length);
        }

        private static AndroidJavaObject CallStaticOnce(string className, string methodName, params object[] args)
        {
            using var ajc = new AndroidJavaClass(className);
            return ajc.CallStatic<AndroidJavaObject>(methodName, args);
        }
    }
}