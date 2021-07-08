using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDJK.PlayMode.Background
{
    public class UpscrollCustomBackgroundObjectYPos : MonoBehaviour
    {
        void Awake()
        {
            if (GameManager.UpScroll)
            {
                Transform[] array = GetComponentsInChildren<Transform>();
                for (int i = 0; i < array.Length; i++)
                {
                    Transform item = array[i];
                    item.position = new Vector2(item.position.x, -item.position.y);
                }
            }
        }
    }
}