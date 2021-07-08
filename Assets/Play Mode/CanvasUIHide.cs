using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDJK.PlayMode.UI
{
    public class CanvasUIHide : MonoBehaviour
    {
        public Canvas canvas;
        bool tempUIHide = false;

        void Update()
        {
            if (tempUIHide != PlayerManager.UIHide)
            {
                if (PlayerManager.UIHide)
                    canvas.enabled = false;
                else
                    canvas.enabled = true;
                tempUIHide = PlayerManager.UIHide;
            }
        }
    }
}