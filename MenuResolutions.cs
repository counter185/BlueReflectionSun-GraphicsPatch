using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlueRefSun_GraphicsPatch
{
    public class MenuResolutions : MenuBase
    {
        readonly Dictionary<string, HashSet<int>> resolutionsRefreshRates = new();
        public MenuResolutions()
        {
            foreach (Resolution a in Screen.resolutions)
            {
                string key = a.width + "x" + a.height;
                if (!resolutionsRefreshRates.ContainsKey(key))
                {
                    resolutionsRefreshRates.Add(key, new HashSet<int>());
                }
                resolutionsRefreshRates[key].Add(a.refreshRate);

            }
            //resolutions = Screen.resolutions;
            maxSel = resolutionsRefreshRates.Count + 2;
        }

        public static bool fullscreen = false;
        public static int resW = 1280;
        public static int resH = 720;
        public static int refreshRate = 60;

        int selection = 0;
        int maxSel;

        public override void DrawAt(int x, int y, int height, MonoBehaviour caller, GUIStyle textStyle)
        {
            height = 160 + resolutionsRefreshRates.Count * 20;
            drawBG(x - 5, y - 5, 810, height);
            GUI.Label(new Rect(x, y, 300, 100),      "Resolution options", textStyle);
            GUI.Label(new Rect(x, y + 20, 300, 100), "------------------------------------------------------", textStyle);

            GUI.Label(new Rect(x, y + 40, 300, 100), "    Fullscreen: " + (fullscreen ? "ON" : "OFF") + "     Current resolution: " + resW + "x" + resH, textStyle);
            GUI.Label(new Rect(x, y + 60, 300, 100), "    Refresh rate: " + refreshRate, textStyle);

            int yd = y + 80;
            foreach (string a in resolutionsRefreshRates.Keys)
            {
                GUI.Label(new Rect(x, yd, 300, 100), "    " + a, textStyle);
                yd += 20;
            }
            
            //GUI.Label(new Rect(x, y + 60, 300, 100), "    Framerate options", textStyle);

            GUI.Label(new Rect(x, y + 40 + (20 * selection), 300, 100), ">", textStyle);

            GUI.Label(new Rect(x, y + height - 30, 300, 100), "\u2191/\u2193: Move, \u2192: Set, \u2190: Back, F11: Close", textStyle);
        }

        public override void HandleInput(KeyCode key, Plugin parent)
        {
            switch (key)
            {
                case KeyCode.UpArrow:
                    selection--;
                    selection = selection == -1 ? maxSel-1 : selection;
                    break;
                case KeyCode.DownArrow:
                    selection++;
                    selection %= maxSel;
                    break;
                case KeyCode.LeftArrow:
                    parent.SaveSettings();
                    parent.MenuBack();
                    break;
                case KeyCode.RightArrow:
                    if (selection == 0)
                    {
                        fullscreen = !fullscreen;
                    }
                    else if (selection == 1)
                    {
                        string lkey = resW + "x" + resH;
                        if (resolutionsRefreshRates.ContainsKey(lkey))
                        {
                            HashSet<int> rrates = resolutionsRefreshRates[lkey];
                            int highestIndex = -1;
                            foreach (int x in rrates)
                            {
                                if (x <= refreshRate)
                                {
                                    highestIndex++;
                                } else
                                {
                                    break;
                                }

                            }
                            highestIndex++;
                            highestIndex %= rrates.Count;
                            refreshRate = rrates.ElementAt(highestIndex);
                        } else
                        {
                            refreshRate = 60;
                        }
                    }
                    else if (selection > 1)
                    {
                        string[] split = resolutionsRefreshRates.ElementAt(selection - 2).Key.Split('x');
                        resW = int.Parse(split[0]);
                        resH = int.Parse(split[1]);
                        refreshRate = resolutionsRefreshRates.ElementAt(selection - 2).Value.Last();
                    }
                    break;
            }
        }
    }
}