using System.Collections;
using System.Collections.Generic;
using SDJK.Camera;
using UnityEngine;
using UnityEngine.Video;

namespace SDJK.Renderer
{
    public class CanvasRenderCamera : MonoBehaviour
    {
        public Canvas canvas;

        void Update() => canvas.worldCamera = MainCamera.Camera;
    }
}