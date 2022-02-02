using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDJK.Scene;
using System.IO;
using UnityEngine.UI;
using SDJK.PlayMode.UI;
using SDJK.EditMode;
using UnityEngine.Networking;
using DiscordPresence;
using SDJK.Effect;
using Newtonsoft.Json;
using SDJK.Camera;
using SDJK.Language;
using SDJK.PlayMode.Score;
using SDJK.PlayMode.UI.Background;
using Application = UnityEngine.Application;
using SDJK.PlayMode.Sound;
using SFB;

namespace SDJK.PlayMode
{
    public class PlayerManager : MonoBehaviour
    {
        public static bool AUse = false;
        public static bool SUse = false;
        public static bool DUse = false;
        public static bool JUse = false;
        public static bool KUse = false;
        public static bool LUse = false;

        public static int Combo = 0;

        public static PlayerManager playerManager;

        public AudioSource audioSource;
        public GameObject prefab;
        public JudgmentText JudgmentText;
        public JudgmentText JudgmentImage;
        public JudgmentText DelayText;

        public GameObject _bar;
        public static GameObject bar;

        public Text ComboText;

        public static double HitSoundCurrentBeat = -4;
        public static double VisibleCurrentBeat = -4;
        public static double JudgmentCurrentBeat = -4;

        public static double StartDelay = 0;
        public static double BeatTimer = 0;

        public bool _Editor = false;
        public static bool Editor = false;

        public static bool MapPlay = false;

        public Outline JudgemntTextLine;
        public Outline JudgemntTextLine2;

        public static double MaxHP = 100;
        public static double HP = MaxHP * 0.5f;
        public Image HPBackgroundImage;
        public Image HPImage;

        public static MapData mapData = new MapData();
        public static Effect effect = new Effect();

        public static bool AutoMode = false;
        public static bool PracticeMode = false;

        public InputSystem A;
        public InputSystem S;
        public InputSystem D;
        public InputSystem J;
        public InputSystem K;
        public InputSystem L;

        public static string MapPath = "";
        public static string MapJsonPath = "";
        public static bool isEditorMapPlay = false;

        public BackgroundImage BackgroundImage;
        public BackgroundVideo BackgroundVideo;
        
        static bool AutoLoad = true;
        
        static double time = 0;
        
        static double BPMCurrentBeat = 0;
        static double BPMTimer = 0;

        double VisibleCurrentBeatLerp = 0;

        public static bool UIHide = false;

        public RectTransform ComboPos;

        public bool HitCurrentBeatStop = true;

        public Canvas Touch;

        //public MidiPlayer MidiPlayer;

        //스크립트를 뜯을때, 주석도 좋지만 함수 이름이나 변수 이름이나 스크립트 또는 클래스의 이름을 보면 도움이 됩니다

        void OnEnable()
        {
            //public 변수를 static에 넣기
            playerManager = this;
            Editor = _Editor;
            bar = _bar;

            if (GameManager.EditorOptimization && Editor)
            {
                ComboText.gameObject.SetActive(false);
                HPBackgroundImage.gameObject.SetActive(false);
                HPImage.gameObject.SetActive(false);
            }

            //맵 생성
            MapReset();
            if (!AutoLoad)
                MapDataLoad(MapJsonPath);
            else if (!Editor)
                MapLoad();
                

            AutoLoad = true;

            //콤보 색상 변경 (흰색, 회색)
            if (!GameManager.Optimization)
                StartCoroutine(ComboColorAni());

            if (GameManager.UpScroll)
            {
                ComboText.rectTransform.anchorMin = new Vector2(0.5f, 1);
                ComboText.rectTransform.anchorMax = new Vector2(0.5f, 1);
                ComboText.rectTransform.anchoredPosition = new Vector2(ComboText.rectTransform.anchoredPosition.x, -ComboText.rectTransform.anchoredPosition.y);
                ComboPos.anchoredPosition = new Vector2(GameManager.ComboPos.x, -GameManager.ComboPos.y);
                HPImage.fillOrigin = 1;
            }
            else
                ComboPos.anchoredPosition = GameManager.ComboPos;

            Touch.scaleFactor = GameManager.TouchButtonSize * (Screen.width / 552.4f) * 0.5f;
        }

