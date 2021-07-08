using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace  SDJK.PlayMode.UI
{
    public class UiScale : MonoBehaviour
    {
        public CanvasScaler canvasScaler;
        
        void Update()
        {
            if (PlayerManager.effect.UiZoomLerp != 0 && PlayerManager.effect.UiZoomLerp != 1)
                canvasScaler.scaleFactor = Mathf.Lerp(canvasScaler.scaleFactor, (float) ((Screen.height / 720f) * (1f / PlayerManager.effect.UiZoom)), (float) (PlayerManager.effect.UiZoomLerp * GameManager.FpsDeltaTime * PlayerManager.playerManager.audioSource.pitch));
            else
                canvasScaler.scaleFactor = (float) ((Screen.height / 720f) * (1f / PlayerManager.effect.UiZoom));
        }
    }
}