using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDJK.Camera;
using SDJK.PlayMode.Score;
using SDJK.PlayMode.Sound;
using UnityEngine.Serialization;

namespace SDJK.PlayMode
{
    public class InputSystem : MonoBehaviour
    {
        public KeyCode keyCode;
        public Image image;
        public Image image2;
        public Text text;

        public List<double> AutoPlayCorrectionList = new List<double>();
        public double AutoPlayCorrection = 0;

        int i = 0;
        int holdi = 0;
        List<double> Delay;
        List<double> HoldDelay;

        public static List<Note> A = new List<Note>();
        public static List<Note> S = new List<Note>();
        public static List<Note> D = new List<Note>();
        public static List<Note> J = new List<Note>();
        public static List<Note> K = new List<Note>();
        public static List<Note> L = new List<Note>();

        public bool Background = false;

        public bool HoldStop = false;

        bool RedColor = false;

        void Start()
        {
            //숨기기
            if (keyCode == KeyCode.A && !PlayerManager.AUse)
                gameObject.SetActive(false);
            if (keyCode == KeyCode.S && !PlayerManager.SUse)
                gameObject.SetActive(false);
            if (keyCode == KeyCode.D && !PlayerManager.DUse)
                gameObject.SetActive(false);
            if (keyCode == KeyCode.J && !PlayerManager.JUse)
                gameObject.SetActive(false);
            if (keyCode == KeyCode.K && !PlayerManager.KUse)
                gameObject.SetActive(false);
            if (keyCode == KeyCode.L && !PlayerManager.LUse)
                gameObject.SetActive(false);

            if (Background)
            {
                GetComponent<InputSystem>().enabled = false;
                return;
            }

            //딜레이 설정
            if (keyCode == KeyCode.A)
            {
                Delay = PlayerManager.mapData.A;
                HoldDelay = PlayerManager.mapData.HoldA;
            }
            else if (keyCode == KeyCode.S)
            {
                Delay = PlayerManager.mapData.S;
                HoldDelay = PlayerManager.mapData.HoldS;
            }
            else if (keyCode == KeyCode.D)
            {
                Delay = PlayerManager.mapData.D;
                HoldDelay = PlayerManager.mapData.HoldD;
            }
            else if (keyCode == KeyCode.J)
            {
                Delay = PlayerManager.mapData.J;
                HoldDelay = PlayerManager.mapData.HoldJ;
            }
            else if (keyCode == KeyCode.K)
            {
                Delay = PlayerManager.mapData.K;
                HoldDelay = PlayerManager.mapData.HoldK;
            }
            else if (keyCode == KeyCode.L)
            {
                Delay = PlayerManager.mapData.L;
                HoldDelay = PlayerManager.mapData.HoldL;
            }

            if (GameManager.UpScroll)
                image2.rectTransform.anchoredPosition = new Vector2(0, 13.5f);
        }

