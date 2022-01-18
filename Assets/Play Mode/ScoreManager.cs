using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.PlayMode.Score
{
    public class ScoreManager : MonoBehaviour
    {
        public static List<double> AccuracyList = new List<double>();
        public static double Accuracy = 0;

        public static int MaxScore = 0;
        public static int Score = 0;
        public static string Rank = "SSS";

        public static int IndirectMiss = 0;
        public static int Miss = 0;

        public static bool AllBeatDelayZero = true;
        public static bool AllSick = true;
        public static bool AllPerfect = true;

        StringBuilder tempText = new StringBuilder();
        public Text text;

        public static bool DebugMode = false;

        void Start()
        {
            if (GameManager.EditorOptimization && PlayerManager.Editor)
                text.gameObject.SetActive(false);
            else
                text.gameObject.SetActive(true);

            if (GameManager.UpScroll)
            {
                text.rectTransform.anchorMin = Vector2.zero;
                text.rectTransform.anchorMax = Vector2.zero;
                text.rectTransform.anchoredPosition = new Vector2(text.rectTransform.anchoredPosition.x, -text.rectTransform.anchoredPosition.y);
                text.alignment = TextAnchor.LowerLeft;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F3))
                DebugMode = !DebugMode;
        }

        void FixedUpdate()
        {
            Accuracy = 0;
            
            foreach (double VARIABLE in AccuracyList)
                Accuracy += VARIABLE;

            if (AccuracyList.Count != 0)
                Accuracy /= AccuracyList.Count;
            else
                Accuracy = 0;

            if (AllBeatDelayZero && AllSick && AllPerfect && Accuracy == 0 && IndirectMiss == 0 && Miss == 0)
                Rank = "SSS";
            else if (AllSick && AllPerfect && IndirectMiss == 0 && Miss == 0)
                Rank = "SS";
            else if (AllPerfect && IndirectMiss == 0 && Miss == 0)
                Rank = "S";
            else if (IndirectMiss <= 0 && Miss <= 0)
                Rank = "A";
            else if (IndirectMiss <= 50 && Miss <= 40)
                Rank = "B";
            else if (IndirectMiss <= 100 && Miss <= 80)
                Rank = "C";
            else if (IndirectMiss <= 150 && Miss <= 120)
                Rank = "D";
            else if (IndirectMiss <= 200 && Miss <= 160)
                Rank = "E";
            else if (IndirectMiss <= 250 && Miss <= 200)
                Rank = "F";
            else
            {
                if (IndirectMiss >= 1000)
                    Rank = "Is the keyboard okay?";
                else if (Miss >= 1000)
                    Rank = "AFK";
                else
                    Rank = "F";
            }

            if (!PlayerManager.UIHide)
                Rerender();
        }

        public void Rerender()
        {
            tempText.Clear();

            if (DebugMode)
            {
                tempText.Append("<color=#EE0000>FPS</color> ");
                tempText.Append($"<color=#FFFFFF>{GameManager.FPS}</color>\n\n");
                
                tempText.Append("<color=#EE0000>FPS (1 / Delta Time)</color> ");
                tempText.Append($"<color=#FFFFFF>{Mathf.RoundToInt(1f / GameManager.DeltaTime)}</color>\n");
                tempText.Append("<color=#EE0000>Unscaled FPS (1 / Unscaled Delta Time)</color> ");
                tempText.Append($"<color=#FFFFFF>{Mathf.RoundToInt(1f / GameManager.UnscaledDeltaTime)}</color>\n\n");

                tempText.Append("<color=#EE0000>Delta Time</color> ");
                tempText.Append(
                    $"<color=#FFFFFF>{Mathf.RoundToInt((float) (GameManager.DeltaTime * 1000)) * 0.001}</color>\n");
                tempText.Append("<color=#EE0000>Unscaled Delta Time</color> ");
                tempText.Append(
                    $"<color=#FFFFFF>{Mathf.RoundToInt((float) (GameManager.UnscaledDeltaTime * 1000)) * 0.001}</color>\n\n");

                tempText.Append("<color=#EE0000>Fps Delta Time (Delta Time * 60)</color> ");
                tempText.Append(
                    $"<color=#FFFFFF>{Mathf.RoundToInt((float) (GameManager.FpsDeltaTime * 1000)) * 0.001}</color>\n");

                tempText.Append("<color=#EE0000>Unscaled Fps Delta Time (Unscaled Delta Time * 60)</color> ");
                tempText.Append(
                    $"<color=#FFFFFF>{Mathf.RoundToInt((float) (GameManager.FpsUnscaledDeltaTime * 1000)) * 0.001}</color>\n\n");
            }

            //<color=#DDDD00>Accuracy</color> <color=#FFFFFFAA>0</color> <color=#FFFFFFAA>(100%)</color>
            tempText.Append("<color=#DDDD00>Accuracy</color> ");
            
            if (Accuracy == 0)
                tempText.Append($"<color=#FFFFFFAA>{Mathf.RoundToInt((float)(GameManager.Lerp(100, 0, GameManager.Abs(Accuracy) / 0.75) * 1000)) * 0.001}%</color>\n");
            else
                tempText.Append($"<color=#FFFFFF>{Mathf.RoundToInt((float)(GameManager.Lerp(100, 0, GameManager.Abs(Accuracy) / 0.75) * 1000)) * 0.001}%</color>\n");
            
            //<color=#00CC00>Score</color> <color=#FFFFFFAA>0 (0%)</color>
            tempText.Append("<color=#00CC00>Score</color> ");

            if (MaxScore != 0)
                if (Score == MaxScore)
                    tempText.Append($"<color=#FFFFFFAA>{Score} ({(int)((float)Score / (float)MaxScore * 100f)}%)</color>\n");
                else
                    tempText.Append($"<color=#FFFFFF>{Score} ({(int)((float)Score / (float)MaxScore * 100f)}%)</color>\n");
            else if (Score == MaxScore)
                tempText.Append($"<color=#FFFFFFAA>{Score} (100%)</color>\n");
            else
                tempText.Append($"<color=#FFFFFF>{Score} (100%)</color>\n");

            //<color=#00CFFF>SSS</color>
            if (Rank == "SSS")
                tempText.Append("<color=#00CFFF>To be honest, are you using hack? (SSS)</color>\n\n");
            else if (Rank == "SS")
                tempText.Append("<color=#00AAFF>Wooow, you are sooo pro!! (SS)</color>\n\n");
            else if (Rank == "S")
                tempText.Append("<color=#0080FF>S</color>\n\n");
            else if (Rank == "A")
                tempText.Append("<color=#DDDD00>A</color>\n\n");
            else if (Rank == "B")
                tempText.Append("<color=#AAAAAA>B</color>\n\n");
            else if (Rank == "C")
                tempText.Append("<color=#888888>C</color>\n\n");
            else if (Rank == "D")
                tempText.Append("<color=#666666>D</color>\n\n");
            else if (Rank == "E")
                tempText.Append("<color=#444444>E</color>\n\n");
            else if (Rank == "F")
                tempText.Append("<color=#222222>You're not good at rhythm games, are you? (F)</color>\n\n");
            else if (Rank == "AFK")
                tempText.Append("<color=#FFFFFF>Stop afk and press the keyboard... (AFK)</color>\n\n");
            else if (Rank == "Is the keyboard okay?")
                tempText.Append("<color=#FFFFFF>You're crushing keyboard LOLOL (Is the keyboard okay?)</color>\n\n");
            
            //<color=#EE0000>Indirect Miss</color> <color=#FFFFFFAA>0</color>
            tempText.Append("<color=#EE0000>Indirect Miss</color> ");
            
            if (IndirectMiss == 0)
                tempText.Append($"<color=#FFFFFFAA>{IndirectMiss}</color>\n");
            else
                tempText.Append($"<color=#FFFFFF>{IndirectMiss}</color>\n");
            
            //<color=#EE0000>Miss</color> <color=#FFFFFFAA>0</color>
            tempText.Append("<color=#EE0000>Miss</color> ");
            
            if (Miss == 0)
                tempText.Append($"<color=#FFFFFFAA>{Miss}</color>");
            else
                tempText.Append($"<color=#FFFFFF>{Miss}</color>");

            if (PlayerManager.AutoMode && !PlayerManager.PracticeMode)
                tempText.Append("\n\nAuto Mode\n\n");
            else if (!PlayerManager.AutoMode && PlayerManager.PracticeMode)
                tempText.Append("\n\nPractice Mode");
            else if (PlayerManager.AutoMode && PlayerManager.PracticeMode)
            {
                tempText.Append("\n\nAuto Mode\n");
                tempText.Append("Practice Mode\n\n");
            }

            if (PlayerManager.AutoMode && DebugMode)
            {
                tempText.Append($"A Delay: {Mathf.RoundToInt((float)(PlayerManager.playerManager.A.AutoPlayCorrection * 1000)) * 0.001}\n");
                tempText.Append($"S Delay: {Mathf.RoundToInt((float)(PlayerManager.playerManager.S.AutoPlayCorrection * 1000)) * 0.001}\n");
                tempText.Append($"D Delay: {Mathf.RoundToInt((float)(PlayerManager.playerManager.D.AutoPlayCorrection * 1000)) * 0.001}\n");
                tempText.Append($"J Delay: {Mathf.RoundToInt((float)(PlayerManager.playerManager.J.AutoPlayCorrection * 1000)) * 0.001}\n");
                tempText.Append($"K Delay: {Mathf.RoundToInt((float)(PlayerManager.playerManager.K.AutoPlayCorrection * 1000)) * 0.001}\n");
                tempText.Append($"L Delay: {Mathf.RoundToInt((float)(PlayerManager.playerManager.L.AutoPlayCorrection * 1000)) * 0.001}");
            }

            text.text = tempText.ToString();
        }
    }
}