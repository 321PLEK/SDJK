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

        public bool A = false;
        public bool S = false;
        public bool D = false;
        public bool J = false;
        public bool K = false;
        public bool L = false;

        void Start() => isBoxCollider2DNotNull = boxCollider2D != null;

        void Update()
        {
            if (A)
            {
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, (float)((GameManager.Abs(MainCamera.UiZoom) - 1) * 8) + PlayerManager.effect.ABarPos.y);
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (float)((-GameManager.Abs(MainCamera.UiZoom) + 1) * 8 - MainCamera.UiPosY) + PlayerManager.effect.ABarPos.y);
            }
            else if (S)
            {
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, (float)((GameManager.Abs(MainCamera.UiZoom) - 1) * 8) + PlayerManager.effect.SBarPos.y);
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (float)((-GameManager.Abs(MainCamera.UiZoom) + 1) * 8 - MainCamera.UiPosY) + PlayerManager.effect.SBarPos.y);
            }
            else if (D)
            {
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, (float)((GameManager.Abs(MainCamera.UiZoom) - 1) * 8) + PlayerManager.effect.DBarPos.y);
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (float)((-GameManager.Abs(MainCamera.UiZoom) + 1) * 8 - MainCamera.UiPosY) + PlayerManager.effect.DBarPos.y);
            }
            else if (J)
            {
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, (float)((GameManager.Abs(MainCamera.UiZoom) - 1) * 8) + PlayerManager.effect.JBarPos.y);
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (float)((-GameManager.Abs(MainCamera.UiZoom) + 1) * 8 - MainCamera.UiPosY) + PlayerManager.effect.JBarPos.y);
            }
            else if (K)
            {
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, (float)((GameManager.Abs(MainCamera.UiZoom) - 1) * 8) + PlayerManager.effect.KBarPos.y);
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (float)((-GameManager.Abs(MainCamera.UiZoom) + 1) * 8 - MainCamera.UiPosY) + PlayerManager.effect.KBarPos.y);
            }
            else if (L)
            {
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, (float)((GameManager.Abs(MainCamera.UiZoom) - 1) * 8) + PlayerManager.effect.LBarPos.y);
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, (float)((-GameManager.Abs(MainCamera.UiZoom) + 1) * 8 - MainCamera.UiPosY) + PlayerManager.effect.LBarPos.y);
            }

            if (isBoxCollider2DNotNull)
                boxCollider2D.size = rectTransform.sizeDelta + Vector2.up * 16;
        }
    }
}