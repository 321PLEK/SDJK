using SDJK.Language;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.SystemUI
{
    public class SystemUI : MonoBehaviour
    {
        public static SystemUI systemUI;
        public RectTransform TopUI;
        public RectTransform RightUI;

        public Image PlayerImage;
        public Text PlayerName;
        public Text Level;
        public Image LevelBar;
        public Text NumberOfMap;
        public Text NumberOfMapsCleared;
        public Text UpTime;
        public Image Volume;
        public Text VolumeText;

        public int hour = 0;
        public int minute = 0;
        public int second = 0;
        public float timer = 0;

        public string hourText = "0";
        public string minuteText = "00";
        public string secondText = "00";

        public float VolumeTimer = 0;

        public int tempLevel = 0;
        public float tempVolume = 0;

        void OnEnable()
        {
            systemUI = this;
            Renderer();
        }

        void Update()
        {
            if (MainMenu.MainMenu.Esc || Input.mousePosition.y >= Screen.height)
                TopUI.anchoredPosition = Vector2.Lerp(TopUI.anchoredPosition, Vector2.zero, 0.15f * GameManager.FpsDeltaTime);
            else
                TopUI.anchoredPosition = Vector2.Lerp(TopUI.anchoredPosition, new Vector2(0, TopUI.sizeDelta.y), 0.15f * GameManager.FpsDeltaTime);

            if (VolumeTimer > 0 || Input.mousePosition.x >= Screen.width)
                RightUI.anchoredPosition = Vector2.Lerp(RightUI.anchoredPosition, Vector2.zero, 0.15f * GameManager.FpsDeltaTime);
            else
                RightUI.anchoredPosition = Vector2.Lerp(RightUI.anchoredPosition, new Vector2(RightUI.sizeDelta.x, 0), 0.15f * GameManager.FpsDeltaTime);

            if (VolumeTimer > 0)
                VolumeTimer -= GameManager.DeltaTime;

            RightUI.anchorMax = new Vector2(RightUI.anchorMax.x, (TopUI.anchoredPosition.y - TopUI.sizeDelta.y + 1080) / 1080);
            Volume.fillAmount = GameManager.Lerp(Volume.fillAmount, GameManager.MainVolume * 0.5f, 0.15f);
            if (tempVolume != GameManager.MainVolume)
            {
                VolumeText.text = Mathf.RoundToInt(GameManager.MainVolume * 100) + "%";
                tempVolume = GameManager.MainVolume;
            }

            if (tempLevel != GameManager.PlayerLevel)
            {
                Level.text = LevelLang.Replace("%number", GameManager.PlayerLevel.ToString());
                tempLevel = GameManager.PlayerLevel;
            }

            LevelBar.fillAmount = GameManager.Lerp(LevelBar.fillAmount, (float)(GameManager.PlayerExp * 0.001), 0.15f);

            if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                VolumeTimer = 1;
                GameManager.MainVolume -= 0.1f;

                if (GameManager.MainVolume < 0)
                    GameManager.MainVolume = 0;
                else if (GameManager.MainVolume > 2)
                    GameManager.MainVolume = 2;
                if (GameManager.MainVolume >= 1.5f)
                    VolumeText.color = new Color(0.6f, 0, 0);
                else
                    VolumeText.color = Color.white;
            }

            if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                VolumeTimer = 1;
                GameManager.MainVolume += 0.1f;

                if (GameManager.MainVolume < 0)
                    GameManager.MainVolume = 0;
                else if (GameManager.MainVolume > 2)
                    GameManager.MainVolume = 2;
                if (GameManager.MainVolume >= 1.5f)
                    VolumeText.color = new Color(0.6f, 0, 0);
                else
                    VolumeText.color = Color.white;
            }

            timer += GameManager.DeltaTime;
            if (timer >= 1)
            {
                second++;
                timer = 0;
                UpTime.text = UpTimeLang.Replace("%hour", hourText).Replace("%minute", minuteText).Replace("%second", secondText);
            }
            else if (second >= 60)
            {
                minute++;
                second = 0;
            }
            else if (minute >= 60)
            {
                hour++;
                minute = 0;
            }

            if (second >= 10)
                secondText = second.ToString();
            else
                secondText = "0" + second;

            if (minute >= 10)
                minuteText = minute.ToString();
            else
                minuteText = "0" + minute;

            hourText = hour.ToString();
        }

        string LevelLang;
        string NumberOfMapLang;
        string NumberOfMapsClearedLang;
        string UpTimeLang;

        public void Renderer()
        {
            LevelLang = LangManager.LangLoad(LangManager.Lang, "system_ui.level");
            NumberOfMapLang = LangManager.LangLoad(LangManager.Lang, "system_ui.number_of_map");
            NumberOfMapsClearedLang = LangManager.LangLoad(LangManager.Lang, "system_ui.number_of_maps_cleared");
            UpTimeLang = LangManager.LangLoad(LangManager.Lang, "system_ui.up_time");

            Level.text = LevelLang.Replace("%number", GameManager.PlayerLevel.ToString());
            NumberOfMap.text = NumberOfMapLang.Replace("%number", MainMenu.MainMenu.mainMenu.AllLevelList.Count.ToString());

            int i = 0;
            foreach (var item in GameManager.mapRecord)
                if (item.Value == 100)
                    i++;
            NumberOfMapsCleared.text = NumberOfMapsClearedLang.Replace("%number", i.ToString());

            UpTime.text = UpTimeLang.Replace("%hour", hourText).Replace("%minute", minuteText).Replace("%second", secondText);

            PlayerName.text = GameManager.NickName;

            if (File.Exists(GameManager.ProfilePicturePath))
            {
                byte[] byteTexture = File.ReadAllBytes(GameManager.ProfilePicturePath);
                if (byteTexture.Length > 0)
                {
                    Texture2D TextureLoadTemp = new Texture2D(0, 0);
                    TextureLoadTemp.name = "temp";
                    TextureLoadTemp.LoadImage(byteTexture);
                    TextureLoadTemp.filterMode = FilterMode.Bilinear;

                    Rect rect = new Rect(0, 0, TextureLoadTemp.width, TextureLoadTemp.height);
                    Vector2 pivot = Vector2.zero;
                    PlayerImage.sprite = Sprite.Create(TextureLoadTemp, rect, pivot);
                    TextureLoadTemp = null;
                    Resources.UnloadUnusedAssets();
                }
            }
        }
    }
}