        IEnumerator ComboColorAni()
        {
            while (true)
            {
                ComboText.color = Color.white;
                yield return new WaitForSeconds(0.1f);
                ComboText.color = new Color(0.5f, 0.5f, 0.5f);
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        public static void MapReset()
        {
            //맵, 변수 리셋
            MapPlay = false;
            if (!Editor)
                MapPlay = true;

            if (Editor)
            {
                AutoMode = false;
                PracticeMode = false;
            }

            AUse = false;
            SUse = false;
            DUse = false;
            JUse = false;
            KUse = false;
            LUse = false;

            if (Editor)
            {
                AUse = true;
                SUse = true;
                DUse = true;
                JUse = true;
                KUse = true;
                LUse = true;
            }

            mapData.A.Clear();
            mapData.S.Clear();
            mapData.D.Clear();
            mapData.J.Clear();
            mapData.K.Clear();
            mapData.L.Clear();
            mapData.HoldA.Clear();
            mapData.HoldS.Clear();
            mapData.HoldD.Clear();
            mapData.HoldJ.Clear();
            mapData.HoldK.Clear();
            mapData.HoldL.Clear();
            mapData.AllBeat.Clear();
            mapData.Artist = "none";
            mapData.BGMName = "none";

            BeatTimer = 0;
            
            Combo = 0;

            HitSoundCurrentBeat = -4;
            VisibleCurrentBeat = -4;
            JudgmentCurrentBeat = -4;
            playerManager.audioSource.time = 0;

            playerManager.JudgmentText.text.text = "";
            playerManager.JudgmentText.text.color = Color.white;
            playerManager.JudgmentImage.image.enabled = false;
            playerManager.JudgemntTextLine.effectColor = Color.black;
            playerManager.JudgemntTextLine2.effectColor = Color.black;

            ScoreManager.Accuracy = 0;
            ScoreManager.AccuracyList.Clear();
            ScoreManager.Score = 0;
            ScoreManager.MaxScore = 0;
            ScoreManager.Rank = "SSS";
            ScoreManager.IndirectMiss = 0;
            ScoreManager.Miss = 0;

            ScoreManager.AllBeatDelayZero = true;
            ScoreManager.AllSick = true;
            ScoreManager.AllPerfect = true;

            GameManager.GameSpeed = 1;

            if (isEditorMapPlay)
                NoteManager.notes = new List<Note>();
            
            InputSystem.A.Clear();
            InputSystem.S.Clear();
            InputSystem.D.Clear();
            InputSystem.J.Clear();
            InputSystem.K.Clear();
            InputSystem.L.Clear();
            
            mapData.Effect = new EffectList();

            EffectManager.EffectReset();

            //이펙트 인덱스를 전부 0으로
            EffectManager.EffectAllIndexSet(0);

            mapData.Effect.Volume = 0.4;

            BPMCurrentBeat = 0;
            BPMTimer = 0;

            playerManager.HitCurrentBeatStop = true;
        }

        static void EffectAdd()
        {
            if (mapData.Effect.Camera.CameraPosEffect.Count == 0)
                mapData.Effect.Camera.CameraPosEffect.Add(new CameraPosEffect());
            if (mapData.Effect.Camera.UiPosEffect.Count == 0)
                mapData.Effect.Camera.UiPosEffect.Add(new UiPosEffect());

            if (mapData.Effect.Camera.CameraZoomEffect.Count == 0)
                mapData.Effect.Camera.CameraZoomEffect.Add(new CameraZoomEffect());
            if (mapData.Effect.Camera.UiZoomEffect.Count == 0)
                mapData.Effect.Camera.UiZoomEffect.Add(new UiZoomEffect());

            if (mapData.Effect.BPMEffect.Count == 0)
                mapData.Effect.BPMEffect.Add(new BPMEffect());

            if (mapData.Effect.PitchEffect.Count == 0)
                mapData.Effect.PitchEffect.Add(new PitchEffect());
            if (mapData.Effect.BeatYPosEffect.Count == 0)
                mapData.Effect.BeatYPosEffect.Add(new BeatYPosEffect());
            if (mapData.Effect.VolumeEffect.Count == 0)
                mapData.Effect.VolumeEffect.Add(new VolumeEffect());

            if (mapData.Effect.HPAddValueEffect.Count == 0)
                mapData.Effect.HPAddValueEffect.Add(new HPAddValueEffect());
            if (mapData.Effect.HPRemoveEffect.Count == 0)
                mapData.Effect.HPRemoveEffect.Add(new HPRemoveEffect());
            if (mapData.Effect.HPRemoveValueEffect.Count == 0)
                mapData.Effect.HPRemoveValueEffect.Add(new HPRemoveValueEffect());
            if (mapData.Effect.MaxHPValueEffect.Count == 0)
                mapData.Effect.MaxHPValueEffect.Add(new MaxHPValueEffect());

            if (mapData.Effect.JudgmentSizeEffect.Count == 0)
                mapData.Effect.JudgmentSizeEffect.Add(new JudgmentSizeEffect());

            if (mapData.Effect.ABarPosEffect.Count == 0)
                mapData.Effect.ABarPosEffect.Add(new SDJK.Effect.BarPosEffect());
            if (mapData.Effect.SBarPosEffect.Count == 0)
                mapData.Effect.SBarPosEffect.Add(new SDJK.Effect.BarPosEffect());
            if (mapData.Effect.DBarPosEffect.Count == 0)
                mapData.Effect.DBarPosEffect.Add(new SDJK.Effect.BarPosEffect());
            if (mapData.Effect.JBarPosEffect.Count == 0)
                mapData.Effect.JBarPosEffect.Add(new SDJK.Effect.BarPosEffect());
            if (mapData.Effect.KBarPosEffect.Count == 0)
                mapData.Effect.KBarPosEffect.Add(new SDJK.Effect.BarPosEffect());
            if (mapData.Effect.LBarPosEffect.Count == 0)
                mapData.Effect.LBarPosEffect.Add(new SDJK.Effect.BarPosEffect());
        }

        void MapLoad()
        {
            if (!isEditorMapPlay)
            {
                if (Editor)
                {
                    //FileBrowser.ShowLoadDialog(MapLoadOnSucess, null, FileBrowser.PickMode.Files, false, null, "", "Map Load...", "Load");
                    string path = MapLoadScreen();
                    
                    if (path != "")
                        MapDataLoad(path);
                }
                else
                {
                    //다시 한번 더 맵 리셋
                    MapReset();

                    //Tile Create
                    string json = ResourcesManager.Search<string>(ResourcesManager.GetStringNameSpace(GameManager.Level, out string Name), ResourcesManager.MapPath + Name);

                    mapData = JsonConvert.DeserializeObject<MapData>(json);

                    for (int i = 0; i < mapData.A.Count; i++)
                    {
                        if (mapData.A[i] != mapData.AllBeat[mapData.AllBeat.Count - 1])
                            NoteAdd(KeyCode.A, mapData.A[i], i);
                    }
                    for (int i = 0; i < mapData.S.Count; i++)
                    {
                        if (mapData.S[i] != mapData.AllBeat[mapData.AllBeat.Count - 1])
                            NoteAdd(KeyCode.S, mapData.S[i], i);
                    }
                    for (int i = 0; i < mapData.D.Count; i++)
                    {
                        if (mapData.D[i] != mapData.AllBeat[mapData.AllBeat.Count - 1])
                            NoteAdd(KeyCode.D, mapData.D[i], i);
                    }
                    for (int i = 0; i < mapData.J.Count; i++)
                    {
                        if (mapData.J[i] != mapData.AllBeat[mapData.AllBeat.Count - 1])
                            NoteAdd(KeyCode.J, mapData.J[i], i);
                    }
                    for (int i = 0; i < mapData.K.Count; i++)
                    {
                        if (mapData.K[i] != mapData.AllBeat[mapData.AllBeat.Count - 1])
                            NoteAdd(KeyCode.K, mapData.K[i], i);
                    }
                    for (int i = 0; i < mapData.L.Count; i++)
                    {
                        if (mapData.L[i] != mapData.AllBeat[mapData.AllBeat.Count - 1])
                            NoteAdd(KeyCode.L, mapData.L[i], i);
                    }

                    //시작 오프셋 설정
                    if (!Editor)
                        StartDelay = (60 / effect.BPM * 4) + mapData.Offset;
                    else
                        StartDelay = mapData.Offset;

                    //BGM 세팅
                    StartCoroutine(SetBGM());
                    
                    if (!effect.HPRemove)
                        HP = MaxHP * 0.5f;
                    else
                        HP = MaxHP;
                    
                    playerManager.BackgroundImage.Rerender();
                    StartCoroutine(playerManager.BackgroundVideo.Rerender());

                    BPMChange(mapData.Effect.BPM, 0);
                    BPMCurrentBeat = 0;
                    BPMTimer = 0;
                    time = 0;

                    EffectAdd();
                }
            }
            else
                MapDataLoad(MapJsonPath);
        }

        string MapLoadScreen()
        {
            /*//파일오픈창 생성 및 설정
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = LangManager.LangLoad(LangManager.Lang, "editMode.map_load"),
                FileName = ".sdjk",
                Filter = $"{LangManager.LangLoad(LangManager.Lang, "editMode.sdjk_map")}(*.sdjk, *.json) | *.sdjk; *.json;"
            };

            //파일 오픈창 로드
            DialogResult dr = ofd.ShowDialog();*/

            /*//OK버튼 클릭시
            if (dr == DialogResult.OK)
            {
                //File경로와 File명을 모두 가지고 온다.
                string fileFullName = ofd.FileName;

                //File경로 + 파일명 리턴
                MapJsonPath = fileFullName;
                return fileFullName;
            }
            //취소버튼 클릭시 또는 ESC키로 파일창을 종료 했을경우

            if (dr == DialogResult.Cancel)
                return "";*/

            string[] Path = StandaloneFileBrowser.OpenFilePanel(LangManager.LangLoad(LangManager.Lang, "editMode.map_load"), "", "sdjk", false);
            if (Path.Length > 0)
                MapJsonPath = Path[0];
            return MapJsonPath;
        }

        public static void MapDataLoad(string path)
        {
            //다시 한번더 맵 리셋
            MapReset();

            //모든 노트 지우기
            if (NoteManager.notes != null)
            {
                foreach (Note item in NoteManager.notes)
                {
                    if (item != null)
                        Destroy(item.gameObject);
                }
            }

            //Tile Create
            string json = File.ReadAllText(path);

            mapData = JsonConvert.DeserializeObject<MapData>(json);

            for (int i = 0; i < mapData.A.Count; i++)
            {
                if (mapData.A[i] != mapData.AllBeat[mapData.AllBeat.Count - 1] || Editor)
                    NoteAdd(KeyCode.A, mapData.A[i], i);
            }
            for (int i = 0; i < mapData.S.Count; i++)
            {
                if (mapData.S[i] != mapData.AllBeat[mapData.AllBeat.Count - 1] || Editor)
                    NoteAdd(KeyCode.S, mapData.S[i], i);
            }
            for (int i = 0; i < mapData.D.Count; i++)
            {
                if (mapData.D[i] != mapData.AllBeat[mapData.AllBeat.Count - 1] || Editor)
                    NoteAdd(KeyCode.D, mapData.D[i], i);
            }
            for (int i = 0; i < mapData.J.Count; i++)
            {
                if (mapData.J[i] != mapData.AllBeat[mapData.AllBeat.Count - 1] || Editor)
                    NoteAdd(KeyCode.J, mapData.J[i], i);
            }
            for (int i = 0; i < mapData.K.Count; i++)
            {
                if (mapData.K[i] != mapData.AllBeat[mapData.AllBeat.Count - 1] || Editor)
                    NoteAdd(KeyCode.K, mapData.K[i], i);
            }
            for (int i = 0; i < mapData.L.Count; i++)
            {
                if (mapData.L[i] != mapData.AllBeat[mapData.AllBeat.Count - 1] || Editor)
                    NoteAdd(KeyCode.L, mapData.L[i], i);
            }

            //시작 오프셋 설정
            if (!Editor)
                StartDelay = (60 / effect.BPM * 4) + mapData.Offset;
            else
                StartDelay = mapData.Offset;

            //BGM 세팅
            playerManager.StartCoroutine(SetBGM(path));

            if (!effect.HPRemove)
                HP = MaxHP * 0.5f;
            else
                HP = MaxHP;

            MapJsonPath = path;
            
            MapPath = path.Substring(0, path.LastIndexOf("\\") + 1);
            playerManager.BackgroundImage.Rerender();
            playerManager.StartCoroutine(playerManager.BackgroundVideo.Rerender());

            BPMChange(mapData.Effect.BPM, 0);
            BPMCurrentBeat = 0;
            BPMTimer = 0;
            time = 0;

            EffectManager.EffectReset();

            /*mapData.Effect.Camera.CameraZoomEffect.Clear();
            for (int i = 0; i < 500; i++)
            {
                mapData.Effect.Camera.CameraZoomEffect.Add(new CameraZoomEffect { Beat = 8 * i + 9, Value = 0.95, Lerp = 1 });
                mapData.Effect.Camera.CameraZoomEffect.Add(new CameraZoomEffect { Beat = 8 * i + 9, Value = 1, Lerp = 0.0625 });
            }*/

            EditorManager.hitSound();

            EffectAdd();
        }

        //맵 저장
        static void MapSaveScreen()
        {
            /*//파일오픈창 생성 및 설정
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Title = LangManager.LangLoad(LangManager.Lang, "editMode.map_save");
            ofd.FileName = $"{mapData.BGMName}";
            ofd.Filter = $"{LangManager.LangLoad(LangManager.Lang, "editMode.sdjk_map")}(*.sdjk) | *.sdjk;";
 
            //파일 오픈창 로드
            DialogResult dr = ofd.ShowDialog();
             
            //OK버튼 클릭시
            if (dr == DialogResult.OK)
                //File경로 + 파일명 리턴
                MapSave(ofd.FileName);*/

            string Path = StandaloneFileBrowser.SaveFilePanel(LangManager.LangLoad(LangManager.Lang, "editMode.map_save"), "", "", "sdjk");
            if (Path != "")
                MapSave(Path);
        }

        public static void MapSave(string path)
        {
            string json = JsonConvert.SerializeObject(mapData, Formatting.Indented);
            File.WriteAllText(path, json);
            MapJsonPath = path;
        }

        public static void EditorMapPlay()
        {
            if (MapJsonPath != "")
            {
                string json = JsonConvert.SerializeObject(mapData, Formatting.Indented);
                File.WriteAllText(MapJsonPath, json);
            }
            else
            {
                MapSaveScreen();
                if (MapJsonPath == "")
                    return;
            }

            GameManager.GameSpeed = 1;
            SceneManager.SceneLoading("Play Mode");
            playerManager.enabled = false;
            playerManager.audioSource.enabled = false;
            isEditorMapPlay = true;
            
            AutoMode = false;
            PracticeMode = false;

            if (Input.GetKey(KeyCode.A))
                AutoMode = true;
            if (Input.GetKey(KeyCode.P))
                PracticeMode = true;
        }

        void Update()
        {
            if (!Editor && GameManager.TouchMode && !Touch.gameObject.activeSelf)
                Touch.gameObject.SetActive(true);
            else if (!GameManager.TouchMode && Touch.gameObject.activeSelf)
                Touch.gameObject.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Escape))
                GameManager.EscKey = true;

            if (HP > 0 && HP < 0.001f)
                HP = 0;

            //F1
            if (Input.GetKeyDown(KeyCode.F1))
                UIHide = !UIHide;
            
            //ESC
            if (!Editor)
            {
                if (!isEditorMapPlay)
                {
                    if (GameManager.EscKey && (HP > 0.001f || Editor || PracticeMode))
                    {
                        if (!AutoMode && !PracticeMode)
                        {
                            if (!GameManager.mapRecord.ContainsKey(GameManager.Level))
                                GameManager.mapRecord.Add(GameManager.Level, audioSource.time / audioSource.clip.length * 100);
                            {
                                if (GameManager.mapRecord[GameManager.Level] < audioSource.time / audioSource.clip.length * 100)
                                    GameManager.mapRecord[GameManager.Level] = audioSource.time / audioSource.clip.length * 100;
                            }
                        }

                        Quit();
                    }
                }
                else if (GameManager.EscKey && (HP > 0.001f || Editor || PracticeMode))
                    QuitEditPlay();

                if (audioSource.isPlaying)
                    time = audioSource.time;

                //피치 설정
                audioSource.pitch = (float) (effect.Pitch * GameManager.GameSpeed);
            }

            if (Editor)
            {
                //에디터엔 노트보간이 적용이 안되서, if 구문이 없음
                time = audioSource.time;

                //화살표로 이동하고 있을때, 피치 변경이 막히는걸 방지
                if (!(Input.GetKey(KeyCode.UpArrow) || EditorManager.AutoScroll || Input.GetKey(KeyCode.DownArrow)))
                    audioSource.pitch = 0;

                //컨트롤 + S
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S) && !EditorManager.AutoScroll && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
                {
                    if (MapJsonPath != "")
                        MapSave(MapJsonPath);
                    else
                        MapSaveScreen();
                }
                //컨트롤 + 쉬프트 + S
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S) && !EditorManager.AutoScroll && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
                    MapSaveScreen();

