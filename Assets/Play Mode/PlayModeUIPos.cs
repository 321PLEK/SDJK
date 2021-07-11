using SDJK.Camera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDJK.PlayMode
{
    public class PlayModeUIPos : MonoBehaviour
    {
        public RectTransform rectTransform;

        void Update()
        {
            rectTransform.localScale = new Vector2((float)(1 / MainCamera.CameraZoom), (float)(1 / MainCamera.CameraZoom));
        }
    }
}