        void Update()
        {
            if (!(GameManager.EditorOptimization && PlayerManager.Editor))
            {
                if (!GameManager.Optimization && (PlayerManager.effect.Pitch != 0 || PlayerManager.HP > 0.001f))
                {
                    //눌렀을때, 색상 변경
                    if ((!Input.GetKey(keyCode) || PlayerManager.AutoMode) && image.color.r < 1)
                    {
                        image.color += new Color(0.05f * GameManager.FpsDeltaTime, 0.05f * GameManager.FpsDeltaTime,
                            0.05f * GameManager.FpsDeltaTime);
                        image2.color += new Color(0.05f * GameManager.FpsDeltaTime, 0.05f * GameManager.FpsDeltaTime,
                            0.05f * GameManager.FpsDeltaTime);
                        text.color += new Color(0.05f * GameManager.FpsDeltaTime, 0.05f * GameManager.FpsDeltaTime,
                            0.05f * GameManager.FpsDeltaTime);
                    }

                    if (Input.GetKey(keyCode) && !PlayerManager.AutoMode)
                    {
                        transform.SetAsLastSibling();

                        if (!RedColor)
                        {
                            image.color = new Color(0.2f, 0.2f, 0.2f);
                            image2.color = new Color(0.2f, 0.2f, 0.2f);
                            text.color = new Color(0.2f, 0.2f, 0.2f);
                        }
                        else
                        {
                            image.color = Color.red;
                            image2.color = Color.red;
                            text.color = Color.red;
                        }
                    }
                    else if (RedColor && Input.GetKeyUp(keyCode))
                        RedColor = false;
                }

                if (PlayerManager.Editor)
                {
                    //에디터는 노트가 수시로 변하기 때문에, 리스트 계속 체크
                    if (keyCode == KeyCode.A)
                    {
                        Delay = PlayerManager.mapData.A;
                        HoldDelay = PlayerManager.mapData.HoldA;
                    }
                    else if (keyCode == KeyCode.S)
                    {
                        Delay = PlayerManager.mapData.S;
                        HoldDelay = PlayerManager.mapData.HoldS;
                    }
                    else if (keyCode == KeyCode.D)
                    {
                        Delay = PlayerManager.mapData.D;
                        HoldDelay = PlayerManager.mapData.HoldD;
                    }
                    else if (keyCode == KeyCode.J)
                    {
                        Delay = PlayerManager.mapData.J;
                        HoldDelay = PlayerManager.mapData.HoldJ;
                    }
                    else if (keyCode == KeyCode.K)
                    {
                        Delay = PlayerManager.mapData.K;
                        HoldDelay = PlayerManager.mapData.HoldK;
                    }
                    else if (keyCode == KeyCode.L)
                    {
                        Delay = PlayerManager.mapData.L;
                        HoldDelay = PlayerManager.mapData.HoldL;
                    }

                    //가장 가까운 타일의 딜레이를 체크
                    double delay = GameManager.CloseValue(Delay, PlayerManager.JudgmentCurrentBeat);

                    //판정
                    if (Input.GetKeyDown(keyCode))
                        PlayerManager.Judgment(PlayerManager.JudgmentCurrentBeat - delay);
                }
                else
                {
                    if (!PlayerManager.AutoMode)
                    {
                        int temp = GameManager.CloseValueIndexBinarySearch(Delay, PlayerManager.JudgmentCurrentBeat);

                        if (Delay.Count > i && Input.GetKeyDown(keyCode) && Delay[i] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1])
                        {
                            //즉사 패턴
                            if (temp < HoldDelay.Count && HoldDelay[temp] < 0 && HoldDelay[temp] >= -1)
                            {
                                if (!PlayerManager.Judgment(PlayerManager.JudgmentCurrentBeat - Delay[i], false))
                                {
                                    PlayerManager.HP = 0;

                                    if (!PlayerManager.PracticeMode)
                                        PlayerManager.effect.Pitch = 0;

                                    image.color = Color.red;
                                    image2.color = Color.red;
                                    text.color = Color.red;

                                    RedColor = true;

                                    HoldStop = false;
                                    i++;
                                }
                            }
                            else if (i < HoldDelay.Count && HoldDelay[i] < 0 && HoldDelay[i] >= -1)
                            {
                                i++;
                                if (Delay[i] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1] && !PlayerManager.Judgment(PlayerManager.JudgmentCurrentBeat - Delay[i]))
                                {
                                    NoteHide();
                                    HoldStop = false;
                                    i++;
                                }
                            }
                            else if (!PlayerManager.Judgment(PlayerManager.JudgmentCurrentBeat - Delay[i]))
                            {
                                //일반 판정
                                NoteHide();
                                HoldStop = false;
                                i++;
                            }
                        }
                        else if (i - 1 - holdi >= 0 && i < Delay.Count && i < HoldDelay.Count && Input.GetKey(keyCode) && Delay[i] < PlayerManager.JudgmentCurrentBeat && Delay[i - 1 - holdi] <= PlayerManager.JudgmentCurrentBeat && Delay[i - 1 - holdi] + HoldDelay[i - 1 - holdi] >= PlayerManager.JudgmentCurrentBeat)
                        {
                            //홀드 노트 중간에 있는 노트 판정
                            if (Delay[i] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1])
                            {
                                if (i >= HoldDelay.Count || HoldDelay[i] >= 0)
                                {
                                    PlayerManager.Judgment(0);
                                    NoteHide();
                                }

                                i++;
                                holdi++;
                            }
                        }
                        else if (i - 1 - holdi >= 0 && i - 1 - holdi < HoldDelay.Count && Input.GetKeyUp(keyCode) && HoldDelay[i - 1 - holdi] > 0 && !HoldStop)
                        { 
                            //홀드 판정
                            if (HoldDelay[i - 1 - holdi] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1])
                            {
                                PlayerManager.Judgment(PlayerManager.JudgmentCurrentBeat - (Delay[i - 1 - holdi] + HoldDelay[i - 1 - holdi]));
                                HoldStop = true;
                                holdi = 0;
                            }
                        }
                        else if (Delay.Count <= i && Input.GetKeyDown(keyCode))
                        {
                            //마지막 박자일때
                            PlayerManager.Judgment(-0.76 * PlayerManager.effect.JudgmentSize, false);
                        }
                        else if (Delay.Count > i && Delay[i] < PlayerManager.JudgmentCurrentBeat - 0.75 * PlayerManager.effect.JudgmentSize)
                        {
                            if (temp >= HoldDelay.Count || HoldDelay[temp] >= 0)
                            {
                                //미스
                                if (Delay[i] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1])
                                    PlayerManager.Judgment(PlayerManager.JudgmentCurrentBeat - Delay[i]);
                                
                                NoteHide();

                                HoldStop = false;
                            }

