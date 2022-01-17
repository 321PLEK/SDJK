using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDJK.Camera;
using System;

namespace SDJK.PlayMode
{
    public class NoteManager : MonoBehaviour
    {
        public static NoteManager noteManager;
        public static List<Note> notes = new List<Note>();
        public static List<double> noteBeats = new List<double>();
        public static List<double> noteHoldBeats = new List<double>();
        double tempBeatYPos = 0;
        float tempCameraZPos = 0;

        IEnumerator Start()
        {
            noteManager = this;
            notes.Clear();
            notes.AddRange(noteManager.GetComponentsInChildren<Note>(true));

            while (true)
            {
                Refresh();
                yield return new WaitForFixedUpdate();
            }
        }

        void Update()
        {
            if (tempBeatYPos != PlayerManager.effect.BeatYPos || tempCameraZPos != MainCamera.CameraPos.z)
                Refresh();

            if (!GameManager.UpScroll)
                transform.SetPositionAndRotation(MainCamera.UiPos, Quaternion.Euler(MainCamera.UiRotation));
            else
                transform.SetPositionAndRotation(MainCamera.UiPos + Vector3.up * 11, Quaternion.Euler(MainCamera.UiRotation));
        }

        public static void ListRefresh()
        {
            notes.Clear();
            noteBeats.Clear();
            noteHoldBeats.Clear();
            notes.AddRange(noteManager.GetComponentsInChildren<Note>(true));
            notes.Sort((Note a, Note b) => a.Beat.CompareTo(b.Beat));

            for (int i = 0; i < notes.Count; i++)
            {
                Note note = notes[i];
                noteBeats.Add(note.Beat);
                noteHoldBeats.Add(note.HoldBeat);
            }
        }

        void Refresh()
        {
            if (PlayerManager.HP > 0.001f || PlayerManager.PracticeMode || PlayerManager.Editor || (PlayerManager.HP <= 0.001f && GameManager.GameSpeed > 0.35f))
            {
                if (PlayerManager.Editor)
                    ListRefresh();

                for (int i = 0; i < notes.Count; i++)
                {
                    Note item = notes[i];

                    //노트 자동 숨기기
                    Vector3 pos = item.transform.position;
                    float y = (float)((item.Beat * PlayerManager.effect.BeatYPos) - 5.5f);
                    Vector3 cameraPos = MainCamera.Camera.transform.position;

                    item.transform.localPosition = new Vector3(pos.x, y, MainCamera.CameraPos.z + 14);
                    item.barPosEffect.defaultPos.y = y;
                    item.HoldNote.localPosition = new Vector2(0, (float)(GameManager.Abs(PlayerManager.effect.BeatYPos) * item.HoldBeat));

                    if (!(item.HoldBeat >= -1 && item.HoldBeat < 0))
                    {
                        if (item.HoldBeat > 0 && (Input.GetKey(item.keyCode) && !PlayerManager.Editor) || PlayerManager.AutoMode)
                        {
                            double temp = 0;

                            if (item.Beat < PlayerManager.VisibleCurrentBeat)
                                temp = item.Beat - PlayerManager.VisibleCurrentBeat;

                            if (item.HoldBeat + temp < 0)
                                temp = -item.HoldBeat;

                            item.HoldNote.localScale = new Vector3(1, (float)(1.666666666666667 * GameManager.Abs(PlayerManager.effect.BeatYPos) * (item.HoldBeat + temp)), 1);
                        }
                        else if (!(Input.GetKey(item.keyCode) && !PlayerManager.Editor))
                            item.HoldNote.localScale = new Vector3(1, (float)(1.666666666666667 * GameManager.Abs(PlayerManager.effect.BeatYPos) * item.HoldBeat), 1);
                    }

                    if (!GameManager.IncreasedNoteReadability)
                    {
                        if (!GameManager.UpScroll
                            ? !(Vector2.Distance(new Vector2(0, y + MainCamera.UiPos.y), new Vector2(0, cameraPos.y - MainCamera.CameraPos.y)) < 20f * GameManager.Abs(MainCamera.UiZoom))
                            : !(Vector2.Distance(new Vector2(0, y + MainCamera.UiPos.y + 11), new Vector2(0, cameraPos.y - MainCamera.CameraPos.y)) < 20f * GameManager.Abs(MainCamera.UiZoom)))
                        {
                            item.spriteRenderer.enabled = false;
                            item.enabled = false;
                        }
                        else
                        {
                            item.spriteRenderer.enabled = true;
                            item.enabled = true;
                        }
                    }

                    if (item.Beat + item.HoldBeat >= PlayerManager.VisibleCurrentBeat && item.Beat <= PlayerManager.VisibleCurrentBeat && item.HoldBeat > 0)
                        item.enabled = true;
                }

                tempBeatYPos = PlayerManager.effect.BeatYPos;
                tempCameraZPos = MainCamera.CameraPos.z;
            }
        }
    }
}