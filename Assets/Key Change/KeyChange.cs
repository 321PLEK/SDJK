using Newtonsoft.Json;
using SDJK.Camera;
using SDJK.Language;
using SDJK.PlayMode;
using SDJK.Scene;
using SDJK.Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.MainMenu
{
    public class KeyChange : MonoBehaviour
    {
        public GameObject prefab;
        public AudioSource audioSource;

        public static double NextBeat = 0;
        public static double HitSoundNextBeat = 0;
        public static int HitSoundIndex = 0;

        KeyCode[] keyCodes;

        public KeyCode selectKey;

        public Text text;

        public Text A;
        public Text S;
        public Text D;
        public Text J;
        public Text K;
        public Text L;

        public Image image;

        void Awake()
        {
            string key = GameManager.A.ToString();
            A.text = "";
            A.text += key[0];
            if (key.Length > 1)
                A.text += key[1];

            key = GameManager.S.ToString();
            S.text = "";
            S.text += key[0];
            if (key.Length > 1)
                S.text += key[1];

            key = GameManager.D.ToString();
            D.text = "";
            D.text += key[0];
            if (key.Length > 1)
                D.text += key[1];

            key = GameManager.J.ToString();
            J.text = "";
            J.text += key[0];
            if (key.Length > 1)
                J.text += key[1];

            key = GameManager.K.ToString();
            K.text = "";
            K.text += key[0];
            if (key.Length > 1)
                K.text += key[1];

            key = GameManager.L.ToString();
            L.text = "";
            L.text += key[0];
            if (key.Length > 1)
                L.text += key[1];

            text.text = LangManager.LangLoad(LangManager.Lang, "setting.key_change select_key") + LangManager.LangLoad(LangManager.Lang, "none");

            if (keyCodes == null)
                keyCodes = Enum.GetValues(typeof(KeyCode)) as KeyCode[];

            GameManager.Level = "breakfast";
            string json = ResourcesManager.Search<string>("sdjk", ResourcesManager.MapPath + "breakfast");
            PlayerManager.mapData = JsonConvert.DeserializeObject<MapData>(json);

            SoundManager.PlayBGM(PlayerManager.mapData.BGM, true, (float)PlayerManager.mapData.Effect.Volume, 1, true, false);
            GameManager.BPM = (float)PlayerManager.mapData.Effect.BPM;
            GameManager.StartDelay = (float)PlayerManager.mapData.Offset + GameManager.InputOffset;
            PlayerManager.effect.BeatYPos = PlayerManager.mapData.Effect.BeatYPos;

            for (int i = 0; i < PlayerManager.mapData.A.Count; i++)
            {
                if (PlayerManager.mapData.A[i] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1])
                    NoteAdd(KeyCode.A, PlayerManager.mapData.A[i]);
            }
            for (int i = 0; i < PlayerManager.mapData.S.Count; i++)
            {
                if (PlayerManager.mapData.S[i] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1])
                    NoteAdd(KeyCode.S, PlayerManager.mapData.S[i]);
            }
            for (int i = 0; i < PlayerManager.mapData.D.Count; i++)
            {
                if (PlayerManager.mapData.D[i] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1])
                    NoteAdd(KeyCode.D, PlayerManager.mapData.D[i]);
            }
            for (int i = 0; i < PlayerManager.mapData.J.Count; i++)
            {
                if (PlayerManager.mapData.J[i] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1])
                    NoteAdd(KeyCode.J, PlayerManager.mapData.J[i]);
            }
            for (int i = 0; i < PlayerManager.mapData.K.Count; i++)
            {
                if (PlayerManager.mapData.K[i] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1])
                    NoteAdd(KeyCode.K, PlayerManager.mapData.K[i]);
            }
            for (int i = 0; i < PlayerManager.mapData.L.Count; i++)
            {
                if (PlayerManager.mapData.L[i] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1])
                    NoteAdd(KeyCode.L, PlayerManager.mapData.L[i]);
            }
        }

        IEnumerator HitSoundCoroutine;

        void Start()
        {
            HitSoundCoroutine = HitSound();
            StartCoroutine(HitSoundCoroutine);
        }

        IEnumerator HitSound()
        {
            //리소스팩 히트 사운드
            AudioClip hitSound = ResourcesManager.Search<AudioClip>("sdjk", ResourcesManager.SoundPath + "play mode/hit sound");

            bool temp = false;
            string NameSpace = ResourcesManager.GetStringNameSpace("sdjk:play mode/hit sound", out string ResourceName);
            ResourceName = ResourcesManager.SoundPath + ResourceName;

            foreach (AudioClip item in ResourcesManager.ResourcesPackBGMList)
            {
                if (item.name == ResourceName.Substring(ResourceName.LastIndexOf("/") + 1))
                {
                    temp = true;
                    hitSound = item;
                    break;
                }
            }

            if (!temp)
            {
                foreach (AudioClip[] item in ResourcesManager.BGMList)
                {
                    foreach (AudioClip item2 in item)
                    {
                        if (item2.name == ResourceName.Substring(ResourceName.LastIndexOf("/") + 1))
                        {
                            temp = true;
                            hitSound = item2;
                            break;
                        }
                    }
                }
            }

            if (!temp)
                hitSound = Resources.Load<AudioClip>(ResourceName.Replace("%NameSpace%", NameSpace));

            HitSoundNextBeat = 0;
            double CurrentBeat;

            while (true)
            {
                CurrentBeat = (GameManager.BeatTimer - GameManager.StartDelay) * (GameManager.BPM / 60);

                if (HitSoundIndex < PlayerManager.mapData.AllBeat.Count)
                {
                    if (CurrentBeat >= HitSoundNextBeat)
                    {
                        HitSoundIndex++;
                        HitSoundNextBeat = PlayerManager.mapData.AllBeat[HitSoundIndex] - 1.25;

                        if (HitSoundIndex > 0 && HitSoundIndex < PlayerManager.mapData.AllBeat.Count - 2 && PlayerManager.mapData.AllBeat[HitSoundIndex] != PlayerManager.mapData.AllBeat[HitSoundIndex - 1])
                            audioSource.PlayOneShot(hitSound, audioSource.volume * GameManager.MainVolume);
                        else if (HitSoundIndex <= 0)
                            audioSource.PlayOneShot(hitSound, audioSource.volume * GameManager.MainVolume);
                    }
                }

                yield return null;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                enabled = false;
                StopCoroutine(HitSoundCoroutine);
                SoundManager.StopAll(SoundType.BGM, false);
                SceneManager.SceneLoading("Main Menu");
                return;
            }

            MainCamera.CameraZoom = GameManager.Lerp(MainCamera.CameraZoom, 1, 0.125 * GameManager.FpsDeltaTime);
            if (GameManager.CurrentBeat >= NextBeat)
            {
                MainCamera.CameraZoom = 0.975;
                NextBeat += 2;
            }

            PlayerManager.VisibleCurrentBeat = GameManager.CurrentBeat;

            if (NextBeat - GameManager.CurrentBeat >= 10)
            {
                NextBeat = 0;
                HitSoundIndex = 0;
                HitSoundNextBeat = 0;
            }

            for (int i = 0; i < keyCodes.Length; i++)
            {
                KeyCode keyCode = keyCodes[i];
                if (Input.GetKeyUp(keyCode))
                {
                    selectKey = keyCode;
                    text.text = LangManager.LangLoad(LangManager.Lang, "setting.key_change select_key") + keyCode.ToString();
                }
            }
        }

        void NoteAdd(KeyCode keyCode, double Beat)
        {
            GameObject note = Instantiate(prefab);
            Beat -= 1.25;

            //노트 설정
            if (keyCode == KeyCode.A)
                note.transform.localPosition = new Vector2(-5.535f, (float)(Beat * PlayerManager.effect.BeatYPos - 5.5));
            else if (keyCode == KeyCode.S)
                note.transform.localPosition = new Vector2(-3.321f, (float)(Beat * PlayerManager.effect.BeatYPos - 5.5));
            else if (keyCode == KeyCode.D)
                note.transform.localPosition = new Vector2(-1.107f, (float)(Beat * PlayerManager.effect.BeatYPos - 5.5));
            else if (keyCode == KeyCode.J)
                note.transform.localPosition = new Vector2(1.107f, (float)(Beat * PlayerManager.effect.BeatYPos - 5.5));
            else if (keyCode == KeyCode.K)
                note.transform.localPosition = new Vector2(3.321f, (float)(Beat * PlayerManager.effect.BeatYPos - 5.5));
            else if (keyCode == KeyCode.L)
                note.transform.localPosition = new Vector2(5.535f, (float)(Beat * PlayerManager.effect.BeatYPos - 5.5));
        }

        public void keyChange(string keyCode)
        {
            string selectKey = this.selectKey.ToString();

            if (keyCode.Equals("A"))
            {
                A.text = "";
                A.text += selectKey[0];
                if (selectKey.Length > 1)
                    A.text += selectKey[1];

                GameManager.A = this.selectKey;
            }
            else if (keyCode.Equals("S"))
            {
                S.text = "";
                S.text += selectKey[0];
                if (selectKey.Length > 1)
                    S.text += selectKey[1];

                GameManager.S = this.selectKey;
            }
            else if (keyCode.Equals("D"))
            {
                D.text = "";
                D.text += selectKey[0];
                if (selectKey.Length > 1)
                    D.text += selectKey[1];

                GameManager.D = this.selectKey;
            }
            else if (keyCode.Equals("J"))
            {
                J.text = "";
                J.text += selectKey[0];
                if (selectKey.Length > 1)
                    J.text += selectKey[1];

                GameManager.J = this.selectKey;
            }
            else if (keyCode.Equals("K"))
            {
                K.text = "";
                K.text += selectKey[0];
                if (selectKey.Length > 1)
                    K.text += selectKey[1];

                GameManager.K = this.selectKey;
            }
            else if (keyCode.Equals("L"))
            {
                L.text = "";
                L.text += selectKey[0];
                if (selectKey.Length > 1)
                    L.text += selectKey[1];

                GameManager.L = this.selectKey;
            }
        }
    }
}