                            i++;
                        }
                        else if (Input.GetKeyDown(keyCode))
                            PlayerManager.Judgment(-0.76 * PlayerManager.effect.JudgmentSize, false);

                        while (i < Delay.Count && i >= 0 && Delay[i] < PlayerManager.JudgmentCurrentBeat - 0.75)
                        {
                            if (Delay[i] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1] && (temp >= HoldDelay.Count || HoldDelay[temp] >= 0))
                                PlayerManager.Judgment(PlayerManager.JudgmentCurrentBeat - Delay[i]);

                            i++;
                        }
                    }
                    else
                    {
                        AutoPlayCorrection = 0;
                        
                        //자동 플레이
                        if (i - 1 - holdi >= 0 && i - 1 - holdi < HoldDelay.Count && i < Delay.Count && Delay[i] < PlayerManager.JudgmentCurrentBeat && Delay[i - 1 - holdi] <= PlayerManager.JudgmentCurrentBeat && Delay[i - 1 - holdi] + HoldDelay[i - 1 - holdi] > PlayerManager.JudgmentCurrentBeat && HoldDelay[i - 1 - holdi] > 0 && !HoldStop)
                        {
                            //홀드 노트 중간에 있는 노트 판정
                            if (Delay[i] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1])
                            {
                                if (i >= HoldDelay.Count || HoldDelay[i] >= 0)
                                {
                                    PlayerManager.Judgment(0);
                                    NoteHide();
                                }

                                i++;
                                holdi++;
                            }
                        }
                        else if (Delay.Count > i && Delay[i] <= PlayerManager.JudgmentCurrentBeat)
                        {
                            if (Delay[i] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1] && (i >= HoldDelay.Count || HoldDelay[i] >= 0))
                            {
                                AutoPlayCorrectionList.Add(PlayerManager.JudgmentCurrentBeat - Delay[i]);

                                if (!PlayerManager.PracticeMode)
                                    PlayerManager.Judgment(PlayerManager.JudgmentCurrentBeat - Delay[i]);
                                else
                                    PlayerManager.Judgment(0);

                                if (!GameManager.Optimization)
                                {
                                    transform.SetAsLastSibling();
                                    image.color = new Color(0.2f, 0.2f, 0.2f);
                                    image2.color = new Color(0.2f, 0.2f, 0.2f);
                                    text.color = new Color(0.2f, 0.2f, 0.2f);
                                }

                                NoteHide();

                                HoldStop = false;
                            }

                            i++;
                        }
                        else if (i - 1 - holdi >= 0 && i - 1 - holdi < HoldDelay.Count && Delay[i - 1 - holdi] <= PlayerManager.JudgmentCurrentBeat && Delay[i - 1 - holdi] + HoldDelay[i - 1 - holdi] > PlayerManager.JudgmentCurrentBeat && HoldDelay[i - 1 - holdi] > 0 && !HoldStop)
                        {
                            if (!GameManager.Optimization)
                            {
                                transform.SetAsLastSibling();
                                image.color = new Color(0.2f, 0.2f, 0.2f);
                                image2.color = new Color(0.2f, 0.2f, 0.2f);
                                text.color = new Color(0.2f, 0.2f, 0.2f);
                            }
                        }
                        else if (i - 1 - holdi >= 0 && i - 1 - holdi < HoldDelay.Count && Delay[i - 1 - holdi] + HoldDelay[i - 1 - holdi] <= PlayerManager.JudgmentCurrentBeat && HoldDelay[i - 1 - holdi] > 0 && !HoldStop)
                        {
                            if (HoldDelay[i - 1 - holdi] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1])
                            {
                                if (!PlayerManager.PracticeMode)
                                    PlayerManager.Judgment(PlayerManager.JudgmentCurrentBeat - (Delay[i - 1 - holdi] + HoldDelay[i - 1 - holdi]));
                                else
                                    PlayerManager.Judgment(0);

                                holdi = 0;
                                HoldStop = true;
                            }
                        }

                        while (i < Delay.Count && i >= 0 && Delay[i] < PlayerManager.JudgmentCurrentBeat - 0.75)
                        {
                            if (i >= HoldDelay.Count || HoldDelay[i] >= 0)
                            {
                                AutoPlayCorrectionList.Add(PlayerManager.JudgmentCurrentBeat - Delay[i]);

                                if (!PlayerManager.PracticeMode)
                                    PlayerManager.Judgment(PlayerManager.JudgmentCurrentBeat - Delay[i]);
                                else
                                    PlayerManager.Judgment(0);

                                if (!GameManager.Optimization)
                                {
                                    transform.SetAsLastSibling();
                                    image.color = new Color(0.2f, 0.2f, 0.2f);
                                    image2.color = new Color(0.2f, 0.2f, 0.2f);
                                    text.color = new Color(0.2f, 0.2f, 0.2f);
                                }

                                NoteHide();

                                HoldStop = false;
                            }

                            i++;
                        }
                    }
                }
            }
        }

        public void PhoneControllInput()
        {
            /*if (!PlayerManager.AutoMode)
            {
                if (Delay.Count > i)
                {
                    //판정
                    if (Delay[i] != PlayerManager.mapData.AllBeat[PlayerManager.mapData.AllBeat.Count - 1] && !PlayerManager.Judgment(PlayerManager.JudgmentCurrentBeat - Delay[i]))
                    {
                        NoteHide();
                        i++;
                    }
                }
                else if (Delay.Count <= i)
                {
                    //마지막 박자일때
                    PlayerManager.Judgment(-0.76 * PlayerManager.effect.JudgmentSize, false);
                }
                else
                    PlayerManager.Judgment(-0.76 * PlayerManager.effect.JudgmentSize, false);
            }*/
        }

        public void PhoneControllInputColorChange()
        {
            if (!GameManager.Optimization)
            {
                //눌렀을때, 색상 변경
                transform.SetAsLastSibling();
                image.color = new Color(0.2f, 0.2f, 0.2f);
                image2.color = new Color(0.2f, 0.2f, 0.2f);
                text.color = new Color(0.2f, 0.2f, 0.2f);
            }
        }
        
        static readonly Color color = new Color(0, 1, 0, 0);
            
        void NoteHide()
        {
            //타일 숨기기
            if (!PlayerManager.Editor)
            {
                if (keyCode == KeyCode.A && i < A.Count)
                    A[i].spriteRenderer.color = color;
                if (keyCode == KeyCode.S && i < S.Count)
                    S[i].spriteRenderer.color = color;
                if (keyCode == KeyCode.D && i < D.Count)
                    D[i].spriteRenderer.color = color;
                if (keyCode == KeyCode.J && i < J.Count)
                    J[i].spriteRenderer.color = color;
                if (keyCode == KeyCode.K && i < K.Count)
                    K[i].spriteRenderer.color = color;
                if (keyCode == KeyCode.L && i < L.Count)
                    L[i].spriteRenderer.color = color;
            }
        }
    }
}