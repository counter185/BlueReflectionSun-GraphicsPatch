using UnityEngine;

namespace BlueRefSun_GraphicsPatch
{
    public class MenuMain : MenuBase
    {
        public int selection = 0;

        public override void DrawAt(int x, int y, int height, MonoBehaviour caller, GUIStyle textStyle)
        {
            drawBGDefault(x-5,y-5);
            GUI.Label(new Rect(x, y, 300, 100), "BlueRefSun_GraphicsPatch  |  by github.com/counter185/", textStyle);
            GUI.Label(new Rect(x, y + 20, 300, 100), "------------------------------------------------------", textStyle);

            GUI.Label(new Rect(x, y + 40, 300, 100), "    Resolution options", textStyle);
            GUI.Label(new Rect(x, y + 60, 300, 100), "    Framerate options", textStyle);

            GUI.Label(new Rect(x, y + 40 + (20 * selection), 300, 100), ">", textStyle);

            GUI.Label(new Rect(x, y + height - 23, 300, 100), "\u2191/\u2193: Move, \u2192: Confirm, F11: Close", textStyle);
        }

        public override void HandleInput(KeyCode key, Plugin parent)
        {
            switch (key)
            {
                case KeyCode.UpArrow:
                    selection--;
                    selection = selection == -1 ? 1 : selection;
                    break;
                case KeyCode.DownArrow:
                    selection++;
                    selection %= 2;
                    break;
                case KeyCode.RightArrow:
                    switch (selection)
                    {
                        case 0:
                            parent.AddNewScreen(new MenuResolutions());
                            break;
                        case 1:
                            parent.AddNewScreen(new MenuFramerates());
                            break;
                    }
                    break;
            }
        }
    }
}