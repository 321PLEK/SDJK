using SDJK.PlayMode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDJK
{
    public class SdjkSystem : MonoBehaviour
    {
        public static SdjkSystem sdjkSystem;
        public static double WindowSize = 1;
        public static double tempWindowSize = 1;

        public static Vector2 WindowPos = Vector2.zero;
        public static WindowManager.datumPoint WindowDatumPoint = WindowManager.datumPoint.Center;
        public static WindowManager.datumPoint ScreenDatumPoint = WindowManager.datumPoint.Center;

        public static Vector2 tempWindowPos = Vector2.zero;
        public static WindowManager.datumPoint tempWindowDatumPoint = WindowManager.datumPoint.Center;
        public static WindowManager.datumPoint tempScreenDatumPoint = WindowManager.datumPoint.Center;

        void Awake()
        {
            SdjkSystem[] obj = FindObjectsOfType<SdjkSystem>();
            if (obj.Length == 1)
                DontDestroyOnLoad(gameObject);
            else
                Destroy(gameObject);

            sdjkSystem = this;
        }

#if !UNITY_EDITOR
        void Update()
        {
            if (!(PlayerManager.MapPlay || PlayerManager.Editor) || ((PlayerManager.MapPlay || PlayerManager.Editor) && PlayerManager.mapData.Effect.WindowSizeEffect.Count == 0 && PlayerManager.mapData.Effect.WindowPosEffect.Count == 0) || !((PlayerManager.MapPlay && !GameManager.Optimization) || (PlayerManager.Editor && !GameManager.EditorOptimization)))
            {
                if (Input.GetKeyDown(KeyCode.F11))
                {
                    if (Screen.fullScreen)
                        Screen.SetResolution(1280, 720, false);
                    else
                        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                }
            }

            if ((PlayerManager.MapPlay && !GameManager.Optimization) || (PlayerManager.Editor && !GameManager.EditorOptimization))
            {
                if ((PlayerManager.MapPlay || PlayerManager.Editor) && (PlayerManager.mapData.Effect.WindowSizeEffect.Count > 0 || PlayerManager.mapData.Effect.WindowPosEffect.Count > 0))
                {
                    if (tempWindowSize != WindowSize || tempWindowPos != WindowPos || tempWindowDatumPoint != WindowDatumPoint || tempScreenDatumPoint != ScreenDatumPoint)
                    {
                        if (PlayerManager.effect.WindowPosLerp != 1 && PlayerManager.effect.WindowPosLerp != 0)
                            WindowManager.SetWindowPosition(WindowPos.x, WindowPos.y, (int)(1280 * WindowSize), (int)(720 * WindowSize), WindowDatumPoint, ScreenDatumPoint, true, (float)(PlayerManager.effect.WindowPosLerp * PlayerManager.playerManager.audioSource.pitch));
                        else
                            WindowManager.SetWindowPosition(WindowPos.x, WindowPos.y, (int)(1280 * WindowSize), (int)(720 * WindowSize), WindowDatumPoint, ScreenDatumPoint);
                    }

                    tempWindowSize = WindowSize;
                    tempWindowPos = WindowPos;
                    tempWindowDatumPoint = WindowDatumPoint;
                    tempScreenDatumPoint = ScreenDatumPoint;
                }
            }
        }
#endif
    }
}