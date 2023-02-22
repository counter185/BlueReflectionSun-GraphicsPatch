using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace BlueRefSun_GraphicsPatch
{
    public abstract class MenuBase
    {
        public abstract void HandleInput(KeyCode key, Plugin parent);
        public abstract void DrawAt(int x, int y, int height, MonoBehaviour caller, GUIStyle textStyle);

        public void drawBGDefault(int x, int y)
        {
            drawBG(x, y, 810, 205);
        }

        public void drawBG(int x, int y, int w, int h)
        {
            //this is my workaround for not being able to set GUI.Box transparency.
            //don't tell anyone.
            for (int i = 0; i != 3; i++)
            {
                GUI.Box(new Rect(x, y, w, h), Plugin.menuBgTex);
            }
        }
    }
}
