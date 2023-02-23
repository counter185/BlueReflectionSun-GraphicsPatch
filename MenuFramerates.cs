using BR.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BlueRefSun_GraphicsPatch
{
    public class MenuFramerates : MenuBase
    {
        static readonly Parameter.FrameRateType[] framerates = new Parameter.FrameRateType[] {
            Parameter.FrameRateType.FPS30,
            Parameter.FrameRateType.FPS60,
            Parameter.FrameRateType.FPSMAX
        };

        public static Parameter.FrameRateType currentFramerateMode = Parameter.FrameRateType.FPS60;
        int selection = 0;
        public override void DrawAt(int x, int y, int height, MonoBehaviour caller, GUIStyle textStyle)
        {
            drawBGDefault(x - 5, y - 5);
            GUI.Label(new Rect(x, y, 300, 100), "Framerate options", textStyle);
            GUI.Label(new Rect(x, y + 20, 300, 100), "------------------------------------------------------", textStyle);

            GUI.Label(new Rect(x, y + 40, 300, 100), $"    [{(currentFramerateMode == Parameter.FrameRateType.FPS30 ? "X" : " ")}] 30 FPS", textStyle);
            GUI.Label(new Rect(x, y + 60, 300, 100), $"    [{(currentFramerateMode == Parameter.FrameRateType.FPS60 ? "X" : " ")}] 60 FPS", textStyle);
            GUI.Label(new Rect(x, y + 80, 300, 100), $"    [{(currentFramerateMode == Parameter.FrameRateType.FPSMAX ? "X" : " ")}] Max FPS", textStyle);

            GUI.Label(new Rect(x, y + 40 + (20 * selection), 300, 100), ">", textStyle);

            GUI.Label(new Rect(x, y + height - 23, 300, 100), "\u2191/\u2193: Move, \u2190: Back, \u2192: Set, F11: Close", textStyle);
        }

        public override void HandleInput(KeyCode key, Plugin parent)
        {
            switch (key)
            {
                case KeyCode.UpArrow:
                    selection--;
                    selection = selection == -1 ? 2 : selection;
                    break;
                case KeyCode.DownArrow:
                    selection++;
                    selection %= 3;
                    break;
                case KeyCode.LeftArrow:
                    parent.SaveSettings();
                    parent.MenuBack();
                    break;
                case KeyCode.RightArrow:
                    currentFramerateMode = framerates[selection];
                    break;
            }
        }
    }
}