                //컨트롤 + L
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L) && !EditorManager.AutoScroll && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
                    MapLoad();

                //Enter
                if (Input.GetKey(KeyCode.Return) && !EditorManager.AutoScroll && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
                    EditorMapPlay();
            }

            //히트 사운드 전용 (플레이어 오프셋 X) Current Beat 변수
            if (Editor || !HitCurrentBeatStop)
                HitSoundCurrentBeat = (BeatTimer + (time - BPMTimer) - StartDelay) * (effect.BPM / 60) + BPMCurrentBeat;
            if (Editor)
                HitSoundCurrentBeat += 1;

            //플레이어 전용 (플레이어 오프셋 O) Current Beat 변수
            double CurrentBeat = (BeatTimer + (time - BPMTimer) - StartDelay - (GameManager.InputOffset * audioSource.pitch)) * (effect.BPM / 60) + BPMCurrentBeat;
            VisibleCurrentBeatLerp = Mathf.Lerp((float)VisibleCurrentBeatLerp, (float)CurrentBeat, (float)(effect.BPM / 60.0 * 0.2 * GameManager.FpsDeltaTime));
            /*if (GameManager.NoteInterpolation)
                VisibleCurrentBeatLerp = Mathf.Lerp((float)VisibleCurrentBeatLerp, (float)CurrentBeat, (float)(effect.BPM / 60.0 * 0.2 * GameManager.FpsDeltaTime));
            else
                VisibleCurrentBeatLerp = CurrentBeat;*/

