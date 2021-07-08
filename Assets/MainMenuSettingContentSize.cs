using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDJK.MainMenu
{
    public class MainMenuSettingContentSize : MonoBehaviour
    {
        public RectTransform rectTransform;

        void Update()
        {
            int i = 0;
            foreach (Transform child in transform)
                i++;

            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 52 * i + 7);
        }
    }
}