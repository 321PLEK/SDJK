using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDJK.PlayMode;
using UnityEngine.Rendering.PostProcessing;

namespace SDJK.Camera
{
    public class MainCamera : MonoBehaviour
    {
        public static Vector3 CameraPos = new Vector3(0, 0, -14);
        public static Vector3 UiPos = new Vector3(0, 0, 0);
        public static double UiPosY = 0;
        public static double UiZoom = 1;
        public static double CameraZoom = 1;
        public static Vector3 UiRotation = Vector3.zero;
        public static Vector3 CameraRotation = Vector3.zero;

        public static PosMode posMode;

        public Vector3 _CameraPos = CameraPos;
        public Vector3 _UiPos = UiPos;
        public double _UiPosY = UiPosY;
        public double _UiZoom = UiZoom;
        public double _CameraZoom = CameraZoom;
        public Vector3 _UiRotation = UiRotation;
        public Vector3 _CameraRotation = CameraRotation;

        public static MainCamera mainCamera;
        public static UnityEngine.Camera Camera;

        public PostProcessVolume PostProcessVolume;

        void OnEnable()
        {
            Camera = GetComponent<UnityEngine.Camera>();
            mainCamera = this;
        }

        void Update()
        {
            Camera.orthographicSize = (float)(8 * UiZoom * CameraZoom);

            int offset;
            if (GameManager.UpScroll)
                offset = -8;
            else
                offset = 8;

            if (posMode == PosMode.Player)
                transform.position = new Vector3(CameraPos.x, (float)((PlayerManager.VisibleCurrentBeat * PlayerManager.effect.BeatYPos) + UiPosY + (GameManager.Abs(UiZoom) - 1) * offset + CameraPos.y), CameraPos.z * Mathf.Abs(Camera.orthographicSize / 8)) + UiPos;
            else if (posMode == PosMode.World)
                transform.position = CameraPos;

            transform.eulerAngles = UiRotation + CameraRotation;
            if (Camera.orthographicSize < 0 && !Camera.orthographic)
                transform.eulerAngles += Vector3.forward * 180;

            _CameraPos = CameraPos;
            _UiPos = UiPos;
            _CameraZoom = CameraZoom;
            _CameraRotation = CameraRotation;
            _UiPosY = UiPosY;
            _UiZoom = UiZoom;
            _UiRotation = UiRotation;
        }
    }

    public enum PosMode
    {
        Player,
        World
    }
}