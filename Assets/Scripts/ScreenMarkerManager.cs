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
        }

        public void PrintTileTextAndImage()
        {
            _screenMarkerPlugin.SetTextTileMode("Hello", null, 0, "4c000000", 30, 50, 50);
            _screenMarkerPlugin.SetImageTileMode(_image, -30, 0, 20);
        }

        public void Reset()
        {
            _screenMarkerPlugin.UnsetTextTileMode();
            _screenMarkerPlugin.UnsetImageTileMode();
            _screenMarkerPlugin.ShowScreenMarker();
        }
    }
}
