using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
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
        AXIS_CLIP = 8,
        AXIS_PULL_AFTER = 4,
        AXIS_PULL_BEFORE = 2,
        AXIS_SPECIFIED = 1,
        AXIS_X_SHIFT = 0,
        AXIS_Y_SHIFT = 4,
        BOTTOM = 80,
        CENTER = 17,
        CENTER_HORIZONTAL = 1,
        CENTER_VERTICAL = 16,
        CLIP_HORIZONTAL = 8,
        CLIP_VERTICAL = 128,
        DISPLAY_CLIP_HORIZONTAL = 16777216,
        DISPLAY_CLIP_VERTICAL = 268435456,
        END = 8388613,
        FILL = 119,
        FILL_HORIZONTAL = 7,
        FILL_VERTICAL = 112,
        HORIZONTAL_GRAVITY_MASK = 7,
        LEFT = 3,
        NO_GRAVITY = 0,
        RELATIVE_HORIZONTAL_GRAVITY_MASK = 8388615,
        RELATIVE_LAYOUT_DIRECTION = 8388608,
        RIGHT = 5,
        START = 8388611,
        TOP = 48,
        VERTICAL_GRAVITY_MASK = 112
    }


    public class Font
    {
        public string FontName { get; set; } = null;
        public int FontSize { get; set; } = 20;
        public FontType FontType { get; set; } = FontType.NORMAL;
        public string ColorString { get; set; } = "a0000000";
    }

    public interface IScreenMarker
    {
        void InitScreenMarker(string userInfo, Texture2D defaultImage);
        void ShowScreenMarker();
        void HideScreenMarker();
        void SetScreenMarkerAlpha(float alpha);

        void AddTextWithCenter(
            int x, int y, string text,
            string fontName,
            float fontSize,
            string colorString,
            float angle);
        void AddTextWithRect(
            int x, int y, int width, int height,
            string text, 
            string fontName,
            float fontSize,
            string colorString,
            float angle,
            int align,
            bool useSizeToFit);
        void AddTextWithRect(
            int x, int y, int width, int height,
            string text);

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
