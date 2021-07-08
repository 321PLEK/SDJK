using SDJK.Camera;
using SDJK.Effect;
using SDJK.Language;
using SDJK.PlayMode;
using SDJK.PlayMode.Score;
using SDJK.PlayMode.Sound;
using SDJK.Scene;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.EditMode
{
    public class EditorManager : MonoBehaviour
    {
        public float Time;
        float tempTime;

        public Text MouseYBeatText;
        public Text CurrentBeatText;
        public Text TimeText;
        public Slider TimeSlider;
        public RectTransform TimeSliderRectTransform;

        public static bool AutoScroll = false;

        static bool Pause = true;

        double MouseYBeat = 0;
        
        public static EditorManager editorManager;

        Note selectNote;
        double selectNoteMouseYBeat = 0;

        void Awake()
        {
            editorManager = this;

            if (GameManager.UpScroll)
            {
                TimeSliderRectTransform.anchorMin = Vector2.up;
                TimeSliderRectTransform.anchorMax = Vector2.one;
                TimeSliderRectTransform.anchoredPosition = new Vector2(TimeSliderRectTransform.anchoredPosition.x, -TimeSliderRectTransform.anchoredPosition.y);

                MouseYBeatText.rectTransform.anchoredPosition = new Vector2(MouseYBeatText.rectTransform.anchoredPosition.x, -MouseYBeatText.rectTransform.anchoredPosition.y - 52);
                CurrentBeatText.rectTransform.anchoredPosition = new Vector2(CurrentBeatText.rectTransform.anchoredPosition.x, -CurrentBeatText.rectTransform.anchoredPosition.y - 52);
                TimeText.rectTransform.anchoredPosition = new Vector2(TimeText.rectTransform.anchoredPosition.x, -TimeText.rectTransform.anchoredPosition.y - 52);
                MouseYBeatText.rectTransform.anchorMin = Vector2.up;
                CurrentBeatText.rectTransform.anchorMin = Vector2.up;
                TimeText.rectTransform.anchorMin = Vector2.up;
                MouseYBeatText.rectTransform.anchorMax = Vector2.up;
                CurrentBeatText.rectTransform.anchorMax = Vector2.up;
                TimeText.rectTransform.anchorMax = Vector2.up;
            }
        }

        public static void hitSound()
        {
            if (editorManager != null)
            {
                PlayerManager.HitSoundCurrentBeat = (PlayerManager.BeatTimer + editorManager.Time - PlayerManager.StartDelay) * (PlayerManager.effect.BPM / 60) + 1;

                int temp = PlayerManager.mapData.AllBeat.IndexOf(GameManager.CloseValue(PlayerManager.mapData.AllBeat,
                    PlayerManager.HitSoundCurrentBeat));
                if (temp >= PlayerManager.mapData.AllBeat.Count - 1)
                    HitSound.BeatIndex = PlayerManager.mapData.AllBeat.Count - 1;
                else
                    HitSound.BeatIndex = temp;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PlayerManager.Quit();
                editorManager.enabled = false;
            }
            
            AudioMove();

            TimeSlider.maxValue = PlayerManager.playerManager.audioSource.clip.length;

            if (!GameManager.UpScroll)
            {
                if (Input.GetKey(KeyCode.T))
                    TimeSliderRectTransform.anchoredPosition = Vector3.Lerp(TimeSliderRectTransform.anchoredPosition, Vector3.up * 29, 0.2f * GameManager.FpsDeltaTime);
                else
                    TimeSliderRectTransform.anchoredPosition = Vector3.Lerp(TimeSliderRectTransform.anchoredPosition, Vector3.up * -21, 0.2f * GameManager.FpsDeltaTime);
            }
            else
            {
                if (Input.GetKey(KeyCode.T))
                    TimeSliderRectTransform.anchoredPosition = Vector3.Lerp(TimeSliderRectTransform.anchoredPosition, Vector3.down * 29, 0.2f * GameManager.FpsDeltaTime);
                else
                    TimeSliderRectTransform.anchoredPosition = Vector3.Lerp(TimeSliderRectTransform.anchoredPosition, Vector3.down * -21, 0.2f * GameManager.FpsDeltaTime);
            }

            if (Input.GetKey(KeyCode.T) || RightClickMenu.Show)
            {
                selectNote = null;
                selectNoteMouseYBeat = MouseYBeat;
            }


            if (Input.GetKeyDown(KeyCode.Space))
                AutoScroll = !AutoScroll;

            MouseY();
            MouseBarAdd();
        }

        void AudioMove()
        {
            if (Time < 10 && !PlayerManager.playerManager.audioSource.isPlaying && !Pause)
            {
                Time = PlayerManager.playerManager.audioSource.clip.length - 0.001f;
                PlayerManager.playerManager.audioSource.time = Time;

                HitSound.BeatIndex = PlayerManager.mapData.AllBeat.Count - 2;
                EffectManager.CameraPosIndex = PlayerManager.mapData.Effect.Camera.CameraPosEffect.Count - 1;
                EffectManager.CameraZoomIndex = PlayerManager.mapData.Effect.Camera.CameraZoomEffect.Count - 1;
                EffectManager.UiPosIndex = PlayerManager.mapData.Effect.Camera.UiPosEffect.Count - 1;
                EffectManager.UiZoomIndex = PlayerManager.mapData.Effect.Camera.UiZoomEffect.Count - 1;
            }
            else if (Time >= PlayerManager.playerManager.audioSource.clip.length - 10 && !PlayerManager.playerManager.audioSource.isPlaying && !Pause)
            {
                Time = 0;
                PlayerManager.playerManager.audioSource.time = Time;

                HitSound.BeatIndex = 0;
                EffectManager.EffectAllIndexSet(0);
                PlayerManager.BPMChange(PlayerManager.mapData.Effect.BPM, 0);
            }

            if (!PlayerManager.playerManager.audioSource.isPlaying && PlayerManager.playerManager.audioSource.enabled)
            {
                PlayerManager.playerManager.audioSource.Play();
                if (Input.GetKey(KeyCode.UpArrow) || AutoScroll || Input.GetKey(KeyCode.DownArrow))
                    PlayerManager.playerManager.audioSource.time = Time;
            }

            if ((!(Input.GetKey(KeyCode.UpArrow) || AutoScroll || Input.GetKey(KeyCode.DownArrow))))
            {
                PlayerManager.playerManager.audioSource.Pause();
                if (tempTime != Time)
                    PlayerManager.playerManager.audioSource.UnPause();

                PlayerManager.playerManager.audioSource.time = Time;
                tempTime = Time;

                Pause = true;
            }

            if ((Input.GetKey(KeyCode.UpArrow) || AutoScroll))
            {
                PlayerManager.playerManager.audioSource.pitch = (float)(1 * PlayerManager.effect.Pitch * GameManager.GameSpeed);
                if (Input.GetKey(KeyCode.LeftControl))
                    PlayerManager.playerManager.audioSource.pitch = (float) (2 * PlayerManager.effect.Pitch * GameManager.GameSpeed);
                if (Input.GetKey(KeyCode.LeftShift))
                    PlayerManager.playerManager.audioSource.pitch = (float) (4 * PlayerManager.effect.Pitch * GameManager.GameSpeed);
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
                    PlayerManager.playerManager.audioSource.pitch = (float) (8 * PlayerManager.effect.Pitch * GameManager.GameSpeed);
                PlayerManager.playerManager.audioSource.UnPause();
                Time = PlayerManager.playerManager.audioSource.time;
                tempTime = Time;

                Pause = false;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                PlayerManager.playerManager.audioSource.pitch = (float) (-1 * PlayerManager.effect.Pitch * GameManager.GameSpeed);
                if (Input.GetKey(KeyCode.LeftControl))
                    PlayerManager.playerManager.audioSource.pitch = (float) (-2 * PlayerManager.effect.Pitch * GameManager.GameSpeed);
                if (Input.GetKey(KeyCode.LeftShift))
                    PlayerManager.playerManager.audioSource.pitch = (float) (-4 * PlayerManager.effect.Pitch * GameManager.GameSpeed);
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
                    PlayerManager.playerManager.audioSource.pitch = (float) (-8 * PlayerManager.effect.Pitch * GameManager.GameSpeed);
                PlayerManager.playerManager.audioSource.UnPause();
                Time = PlayerManager.playerManager.audioSource.time;
                tempTime = Time;

                Pause = false;
            }
        }

        void MouseY()
        {
            if (!GameManager.UpScroll)
                MouseYBeat = (MainCamera.Camera.ScreenToWorldPoint(Input.mousePosition - Vector3.forward * MainCamera.Camera.transform.position.z).y + 5.5f + MainCamera.UiPosY) / PlayerManager.effect.BeatYPos;
            else
                MouseYBeat = (MainCamera.Camera.ScreenToWorldPoint(Input.mousePosition - Vector3.forward * MainCamera.Camera.transform.position.z).y - 5.5f + MainCamera.UiPosY) / PlayerManager.effect.BeatYPos;
            if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
            {
                if (MouseYBeat < (45f / 180f) + (int)MouseYBeat)
                    MouseYBeat = 0 + (int)MouseYBeat;
                else if (MouseYBeat >= (45f / 180f) + (int)MouseYBeat && MouseYBeat < (90f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (45f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (90f / 180f) + (int)MouseYBeat && MouseYBeat < (135f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (90f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (135f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (135f / 180f) + (int)MouseYBeat;
            }
            else if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
            {
                if (MouseYBeat < (30f / 180f) + (int)MouseYBeat)
                    MouseYBeat = 0 + (int)MouseYBeat;
                else if (MouseYBeat >= (30f / 180f) + (int)MouseYBeat && MouseYBeat < (60f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (30f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (60f / 180f) + (int)MouseYBeat && MouseYBeat < (90f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (60f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (90f / 180f) + (int)MouseYBeat && MouseYBeat < (120f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (90f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (120f / 180f) + (int)MouseYBeat && MouseYBeat < (150f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (120f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (150f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (150f / 180f) + (int)MouseYBeat;
            }
            else if (Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
            {
                if (MouseYBeat < (15f / 180f) + (int)MouseYBeat)
                    MouseYBeat = 0 + (int)MouseYBeat;
                else if (MouseYBeat >= (15f / 180f) + (int)MouseYBeat && MouseYBeat < (75f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (15f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (75f / 180f) + (int)MouseYBeat && MouseYBeat < (90f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (75f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (90f / 180f) + (int)MouseYBeat && MouseYBeat < (105f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (90f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (105f / 180f) + (int)MouseYBeat && MouseYBeat < (165f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (105f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (165f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (165f / 180f) + (int)MouseYBeat;
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
            {
                if (MouseYBeat < (15f / 180f) + (int)MouseYBeat)
                    MouseYBeat = 0 + (int)MouseYBeat;
                else if (MouseYBeat >= (15f / 180f) + (int)MouseYBeat && MouseYBeat < (30f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (15f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (30f / 180f) + (int)MouseYBeat && MouseYBeat < (45f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (30f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (45f / 180f) + (int)MouseYBeat && MouseYBeat < (60f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (45f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (60f / 180f) + (int)MouseYBeat && MouseYBeat < (75f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (60f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (75f / 180f) + (int)MouseYBeat && MouseYBeat < (90f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (75f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (90f / 180f) + (int)MouseYBeat && MouseYBeat < (105f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (90f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (105f / 180f) + (int)MouseYBeat && MouseYBeat < (120f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (105f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (120f / 180f) + (int)MouseYBeat && MouseYBeat < (135f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (120f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (135f / 180f) + (int)MouseYBeat && MouseYBeat < (150f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (135f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (150f / 180f) + (int)MouseYBeat && MouseYBeat < (165f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (150f / 180f) + (int)MouseYBeat;
                else if (MouseYBeat >= (165f / 180f) + (int)MouseYBeat)
                    MouseYBeat = (165f / 180f) + (int)MouseYBeat;
            }
        }

        void MouseBarAdd()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.T) && !RightClickMenu.Show)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(MainCamera.Camera.ScreenToWorldPoint(Input.mousePosition - Vector3.forward * MainCamera.Camera.transform.position.z), Vector2.up, 0.01f, LayerMask.GetMask("UI"));
                selectNote = null;
                selectNoteMouseYBeat = MouseYBeat;

                if (raycastHit2D.collider != null)
                {
                    int index = NoteManager.noteBeats.BinarySearch(MouseYBeat);

                    if (raycastHit2D.collider.gameObject.name == "A")
                    {
                        if (PlayerManager.mapData.A.Contains(MouseYBeat))
                        {
                            PlayerManager.BarRemove(KeyCode.A, MouseYBeat);
                            PlayerManager.mapData.A.Remove(MouseYBeat);

                            NoteManager.ListRefresh();
                            PlayerManager.mapData.AllBeat.Remove(MouseYBeat);
                            if (NoteManager.noteHoldBeats[index] != 0)
                                PlayerManager.mapData.AllBeat.Remove(MouseYBeat + NoteManager.noteHoldBeats[index]);

                            PlayerManager.mapData.HoldA.Clear();
                            for (int i = 0; i < InputSystem.A.Count; i++)
                                PlayerManager.mapData.HoldA.Add(InputSystem.A[i].HoldBeat);

                            return;
                        }

                        selectNote = PlayerManager.NoteAdd(KeyCode.A, MouseYBeat, -1);
                        PlayerManager.mapData.A.Add(MouseYBeat);
                        PlayerManager.mapData.AllBeat.Add(MouseYBeat);
                        PlayerManager.mapData.A.Sort();
                        PlayerManager.mapData.AllBeat.Sort();
                        InputSystem.A.Sort((Note a, Note b) => a.Beat.CompareTo(b.Beat));

                        NoteManager.ListRefresh();
                        PlayerManager.mapData.HoldA.Clear();
                        for (int i = 0; i < InputSystem.A.Count; i++)
                            PlayerManager.mapData.HoldA.Add(InputSystem.A[i].HoldBeat);
                    }
                    else if (raycastHit2D.collider.gameObject.name == "S")
                    {
                        if (PlayerManager.mapData.S.Contains(MouseYBeat))
                        {
                            PlayerManager.BarRemove(KeyCode.S, MouseYBeat);
                            PlayerManager.mapData.S.Remove(MouseYBeat);

                            NoteManager.ListRefresh();
                            PlayerManager.mapData.AllBeat.Remove(MouseYBeat);
                            if (NoteManager.noteHoldBeats[index] != 0)
                                PlayerManager.mapData.AllBeat.Remove(MouseYBeat + NoteManager.noteHoldBeats[index]);

                            PlayerManager.mapData.HoldS.Clear();
                            for (int i = 0; i < InputSystem.S.Count; i++)
                                PlayerManager.mapData.HoldS.Add(InputSystem.S[i].HoldBeat);

                            return;
                        }

                        selectNote = PlayerManager.NoteAdd(KeyCode.S, MouseYBeat, -1);
                        PlayerManager.mapData.S.Add(MouseYBeat);
                        PlayerManager.mapData.AllBeat.Add(MouseYBeat);
                        PlayerManager.mapData.S.Sort();
                        PlayerManager.mapData.AllBeat.Sort();
                        InputSystem.S.Sort((Note a, Note b) => a.Beat.CompareTo(b.Beat));

                        NoteManager.ListRefresh();
                        PlayerManager.mapData.HoldS.Clear();
                        for (int i = 0; i < InputSystem.S.Count; i++)
                            PlayerManager.mapData.HoldS.Add(InputSystem.S[i].HoldBeat);
                    }
                    else if (raycastHit2D.collider.gameObject.name == "D")
                    {
                        if (PlayerManager.mapData.D.Contains(MouseYBeat))
                        {
                            PlayerManager.BarRemove(KeyCode.D, MouseYBeat);
                            PlayerManager.mapData.D.Remove(MouseYBeat);

                            NoteManager.ListRefresh();
                            PlayerManager.mapData.AllBeat.Remove(MouseYBeat);
                            if (NoteManager.noteHoldBeats[index] != 0)
                                PlayerManager.mapData.AllBeat.Remove(MouseYBeat + NoteManager.noteHoldBeats[index]);

                            PlayerManager.mapData.HoldD.Clear();
                            for (int i = 0; i < InputSystem.D.Count; i++)
                                PlayerManager.mapData.HoldD.Add(InputSystem.D[i].HoldBeat);

                            return;
                        }

                        selectNote = PlayerManager.NoteAdd(KeyCode.D, MouseYBeat, -1);
                        PlayerManager.mapData.D.Add(MouseYBeat);
                        PlayerManager.mapData.AllBeat.Add(MouseYBeat);
                        PlayerManager.mapData.D.Sort();
                        PlayerManager.mapData.AllBeat.Sort();
                        InputSystem.D.Sort((Note a, Note b) => a.Beat.CompareTo(b.Beat));

                        NoteManager.ListRefresh();
                        PlayerManager.mapData.HoldD.Clear();
                        for (int i = 0; i < InputSystem.D.Count; i++)
                            PlayerManager.mapData.HoldD.Add(InputSystem.D[i].HoldBeat);
                    }
                    else if (raycastHit2D.collider.gameObject.name == "J")
                    {
                        if (PlayerManager.mapData.J.Contains(MouseYBeat))
                        {
                            PlayerManager.BarRemove(KeyCode.J, MouseYBeat);
                            PlayerManager.mapData.J.Remove(MouseYBeat);

                            NoteManager.ListRefresh();
                            PlayerManager.mapData.AllBeat.Remove(MouseYBeat);
                            if (NoteManager.noteHoldBeats[index] != 0)
                                PlayerManager.mapData.AllBeat.Remove(MouseYBeat + NoteManager.noteHoldBeats[index]);

                            PlayerManager.mapData.HoldJ.Clear();
                            for (int i = 0; i < InputSystem.J.Count; i++)
                                PlayerManager.mapData.HoldJ.Add(InputSystem.J[i].HoldBeat);

                            return;
                        }

                        selectNote = PlayerManager.NoteAdd(KeyCode.J, MouseYBeat, -1);
                        PlayerManager.mapData.J.Add(MouseYBeat);
                        PlayerManager.mapData.AllBeat.Add(MouseYBeat);
                        PlayerManager.mapData.J.Sort();
                        PlayerManager.mapData.AllBeat.Sort();
                        InputSystem.J.Sort((Note a, Note b) => a.Beat.CompareTo(b.Beat));

                        NoteManager.ListRefresh();
                        PlayerManager.mapData.HoldJ.Clear();
                        for (int i = 0; i < InputSystem.J.Count; i++)
                            PlayerManager.mapData.HoldJ.Add(InputSystem.J[i].HoldBeat);
                    }
                    else if (raycastHit2D.collider.gameObject.name == "K")
                    {
                        if (PlayerManager.mapData.K.Contains(MouseYBeat))
                        {
                            PlayerManager.BarRemove(KeyCode.K, MouseYBeat);
                            PlayerManager.mapData.K.Remove(MouseYBeat);

                            NoteManager.ListRefresh();
                            PlayerManager.mapData.AllBeat.Remove(MouseYBeat);
                            if (NoteManager.noteHoldBeats[index] != 0)
                                PlayerManager.mapData.AllBeat.Remove(MouseYBeat + NoteManager.noteHoldBeats[index]);

                            PlayerManager.mapData.HoldK.Clear();
                            for (int i = 0; i < InputSystem.K.Count; i++)
                                PlayerManager.mapData.HoldK.Add(InputSystem.K[i].HoldBeat);

                            return;
                        }

                        selectNote = PlayerManager.NoteAdd(KeyCode.K, MouseYBeat, -1);
                        PlayerManager.mapData.K.Add(MouseYBeat);
                        PlayerManager.mapData.AllBeat.Add(MouseYBeat);
                        PlayerManager.mapData.K.Sort();
                        PlayerManager.mapData.AllBeat.Sort();
                        InputSystem.K.Sort((Note a, Note b) => a.Beat.CompareTo(b.Beat));

                        NoteManager.ListRefresh();
                        PlayerManager.mapData.HoldK.Clear();
                        for (int i = 0; i < InputSystem.K.Count; i++)
                            PlayerManager.mapData.HoldK.Add(InputSystem.K[i].HoldBeat);
                    }
                    else if (raycastHit2D.collider.gameObject.name == "L")
                    {
                        if (PlayerManager.mapData.L.Contains(MouseYBeat))
                        {
                            PlayerManager.BarRemove(KeyCode.L, MouseYBeat);
                            PlayerManager.mapData.L.Remove(MouseYBeat);

                            NoteManager.ListRefresh();
                            PlayerManager.mapData.AllBeat.Remove(MouseYBeat);
                            if (NoteManager.noteHoldBeats[index] != 0)
                                PlayerManager.mapData.AllBeat.Remove(MouseYBeat + NoteManager.noteHoldBeats[index]);

                            PlayerManager.mapData.HoldL.Clear();
                            for (int i = 0; i < InputSystem.L.Count; i++)
                                PlayerManager.mapData.HoldL.Add(InputSystem.L[i].HoldBeat);

                            return;
                        }

                        selectNote = PlayerManager.NoteAdd(KeyCode.L, MouseYBeat, -1);
                        PlayerManager.mapData.L.Add(MouseYBeat);
                        PlayerManager.mapData.AllBeat.Add(MouseYBeat);
                        PlayerManager.mapData.L.Sort();
                        PlayerManager.mapData.AllBeat.Sort();
                        InputSystem.L.Sort((Note a, Note b) => a.Beat.CompareTo(b.Beat));

                        NoteManager.ListRefresh();
                        PlayerManager.mapData.HoldL.Clear();
                        for (int i = 0; i < InputSystem.L.Count; i++)
                            PlayerManager.mapData.HoldL.Add(InputSystem.L[i].HoldBeat);
                    }
                }

                //0.08333333333333333, 0.1666666666666667, 0.25, 0.33333333333333, 0.4166666666666667, 0.5, 0.5833333333333333, 0.6666666666666667, 0.75, 0.8333333333333333, 0.9166666666666667, 1
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                if (selectNote != null)
                {
                    if (MouseYBeat - selectNoteMouseYBeat != 0)
                    {
                        selectNote.HoldBeat = MouseYBeat - selectNoteMouseYBeat;
                        selectNote.HoldNote.localScale = new Vector3(1, (float)(1.666666666666667 * GameManager.Abs(PlayerManager.effect.BeatYPos) * (MouseYBeat - selectNoteMouseYBeat)), 1);
                        selectNote.HoldNote.localPosition = new Vector2(0, (float)(GameManager.Abs(PlayerManager.effect.BeatYPos) * selectNote.HoldBeat));
                        selectNote.spriteRenderer.color = Color.green;

                        //즉사 패턴
                        if (selectNote.HoldBeat < 0 && selectNote.HoldBeat >= -1)
                        {
                            selectNote.HoldNote.localScale = Vector2.zero;
                            selectNote.spriteRenderer.color = Color.red;
                        }
                    }
                    else
                        selectNote.HoldNote.localScale = Vector2.zero;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (selectNote != null && MouseYBeat - selectNoteMouseYBeat != 0)
                {
                    //즉사 패턴
                    if (selectNote.HoldBeat < 0 && selectNote.HoldBeat >= -1)
                    {
                        selectNote.HoldBeat = -1;
                        ScoreManager.MaxScore -= 100;

                        PlayerManager.mapData.AllBeat.Remove(selectNoteMouseYBeat);
                        PlayerManager.mapData.AllBeat.Remove(MouseYBeat);
                    }
                    else if (selectNote.HoldBeat > 0)
                    {
                        PlayerManager.mapData.AllBeat.Add(MouseYBeat);
                        PlayerManager.mapData.AllBeat.Sort();
                    }

                    if (selectNote.keyCode == KeyCode.A)
                    {
                        PlayerManager.mapData.HoldA.Clear();
                        for (int i = 0; i < InputSystem.A.Count; i++)
                            PlayerManager.mapData.HoldA.Add(InputSystem.A[i].HoldBeat);
                    }
                    else if (selectNote.keyCode == KeyCode.S)
                    {
                        PlayerManager.mapData.HoldS.Clear();
                        for (int i = 0; i < InputSystem.S.Count; i++)
                            PlayerManager.mapData.HoldS.Add(InputSystem.S[i].HoldBeat);
                    }
                    else if (selectNote.keyCode == KeyCode.D)
                    {
                        PlayerManager.mapData.HoldD.Clear();
                        for (int i = 0; i < InputSystem.D.Count; i++)
                            PlayerManager.mapData.HoldD.Add(InputSystem.D[i].HoldBeat);
                    }
                    else if (selectNote.keyCode == KeyCode.J)
                    {
                        PlayerManager.mapData.HoldJ.Clear();
                        for (int i = 0; i < InputSystem.J.Count; i++)
                            PlayerManager.mapData.HoldJ.Add(InputSystem.J[i].HoldBeat);
                    }
                    else if (selectNote.keyCode == KeyCode.K)
                    {
                        PlayerManager.mapData.HoldK.Clear();
                        for (int i = 0; i < InputSystem.K.Count; i++)
                            PlayerManager.mapData.HoldK.Add(InputSystem.K[i].HoldBeat);
                    }
                    else if (selectNote.keyCode == KeyCode.L)
                    {
                        PlayerManager.mapData.HoldL.Clear();
                        for (int i = 0; i < InputSystem.L.Count; i++)
                            PlayerManager.mapData.HoldL.Add(InputSystem.L[i].HoldBeat);
                    }
                }
            }
        }

        public double tempCurrentBeat = 0;
        public float tempTime2 = 0;
        public float tempAudioClipLength = 0;
        public float tempMouseY = 0;

        public string MouseYBeatLang = "Mouse Beat: ";
        public string CurrentBeatLang = "Current Beat: ";
        public string TimeLang = "Time: ";

        IEnumerator Start()
        {
            CurrentBeatLang = LangManager.LangLoad(LangManager.Lang, "editMode.currentBeat");
            TimeLang = LangManager.LangLoad(LangManager.Lang, "editMode.time");
            MouseYBeatLang = LangManager.LangLoad(LangManager.Lang, "editMode.mouse_y_beat");
            //PlayerManager.playerManager.MidiPlayer.Play();

            while (true)
            {
                if (!PlayerManager.UIHide)
                {
                    if (tempCurrentBeat != PlayerManager.HitSoundCurrentBeat)
                        CurrentBeatText.text = CurrentBeatLang + (Mathf.RoundToInt((float)(PlayerManager.HitSoundCurrentBeat * 1000)) * 0.001);
                    if (tempTime2 != Time)
                        TimeText.text = TimeLang + ((double)Mathf.Round(Time * 1000) * 0.001);

                    if (Input.GetKey(KeyCode.T))
                        TimeSlider.value = Time;

                    if (tempMouseY != MainCamera.Camera.ScreenToWorldPoint(Input.mousePosition - Vector3.forward * MainCamera.Camera.transform.position.z).y)
                    {
                        MouseYBeatText.text = MouseYBeatLang + ((int)(MouseYBeat * 100) * 0.01);
                        if (Input.GetKey(KeyCode.LeftControl))
                            MouseYBeatText.text = MouseYBeatLang + ((int)(MouseYBeat * 1000) * 0.001);
                        tempMouseY = MainCamera.Camera.ScreenToWorldPoint(Input.mousePosition - Vector3.forward * MainCamera.Camera.transform.position.z).y;
                    }

                    tempAudioClipLength = PlayerManager.playerManager.audioSource.clip.length;
                    tempCurrentBeat = PlayerManager.HitSoundCurrentBeat;
                }

                yield return new WaitForSecondsRealtime(GameManager.FixedDeltaTime);
            }
        }

        public void TimeChange()
        {
            Time = TimeSlider.value;
            PlayerManager.HitSoundCurrentBeat = (PlayerManager.BeatTimer + Time - PlayerManager.StartDelay) * (PlayerManager.effect.BPM / 60) + 1;
            
            int temp = PlayerManager.mapData.AllBeat.IndexOf(GameManager.CloseValue(PlayerManager.mapData.AllBeat, PlayerManager.HitSoundCurrentBeat));
            if (temp >= PlayerManager.mapData.AllBeat.Count - 1)
                HitSound.BeatIndex = PlayerManager.mapData.AllBeat.Count - 1;
            else
                HitSound.BeatIndex = temp;
        }

        public void FNFCameraZoom()
        {
            DialogResult dialogResult = MessageBox.Show(LangManager.LangLoad(LangManager.Lang, "editMode.fnf_camera_zoom warning"), LangManager.LangLoad(LangManager.Lang, "editMode.fnf_camera_zoom"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                int loop = 0;
                dialogResult = MessageBox.Show(LangManager.LangLoad(LangManager.Lang, "editMode.fnf_camera_zoom 500"), LangManager.LangLoad(LangManager.Lang, "editMode.fnf_camera_zoom"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                    loop = 500;
                else
                {
                    dialogResult = MessageBox.Show(LangManager.LangLoad(LangManager.Lang, "editMode.fnf_camera_zoom 1000"), LangManager.LangLoad(LangManager.Lang, "editMode.fnf_camera_zoom"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                        loop = 1000;
                    else
                    {
                        dialogResult = MessageBox.Show(LangManager.LangLoad(LangManager.Lang, "editMode.fnf_camera_zoom 5000"), LangManager.LangLoad(LangManager.Lang, "editMode.fnf_camera_zoom"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dialogResult == DialogResult.Yes)
                            loop = 5000;
                        else
                        {
                            dialogResult = MessageBox.Show(LangManager.LangLoad(LangManager.Lang, "editMode.fnf_camera_zoom 10000"), LangManager.LangLoad(LangManager.Lang, "editMode.fnf_camera_zoom"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dialogResult == DialogResult.Yes)
                                loop = 10000;
                        }
                    }
                }


                PlayerManager.mapData.Effect.Camera.CameraZoomEffect.Clear();
                for (int i = 0; i < loop; i++)
                {
                    PlayerManager.mapData.Effect.Camera.CameraZoomEffect.Add(new CameraZoomEffect { Beat = 4 * i + 5, Value = 0.95, Lerp = 1 });
                    PlayerManager.mapData.Effect.Camera.CameraZoomEffect.Add(new CameraZoomEffect { Beat = 4 * i + 5, Value = 1, Lerp = 0.0625 });
                }
            }

            RightClickMenu.Show = false;
        }

        public void HitSoundReset()
        {
            DialogResult dialogResult = MessageBox.Show(LangManager.LangLoad(LangManager.Lang, "editMode.hit_sound_reset warning"), LangManager.LangLoad(LangManager.Lang, "editMode.hit_sound_reset"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                NoteManager.ListRefresh();
                PlayerManager.mapData.AllBeat.Clear();

                for (int i = 0; i < NoteManager.noteBeats.Count; i++)
                {
                    double item = NoteManager.noteHoldBeats[i];
                    if (!(item >= -1 && item < 0))
                        PlayerManager.mapData.AllBeat.Add(NoteManager.noteBeats[i]);
                }

                for (int i = 0; i < NoteManager.noteHoldBeats.Count; i++)
                {
                    double item = NoteManager.noteHoldBeats[i];
                    if (item > 0)
                        PlayerManager.mapData.AllBeat.Add(NoteManager.noteBeats[i] + item);
                }

                PlayerManager.mapData.AllBeat.Sort();
            }

            RightClickMenu.Show = false;
        }

        public void AllNoteReset()
        {
            DialogResult dialogResult = MessageBox.Show(LangManager.LangLoad(LangManager.Lang, "editMode.note_reset warning"), LangManager.LangLoad(LangManager.Lang, "editMode.note_reset"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                dialogResult = MessageBox.Show(LangManager.LangLoad(LangManager.Lang, "editMode.note_reset warning2"), LangManager.LangLoad(LangManager.Lang, "editMode.note_reset"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    dialogResult = MessageBox.Show(LangManager.LangLoad(LangManager.Lang, "editMode.note_reset warning3"), LangManager.LangLoad(LangManager.Lang, "editMode.note_reset"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        RightClickMenu.Show = false;

                        if (PlayerManager.MapJsonPath != "")
                        {
                            PlayerManager.mapData.A.Clear();
                            PlayerManager.mapData.S.Clear();
                            PlayerManager.mapData.D.Clear();
                            PlayerManager.mapData.J.Clear();
                            PlayerManager.mapData.K.Clear();
                            PlayerManager.mapData.L.Clear();
                            PlayerManager.mapData.HoldA.Clear();
                            PlayerManager.mapData.HoldS.Clear();
                            PlayerManager.mapData.HoldD.Clear();
                            PlayerManager.mapData.HoldJ.Clear();
                            PlayerManager.mapData.HoldK.Clear();
                            PlayerManager.mapData.HoldL.Clear();
                            PlayerManager.mapData.AllBeat.Clear();

                            PlayerManager.MapSave(PlayerManager.MapJsonPath);
                            PlayerManager.MapDataLoad(PlayerManager.MapJsonPath);
                        }
                    }
                }
            }

            RightClickMenu.Show = false;
        }
    }
}