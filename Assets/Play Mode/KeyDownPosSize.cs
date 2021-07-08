using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDJK.Camera;

namespace SDJK.PlayMode
{
    public class KeyDownPosSize : MonoBehaviour
    {
        public RectTransform rectTransform;
        public BoxCollider2D boxCollider2D;
        bool isBoxCollider2DNotNull;

        void Start() => isBoxCollider2DNotNull = boxCollider2D != null;

        void Update()
        {
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, (float)((GameManager.Abs(MainCamera.UiZoom) - 1) * 8));
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (float)((-GameManager.Abs(MainCamera.UiZoom) + 1) * 8 - MainCamera.UiPosY));
            
            if (isBoxCollider2DNotNull)
                boxCollider2D.size = rectTransform.sizeDelta + Vector2.up * 16;
        }
    }
}