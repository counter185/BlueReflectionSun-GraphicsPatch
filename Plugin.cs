using BepInEx;
using BR.UI;
using System;
using UnityEngine;

namespace BlueRefSun_GraphicsPatch
{
    [BepInPlugin("pl.cntrpl.bluerefsungraphicspatch", "BRS Graphics Patch", "1.0.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin pl.cntrpl.bluerefsungraphicspatch is loaded!");
            Parameter.TargetFrameRateType = Parameter.FrameRateType.FPS60;
            Screen.SetResolution(1920, 1080, true);
        }

        private void Update()
        {
            //needs to be done in update because the game likes to change this value at random times
            Parameter.TargetFrameRateType = Parameter.FrameRateType.FPS60;
            if (Input.GetKeyDown(KeyCode.F12))
            {
                Screen.SetResolution(1920, 1080, true);
            }
        }
    }
}
