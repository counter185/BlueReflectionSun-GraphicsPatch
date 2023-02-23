using BepInEx;
using BR.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

namespace BlueRefSun_GraphicsPatch
{
    

    [BepInPlugin("pl.cntrpl.bluerefsungraphicspatch", "BRS Graphics Patch", "1.0.0.0")]
    public class Plugin : BaseUnityPlugin
    {

        public static Texture2D menuBgTex;
        private void Awake()
        {
            menuBgTex = new Texture2D(2,2, TextureFormat.RGBA32, false);
            menuBgTex.SetPixels32(new Color32[] { new Color32(0,0,0,0xEE), new Color32(0, 0, 0, 0xEE) , new Color32(0, 0, 0, 0xEE) , new Color32(0, 0, 0, 0xEE) });

            // Plugin startup logic
            Logger.LogInfo($"Plugin pl.cntrpl.bluerefsungraphicspatch is loaded!");
            LoadSettings();
        }

        public Stack<MenuBase> menuStack = new();

        public void LoadSettings()
        {
            try
            {
                string[] configFile = File.ReadAllLines("brsun-graphics-config.txt");
                foreach (string a in configFile)
                {
                    if (int.TryParse(a.Split('=').Last(), out int val))
                    {
                        if (a.StartsWith("resolutionWidth="))
                        {
                            MenuResolutions.resW = val;
                        }
                        else if (a.StartsWith("resolutionHeight="))
                        {
                            MenuResolutions.resH = val;
                        }
                        else if (a.StartsWith("refreshRate="))
                        {
                            MenuResolutions.refreshRate = val;
                        }
                    }
                }
            } catch (Exception)
            {
                Logger.LogInfo("Failed to load config file.");
            }
        }

        public void SaveSettings()
        {
            File.WriteAllLines("brsun-graphics-config.txt", new string[] { 
                "resolutionWidth=" + MenuResolutions.resW + "\n"
                + "resolutionHeight=" + MenuResolutions.resH + "\n"
                + "refreshRate=" + MenuResolutions.refreshRate + "\n"
            });
        }

        public void AddNewScreen(MenuBase menu)
        {
            menuStack.Push(menu);
        }
        public void MenuBack()
        {
            menuStack.Pop();
        }

        public KeyCode[] passthroughKeyCodes = new KeyCode[] {
            KeyCode.Alpha0,
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.RightArrow,
            KeyCode.LeftArrow,
        };
        private void Update()
        {
            //needs to be done in update because the game likes to change this value at random times
            Parameter.TargetFrameRateType = MenuFramerates.currentFramerateMode;

            //this reports wrong values:
            Resolution currentRes = Screen.currentResolution;
            //it should report the window size. instead, it reports the size of the actual screen.

            if (currentRes.width != MenuResolutions.resW || currentRes.height != MenuResolutions.resH || Screen.fullScreen != MenuResolutions.fullscreen)
            {
                if (currentRes.width != MenuResolutions.resW || currentRes.height != MenuResolutions.resH)
                {
                    Logger.LogInfo($"Setting resolution: {currentRes.width} -> {MenuResolutions.resW}, {currentRes.height} -> {MenuResolutions.resH}");
                }
                Screen.SetResolution(MenuResolutions.resW, MenuResolutions.resH, MenuResolutions.fullscreen);
                
            }

            if (Input.GetKeyDown(KeyCode.F12))
            {
                Screen.SetResolution(1920, 1080, true);
            } else if (Input.GetKeyDown(KeyCode.F11))
            {
                if (menuStack.Count == 0)
                {
                    AddNewScreen(new MenuMain());
                } else
                {
                    menuStack.Clear();
                }
            }

            if (menuStack.Count != 0)
            {
                foreach (KeyCode pKey in passthroughKeyCodes)
                {
                    if (Input.GetKeyDown(pKey))
                    {
                        menuStack.Peek().HandleInput(pKey, this);
                    }
                }
            }
        }

        private void OnGUI()
        {
            if (menuStack.Count != 0)
            {
                GUIStyle textStyle = new()
                {
                    fontSize = 20
                };
                textStyle.normal.textColor = Color.white;


                menuStack.Peek().DrawAt(10, 10, 200, this, textStyle);

            }
        }
    }
}
