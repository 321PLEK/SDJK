using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Newtonsoft.Json;
using SDJK.PlayMode;
using UnityEngine;

namespace SDJK
{
    public class GameManager : MonoBehaviour
    {
        public static float DeltaTime = 0;
        public static float UnscaledDeltaTime = 0;
        public static float FpsDeltaTime = 1;
        public static float FpsUnscaledDeltaTime = 0;
        public static float GameSpeed = 1;
        public static float FixedDeltaTime = 0.05f;

        public static float BPM = 180;

        public static float CurrentBeat = 0;
        public static float BeatTimer = 0;
        public static float Pitch = 1;
        public static float StartDelay = 0.85f;

        public static float MainVolume = 1;

        public static string Level = "Rainbow Tylenol";
        public static int AllLevelIndex = 0;
        public static int LevelIndex = 0;
        public static int ExtraLevelIndex = 0;

        public static bool Pause = false;

        public float _GameSpeed = 1;

        public static bool Optimization = false;
        public static bool NoteInterpolation = false;
        public static float InputOffset = 0.095f;
        public static bool EditorOptimization = false;

        public static bool OsuHitSound = false;

        public static bool UpScroll = false;

        public static int FPSLimit = 120;

        public static Color AlphaZero = new Color(1, 1, 1, 0);

        public static bool Ratio_9_16 = false;

        public static bool UpKey = false;
        public static bool DownKey = false;
        public static bool LeftKey = false;
        public static bool RightKey = false;
        public static bool EnterKey = false;
        public static bool AKey = false;
        public static bool PKey = false;
        
        [SerializeField] Font[] fonts;

        public static KeyCode A = KeyCode.A;
        public static KeyCode S = KeyCode.S;
        public static KeyCode D = KeyCode.D;
        public static KeyCode J = KeyCode.J;
        public static KeyCode K = KeyCode.K;
        public static KeyCode L = KeyCode.L;

        public static Vector2 ComboPos = Vector2.zero;

        public static bool AllowIndirectMiss = false;

        public static string ProfilePicturePath = "";
        public static string NickName = "none";
        public static int PlayerLevel = 0;
        public static double PlayerExp = 0;

        public static Dictionary<string, double> mapRecord = new Dictionary<string, double>();
        public static Dictionary<string, double> mapAccuracy = new Dictionary<string, double>();
        public static Dictionary<string, string> mapRank = new Dictionary<string, string>();

        void Awake()
        {
            QualitySettings.vSyncCount = 0;

            foreach (Font font in fonts)
            {
                Material mat = font.material;
                Texture txtr = mat.mainTexture;
                txtr.filterMode = FilterMode.Point;
            }
            
            if (Screen.width < Screen.height)
                Ratio_9_16 = true;
            else
                Ratio_9_16 = false;
        }

        public static int FPS = 60;
        int FPS2 = 0;

        IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                FPS = FPS2;
                FPS2 = 0;
            }
        }

        void Update()
        {
            FPS2++;
            
            DeltaTime = Time.deltaTime;
            UnscaledDeltaTime = Time.unscaledDeltaTime;
            FpsDeltaTime = 60 * DeltaTime;
            FpsUnscaledDeltaTime = 60 * UnscaledDeltaTime;

            BeatTimer += DeltaTime * Pitch;
            CurrentBeat = (BeatTimer - StartDelay - InputOffset) * (BPM / 60);

            Time.timeScale = (float)Abs(GameSpeed);
            _GameSpeed = GameSpeed;

            if (Application.isFocused)
            {
                if (!MainMenu.MainMenu.Esc)
                {
                    if (FPSLimit > 0)
                        Application.targetFrameRate = FPSLimit;
                    else
                        Application.targetFrameRate = -1;
                }
                else
                {
                    if (MainMenu.MainMenu.AFKTimer < 60)
                        Application.targetFrameRate = 70;
                    else
                        Application.targetFrameRate = 30;
                }
            }
            else
                Application.targetFrameRate = 20;

            foreach (Font font in fonts)
            {
                Material mat = font.material;
                Texture txtr = mat.mainTexture;
                txtr.filterMode = FilterMode.Point;
            }

            if (Screen.width < Screen.height)
                Ratio_9_16 = true;
            else
                Ratio_9_16 = false;

            if (PlayerExp >= 1000)
            {
                PlayerLevel++;
                PlayerExp = 0;
            }
        }

        void LateUpdate()
        {
            if (PlayerManager.playerManager != null)
            {
                if (FPS >= 120 && !Optimization)
                    Time.fixedDeltaTime = 60f / (float)PlayerManager.effect.BPM * 0.25f * (1 / (float)Abs(PlayerManager.playerManager.audioSource.pitch));
                else
                    Time.fixedDeltaTime = 0.25f;
            }

            FixedDeltaTime = Time.fixedDeltaTime;
        }

        /// <summary>
        /// min, max 사이로 변수를 맞춰줍니다
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Clamp(int value, int min, int max)
        {
            if (value > max)
                return max;
            else if (value < min)
                return min;
            else
                return value;
        }

        /// <summary>
        /// min, max 사이로 변수를 맞춰줍니다
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Clamp(float value, float min, float max)
        {
            if (value > max)
                return max;
            else if (value < min)
                return min;
            else
                return value;
        }

        /// <summary>
        /// min, max 사이로 변수를 맞춰줍니다
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double Clamp(double value, double min, double max)
        {
            if (value > max)
                return max;
            else if (value < min)
                return min;
            else
                return value;
        }

        /// <summary>
        /// 절대값
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int Abs(int num)
        {
            if (num < 0)
                return num * -1;
            else
                return num;
        }

        /// <summary>
        /// 절대값
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static float Abs(float num)
        {
            if (num < 0)
                return num * -1;
            else
                return num;
        }

        /// <summary>
        /// 절대값
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static double Abs(double num)
        {
            if (num < 0)
                return num * -1;
            else
                return num;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="data">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static int CloseValue(List<int> data, int target)
        {
            if (data.Count > 0)
                return data.Aggregate((x, y) => Abs(x - target) < Abs(y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="data">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static float CloseValue(List<float> data, float target)
        {
            if (data.Count > 0)
                return data.Aggregate((x, y) => Abs(x - target) < Abs(y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾습니다
        /// </summary>
        /// <param name="data">리스트</param>
        /// <param name="target">기준</param>
        /// <returns></returns>
        public static double CloseValue(List<double> data, double target)
        {
            if (data.Count > 0)
                return data.Aggregate((x, y) => Abs(x - target) < Abs(y - target) ? x : y);

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 인덱스를 반환합니다
        /// </summary>
        /// <param name="data"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndex(List<double> data, double target)
        {
            if (data.Count > 0)
                return data.IndexOf(data.Aggregate((x, y) => Abs(x - target) < Abs(y - target) ? x : y));

            return 0;
        }

        /// <summary>
        /// 가장 가까운 수를 찾고 이진 검색으로 인덱스를 반환합니다
        /// </summary>
        /// <param name="data"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CloseValueIndexBinarySearch(List<double> data, double target)
        {
            if (data.Count > 0)
                return data.BinarySearch(data.Aggregate((x, y) => Abs(x - target) < Abs(y - target) ? x : y));

            return 0;
        }

        /// <summary>
        /// 선형 보간 (a + (b - a) * t)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float Lerp(float a, float b, float t) => a + (b - a) * t;

        /// <summary>
        /// 선형 보간 (a + (b - a) * t)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static double Lerp(double a, double b, double t) => a + (b - a) * t;
    }

    public class JVector2
    {
        [JsonProperty("x")]
        public float x;
        [JsonProperty("y")]
        public float y;

        public JVector2() => x = y = 0f;

        public JVector2(Vector2 v)
        {
            x = v.x;
            y = v.y;
        }

        public JVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public JVector2(float f) => x = y = f;

        public static Vector2 JVector2ToVector2(JVector2 jVector2) => new Vector2(jVector2.x, jVector2.y);
    }

    public class JVector3
    {
        [JsonProperty("x")]
        public float x;
        [JsonProperty("y")]
        public float y;
        [JsonProperty("z")]
        public float z;

        public JVector3() => x = y = z = 0f;

        public JVector3(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public JVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public JVector3(float f) => x = y = z = f;

        public static Vector3 JVector3ToVector3(JVector3 jVector3) => new Vector3(jVector3.x, jVector3.y, jVector3.z);
    }

    public class JColor32
    {
        [JsonProperty("r")]
        public byte r;
        [JsonProperty("g")]
        public byte g;
        [JsonProperty("b")]
        public byte b;
        [JsonProperty("a")]
        public byte a;

        public JColor32() => r = g = b = a = 0;

        public JColor32(Color32 v)
        {
            r = v.r;
            g = v.g;
            b = v.b;
            a = v.a;
        }

        public JColor32(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public JColor32(byte f) => r = g = b = a = f;

        public static Color JColorToColor(JColor32 jColor32) => new Color32(jColor32.r, jColor32.g, jColor32.b, jColor32.a);
    }
}