            VisibleCurrentBeat = VisibleCurrentBeatLerp;
            if (Editor)
                VisibleCurrentBeat += 1;

            if (GameManager.NoteInterpolation)
                VisibleCurrentBeat += 0.2 * (60.0 / effect.BPM) * audioSource.pitch;

            //판정 전용 (플레이어 오프셋 O) Current Beat 변수
            if (GameManager.NoteInterpolation)
                JudgmentCurrentBeat = VisibleCurrentBeat;
            else
            {
                JudgmentCurrentBeat = ((BeatTimer + (time - BPMTimer) - StartDelay - (GameManager.InputOffset * audioSource.pitch)) * (effect.BPM / 60) + BPMCurrentBeat);

                if (Editor)
                    JudgmentCurrentBeat += 1;
            }

            //HP Bar
            if (!(GameManager.EditorOptimization && Editor))
            {
                if (!UIHide)
                {
                    HPImage.fillAmount = (float)(HP / 100);

                    HP = GameManager.Clamp(HP, 0, MaxHP);

                    //콤포 텍스트
                    if (Combo != 0)
                        ComboText.text = Combo.ToString();
                    else
                        ComboText.text = "";
                }
                
                //게임 리스타트
                if (HP <= 0 && !(Editor || PracticeMode) || Input.GetKeyDown(KeyCode.R) && !Editor)
                {
                    if (!isEditorMapPlay && !AutoMode && !PracticeMode)
                    {
                        if (!GameManager.mapRecord.ContainsKey(GameManager.Level))
                            GameManager.mapRecord.Add(GameManager.Level, audioSource.time / audioSource.clip.length * 100);
                        {
                            if (GameManager.mapRecord[GameManager.Level] < audioSource.time / audioSource.clip.length * 100)
                                GameManager.mapRecord[GameManager.Level] = audioSource.time / audioSource.clip.length * 100;
                        }
                    }

                    HP = 0;
                    StartCoroutine(Restart());
                }

                if (HP > 0.001f || PracticeMode)
                {
                    //자동 나가기
                    if (mapData.AllBeat.Count != 0 && mapData.AllBeat[mapData.AllBeat.Count - 1] <= HitSoundCurrentBeat - 3 && !Editor && !isEditorMapPlay)
                    {
                        if (!AutoMode && !PracticeMode)
                        {
                            if (!GameManager.mapRecord.ContainsKey(GameManager.Level))
                                GameManager.mapRecord.Add(GameManager.Level, 100);
                            else
                                GameManager.mapRecord[GameManager.Level] = 100;

                            if (!GameManager.mapAccuracy.ContainsKey(GameManager.Level))
                                GameManager.mapAccuracy.Add(GameManager.Level, ScoreManager.Accuracy);
                            else
                            {
                                if (GameManager.mapAccuracy[GameManager.Level] > GameManager.Abs(ScoreManager.Accuracy))
                                    GameManager.mapAccuracy[GameManager.Level] = ScoreManager.Accuracy;
                            }

                            if (!GameManager.mapRank.ContainsKey(GameManager.Level))
                                GameManager.mapRank.Add(GameManager.Level, ScoreManager.Rank);
                            else
                            {
                                if (GameManager.mapRank[GameManager.Level] == "F" && (ScoreManager.Rank == "E" || ScoreManager.Rank == "D" || ScoreManager.Rank == "C" || ScoreManager.Rank == "B" || ScoreManager.Rank == "A" || ScoreManager.Rank == "S" || ScoreManager.Rank == "SS" || ScoreManager.Rank == "SSS"))
                                    GameManager.mapRank[GameManager.Level] = ScoreManager.Rank;
                                else if (GameManager.mapRank[GameManager.Level] == "E" && (ScoreManager.Rank == "C" || ScoreManager.Rank == "B" || ScoreManager.Rank == "A" || ScoreManager.Rank == "S" || ScoreManager.Rank == "SS" || ScoreManager.Rank == "SSS"))
                                    GameManager.mapRank[GameManager.Level] = ScoreManager.Rank;
                                else if (GameManager.mapRank[GameManager.Level] == "C" && (ScoreManager.Rank == "B" || ScoreManager.Rank == "A" || ScoreManager.Rank == "S" || ScoreManager.Rank == "SS" || ScoreManager.Rank == "SSS"))
                                    GameManager.mapRank[GameManager.Level] = ScoreManager.Rank;
                                else if (GameManager.mapRank[GameManager.Level] == "B" && (ScoreManager.Rank == "A" || ScoreManager.Rank == "S" || ScoreManager.Rank == "SS" || ScoreManager.Rank == "SSS"))
                                    GameManager.mapRank[GameManager.Level] = ScoreManager.Rank;
                                else if (GameManager.mapRank[GameManager.Level] == "A" && (ScoreManager.Rank == "S" || ScoreManager.Rank == "SS" || ScoreManager.Rank == "SSS"))
                                    GameManager.mapRank[GameManager.Level] = ScoreManager.Rank;
                                else if (GameManager.mapRank[GameManager.Level] == "S" && (ScoreManager.Rank == "SS" || ScoreManager.Rank == "SSS"))
                                    GameManager.mapRank[GameManager.Level] = ScoreManager.Rank;
                                else if (GameManager.mapRank[GameManager.Level] == "SS" && (ScoreManager.Rank == "SSS"))
                                    GameManager.mapRank[GameManager.Level] = ScoreManager.Rank;
                            }

                            if (mapData.Difficulty == "very_easy")
                                GameManager.PlayerExp += 20;
                            else if (mapData.Difficulty == "easy")
                                GameManager.PlayerExp += 40;
                            else if (mapData.Difficulty == "normal")
                                GameManager.PlayerExp += 60;
                            else if (mapData.Difficulty == "hard")
                                GameManager.PlayerExp += 80;
                            else if (mapData.Difficulty == "very_hard")
                                GameManager.PlayerExp += 100;

                            SystemUI.SystemUI.systemUI.Renderer();
                        }

                        Quit();
                    }
                    else if (mapData.AllBeat.Count != 0 && mapData.AllBeat[mapData.AllBeat.Count - 1] <= HitSoundCurrentBeat - 3 && !Editor)
                        QuitEditPlay();
                }

                //HP Remove
                if (effect.HPRemove && 2 <= mapData.AllBeat.Count && JudgmentCurrentBeat < mapData.AllBeat[mapData.AllBeat.Count - 2] && JudgmentCurrentBeat >= 0 && (HP > 0.001f || Editor || PracticeMode))
                    HP -= (effect.HPRemoveValue * GameManager.FpsDeltaTime * (effect.BPM / 60));

                //비네팅
                if (!UIHide)
                {
                    if (!Editor)
                        MainCamera.mainCamera.PostProcessVolume.weight = (float)(1 - (HP - MaxHP * 0.3f) / (MaxHP * 2));
                    else if (MainCamera.mainCamera.PostProcessVolume.weight != 0)
                        MainCamera.mainCamera.PostProcessVolume.weight = 0;
                }
                else
                {
                    MainCamera.mainCamera.PostProcessVolume.weight = 0;
                }
            }
        }

        void LateUpdate() => GameManager.EscKey = false;

        public void EscKey() => GameManager.EscKey = true;

        public static void BPMChange(double BPM, double Delay)
        {
            if (!Editor)
                BPMCurrentBeat = Delay;
            else
                BPMCurrentBeat = Delay + (BPM / mapData.Effect.BPM) - 1;

            BPMTimer = 0;
            double temp = 0;
            for (int i = 0; i < mapData.Effect.BPMEffect.Count; i++)
            {
                if (mapData.Effect.BPMEffect[0].Beat > Delay)
                    break;

                double tempBPM;
                if (i - 1 < 0)
                    tempBPM = mapData.Effect.BPM;
                else
                    tempBPM = mapData.Effect.BPMEffect[i - 1].Value;

                BPMTimer += 60.0 / tempBPM * (mapData.Effect.BPMEffect[i].Beat - temp);
                temp = mapData.Effect.BPMEffect[i].Beat;

                if (mapData.Effect.BPMEffect[i].Beat >= Delay)
                    break;
            }

            effect.BPM = BPM;
        }
        
        void FixedUpdate()
        {
            if (Application.platform != RuntimePlatform.Android)
            {
                //Discord
                if (!Editor)
                    PresenceManager.UpdatePresence("Map Play", mapData.Artist + " - " + mapData.BGMName);
                else
                    PresenceManager.UpdatePresence("Map Editor", mapData.Artist + " - " + mapData.BGMName);
            }
        }

        public void PhoneControllQuit()
        {
            if (!isEditorMapPlay)
            {
                if (HP > 0.001f || Editor || PracticeMode)
                    Quit();
            }
            else if (HP > 0.001f || Editor || PracticeMode)
                QuitEditPlay();
        }

        public static void Quit()
        {
            if (HP <= 0)
                HP = 0.001f;
            
            GameManager.GameSpeed = 1;
            SceneManager.SceneLoading("Main Menu");
            playerManager.enabled = false;
            playerManager.audioSource.enabled = false;
            NoteManager.notes = new List<Note>();
            
            MapJsonPath = "";
        }
        
        static void QuitEditPlay()
        {
            if (HP <= 0)
                HP = 0.001f;
            
            GameManager.GameSpeed = 1;
            SceneManager.SceneLoading("Editor Mode");
            playerManager.enabled = false;
            playerManager.audioSource.enabled = false;
            isEditorMapPlay = false;
            NoteManager.notes = new List<Note>();
            
            AutoMode = false;
            PracticeMode = false;
            
            AutoLoad = false;
        }
        
        public IEnumerator Restart()
        {
            if (HP <= 0)
                HP = 0.001f;
            
            while (GameManager.GameSpeed >= 0.25f)
            {
                GameManager.GameSpeed = Mathf.Lerp(GameManager.GameSpeed, 0, 0.01f * GameManager.FpsUnscaledDeltaTime);
                yield return null;
            }
            
            GameManager.GameSpeed = 1;
            SceneManager.SceneLoading("Play Mode", true);
            playerManager.enabled = false;
            playerManager.audioSource.enabled = false;
        }

        static IEnumerator SetBGM(string Path = "")
        {
            bool temp = false;
            ResourcesManager.GetStringNameSpace(mapData.BGM, out string ResourceName);
            ResourceName = ResourcesManager.BGMPath + ResourceName;
            
            //리소스팩에서 리소스를 가져오기
            foreach (AudioClip item in ResourcesManager.ResourcesPackBGMList)
            {
                if (item.name == ResourceName.Substring(ResourceName.LastIndexOf("/") + 1))
                {
                    playerManager.audioSource.clip = item;
                    temp = true;
                }
            }
            
            //맵 파일에서 리소스를 가져오기
            if (!temp)
            {
                Path = Path.Substring(0, Path.LastIndexOf("\\") + 1);
                if (File.Exists(Path + mapData.BGM + ".mp3"))
                {
                    UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(Path + mapData.BGM + ".mp3", AudioType.MPEG);
                    yield return www.SendWebRequest();

                    if (www.result == UnityWebRequest.Result.ConnectionError)
                        Debug.Log(www.error);
                    else
                        playerManager.audioSource.clip = DownloadHandlerAudioClip.GetContent(www);

                    temp = true;

                    www.Dispose();
                }
                else if (File.Exists(Path + mapData.BGM + ".ogg"))
                {
                    UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(Path + mapData.BGM + ".ogg", AudioType.OGGVORBIS);
                    yield return www.SendWebRequest();
                    if (www.result == UnityWebRequest.Result.ConnectionError)
                        Debug.Log(www.error);
                    else
                        playerManager.audioSource.clip = DownloadHandlerAudioClip.GetContent(www);

                    temp = true;

                    www.Dispose();
                }
                /*else if (File.Exists(Path + mapData.BGM + ".mid"))
                {
                    audioSource.clip = null;
                    MidiPlayer.midiSource.streamingAssetPath = Path + mapData.BGM + ".mid";
                    MidiPlayer.enabled = true;
                    temp = true;
                }*/
            }

            //원본 리소스를 가져오기
            if (!temp)
            {
                foreach (AudioClip[] item in ResourcesManager.BGMList)
                {
                    foreach (AudioClip item2 in item)
                    {
                        if (item2.name == ResourceName.Substring(ResourceName.LastIndexOf("/") + 1))
                            playerManager.audioSource.clip = item2;
                    }
                }
            }

            //BGM 재생
            if (!Editor)
                playerManager.StartCoroutine(BGMPlay());
        }

        static IEnumerator BGMPlay()
        {
            HitSoundCurrentBeat = -10;
            BeatTimer = 0;

            yield return new WaitForSeconds((float)(60.0 / mapData.Effect.BPM));

            //설정한 오프셋 만큼 기다리기
            while (BeatTimer < StartDelay + 60 / effect.BPM - mapData.Offset)
            {
                BeatTimer += GameManager.UnscaledDeltaTime * effect.Pitch * GameManager.GameSpeed;
                yield return null;
                HitSoundCurrentBeat = (BeatTimer + (time - BPMTimer) - StartDelay) * (effect.BPM / 60) + BPMCurrentBeat;
            }
            BeatTimer = StartDelay + 60 / effect.BPM - mapData.Offset;

            /*Debug.Log(File.Exists(MapPath + mapData.BGM + ".mid")); 
            if (File.Exists(MapPath + mapData.BGM + ".mid"))
                MidiPlayer.Play();
            else*/
            playerManager.HitCurrentBeatStop = false;
            playerManager.audioSource.Play();
            
            //BGM 재생후, 끝났을때 타이머를 가동시켜서 노트를 이어서 재생하기
            float Timer = 0;
            while (playerManager.audioSource.isPlaying)
            {
                Timer = playerManager.audioSource.time;
                yield return null;
            }

            BeatTimer = Timer;
            while (true)
            {
                BeatTimer += GameManager.UnscaledDeltaTime * effect.Pitch * GameManager.GameSpeed;
                yield return null;
            }
        }

        public enum Key
        {
            A,
            S,
            D,
            J,
            K,
            L
        }

        public static Note NoteAdd(KeyCode keyCode, double Beat, int index)
        {
            Note note = Instantiate(playerManager.prefab, bar.transform).GetComponent<Note>();
            note.Beat = Beat;

            note.keyCode = keyCode;

            //노트가 없을땐, Input 바를 숨기기 위해서 있는 변수
            if (keyCode == KeyCode.A)
            {
                AUse = true;
                InputSystem.A.Add(note);
                note.keyCode = GameManager.A;
            }
            else if (keyCode == KeyCode.S)
            {
                SUse = true;
                InputSystem.S.Add(note);
                note.keyCode = GameManager.S;
            }
            else if (keyCode == KeyCode.D)
            {
                DUse = true;
                InputSystem.D.Add(note);
                note.keyCode = GameManager.D;
            }
            else if (keyCode == KeyCode.J)
            {
                JUse = true;
                InputSystem.J.Add(note);
                note.keyCode = GameManager.J;
            }
            else if (keyCode == KeyCode.K)
            {
                KUse = true;
                InputSystem.K.Add(note);
                note.keyCode = GameManager.K;
            }
            else if (keyCode == KeyCode.L)
            {
                LUse = true;
                InputSystem.L.Add(note);
                note.keyCode = GameManager.L;
            }

            //최대 스코어 설정
            ScoreManager.MaxScore += 100;

            double offset;
            if (GameManager.UpScroll)
                offset = -5.5;
            else
                offset = 5.5;

            //노트 설정
            if (keyCode == KeyCode.A)
            {
                note.transform.localPosition = new Vector2(-5.535f, (float)(Beat * effect.BeatYPos - offset));

                if (index >= 0)
                {
                    if (index < mapData.HoldA.Count)
                    {
                        note.HoldNote.localScale = new Vector3(1, (float)(1.666666666666667 * mapData.Effect.BeatYPos * mapData.HoldA[index]), 1);
                        note.HoldNote.localPosition = new Vector2(0, (float)(mapData.Effect.BeatYPos * mapData.HoldA[index]));
                        note.HoldBeat = mapData.HoldA[index];
                    }
                    else
                        mapData.HoldA.Add(0);
                }
            }
            else if (keyCode == KeyCode.S)
            {
                note.transform.localPosition = new Vector2(-3.321f, (float)(Beat * effect.BeatYPos - offset));

                if (index >= 0)
                {
                    if (index < mapData.HoldS.Count)
                    {
                        note.HoldNote.localScale = new Vector3(1, (float)(1.666666666666667 * mapData.Effect.BeatYPos * mapData.HoldS[index]), 1);
                        note.HoldNote.localPosition = new Vector2(0, (float)(mapData.Effect.BeatYPos * mapData.HoldS[index]));
                        note.HoldBeat = mapData.HoldS[index];
                    }
                    else
                        mapData.HoldS.Add(0);
                }
            }
            else if (keyCode == KeyCode.D)
            {
                note.transform.localPosition = new Vector2(-1.107f, (float)(Beat * effect.BeatYPos - offset));

                if (index >= 0)
                {
                    if (index < mapData.HoldD.Count)
                    {
                        note.HoldNote.localScale = new Vector3(1, (float)(1.666666666666667 * mapData.Effect.BeatYPos * mapData.HoldD[index]), 1);
                        note.HoldNote.localPosition = new Vector2(0, (float)(mapData.Effect.BeatYPos * mapData.HoldD[index]));
                        note.HoldBeat = mapData.HoldD[index];
                    }
                    else
                        mapData.HoldD.Add(0);
                }
            }
            else if (keyCode == KeyCode.J)
            {
                note.transform.localPosition = new Vector2(1.107f, (float)(Beat * effect.BeatYPos - offset));

                if (index >= 0)
                {
                    if (index < mapData.HoldJ.Count)
                    {
                        note.HoldNote.localScale = new Vector3(1, (float)(1.666666666666667 * mapData.Effect.BeatYPos * mapData.HoldJ[index]), 1);
                        note.HoldNote.localPosition = new Vector2(0, (float)(mapData.Effect.BeatYPos * mapData.HoldJ[index]));
                        note.HoldBeat = mapData.HoldJ[index];
                    }
                    else
                        mapData.HoldJ.Add(0);
                }
            }
            else if (keyCode == KeyCode.K)
            {
                note.transform.localPosition = new Vector2(3.321f, (float)(Beat * effect.BeatYPos - offset));

                if (index >= 0)
                {
                    if (index < mapData.HoldK.Count)
                    {
                        note.HoldNote.localScale = new Vector3(1, (float)(1.666666666666667 * mapData.Effect.BeatYPos * mapData.HoldK[index]), 1);
                        note.HoldNote.localPosition = new Vector2(0, (float)(mapData.Effect.BeatYPos * mapData.HoldK[index]));
                        note.HoldBeat = mapData.HoldK[index];
                    }
                    else
                        mapData.HoldK.Add(0);
                }
            }
            else if (keyCode == KeyCode.L)
            {
                note.transform.localPosition = new Vector2(5.535f, (float)(Beat * effect.BeatYPos - offset));

                if (index >= 0)
                {
                    if (index < mapData.HoldL.Count)
                    {
                        note.HoldNote.localScale = new Vector3(1, (float)(1.666666666666667 * mapData.Effect.BeatYPos * mapData.HoldL[index]), 1);
                        note.HoldNote.localPosition = new Vector2(0, (float)(mapData.Effect.BeatYPos * mapData.HoldL[index]));
                        note.HoldBeat = mapData.HoldL[index];
                    }
                    else
                        mapData.HoldL.Add(0);
                }
            }

            if (note.HoldBeat > 0)
                ScoreManager.MaxScore += 100;

            //즉사 패턴
            if (note.HoldBeat < 0 && note.HoldBeat >= -1)
            {
                note.HoldNote.localScale = Vector2.zero;
                note.spriteRenderer.color = Color.red;

                ScoreManager.MaxScore -= 100;
            }

            return note;
        }

        public static void NoteRemove(KeyCode keyCode, double Beat)
        {
            //에디터 전용
            for (int i = 0; i < NoteManager.notes.Count; i++)
            {
                Note item = NoteManager.notes[i];
                if (item.Beat == Beat && item.keyCode == keyCode)
                {
                    if (keyCode == GameManager.A)
                        InputSystem.A.Remove(item);
                    else if (keyCode == GameManager.S)
                        InputSystem.S.Remove(item);
                    else if (keyCode == GameManager.D)
                        InputSystem.D.Remove(item);
                    else if (keyCode == GameManager.J)
                        InputSystem.J.Remove(item);
                    else if (keyCode == GameManager.K)
                        InputSystem.K.Remove(item);
                    else if (keyCode == GameManager.L)
                        InputSystem.L.Remove(item);
                    
                    if (item.HoldBeat > 0 && item.HoldBeat < -1)
                        ScoreManager.MaxScore -= 100;

                    Destroy(item.gameObject);
                }
            }
        }

        public static bool Judgment(double Delay, bool Score = true, bool NoteHide = true)
        {
            //판정 (비트 기준)
            if (Delay > 0.75 * effect.JudgmentSize)
            {
                JudgmentResult("Miss", -5, false, Delay, NoteHide, Score);
                ScoreManager.AllPerfect = false;
                ScoreManager.AllSick = false;
                return true;
            }
            else if (Delay > 0.5 * effect.JudgmentSize && Delay <= 0.75 * effect.JudgmentSize)
            {
                JudgmentResult("Early", -2.5, true, Delay, NoteHide, Score);
                ScoreManager.AllPerfect = false;
                ScoreManager.AllSick = false;
                return false;
            }
            else if (Delay > 0.25 * effect.JudgmentSize && Delay <= 0.5 * effect.JudgmentSize)
            {
                JudgmentResult("Good", 0.5f, true, Delay, NoteHide, Score);
                ScoreManager.AllPerfect = false;
                ScoreManager.AllSick = false;
                return false;
            }
            else if (Delay > 0.125 * effect.JudgmentSize && Delay <= 0.25 * effect.JudgmentSize)
            {
                JudgmentResult("Great", 1, true, Delay, NoteHide, Score);
                ScoreManager.AllPerfect = false;
                ScoreManager.AllSick = false;
                return false;
            }
            else if (Delay > 0.03125f * effect.JudgmentSize && Delay <= 0.125 * effect.JudgmentSize)
            {
                JudgmentResult("Perfect", 2, true, Delay, NoteHide, Score);
                ScoreManager.AllSick = false;
                return false;
            }
            else if (Delay >= -0.03125 * effect.JudgmentSize && Delay <= 0.03125 * effect.JudgmentSize)
            {
                JudgmentResult("Sick!!", 4, true, Delay, NoteHide, Score);
                return false;
            }
            else if (Delay >= -0.125 * effect.JudgmentSize && Delay < -0.03125 * effect.JudgmentSize)
            {
                JudgmentResult("Perfect", 2, true, Delay, NoteHide, Score);
                ScoreManager.AllSick = false;
                return false;
            }
            else if (Delay >= -0.25 * effect.JudgmentSize && Delay < -0.125 * effect.JudgmentSize)
            {
                JudgmentResult("Great", 1, true, Delay, NoteHide, Score);
                ScoreManager.AllPerfect = false;
                ScoreManager.AllSick = false;
                return false;
            }
            else if (Delay >= -0.5 * effect.JudgmentSize && Delay < -0.25 * effect.JudgmentSize)
            {
                JudgmentResult("Good", 0.5f, true, Delay, NoteHide, Score);
                ScoreManager.AllPerfect = false;
                ScoreManager.AllSick = false;
                return false;
            }
            else if (Delay >= -0.75 * effect.JudgmentSize && Delay < -0.5 * effect.JudgmentSize)
            {
                JudgmentResult("Early", -2.5, true, Delay, NoteHide, Score);
                ScoreManager.AllPerfect = false;
                ScoreManager.AllSick = false;
                return false;
            }
            else if (Delay < -0.75 * effect.JudgmentSize)
            {
                if (!GameManager.AllowIndirectMiss)
                {
                    JudgmentResult("Miss", -5, false, Delay, NoteHide, Score);
                    ScoreManager.AllPerfect = false;
                    ScoreManager.AllSick = false;
                }
                return true;
            }

            return true;
        }

        public static void JudgmentResult(string Text, double hp, bool combo, double Delay, bool notIndirect, bool Score)
        {
            if (hp > 0 && effect.HPRemove)
                hp = 20 * (effect.BPM / 60) * effect.HPAddValue;
            else
                hp *= effect.BPM / 60 * effect.HPAddValue;

            if (Text == "Sick!!" && !GameManager.Optimization)
            {
                //무지개
                playerManager.JudgmentImage.PosReset();

                playerManager.JudgmentText.text.text = Text;
                playerManager.JudgmentText.text.color = Color.white;
                playerManager.JudgemntTextLine.effectColor = Color.clear;
                playerManager.JudgemntTextLine2.effectColor = Color.clear;
                
                if (!notIndirect)
                    playerManager.DelayText.text.text = "Indirect";
                else
                    playerManager.DelayText.text.text = (Mathf.RoundToInt((float)(Delay * 1000)) * 0.001) + " Beat";
            }
            else
            {
                //일반
                if (!GameManager.Optimization)
                    playerManager.JudgmentImage.PosReset();

                playerManager.JudgmentText.text.text = Text;
                playerManager.JudgmentText.text.color = Color.white;
                playerManager.JudgmentImage.image.enabled = false;
                playerManager.JudgemntTextLine.effectColor = Color.black;
                playerManager.JudgemntTextLine2.effectColor = Color.black;
                
                if (!notIndirect)
                    playerManager.DelayText.text.text = "Indirect";
                else
                    playerManager.DelayText.text.text = (Mathf.RoundToInt((float)(Delay * 1000)) * 0.001) + " Beat";
            }

            //콤보 리셋
            if (combo && Score)
                Combo++;
            else
                Combo = 0;

            if (Score)
            {
                //점수 (딜레이에 비례)
                if (combo)
                    ScoreManager.Score += (int)(100 / (GameManager.Abs(Mathf.RoundToInt((float)(Delay * 1000)) * 0.001) + 1));
                else
                    ScoreManager.Score -= (int)((GameManager.Abs(Mathf.RoundToInt((float)(Delay * 1000)) * 0.001) + 1) * 100);

                ScoreManager.AccuracyList.Add(Mathf.RoundToInt((float)(Delay * 1000)) * 0.001);


                //미스
                if (!combo)
                {
                    if (Delay < 0)
                        ScoreManager.IndirectMiss++;
                    else
                        ScoreManager.Miss++;
                }

                //SSS 랭크
                if (Mathf.RoundToInt((float)(Delay * 1000)) * 0.001 != 0)
                    ScoreManager.AllBeatDelayZero = false;

                //HP
                if (HP > 0.001f || Editor || PracticeMode)
                    HP += hp;
            }

            //HitSound
            if (GameManager.OsuHitSound)
            {
                if (mapData.AllBeat.Count >= 1 && HitSoundCurrentBeat < mapData.AllBeat[0])
                    HitSound.audioSource.PlayOneShot(HitSound.startHitSound, HitSound.audioSource.volume * GameManager.MainVolume);
                else if (mapData.AllBeat.Count >= 2 && HitSoundCurrentBeat >= mapData.AllBeat[mapData.AllBeat.Count - 1])
                    HitSound.audioSource.PlayOneShot(HitSound.endHitSound, HitSound.audioSource.volume * GameManager.MainVolume);
                else
                    HitSound.audioSource.PlayOneShot(HitSound.hitSound, HitSound.audioSource.volume * GameManager.MainVolume);
            }
        }
    }

    [System.Serializable]
    public class MapData
    {
        public List<double> A = new List<double>();
        public List<double> S = new List<double>();
        public List<double> D = new List<double>();
        public List<double> J = new List<double>();
        public List<double> K = new List<double>();
        public List<double> L = new List<double>();

        public List<double> HoldA = new List<double>();
        public List<double> HoldS = new List<double>();
        public List<double> HoldD = new List<double>();
        public List<double> HoldJ = new List<double>();
        public List<double> HoldK = new List<double>();
        public List<double> HoldL = new List<double>();

        public List<double> AllBeat = new List<double>();

        public EffectList Effect = new EffectList();
        
        public string BGM = "";
        public string Background = "";
        public string BackgroundNight = "";
        public string VideoBackground = "";
        public string VideoBackgroundNight = "";
        public double VideoOffset = 0;
        
        public string Artist = "none";
        public string BGMName = "none";
        public double Offset;
        public string Difficulty = "";

        public bool HitSoundSimultaneousPlayAllow = false;

        public string Cover = "";
        public string CoverNight = "";
    }

    public class Effect
    {
        public double BPM = 100;
        public double Pitch = 1;
        public double PitchLerp = 1;
        public double BeatYPos = 3;
        public double BeatYPosLerp = 1;
        public double Volume = 1;
        public double VolumeLerp = 1;

        public double CameraZoom = 1;
        public double CameraZoomLerp = 1;
        public double UiZoom = 1;
        public double UiZoomLerp = 1;
        public JVector3 CameraPos = new JVector3();
        public double CameraPosLerp = 1;
        public JVector3 UiPos = new JVector3();
        public double UiPosLerp = 1;

        public double HPAddValue = 1;
        public double HPAddValueLerp = 1;
        public bool HPRemove = true;
        public double HPRemoveValue = 0.5f;
        public double HPRemoveValueLerp = 1;
        public double MaxHPValue = 100;
        public double MaxHPValueLerp = 1;

        public double JudgmentSize = 1;

        public double WindowSize = 1;
        public double WindowSizeLerp = 1;

        public JVector2 WindowPos = new JVector2();
        public WindowManager.datumPoint WindowDatumPoint;
        public WindowManager.datumPoint ScreenDatumPoint;
        public double WindowPosLerp = 1;

        public JVector3 ABarPos = new JVector3();
        public double ABarPosLerp = 0;
        public JVector3 SBarPos = new JVector3();
        public double SBarPosLerp = 0;
        public JVector3 DBarPos = new JVector3();
        public double DBarPosLerp = 0;
        public JVector3 JBarPos = new JVector3();
        public double JBarPosLerp = 0;
        public JVector3 KBarPos = new JVector3();
        public double KBarPosLerp = 0;
        public JVector3 LBarPos = new JVector3();
        public double LBarPosLerp = 0;
    }
}

