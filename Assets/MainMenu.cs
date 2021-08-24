using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDJK.Language;
using SDJK.Sound;
using SDJK.Scene;
using SDJK.PlayMode;
using DiscordPresence;
using Newtonsoft.Json;
using UnityEngineInternal;
using SDJK.PlayMode.UI.Background;
using System.Windows.Forms;
using Application = UnityEngine.Application;

namespace SDJK.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        public static MainMenu mainMenu;
        public RectTransform mainMenuRectTransform;

        public RectTransform Logo;

        public CanvasScaler CanvasScaler;

        public Image LevelCover;

        public RectTransform SelectUI;
        public GameObject MapSelect;
        public GameObject EditorSelect;
        public GameObject LangSetting;
        public GameObject ResourcePackSetting;
        public GameObject AccountSetting;
        public GameObject Setting;

        public Text LevelInfo;

        public Slider VolumeSlider;
        public Text VolumeText;
        public InputField OffsetInputField;
        public InputField FPSLimitInputField;
        public InputField NickNameInputField;

        public BackgroundImage backgroundImage;
        public BackgroundVideo backgroundVideo;

        public static float NextBeat;

        public List<Image> MainMenuButton = new List<Image>();
        public int ButtonWidth = 0;
        public static int ButtonSelect = 0;

        bool LogoAniEnd = false;

        public List<string> AllLevelList = new List<string>();
        public List<string> LevelList = new List<string>();
        public List<string> ExtraLevelList = new List<string>();

        static GameObject GameObject;

        public GameObject PhoneControll;

        public Text PlayingText;
        public RectTransform GameStartText;

        public static bool Esc = true;
        public static float AFKTimer = 0;

        void Awake()
        {
            mainMenu = this;
            GameObject = gameObject;
        }

        void Start()
        {
            /*string json = ResourcesManager.Search<string>(ResourcesManager.GetStringNameSpace(GameManager.Level, out string Name), ResourcesManager.MapPath + Name);
            MapData mapData = JsonConvert.DeserializeObject<MapData>(json);

            GameManager.BPM = 180;
            GameManager.StartDelay = 0.7f;

            LevelCover.sprite = ResourcesManager.Search<Sprite>(ResourcesManager.GetStringNameSpace("cover/" + mapData.Cover, out string temp), ResourcesManager.GuiTexturePath + temp);
            LevelInfo.text = mapData.Artist + " - " + mapData.BGMName;
            LevelInfo.text += "\n" + LangManager.LangLoad(LangManager.Lang, "main_menu.level_select.difficulty") + " - " + LangManager.LangLoad(LangManager.Lang, "main_menu.level_select.difficulty." + mapData.Difficulty);
            LevelInfo.text += "\nBPM - " + mapData.Effect.BPM;

            SoundManager.PlayBGM("Main Menu", true, 0.3f, 1, true, false);
            GameManager.CurrentBeat = -1;
            GameManager.BeatTimer = 0;
            NextBeat = 0;*/

            OptimizationButtonText = _OptimizationButtonText;
            NoteInterpolationButtonText = _NoteInterpolationButtonText;
            EditorOptimizationButtonText = _EditorOptimizationButtonText;
            OsuHitSoundButtonText = _OsuHitSoundButtonText;
            UpScrollText = _UpScrollText;
            AllowIndirectMissText = _AllowIndirectMissText;
            LangReload();

            if (ButtonSelect.Equals(0) || ButtonSelect.Equals(1))
            {
                MapSelect.SetActive(true);
                EditorSelect.SetActive(false);
                LangSetting.SetActive(false);
                ResourcePackSetting.SetActive(false);
                AccountSetting.SetActive(false);
                Setting.SetActive(false);
            }
            else if (ButtonSelect.Equals(2))
            {
                MapSelect.SetActive(false);
                EditorSelect.SetActive(true);
                LangSetting.SetActive(false);
                ResourcePackSetting.SetActive(false);
                AccountSetting.SetActive(false);
                Setting.SetActive(false);
            }
            else if (ButtonSelect.Equals(3))
            {
                MapSelect.SetActive(false);
                EditorSelect.SetActive(false);
                LangSetting.SetActive(true);
                ResourcePackSetting.SetActive(false);
                AccountSetting.SetActive(false);
                Setting.SetActive(false);
            }
            else if (ButtonSelect.Equals(4))
            {
                MapSelect.SetActive(false);
                EditorSelect.SetActive(false);
                LangSetting.SetActive(false);
                ResourcePackSetting.SetActive(true);
                AccountSetting.SetActive(false);
                Setting.SetActive(false);
            }
            else if (ButtonSelect.Equals(5))
            {
                MapSelect.SetActive(false);
                EditorSelect.SetActive(false);
                LangSetting.SetActive(false);
                ResourcePackSetting.SetActive(false);
                AccountSetting.SetActive(true);
                Setting.SetActive(false);
            }
            else if (ButtonSelect.Equals(6))
            {
                MapSelect.SetActive(false);
                EditorSelect.SetActive(false);
                LangSetting.SetActive(false);
                ResourcePackSetting.SetActive(false);
                AccountSetting.SetActive(false);
                Setting.SetActive(true);
            }

            if (!GameManager.Ratio_9_16)
                CanvasScaler.referenceResolution = new Vector2(1280, 720);
            else
                CanvasScaler.referenceResolution = new Vector2(600, 1280);
            
            Logo.anchoredPosition = new Vector2(CanvasScaler.referenceResolution.x * 0.5f, -CanvasScaler.referenceResolution.y - 155);

            //Button Sort
            ButtonSort(false);

            ResourcesManager.resourcesManager.mainMenu = this;
            ResourcesManager.LevelRefresh();
            ResourcesManager.ExtraLevelRefresh();

            if (Esc)
            {
                GameManager.AllLevelIndex = UnityEngine.Random.Range(0, AllLevelList.Count);
                GameManager.Level = AllLevelList[GameManager.AllLevelIndex];
                LevelRefresh();
            }
            else 
                LevelRefresh();
        }

        void Update()
        {
            if (GameManager.Ratio_9_16)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                    GameManager.LeftKey = true;
                if (Input.GetKeyDown(KeyCode.DownArrow))
                    GameManager.RightKey = true;
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    GameManager.UpKey = true;
                if (Input.GetKeyDown(KeyCode.RightArrow))
                    GameManager.DownKey = true;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                    GameManager.UpKey = true;
                if (Input.GetKeyDown(KeyCode.DownArrow))
                    GameManager.DownKey = true;
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    GameManager.LeftKey = true;
                if (Input.GetKeyDown(KeyCode.RightArrow))
                    GameManager.RightKey = true;
            }

            if (Input.GetKeyDown(KeyCode.Return))
                GameManager.EnterKey = true;

            AFKTimer += GameManager.DeltaTime;

            if (Input.anyKeyDown)
                AFKTimer = 0;
            else if (AFKTimer >= 60)
                Esc = true;

            if ((GameManager.CurrentBeat < 0 && !LogoAniEnd) || Esc)
            {
                Logo.anchoredPosition = Vector2.Lerp(Logo.anchoredPosition, new Vector2(CanvasScaler.referenceResolution.x * 0.5f, -mainMenuRectTransform.sizeDelta.y * 0.5f), 0.15f * GameManager.FpsDeltaTime);
                Logo.localScale = Vector2.Lerp(Logo.localScale, Vector2.one, 0.15f * GameManager.FpsDeltaTime);
                SelectUI.anchoredPosition = Vector2.Lerp(SelectUI.anchoredPosition, new Vector2(664, 0), 0.15f * GameManager.FpsDeltaTime);
                if (!(GameManager.CurrentBeat < 0 && !LogoAniEnd && !Esc) && GameManager.Ratio_9_16)
                    PlayingText.rectTransform.anchoredPosition = Vector2.Lerp(PlayingText.rectTransform.anchoredPosition, new Vector2(10, -40), 0.15f * GameManager.FpsDeltaTime);
                else
                    PlayingText.rectTransform.anchoredPosition = Vector2.Lerp(PlayingText.rectTransform.anchoredPosition, new Vector2(10, 10), 0.15f * GameManager.FpsDeltaTime);
                backgroundVideo.videoPlayer.targetCameraAlpha = GameManager.Lerp(backgroundVideo.videoPlayer.targetCameraAlpha, 0.4f, 0.15f * GameManager.FpsDeltaTime);
                if (backgroundImage.image.sprite != null)
                    backgroundImage.image.color = Color.Lerp(backgroundImage.image.color, new Color(1, 1, 1, 0.4f), 0.15f * GameManager.FpsDeltaTime);

                if (Esc)
                    GameStartText.anchoredPosition = Vector2.Lerp(GameStartText.anchoredPosition, new Vector2(-10, 10), 0.15f * GameManager.FpsDeltaTime);

                if (!GameManager.Ratio_9_16)
                {
                    CanvasScaler.referenceResolution = new Vector2(1280, 720);
                    SelectUI.localScale = Vector3.one;
                    SelectUI.pivot = new Vector2(0.5f, 0.5f);
                    ButtonWidth = 4;
                }
                else
                {
                    CanvasScaler.referenceResolution = new Vector2(600, 1280);
                    SelectUI.localScale = Vector3.one * 0.693f;
                    SelectUI.pivot = Vector2.right;
                    ButtonWidth = 1;
                }

                ButtonWidth = GameManager.Clamp(ButtonWidth, 1, MainMenuButton.Count);

                //Button Sort
                ButtonSort();

                if (NextBeat <= GameManager.CurrentBeat)
                {
                    NextBeat += 1;
                    Logo.anchoredPosition += new Vector2(0, 5);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    LogoAniEnd = true;
                    Esc = false;
                }

                if (GameManager.LeftKey)
                {
                    GameManager.AllLevelIndex--;

                    if (GameManager.AllLevelIndex < 0)
                        GameManager.AllLevelIndex = AllLevelList.Count - 1;

                    GameManager.Level = AllLevelList[GameManager.AllLevelIndex];
                    LevelRefresh();
                }
                else if (GameManager.RightKey)
                {
                    GameManager.AllLevelIndex++;

                    if (GameManager.AllLevelIndex > AllLevelList.Count - 1)
                        GameManager.AllLevelIndex = 0;

                    GameManager.Level = AllLevelList[GameManager.AllLevelIndex];
                    LevelRefresh();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    Esc = true;

                LogoAniEnd = true;

                if (!PhoneControll.activeSelf && Application.platform == RuntimePlatform.Android)
                    PhoneControll.SetActive(true);
                
                /*if (!b)
                {
                    string json = ResourcesManager.Search<string>(ResourcesManager.GetStringNameSpace(GameManager.Level.ToString(), out string Name), ResourcesManager.MapPath + Name);
                    MapData mapData = JsonConvert.DeserializeObject<MapData>(json);

                    GameManager.BPM = (float) mapData.Effect.BPM;
                    GameManager.StartDelay = (float) mapData.Offset;

                    SoundManager.StopAll(SoundType.BGM, false);
                    SoundManager.PlayBGM(mapData.BGM, true, (float)mapData.Effect.Volume, 1, true, false);

                    GameManager.BeatTimer = 0;
                    GameManager.CurrentBeat = -1;
                    NextBeat = 0;
                }*/

                if (GameManager.Ratio_9_16)
                    Logo.anchoredPosition = Vector2.Lerp(Logo.anchoredPosition, new Vector2(358, -167), 0.15f * GameManager.FpsDeltaTime);
                else
                    Logo.anchoredPosition = Vector2.Lerp(Logo.anchoredPosition, new Vector2(300, -190), 0.15f * GameManager.FpsDeltaTime);

                Logo.localScale = Vector2.Lerp(Logo.localScale, new Vector2(0.9f, 0.9f), 0.15f * GameManager.FpsDeltaTime);
                SelectUI.anchoredPosition = Vector2.Lerp(SelectUI.anchoredPosition, new Vector2(0, 0), 0.15f * GameManager.FpsDeltaTime);
                if (GameManager.Ratio_9_16)
                    PlayingText.rectTransform.anchoredPosition = Vector2.Lerp(PlayingText.rectTransform.anchoredPosition, new Vector2(10, -40), 0.15f * GameManager.FpsDeltaTime);
                else
                    PlayingText.rectTransform.anchoredPosition = Vector2.Lerp(PlayingText.rectTransform.anchoredPosition, new Vector2(10, 10), 0.15f * GameManager.FpsDeltaTime);
                GameStartText.anchoredPosition = Vector2.Lerp(GameStartText.anchoredPosition, new Vector2(-10, -40), 0.15f * GameManager.FpsDeltaTime);
                backgroundVideo.videoPlayer.targetCameraAlpha = GameManager.Lerp(backgroundVideo.videoPlayer.targetCameraAlpha, 0.15f, 0.15f * GameManager.FpsDeltaTime);
                if (backgroundImage.image.sprite != null)
                    backgroundImage.image.color = Color.Lerp(backgroundImage.image.color, new Color(1, 1, 1, 0.15f), 0.15f * GameManager.FpsDeltaTime);

                if (!GameManager.Ratio_9_16)
                {
                    CanvasScaler.referenceResolution = new Vector2(1280, 720);
                    SelectUI.localScale = Vector3.one;
                    SelectUI.pivot = new Vector2(0.5f, 0.5f);
                    ButtonWidth = 4;
                }
                else
                {
                    CanvasScaler.referenceResolution = new Vector2(600, 1280);
                    SelectUI.localScale = Vector3.one * 0.693f;
                    SelectUI.pivot = Vector2.right;
                    ButtonWidth = 1;
                }

                ButtonWidth = GameManager.Clamp(ButtonWidth, 1, MainMenuButton.Count);

                //Button Sort
                ButtonSort();

                //Button Select
                if (GameManager.LeftKey)
                    ButtonSelect -= 1;
                else if (GameManager.RightKey)
                    ButtonSelect += 1;

                if (ButtonSelect > MainMenuButton.Count - 1)
                    ButtonSelect = 0;
                else if (ButtonSelect < 0)
                    ButtonSelect = MainMenuButton.Count - 1;

                //Screen
                if (GameManager.LeftKey || GameManager.RightKey)
                {
                    if (ButtonSelect.Equals(0) || ButtonSelect.Equals(1))
                    {
                        MapSelect.SetActive(true);
                        EditorSelect.SetActive(false);
                        LangSetting.SetActive(false);
                        ResourcePackSetting.SetActive(false);
                        AccountSetting.SetActive(false);
                        Setting.SetActive(false);
                    }
                    else if (ButtonSelect.Equals(2))
                    {
                        MapSelect.SetActive(false);
                        EditorSelect.SetActive(true);
                        LangSetting.SetActive(false);
                        ResourcePackSetting.SetActive(false);
                        AccountSetting.SetActive(false);
                        Setting.SetActive(false);
                    }
                    else if (ButtonSelect.Equals(3))
                    {
                        MapSelect.SetActive(false);
                        EditorSelect.SetActive(false);
                        LangSetting.SetActive(true);
                        ResourcePackSetting.SetActive(false);
                        AccountSetting.SetActive(false);
                        Setting.SetActive(false);
                    }
                    else if (ButtonSelect.Equals(4))
                    {
                        MapSelect.SetActive(false);
                        EditorSelect.SetActive(false);
                        LangSetting.SetActive(false);
                        ResourcePackSetting.SetActive(true);
                        AccountSetting.SetActive(false);
                        Setting.SetActive(false);
                    }
                    else if (ButtonSelect.Equals(5))
                    {
                        MapSelect.SetActive(false);
                        EditorSelect.SetActive(false);
                        LangSetting.SetActive(false);
                        ResourcePackSetting.SetActive(false);
                        AccountSetting.SetActive(true);
                        Setting.SetActive(false);
                    }
                    else if (ButtonSelect.Equals(6))
                    {
                        MapSelect.SetActive(false);
                        EditorSelect.SetActive(false);
                        LangSetting.SetActive(false);
                        ResourcePackSetting.SetActive(false);
                        AccountSetting.SetActive(false);
                        Setting.SetActive(true);
                    }
                }

                //Map Start
                if (ButtonSelect.Equals(0) && GameManager.EnterKey)
                {
                    SceneManager.SceneLoading("Play Mode");
                    enabled = false;
                    
                    PlayerManager.AutoMode = false;
                    PlayerManager.PracticeMode = false;

                    if (Input.GetKey(KeyCode.A) || GameManager.AKey)
                        PlayerManager.AutoMode = true;
                    if (Input.GetKey(KeyCode.P) || GameManager.PKey)
                        PlayerManager.PracticeMode = true;
                }

                //Map Start
                if (ButtonSelect.Equals(1) && GameManager.EnterKey)
                {
                    SceneManager.SceneLoading("Play Mode");
                    enabled = false;

                    PlayerManager.AutoMode = false;
                    PlayerManager.PracticeMode = false;

                    if (Input.GetKey(KeyCode.A) || GameManager.AKey)
                        PlayerManager.AutoMode = true;
                    if (Input.GetKey(KeyCode.P) || GameManager.PKey)
                        PlayerManager.PracticeMode = true;
                }

                //Editor Start
                if (ButtonSelect.Equals(2) && GameManager.EnterKey)
                {
                    SceneManager.SceneLoading("Editor Mode");
                    enabled = false;
                }

                //BGM Change
                if ((GameManager.LeftKey || GameManager.RightKey || GameManager.UpKey || GameManager.DownKey) && (ButtonSelect.Equals(0) || ButtonSelect.Equals(1)))
                {
                    //Level Mix Max
                    if (ButtonSelect.Equals(0))
                    {
                        if (GameManager.UpKey)
                            GameManager.LevelIndex -= 1;
                        else if (GameManager.DownKey)
                            GameManager.LevelIndex += 1;

                        if (GameManager.LevelIndex < 0)
                            GameManager.LevelIndex = LevelList.Count - 1;
                        else if (GameManager.LevelIndex >= LevelList.Count)
                            GameManager.LevelIndex = 0;

                        GameManager.Level = LevelList[GameManager.LevelIndex];
                    }

                    //Level Mix Max
                    if (ButtonSelect.Equals(1))
                    {
                        if (GameManager.UpKey)
                            GameManager.ExtraLevelIndex -= 1;
                        else if (GameManager.DownKey)
                            GameManager.ExtraLevelIndex += 1;

                        if (GameManager.ExtraLevelIndex < 0)
                            GameManager.ExtraLevelIndex = ExtraLevelList.Count - 1;
                        else if (GameManager.ExtraLevelIndex >= ExtraLevelList.Count)
                            GameManager.ExtraLevelIndex = 0;

                        GameManager.Level = ExtraLevelList[GameManager.ExtraLevelIndex];
                    }

                    LevelRefresh();

                    /*if (ButtonSelect.Equals(0) || ButtonSelect.Equals(1))
                    {
                        
                    }
                    else
                    {
                        GameManager.StartDelay = 0.85f;
                        GameManager.BPM = 180;

                        SoundManager.StopAll(SoundType.BGM, false);
                        SoundManager.PlayBGM("Main Menu", true, 0.3f, 1, true, false);

                        GameManager.BeatTimer = 0;
                        GameManager.CurrentBeat = -1;
                        NextBeat = 0;
                    }*/
                }

                if (NextBeat <= GameManager.CurrentBeat)
                {
                    NextBeat += 1;
                    Logo.anchoredPosition += new Vector2(0, 5);
                }

                //Setting UI Change
                VolumeSlider.value = GameManager.MainVolume * 100;
                VolumeText.text = VolumeLang + " " + Mathf.RoundToInt(GameManager.MainVolume * 100) + "%";
            }
        }

        void FixedUpdate()
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                if (AFKTimer < 60)
                {
                    if (ButtonSelect.Equals(0))
                        PresenceManager.UpdatePresence("Main Menu", "Map Select Screen");
                    else if (ButtonSelect.Equals(1))
                        PresenceManager.UpdatePresence("Main Menu", "Extra Map Select Screen");
                    else if (ButtonSelect.Equals(2))
                        PresenceManager.UpdatePresence("Main Menu", "Editor Select Screen");
                    else if (ButtonSelect.Equals(3))
                        PresenceManager.UpdatePresence("Main Menu", "Lang Select Screen");
                    else if (ButtonSelect.Equals(4))
                        PresenceManager.UpdatePresence("Main Menu", "Resource Pack Select Screen");
                    else if (ButtonSelect.Equals(5))
                        PresenceManager.UpdatePresence("Main Menu", "Setting Screen");
                }
                else
                    PresenceManager.UpdatePresence("AFK...", "AFK Time: " + Mathf.RoundToInt(AFKTimer));
            }
        }

        public static void LevelRefresh()
        {
            string json = ResourcesManager.Search<string>(ResourcesManager.GetStringNameSpace(GameManager.Level, out string Name), ResourcesManager.MapPath + Name);

            MapData mapData = JsonConvert.DeserializeObject<MapData>(json);
            PlayerManager.mapData = mapData;

            mainMenu.LevelCover.sprite = null;
            Resources.UnloadUnusedAssets();
            mainMenu.LevelCover.sprite = ResourcesManager.Search<Sprite>(ResourcesManager.GetStringNameSpace("cover/" + mapData.Cover, out string temp), ResourcesManager.GuiTexturePath + temp);
            mainMenu.LevelInfo.text = mapData.Artist + " - " + mapData.BGMName;
            mainMenu.LevelInfo.text += "\n" + LangManager.LangLoad(LangManager.Lang, "main_menu.level_select.difficulty") + " - " + LangManager.LangLoad(LangManager.Lang, "main_menu.level_select.difficulty." + mapData.Difficulty);
            mainMenu.LevelInfo.text += "\nBPM - " + mapData.Effect.BPM;
            if (GameManager.mapRecord.ContainsKey(GameManager.Level))
                mainMenu.LevelInfo.text += "\n" + LangManager.LangLoad(LangManager.Lang, "main_menu.level_select.record") + " - " + Mathf.RoundToInt((float)(GameManager.mapRecord[GameManager.Level] * 100)) * 0.01 + "%";
            if (GameManager.mapAccuracy.ContainsKey(GameManager.Level))
                mainMenu.LevelInfo.text += "\n" + LangManager.LangLoad(LangManager.Lang, "main_menu.level_select.accuracy") + " - " + Mathf.RoundToInt((float)(GameManager.Lerp(100, 0, GameManager.Abs(GameManager.mapAccuracy[GameManager.Level]) / 0.75) * 100)) * 0.01 + "%";
            if (GameManager.mapRank.ContainsKey(GameManager.Level))
                mainMenu.LevelInfo.text += "\n" + LangManager.LangLoad(LangManager.Lang, "main_menu.level_select.rank") + " - " + GameManager.mapRank[GameManager.Level];

            GameManager.BPM = (float)mapData.Effect.BPM;
            GameManager.StartDelay = (float)mapData.Offset;
            GameManager.BeatTimer = 0;
            GameManager.CurrentBeat = -1;
            NextBeat = 0;

            SoundManager.StopAll(SoundType.BGM, false);
            SoundManager.PlayBGM(mapData.BGM, true, (float)mapData.Effect.Volume, 1, true, false);

            mainMenu.PlayingText.text = Playing + mapData.Artist + " - " + mapData.BGMName;

            mainMenu.backgroundVideo.StartCoroutine(mainMenu.backgroundVideo.Rerender());
            mainMenu.backgroundImage.Rerender();
        }

        void ButtonSort(bool Lerp = true)
        {
            if ((LogoAniEnd && !Esc) || !Lerp)
            {
                if (!GameManager.Ratio_9_16)
                {
                    int i = 0;
                    int ButtonHeight = Mathf.CeilToInt((float)MainMenuButton.Count / ButtonWidth);
                    for (int ii = ButtonHeight; ii >= 0; ii--)
                    {
                        for (int iii = 0; iii < ButtonWidth; iii++)
                        {
                            if (i >= MainMenuButton.Count)
                                break;

                            Image item = MainMenuButton[i];

                            if (item != null)
                            {
                                if (Lerp)
                                    item.rectTransform.anchoredPosition = Vector2.Lerp(item.rectTransform.anchoredPosition, new Vector2(140 * iii + 100, 130 * ii), 0.15f * GameManager.FpsDeltaTime);
                                else
                                    item.rectTransform.anchoredPosition = new Vector2(140 * iii + 100, item.rectTransform.anchoredPosition.y);

                                if (ButtonSelect == i)
                                    item.color = Color.Lerp(item.color, new Color(0.3f, 0.3f, 0.3f), 0.1f * GameManager.FpsDeltaTime);
                                else
                                    item.color = Color.Lerp(item.color, Color.white, 0.1f * GameManager.FpsDeltaTime);

                                i++;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < MainMenuButton.Count; i++)
                    {
                        Image item = MainMenuButton[MainMenuButton.Count - 1 - i];
                        item.rectTransform.anchoredPosition = new Vector2(item.rectTransform.anchoredPosition.x, 140 * i + 130);
                        item.rectTransform.anchoredPosition = Vector2.Lerp(item.rectTransform.anchoredPosition, new Vector2(70.5f, item.rectTransform.anchoredPosition.y), 0.15f * GameManager.FpsDeltaTime);

                        if (ButtonSelect == i)
                        {
                            Image image = MainMenuButton[i];
                            image.color = Color.Lerp(image.color, new Color(0.3f, 0.3f, 0.3f), 0.1f * GameManager.FpsDeltaTime);
                        }
                        else
                        {
                            Image image = MainMenuButton[i];
                            image.color = Color.Lerp(image.color, Color.white, 0.1f * GameManager.FpsDeltaTime);
                        }
                    }
                }
            }
            else
            {
                if (!GameManager.Ratio_9_16)
                {
                    for (int i = 0; i < MainMenuButton.Count; i++)
                    {
                        Image item = MainMenuButton[MainMenuButton.Count - 1 - i];
                        item.rectTransform.anchoredPosition = Vector2.Lerp(item.rectTransform.anchoredPosition, new Vector2(item.rectTransform.anchoredPosition.x, -50), 0.15f * GameManager.FpsDeltaTime);
                    }
                }
                else
                {
                    for (int i = 0; i < MainMenuButton.Count; i++)
                    {
                        Image item = MainMenuButton[MainMenuButton.Count - 1 - i];
                        item.rectTransform.anchoredPosition = Vector2.Lerp(item.rectTransform.anchoredPosition, new Vector2(-50, item.rectTransform.anchoredPosition.y), 0.15f * GameManager.FpsDeltaTime);
                    }
                }
            }
        }

        public static string VolumeLang = "Volume";
        public static string On = "On";
        public static string Off = "Off";
        public static string Optimization = "Optimization";
        public static string NoteInterpolation = "Note Interpolation";
        public static string EditorOptimization = "Editor Optimization";
        public static string OsuHitSound = "Osu Hit Sound";
        public static string UpScroll = "Up Scroll";
        public static string Playing = "Playing: ";
        public static string AllowIndirectMiss = "Allow Indirect Miss";

        public static void AllRerender()
        {
            ResourcesManager.NameSpaceRefresh();
            ResourcesManager.LangRefresh();
            ResourcesManager.LevelRefresh();
            ResourcesManager.ExtraLevelRefresh();
            ResourcesManager.BGMRefresh();

            GameManager.LevelIndex = 0;
            GameManager.ExtraLevelIndex = 0;

            if (!ResourcesManager.LangList.Contains(LangManager.Lang))
                LangManager.Lang = "en_us";

            LangReload();

            SystemUI.SystemUI.systemUI.Renderer();

            GameObject.BroadcastMessage("Rerender");

            foreach (Transform item in SoundManager.soundManager.GetComponentsInChildren<Transform>(true))
            {
                SoundPrefab soundPrefab = item.GetComponent<SoundPrefab>();

                if (soundPrefab != null && soundPrefab.BGM)
                    soundPrefab.Reload();
            }

            Resources.UnloadUnusedAssets();
        }

        public static void LangReload()
        {
            VolumeLang = LangManager.LangLoad(LangManager.Lang, "setting.volume");
            On = LangManager.LangLoad(LangManager.Lang, "setting.on");
            Off = LangManager.LangLoad(LangManager.Lang, "setting.off");
            Optimization = LangManager.LangLoad(LangManager.Lang, "setting.optimization");
            NoteInterpolation = LangManager.LangLoad(LangManager.Lang, "setting.note_interpolation");
            EditorOptimization = LangManager.LangLoad(LangManager.Lang, "setting.editor_optimization");
            OsuHitSound = LangManager.LangLoad(LangManager.Lang, "setting.osu_hit_sound");
            UpScroll = LangManager.LangLoad(LangManager.Lang, "setting.up_scroll");
            Playing = LangManager.LangLoad(LangManager.Lang, "main_menu.playing");
            AllowIndirectMiss = LangManager.LangLoad(LangManager.Lang, "setting.allow_indirect_miss");

            if (GameManager.Optimization)
                OptimizationButtonText.text = Optimization + ": " + On;
            else
                OptimizationButtonText.text = Optimization + ": " + Off;

            if (GameManager.NoteInterpolation)
                NoteInterpolationButtonText.text = NoteInterpolation + ": " + On;
            else
                NoteInterpolationButtonText.text = NoteInterpolation + ": " + Off;
            
            if (GameManager.EditorOptimization)
                EditorOptimizationButtonText.text = EditorOptimization + ": " + On;
            else
                EditorOptimizationButtonText.text = EditorOptimization + ": " + Off;

            if (GameManager.OsuHitSound)
                OsuHitSoundButtonText.text = OsuHitSound + ": " + On;
            else
                OsuHitSoundButtonText.text = OsuHitSound + ": " + Off;

            if (GameManager.UpScroll)
                UpScrollText.text = UpScroll + ": " + On;
            else
                UpScrollText.text = UpScroll + ": " + Off;

            if (GameManager.AllowIndirectMiss)
                AllowIndirectMissText.text = AllowIndirectMiss + ": " + On;
            else
                AllowIndirectMissText.text = AllowIndirectMiss + ": " + Off;
        }

        public void SetVolume() => GameManager.MainVolume = VolumeSlider.value * 0.01f;

        public static Text OptimizationButtonText;
        public Text _OptimizationButtonText;
        
        public static Text NoteInterpolationButtonText;
        public Text _NoteInterpolationButtonText;
        
        public static Text EditorOptimizationButtonText;
        public Text _EditorOptimizationButtonText;

        public static Text OsuHitSoundButtonText;
        public Text _OsuHitSoundButtonText;

        public static Text UpScrollText;
        public Text _UpScrollText;

        public static Text AllowIndirectMissText;
        public Text _AllowIndirectMissText;

        public void OptimizationToggle()
        {
            GameManager.Optimization = !GameManager.Optimization;

            if (GameManager.Optimization)
                OptimizationButtonText.text = Optimization + ": " + On;
            else
                OptimizationButtonText.text = Optimization + ": " + Off;
        }
        
        public void NoteInterpolationToggle()
        {
            GameManager.NoteInterpolation = !GameManager.NoteInterpolation;

            if (GameManager.NoteInterpolation)
                NoteInterpolationButtonText.text = NoteInterpolation + ": " + On;
            else
                NoteInterpolationButtonText.text = NoteInterpolation + ": " + Off;
        }
        
        public void EditorOptimizationToggle()
        {
            GameManager.EditorOptimization = !GameManager.EditorOptimization;

            if (GameManager.EditorOptimization)
                EditorOptimizationButtonText.text = EditorOptimization + ": " + On;
            else
                EditorOptimizationButtonText.text = EditorOptimization + ": " + Off;
        }

        public void Offset()
        {
            if (OffsetInputField.text == "")
            {
                GameManager.InputOffset = 0.14f;
                return;
            }

            GameManager.InputOffset = int.Parse(OffsetInputField.text) * 0.001f;
        }

        public void OsuHitSoundToggle()
        {
            GameManager.OsuHitSound = !GameManager.OsuHitSound;

            if (GameManager.OsuHitSound)
                OsuHitSoundButtonText.text = OsuHitSound + ": " + On;
            else
                OsuHitSoundButtonText.text = OsuHitSound + ": " + Off;
        }

        public void UpScrollToggle()
        {
            GameManager.UpScroll = !GameManager.UpScroll;

            if (GameManager.UpScroll)
                UpScrollText.text = UpScroll + ": " + On;
            else
                UpScrollText.text = UpScroll + ": " + Off;
        }

        public void FPSLimit()
        {
            if (FPSLimitInputField.text == "")
            {
                GameManager.FPSLimit = -1;
                return;
            }

            int i = int.Parse(FPSLimitInputField.text);
            if (i < 0)
                FPSLimitInputField.text = "0";

            GameManager.FPSLimit = i;
        }

        public void KeyChange()
        {
            enabled = false;
            SoundManager.StopAll(SoundType.BGM, false);
            SceneManager.SceneLoading("Key Change");
        }

        public void UiPosChange()
        {
            enabled = false;
            SoundManager.StopAll(SoundType.BGM, false);
            SceneManager.SceneLoading("Ui Pos Change");
        }

        public void AllowIndirectMissToggle()
        {
            GameManager.AllowIndirectMiss = !GameManager.AllowIndirectMiss;

            if (GameManager.AllowIndirectMiss)
                AllowIndirectMissText.text = AllowIndirectMiss + ": " + On;
            else
                AllowIndirectMissText.text = AllowIndirectMiss + ": " + Off;
        }

        public void ProfilePictureChange()
        {
            //파일오픈창 생성 및 설정
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = LangManager.LangLoad(LangManager.Lang, "account.change_profile_picture");
            ofd.FileName = ".png";
            ofd.Filter = $"{LangManager.LangLoad(LangManager.Lang, "account.picture")}(*.png, *.jpg) | *.png; *.jpg;";

            //파일 오픈창 로드
            DialogResult dr = ofd.ShowDialog();

            //OK버튼 클릭시
            if (dr == DialogResult.OK)
            {
                //File경로와 File명을 모두 가지고 온다.
                string fileFullName = ofd.FileName;

                //File경로 + 파일명 리턴
                GameManager.ProfilePicturePath = fileFullName;
                SystemUI.SystemUI.systemUI.Renderer();
                return;
            }

            //취소버튼 클릭시 또는 ESC키로 파일창을 종료 했을경우
            if (dr == DialogResult.Cancel)
                return;

            return;
        }

        public void NickNameChange()
        {
            if (NickNameInputField.text == "")
            {
                GameManager.NickName = "none";
                SystemUI.SystemUI.systemUI.Renderer();
                return;
            }

            GameManager.NickName = NickNameInputField.text;
            SystemUI.SystemUI.systemUI.Renderer();
        }

        public void UpKey()
        {
            if (GameManager.Ratio_9_16)
                GameManager.LeftKey = true;
            else
                GameManager.UpKey = true;
        }

        public void DownKey()
        {
            if (GameManager.Ratio_9_16)
                GameManager.RightKey = true;
            else
                GameManager.DownKey = true;
        }

        public void LeftKey()
        {
            if (GameManager.Ratio_9_16)
                GameManager.UpKey = true;
            else
                GameManager.LeftKey = true;
        }

        public void RightKey()
        {
            if (GameManager.Ratio_9_16)
                GameManager.DownKey = true;
            else
                GameManager.RightKey = true;
        }

        public void EnterKey() => GameManager.EnterKey = true;
        public void AKey() => GameManager.AKey = !GameManager.AKey;
        public void PKey() => GameManager.PKey = !GameManager.PKey;

        void LateUpdate()
        {
            GameManager.UpKey = false;
            GameManager.DownKey = false;
            GameManager.LeftKey = false;
            GameManager.RightKey = false;
            GameManager.EnterKey = false;
        }

        public void ResourcePackFolderOpenButton() => System.Diagnostics.Process.Start("explorer.exe", Application.persistentDataPath.Replace("/", "\\") + "\\Resource Pack");
    }
}