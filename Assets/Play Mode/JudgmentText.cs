using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.PlayMode.UI
{
    public class JudgmentText : MonoBehaviour
    {
        public JudgmentText judgmentText;
        public Image image;
        public Text text;
        public Text text2;
        Vector3 velocity;
        public float y;
        public bool UpScroll = true;

        void Awake()
        {
            if (GameManager.UpScroll && UpScroll)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, -transform.localPosition.y);
                y = -y;
            }

            if (image == null)
                judgmentText.enabled = false;

            if (GameManager.Optimization)
            {
                if (image != null)
                    transform.localPosition = Vector3.up * (y + 15);
                judgmentText.enabled = false;
            }
        }

        public void PosReset()
        {
            if (image != null)
                image.enabled = true;

            if (!GameManager.UpScroll)
            {
                velocity = Vector3.up * 3;
                transform.localPosition = Vector3.up * (y + 0.01f);
            }
            else
            {
                velocity = Vector3.down * 3;
                transform.localPosition = Vector3.up * (y - 0.01f);
            }
        }

        void Update()
        {
            if (!GameManager.UpScroll)
            {
                if (transform.localPosition.y <= y)
                {
                    transform.localPosition = Vector3.up * y;
                    if (PlayerManager.Editor)
                    {
                        if (image != null)
                            image.enabled = false;

                        text.text = "";
                        text2.text = "";
                    }
                }
                else
                {
                    velocity -= Vector3.up * 0.2f * GameManager.FpsDeltaTime;
                    transform.localPosition += velocity * GameManager.FpsDeltaTime;
                }
            }
            else
            {
                if (transform.localPosition.y >= y)
                {
                    transform.localPosition = Vector3.up * y;
                    if (PlayerManager.Editor)
                    {
                        if (image != null)
                            image.enabled = false;

                        text.text = "";
                        text2.text = "";
                    }
                }
                else
                {
                    velocity -= Vector3.down * 0.2f * GameManager.FpsDeltaTime;
                    transform.localPosition += velocity * GameManager.FpsDeltaTime;
                }
            }
        }
    }
}