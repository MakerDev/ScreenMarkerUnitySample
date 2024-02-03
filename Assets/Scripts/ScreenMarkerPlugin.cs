using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts
{
    public class ScreenMarkerPlugin : MonoBehaviour
    {
        [SerializeField]
        private Texture2D _defaultImage = null;
        private static IScreenMarker _screenMarker = null;

        private static ScreenMarkerPlugin _instance;

        public static ScreenMarkerPlugin Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ScreenMarkerPlugin();
                }

                return _instance;
            }
        }

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
