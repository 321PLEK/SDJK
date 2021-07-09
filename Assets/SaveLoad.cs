using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SDJK.Language;
using UnityEngine.Serialization;

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

            string jsonData = JsonUtility.ToJson(playerData, true);
            path = Path.Combine(Application.persistentDataPath + "/SaveData/PlayerSaveData.json");
            File.WriteAllText(path, jsonData);

            jsonData = JsonUtility.ToJson(settingData, true);
            path = Path.Combine(Application.persistentDataPath + "/SaveData/SettingSaveData.json");
            File.WriteAllText(path, jsonData);

            jsonData = JsonUtility.ToJson(advancementsData, true);
            path = Path.Combine(Application.persistentDataPath + "/SaveData/AdvancementsSaveData.json");
            File.WriteAllText(path, jsonData);

            jsonData = JsonUtility.ToJson(statisticsData, true);
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
                playerData = JsonUtility.FromJson<PlayerData>(jsonData);
            }

            path = Path.Combine(Application.persistentDataPath + "/SaveData/SettingSaveData.json");
            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);
                settingData = JsonUtility.FromJson<SettingData>(jsonData);
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
        }

        void OnApplicationQuit() => SaveData();
    }

    [System.Serializable]
    public class PlayerData
    {

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

        public KeyCode A = KeyCode.A;
        public KeyCode S = KeyCode.S;
        public KeyCode D = KeyCode.D;
        public KeyCode J = KeyCode.J;
        public KeyCode K = KeyCode.K;
        public KeyCode L = KeyCode.L;
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