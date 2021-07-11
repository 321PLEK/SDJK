using Newtonsoft.Json;
using SDJK.Camera;
using SDJK.PlayMode;
using SDJK.Scene;
using SDJK.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDJK.MainMenu
{
    public class UiPosChange : MonoBehaviour
    {
        public GameObject prefab;
        public AudioSource audioSource;

        public static double NextBeat = 0;
        public static double HitSoundNextBeat = 0;
        public static int HitSoundIndex = 0;

        public RectTransform Combo;

        void Awake()
        {
            GameManager.BPM = 0;
            GameManager.Level = "breakfast";
            GameManager.CurrentBeat = 0;
            GameManager.BeatTimer = 0;
            string json = ResourcesManager.Search<string>("sdjk", ResourcesManager.MapPath + "breakfast");
            PlayerManager.mapData = JsonConvert.DeserializeObject<MapData>(json);

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

            Combo.anchoredPosition = GameManager.ComboPos;

            StartCoroutine(UiPosChangeAwake());
        }

        IEnumerator UiPosChangeAwake()
        {
            yield return new WaitForSeconds(0.5f);

            SoundManager.PlayBGM(PlayerManager.mapData.BGM, true, (float)PlayerManager.mapData.Effect.Volume, 1, true, false);
            GameManager.BPM = (float)PlayerManager.mapData.Effect.BPM;
            GameManager.StartDelay = (float)PlayerManager.mapData.Offset + GameManager.InputOffset;
            NextBeat = 0;
            HitSoundIndex = 0;
            HitSoundNextBeat = 0;
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

            if (Input.GetKey(KeyCode.Mouse0))
            {
                Combo.anchoredPosition = new Vector3(Input.mousePosition.x * (1280f / Screen.width), Input.mousePosition.y * (720f / Screen.height)) - new Vector3(640, 150);
                GameManager.ComboPos = new Vector3(Input.mousePosition.x * (1280f / Screen.width), Input.mousePosition.y * (720f / Screen.height)) - new Vector3(640, 150);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                Combo.anchoredPosition = Vector2.zero;
                GameManager.ComboPos = Vector2.zero;
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
    }
}