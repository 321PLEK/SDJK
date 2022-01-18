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
            int offset;
            if (GameManager.UpScroll)
                offset = -8;
            else
                offset = 8;

            if (MainCamera.posMode == PosMode.Player)
                transform.position = new Vector3(0, (float)((PlayerManager.VisibleCurrentBeat * PlayerManager.effect.BeatYPos) + MainCamera.UiPosY + (GameManager.Abs(MainCamera.UiZoom) - 1) * offset), MainCamera.CameraPos.z + 14) + MainCamera.UiPos;
            else if (MainCamera.posMode == PosMode.World)
                transform.position = Vector3.zero + MainCamera.UiPos;

            transform.eulerAngles = MainCamera.UiRotation;
        }
    }
}