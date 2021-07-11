using SDJK.Camera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.PlayMode.UI
{
    public class UiScale : MonoBehaviour
    {
        public CanvasScaler canvasScaler;
        
        void Update() => canvasScaler.scaleFactor = (float) (Screen.height / 720f * (1f / MainCamera.UiZoom));
    }
}