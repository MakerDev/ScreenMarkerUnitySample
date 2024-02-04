using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

        public void AddTextWithRect()
        {
            _screenMarkerPlugin.AddTextWithCenter(200, 200, "AddTextCen", null, 18, null, 90.0f);
            _screenMarkerPlugin.ShowScreenMarker();
        }

        public void ClearTextAll()
        {
            _screenMarkerPlugin.ClearTextAll();
        }

        public void SetTextTileMode()
        {
#if UNITY_ANDROID
            _screenMarkerPlugin.SetTextTileMode("SetTextTileMode", "NanumGothicBold", 30, null, 20, 200, 50);
#elif UNITY_IOS
            _screenMarkerPlugin.SetTextTileMode("SetTextTileMode", "NanumGothicBold", 30, null, 20);
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
