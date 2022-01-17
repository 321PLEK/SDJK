using SDJK.Camera;
using SDJK.Renderer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDJK.PlayMode
{
    public class Note : MonoBehaviour
    {
        public Note note;
        public int BeatIndex = 0;
        public double Beat = 0;
        public double HoldBeat = 0;
        public KeyCode keyCode;

        public Transform HoldNote;
        public SpriteRenderer holdNoteSpriteRenderer;

        public bool hide = false;

        public SpriteRenderer spriteRenderer;

        public BarPosEffect barPosEffect;

        void Start()
        {
            if (keyCode == GameManager.A)
            {
                transform.localPosition = new Vector2(-5.535f, transform.localPosition.y);
                barPosEffect.A = true;
                barPosEffect.defaultPos = new Vector3(-5.535f, barPosEffect.defaultPos.y, 0);
            }
            else if (keyCode == GameManager.S)
            {
                transform.localPosition = new Vector2(-3.321f, transform.localPosition.y);
                barPosEffect.S = true;
                barPosEffect.defaultPos = new Vector3(-3.321f, barPosEffect.defaultPos.y, 0);
            }
            else if (keyCode == GameManager.D)
            {
                transform.localPosition = new Vector2(-1.107f, transform.localPosition.y);
                barPosEffect.D = true;
                barPosEffect.defaultPos = new Vector3(-1.107f, barPosEffect.defaultPos.y, 0);
            }
            else if (keyCode == GameManager.J)
            {
                transform.localPosition = new Vector2(1.107f, transform.localPosition.y);
                barPosEffect.J = true;
                barPosEffect.defaultPos = new Vector3(1.107f, barPosEffect.defaultPos.y, 0);
            }
            else if (keyCode == GameManager.K)
            {
                transform.localPosition = new Vector2(3.321f, transform.localPosition.y);
                barPosEffect.K = true;
                barPosEffect.defaultPos = new Vector3(3.321f, barPosEffect.defaultPos.y, 0);
            }
            else if (keyCode == GameManager.L)
            {
                transform.localPosition = new Vector2(5.535f, transform.localPosition.y);
                barPosEffect.L = true;
                barPosEffect.defaultPos = new Vector3(5.535f, barPosEffect.defaultPos.y, 0);
            }

            if (GameManager.Optimization && !PlayerManager.Editor)
                note.enabled = false;

            if (GameManager.UpScroll)
                transform.localScale = new Vector3(2, -1, 1);
        }

        //private bool temp = false;

        void Update()
        {
            /*if (PlayerManager.MapPlay && !GameManager.Optimization && (PlayerManager.HP > 0.001f || PlayerManager.Editor || PlayerManager.PracticeMode))
            {
                if (PlayerManager.playerManager.audioSource.pitch >= 0)
                {
                    if (PlayerManager.HitSoundCurrentBeat >= Beat && spriteRenderer.color.a > 0)
                        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - 0.04f * GameManager.FpsDeltaTime);
                    if (PlayerManager.HitSoundCurrentBeat < Beat && spriteRenderer.color.a < 1)
                        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + 0.04f * GameManager.FpsDeltaTime);
                }
                else
                {
                    if (PlayerManager.HitSoundCurrentBeat < Beat && spriteRenderer.color.a > 0)
                        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - 0.04f * GameManager.FpsDeltaTime);
                    if (PlayerManager.HitSoundCurrentBeat >= Beat && spriteRenderer.color.a < 1)
                        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + 0.04f * GameManager.FpsDeltaTime);
                }
            }*/

            if (PlayerManager.Editor)
            {
                if (Beat == PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1])
                    spriteRenderer.color = Color.red;
                else if (HoldBeat >= 0)
                    spriteRenderer.color = Color.green;
            }

            /*if (PlayerManager.HP <= 0.001f && !PlayerManager.Editor && !PlayerManager.PracticeMode && !temp)
            {
                StartCoroutine(Restart());
                temp = true;
            }*/

            if (HoldBeat > 0 && (Input.GetKey(keyCode) && !PlayerManager.Editor) || PlayerManager.AutoMode)
            {
                Transform transform = HoldNote;

                if (transform != null)
                {
                    double temp = 0;

                    if (Beat < PlayerManager.VisibleCurrentBeat)
                        temp = Beat - PlayerManager.VisibleCurrentBeat;

                    if (HoldBeat + temp < 0)
                        temp = -HoldBeat;

                    transform.localScale = new Vector3(1, (float)(1.666666666666667 * GameManager.Abs(PlayerManager.effect.BeatYPos) * (HoldBeat + temp)), 1);
                }
            }
        }

        IEnumerator Restart()
        {
            while (true)
            {
                spriteRenderer.color = new Color(0, 1, 0, spriteRenderer.color.a - 0.03f * GameManager.FpsUnscaledDeltaTime);
                transform.localEulerAngles += Vector3.forward * 0.5f * GameManager.FpsUnscaledDeltaTime;
    
                yield return null;
            }
        }
    }
}