namespace SDJK.Effect
{
    public class EffectList
    {
        public CameraEffect Camera = new CameraEffect();
        
        public double BPM = 100;
        public List<BPMEffect> BPMEffect = new List<BPMEffect>();

        public double BeatYPos = 3;
        public List<BeatYPosEffect> BeatYPosEffect = new List<BeatYPosEffect>();
        
        public double Pitch = 1;
        public List<PitchEffect> PitchEffect = new List<PitchEffect>();
        
        public double Volume = 1;
        public List<VolumeEffect> VolumeEffect = new List<VolumeEffect>();

        public bool HPRemove = true;
        public List<HPRemoveEffect> HPRemoveEffect = new List<HPRemoveEffect>();

        public double HPAddValue = 0.5f;
        public List<HPAddValueEffect> HPAddValueEffect = new List<HPAddValueEffect>();

        public double HPRemoveValue = 0.5f;
        public List<HPRemoveValueEffect> HPRemoveValueEffect = new List<HPRemoveValueEffect>();

        public double MaxHPValue = 100;
        public List<MaxHPValueEffect> MaxHPValueEffect = new List<MaxHPValueEffect>();

        public double JudgmentSize = 1;
        public List<JudgmentSizeEffect> JudgmentSizeEffect = new List<JudgmentSizeEffect>();

