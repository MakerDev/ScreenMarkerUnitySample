using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMarkerPlugin : MonoBehaviour
{
    private AndroidJavaClass _unityClass;
    private AndroidJavaObject _unityActivity;
    private AndroidJavaClass _pluginClass;
    private AndroidJavaObject _pluginInstance;

    [SerializeField]
    private Texture2D _image;
    [SerializeField]
    private string _pluginName = "com.eis.plugin.ScreenMarkerUnity";

    void Start()
    {
        //InitPlugin();
        _unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        _unityActivity = _unityClass.GetStatic<AndroidJavaObject>("currentActivity");

        if (_unityActivity == null)
        {
            Debug.LogError("No Unity Activity");
        }

        _pluginClass = new AndroidJavaClass(_pluginName);

        _pluginClass.CallStatic("implementationTest");
        _pluginClass.CallStatic("setIsUnity", true);
        _pluginInstance = new AndroidJavaObject(_pluginName, _unityActivity, "12345");
    }

    public void PrintBasic()
    {
        if (_pluginInstance == null)
        {
            Debug.LogError("Plugin not initilized");
        }

        _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            _pluginInstance.Call("setTextAll", "This is the moment");
            _pluginInstance.Call("showScreenMarker");
        }));
    }

    public void PrintTiledTextAndImage()
    {
        if (_pluginInstance == null)
        {
            Debug.LogError("Plugin not initilized");
        }

        //var bitmap = ToAndroidBitmap(_image);
        var bitmap = ToAndroidBitmap(_image);

        _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            /*
             * 호출 시 -30.0f와 같이 float 자료형으로 파라미터를 전달해야함. 그렇지 않을 경우,
             * 플러그인의 함수 시그니처와 맞지 않아 적절히 함수가 호출되지 않는다.
             */
            _pluginInstance.Call("setTextTileMode", 200, 60, "Hello", null, 20, 0x4c000000, -30.0f);
            _pluginInstance.Call("setImageTileMode", bitmap, -30.0f);
        }));
    }

    public void Reset()
    {
        if (_pluginInstance == null)
        {
            Debug.LogError("Plugin not initilized");
        }

        _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            _pluginInstance.Call("unsetImageTileMode");
            _pluginInstance.Call("unsetTextTileMode");
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


    private static AndroidJavaObject ToAndroidBitmap(Texture2D tex2D)
    {
        byte[] pngBytes = tex2D.EncodeToPNG();
        return CallStaticOnce("android.graphics.BitmapFactory", "decodeByteArray", pngBytes, 0, pngBytes.Length);
    }

    private static AndroidJavaObject CallStaticOnce(string className, string methodName, params object[] args)
    {
        using var ajc = new AndroidJavaClass(className);
        return ajc.CallStatic<AndroidJavaObject>(methodName, args);
    }
}
