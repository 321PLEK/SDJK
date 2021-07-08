using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDJK.EditMode
{
    public class RightClickMenu : MonoBehaviour
    {
        public RectTransform rectTransform;
        public static bool Show = false;
        Vector2 MousePos;

        public List<GameObject> buttons = new List<GameObject>();
    
        void Update()
        {
            if (!Show && Input.GetKeyUp(KeyCode.Mouse1))
            {
                foreach (GameObject item in buttons)
                    item.SetActive(true);

                Show = true;
            }
            if (Show && Input.GetKeyUp(KeyCode.Mouse0))
                Show = false;
            if (Show && Input.GetKeyUp(KeyCode.Mouse1))
            {
                MousePos = Input.mousePosition;
                rectTransform.anchoredPosition = new Vector2(MousePos.x * (1280f / Screen.width), MousePos.y * (720f / Screen.height));
            }

            if (!Show)
            {
                if (rectTransform.anchoredPosition != Vector2.zero)
                {
                    foreach (GameObject item in buttons)
                        item.SetActive(false);

                    rectTransform.anchoredPosition = Vector2.zero;
                }

                MousePos = Input.mousePosition;
            }
        }
    }
}