        public bool AudioSpectrumUse = false;
        public JColor32 AudioSpectrumColor = new JColor32(255);

        public double WindowSize = 1;
        public List<WindowSizeEffect> WindowSizeEffect = new List<WindowSizeEffect>();

        public JVector2 WindowPos = new JVector2();
        public WindowManager.datumPoint WindowDatumPoint = WindowManager.datumPoint.Center;
        public WindowManager.datumPoint ScreenDatumPoint = WindowManager.datumPoint.Center;
        public List<WindowPosEffect> WindowPosEffect = new List<WindowPosEffect>();

        public JVector3 ABarPos = new JVector3();
        public List<BarPosEffect> ABarPosEffect = new List<BarPosEffect>();
        public JVector3 SBarPos = new JVector3();
        public List<BarPosEffect> SBarPosEffect = new List<BarPosEffect>();
        public JVector3 DBarPos = new JVector3();
        public List<BarPosEffect> DBarPosEffect = new List<BarPosEffect>();
        public JVector3 JBarPos = new JVector3();
        public List<BarPosEffect> JBarPosEffect = new List<BarPosEffect>();
        public JVector3 KBarPos = new JVector3();
        public List<BarPosEffect> KBarPosEffect = new List<BarPosEffect>();
        public JVector3 LBarPos = new JVector3();
        public List<BarPosEffect> LBarPosEffect = new List<BarPosEffect>();

