using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDJK.Camera;
using UnityEngine.UI;

namespace SDJK.PlayMode
{
    public class PlayModeCanvasCamera : MonoBehaviour
    {
        void Update()
        {
            if (MainCamera.posMode == PosMode.Player)
                transform.position = new Vector3(0, (float)((PlayerManager.VisibleCurrentBeat * PlayerManager.effect.BeatYPos) + MainCamera.UiPosY + (GameManager.Abs(MainCamera.UiZoom) - 1) * 8), MainCamera.CameraPos.z + 14) + MainCamera.UiPos;
            else if (MainCamera.posMode == PosMode.World)
                transform.position = Vector3.zero + MainCamera.UiPos;

            transform.eulerAngles = MainCamera.UiRotation;
        }
    }
}