using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Font
    {
        public string FontName { get; set; } = "";
        public int FontSize { get; set; }
        public int FontType { get; set; }
        public string ColorString { get; set; }
    }

    public interface IScreenMarker
    {
        void InitScreenMarker(string userInfo, Texture2D defaultImage);
        void ShowScreenMarker();
        void HideScreenMarker();
        void SetScreenMarkerAlpha(float alpha);
        void AddTextWithRect(
            int x, int y, int width, int height,
            string text, string fontName, float fontSize,
            string colorString,
            float angle,
            int align,
            bool useSizeToFit);
        void AddTextWithRectAndroid(Rect rect, string text);
        void AddTextWithCenterIOS(
            int x, int y,
            string text, string fontName, float fontSize,
            string colorString,
            float angle);
        void ClearTextAll();
        void SetTextAll(string text);
        void SetTextRotateAll(float angle);
        void SetTextColorAll(string colorString);
        void SetTextTileMode(
            string text,
            string fontName,
            float fontSize,
            string colorString,
            float angle,
            int horizontalMargin=50, int verticalMargin=50);
        void UnsetTextTileMode();

        void SetImagePosition(int x, int y);
        void SetImageRotation(float angle);
        void SetImage(Texture2D image);
        void SetImageTileMode(
            Texture2D image,
            float angle,
            int horizontalMargin=0, int verticalMargin=10);
        void SetImageTileModeWithText(
            Texture2D image,
            string text,
            string fontName,
            float fontSize,
            string colorString,
            float angle,
            int horizontalMargin=0, int verticalMargin=20);
        void UnsetImageTileMode();
    }
}