        public List<double> AllBeat = new List<double>();
    }

    public class BPMEffect
    {
        public double Beat = 0;
        public double Value = 100;
    }

    public class HPAddValueEffect
    {
        public double Beat = 0;
        public double Value = 0.5;
        public double Lerp = 1;
    }

    public class HPRemoveEffect
    {
        public double Beat = 0;
        public bool Value = true;
    }

    public class HPRemoveValueEffect
    {
        public double Beat = 0;
        public double Value = 0.5;
        public double Lerp = 1;
    }

    public class MaxHPValueEffect
    {
        public double Beat = 0;
        public double Value = 100;
        public double Lerp = 1;
    }

    public class PitchEffect
    {
        public double Beat = 0;
        public double Value = 1;
        public double Lerp = 1;
    }
    
    public class BeatYPosEffect
    {
        public double Beat = 0;
        public double Value = 3;
        public double Lerp = 1;
    }
    
    public class VolumeEffect
    {
        public double Beat = 0;
        public double Value = 0.4;
        public double Lerp = 1;
    }

    public class CameraEffect
    {
        public double CameraZoom = 1;
        public List<CameraZoomEffect> CameraZoomEffect = new List<CameraZoomEffect>();
        public JVector3 CameraPos = new JVector3(0, 0, -14);
        public List<CameraPosEffect> CameraPosEffect = new List<CameraPosEffect>();

