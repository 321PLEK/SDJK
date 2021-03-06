using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SDJK.Language;
using Newtonsoft.Json;

namespace SDJK.SaveLoad
{
    public class SaveLoad : MonoBehaviour
    {
        public PlayerData _playerData;
        public SettingData _settingData;
        public AdvancementsData _advancementsData;
        public StatisticsData _statisticsData;

        public static PlayerData playerData;
        public static SettingData settingData;
        public static AdvancementsData advancementsData;
        public static StatisticsData statisticsData;

        public static string path;

        void Awake()
        {
            playerData = _playerData;
            settingData = _settingData;
            advancementsData = _advancementsData;
            statisticsData = _statisticsData;

            LoadData();
        }

        //함수 닉값중
        static void saveData()
        {
            path = Path.Combine(Application.persistentDataPath + "/SaveData");

            if (!File.Exists(path))
            {
                path = Path.Combine(Application.persistentDataPath + "/SaveData");
                Directory.CreateDirectory(path);
            }

            string jsonData = JsonConvert.SerializeObject(playerData, Formatting.Indented);
            path = Path.Combine(Application.persistentDataPath + "/SaveData/PlayerSaveData.json");
            File.WriteAllText(path, jsonData);

            jsonData = JsonConvert.SerializeObject(settingData, Formatting.Indented);
            path = Path.Combine(Application.persistentDataPath + "/SaveData/SettingSaveData.json");
            File.WriteAllText(path, jsonData);

            jsonData = JsonConvert.SerializeObject(advancementsData, Formatting.Indented);
            path = Path.Combine(Application.persistentDataPath + "/SaveData/AdvancementsSaveData.json");
            File.WriteAllText(path, jsonData);

            jsonData = JsonConvert.SerializeObject(statisticsData, Formatting.Indented);
            path = Path.Combine(Application.persistentDataPath + "/SaveData/StatisticsSaveData.json");
            File.WriteAllText(path, jsonData);
        }

        static void loadData()
        {
            path = Path.Combine(Application.persistentDataPath + "/SaveData");

            if (!File.Exists(path))
                Directory.CreateDirectory(path);

            path = Path.Combine(Application.persistentDataPath + "/SaveData/PlayerSaveData.json");
            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);
                playerData = JsonConvert.DeserializeObject<PlayerData>(jsonData);
            }

            path = Path.Combine(Application.persistentDataPath + "/SaveData/SettingSaveData.json");
            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);
                settingData = JsonConvert.DeserializeObject<SettingData>(jsonData);
            }
        }

        public static void SaveData()
        {
            settingData.MainVol = GameManager.MainVolume;
            settingData.Lang = LangManager.Lang;
            settingData.ResourcePackPath = ResourcesManager.ResourcePackPath;
            settingData.Optimization = GameManager.Optimization;
            settingData.InputOffset = GameManager.InputOffset;
            settingData.NoteInterpolation = GameManager.NoteInterpolation;
            settingData.EditorOptimization = GameManager.EditorOptimization;
            settingData.OsuHitSound = GameManager.OsuHitSound;
            settingData.UpScroll = GameManager.UpScroll;
            settingData.FPSLimit = GameManager.FPSLimit;

            settingData.A = GameManager.A;
            settingData.S = GameManager.S;
            settingData.D = GameManager.D;
            settingData.J = GameManager.J;
            settingData.K = GameManager.K;
            settingData.L = GameManager.L;

            settingData.ComboPos = new JVector2(GameManager.ComboPos);

            settingData.AllowIndirectMiss = GameManager.AllowIndirectMiss;

            settingData.IncreasedNoteReadability = GameManager.IncreasedNoteReadability;

            settingData.TouchButtonSize = GameManager.TouchButtonSize;

            settingData.BackgroundEnable = GameManager.BackgroundEnable;

            playerData.ProfilePicturePath = GameManager.ProfilePicturePath;
            playerData.NickName = GameManager.NickName;
            playerData.MapRecord = GameManager.mapRecord;
            playerData.MapAccuracy = GameManager.mapAccuracy;
            playerData.MapRank = GameManager.mapRank;

            playerData.PlayerLevel = GameManager.PlayerLevel;
            playerData.PlayerExp = GameManager.PlayerExp;

            saveData();
        }

        public static void LoadData()
        {
            loadData();

            GameManager.MainVolume = settingData.MainVol;
            LangManager.Lang = settingData.Lang;
            ResourcesManager.ResourcePackPath = settingData.ResourcePackPath;
            GameManager.Optimization = settingData.Optimization;
            GameManager.InputOffset = settingData.InputOffset;
            GameManager.NoteInterpolation = settingData.NoteInterpolation;
            GameManager.EditorOptimization = settingData.EditorOptimization;
            GameManager.OsuHitSound = settingData.OsuHitSound;
            GameManager.UpScroll = settingData.UpScroll;
            GameManager.FPSLimit = settingData.FPSLimit;

            GameManager.A = settingData.A;
            GameManager.S = settingData.S;
            GameManager.D = settingData.D;
            GameManager.J = settingData.J;
            GameManager.K = settingData.K;
            GameManager.L = settingData.L;

            GameManager.ComboPos = JVector2.JVector2ToVector2(settingData.ComboPos);

            GameManager.AllowIndirectMiss = settingData.AllowIndirectMiss;

            GameManager.IncreasedNoteReadability = settingData.IncreasedNoteReadability;

            GameManager.TouchButtonSize = settingData.TouchButtonSize;

            GameManager.BackgroundEnable = settingData.BackgroundEnable;

            GameManager.ProfilePicturePath = playerData.ProfilePicturePath;
            GameManager.NickName = playerData.NickName;

            GameManager.mapRecord = playerData.MapRecord;
            GameManager.mapAccuracy = playerData.MapAccuracy;
            GameManager.mapRank = playerData.MapRank;

            GameManager.PlayerLevel = playerData.PlayerLevel;
            GameManager.PlayerExp = playerData.PlayerExp;
        }

        void OnApplicationQuit() => SaveData();
    }

    [System.Serializable]
    public class PlayerData
    {
        public string ProfilePicturePath = "";
        public string NickName = "none";

        public int PlayerLevel = 0;
        public double PlayerExp = 0;

        public Dictionary<string, double> MapRecord = new Dictionary<string, double>();
        public Dictionary<string, double> MapAccuracy = new Dictionary<string, double>();
        public Dictionary<string, string> MapRank = new Dictionary<string, string>();
    }

    [System.Serializable]
    public class SettingData
    {
        public float MainVol = 1;
        public string Lang = "en_us";
        public List<string> ResourcePackPath = new List<string>();
        public bool Optimization = false;
        public float InputOffset = 0.095f;
        public bool NoteInterpolation = false;
        public bool EditorOptimization = false;
        public bool OsuHitSound = false;
        public bool UpScroll = false;
        public int FPSLimit = 120;
        public bool AllowIndirectMiss = false;

        public KeyCode A = KeyCode.A;
        public KeyCode S = KeyCode.S;
        public KeyCode D = KeyCode.D;
        public KeyCode J = KeyCode.J;
        public KeyCode K = KeyCode.K;
        public KeyCode L = KeyCode.L;

        public JVector2 ComboPos = new JVector2();

        public bool IncreasedNoteReadability = false;

        public float TouchButtonSize = 1;

        public bool BackgroundEnable = true;
    }

    [System.Serializable]
    public class AdvancementsData
    {

    }

    [System.Serializable]
    public class StatisticsData
    {

    }
}