using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ScreenMarker;

namespace Assets.Scripts
{
    public class ScreenMarkerManager : MonoBehaviour
    {
        [SerializeField]
        private Texture2D _image = null;

        private IScreenMarker _screenMarkerPlugin = null;

        private void Start()
        {
            _screenMarkerPlugin = ScreenMarkerPlugin.GetScreenMarker();
            _screenMarkerPlugin.InitScreenMarker("12345", _image);
            _screenMarkerPlugin.ShowScreenMarker();
        }

        public void ShowScreenMarker()
        {
            _screenMarkerPlugin.ShowScreenMarker();
        }

        public void SetAlpha()
        {
            _screenMarkerPlugin.SetScreenMarkerAlpha(0.5f);
        }

        public void AddTextWithRect2()
        {
            _screenMarkerPlugin.AddTextWithRect(150, 150, 200, 200, "Hello", null, 18, "a000ff00", 30.0f, (int)Gravity.LEFT, true);
        }

        public void AddTextWithRect()
        {
            _screenMarkerPlugin.AddTextWithCenter(500, 500, "AddTextCen", null, 18, null, 90.0f);
            _screenMarkerPlugin.ShowScreenMarker();
        }

        public void ClearTextAll()
        {
            _screenMarkerPlugin.ClearTextAll();
        }

        public void SetImageTileMode()
        {
            _screenMarkerPlugin.SetImageTileMode(_image, 30);
        }

        public void SetTextTileMode()
        {
#if UNITY_ANDROID
            _screenMarkerPlugin.SetTextTileMode("가평물결", "GapyeongWave", 30, null, 20, 200, 50);
#elif UNITY_IOS
            _screenMarkerPlugin.SetTextTileMode("가평물결", "GapyeongWave", 30, null, 20);
#endif
        }

        public void PrintTileTextAndImage()
        {
            _screenMarkerPlugin.SetImageTileMode(_image, -30, 0, 20);
#if UNITY_ANDROID
            _screenMarkerPlugin.SetTextTileMode("Hello", null, 30, "4c000000", 30.0f, 200, 50);
#elif UNITY_IOS
            _screenMarkerPlugin.SetTextTileMode("Hello", null, 30, "4c000000", 30.0f);
#endif
        }

        public void Reset()
        {
            _screenMarkerPlugin.UnsetImageTileMode();
            _screenMarkerPlugin.UnsetTextTileMode();
            _screenMarkerPlugin.ShowScreenMarker();
        }

        public void HideScreenMarker()
        {
            _screenMarkerPlugin.HideScreenMarker();
        }
    }
}
