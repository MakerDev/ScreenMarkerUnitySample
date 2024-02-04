using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScreenMarker.Native;

namespace ScreenMarker
{
    public class ScreenMarkerPlugin : MonoBehaviour
    {
        private static IScreenMarker _screenMarker = null;

        public static IScreenMarker GetScreenMarker()
        {
            if (_screenMarker != null)
                return _screenMarker;

#if UNITY_IOS
            _screenMarker = new ScreenMarkerIOSPlugin();
#elif UNITY_ANDROID
            _screenMarker = new ScreenMarkerAndroidPlugin();
#endif
            return _screenMarker;
        }
    }
}