        public double UiZoom = 1;
        public List<UiZoomEffect> UiZoomEffect = new List<UiZoomEffect>();
        public JVector3 UiPos = new JVector3();
        public List<UiPosEffect> UiPosEffect = new List<UiPosEffect>();
    }

    public class CameraZoomEffect
    {
        public double Beat = 0;
        public double Value = 1;
        public double Lerp = 1;
    }

    public class CameraPosEffect
    {
        public double Beat = 0;
        public JVector3 Value = new JVector3(0, 0, -14);
        public double Lerp = 1;
    }
    public class UiZoomEffect
    {
        public double Beat = 0;
        public double Value = 1;
        public double Lerp = 1;
    }

    public class UiPosEffect
    {
        public double Beat = 0;
        public JVector3 Value = new JVector3();
        public double Lerp = 1;
    }

    public class JudgmentSizeEffect
    {
        public double Beat = 0;
        public double Value = 1;
    }

    public class WindowSizeEffect
    {
        public double Beat = 0;
        public double Value = 1;
        public double Lerp = 1;
    }

    public class WindowPosEffect
    {
        public double Beat = 0;
        public JVector2 Pos = new JVector2();
        public WindowManager.datumPoint WindowDatumPoint = WindowManager.datumPoint.Center;
        public WindowManager.datumPoint ScreenDatumPoint = WindowManager.datumPoint.Center;
        public double Lerp = 1;
    }

    public class BarPosEffect
    {
        public double Beat = 0;
        public JVector3 Value = new JVector3();
        public double Lerp = 1;
    }
}