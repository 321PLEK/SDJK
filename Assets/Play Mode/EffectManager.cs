using System;
using SDJK.Camera;
using SDJK.PlayMode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDJK.Effect
{
    public class EffectManager : MonoBehaviour
    {
        public static int CameraPosIndex = 0;
        public static int UiPosIndex = 0;
        public static int CameraZoomIndex = 0;
        public static int UiZoomIndex = 0;
        public static int PitchIndex = 0;
        public static int BeatYPosIndex = 0;
        public static int VolumeIndex = 0;
        public static int HPAddValueIndex = 0;
        public static int HPRemoveIndex = 0;
        public static int HPRemoveValueIndex = 0;
        public static int MaxHPValueIndex = 0;
        public static int BPMIndex = 0;
        public static int JudgmentSizeIndex = 0;
        public static int WindowSizeIndex = 0;
        public static int WindowPosIndex = 0;
        public static int ABarPosIndex = 0;
        public static int SBarPosIndex = 0;
        public static int DBarPosIndex = 0;
        public static int JBarPosIndex = 0;
        public static int KBarPosIndex = 0;
        public static int LBarPosIndex = 0;

        public static double HPAddValue = 100;
        public static bool HPRemove = true;
        public static double HPRemoveValue = 0.5f;
        public static double MaxHPValue = 100;
        public static double Pitch = 1;
        public static double BeatYPos = 3;
        public static double JudgmentSize = 0;
        public static JVector3 ABarPos = new JVector3();
        public static JVector3 SBarPos = new JVector3();
        public static JVector3 DBarPos = new JVector3();
        public static JVector3 JBarPos = new JVector3();
        public static JVector3 KBarPos = new JVector3();
        public static JVector3 LBarPos = new JVector3();

        void Update()
        {
            if ((PlayerManager.HP > 0.001f || PlayerManager.Editor || PlayerManager.PracticeMode) && !(GameManager.EditorOptimization && PlayerManager.Editor))
            {
                EffectIndexReset();

                if (PlayerManager.playerManager.audioSource.pitch >= 0)
                {
                    while (CameraPosIndex >= 0 && CameraPosIndex < PlayerManager.mapData.Effect.Camera.CameraPosEffect.Count &&PlayerManager.VisibleCurrentBeat >=PlayerManager.mapData.Effect.Camera.CameraPosEffect[CameraPosIndex].Beat)
                    {
                        PlayerManager.effect.CameraPos = PlayerManager.mapData.Effect.Camera.CameraPosEffect[CameraPosIndex].Value;
                        PlayerManager.effect.CameraPosLerp = PlayerManager.mapData.Effect.Camera.CameraPosEffect[CameraPosIndex].Lerp;
                        CameraPosIndex++;

                        if (PlayerManager.effect.CameraPosLerp == 0 || PlayerManager.effect.CameraPosLerp == 1)
                            MainCamera.CameraPos = JVector3.JVector3ToVector3(PlayerManager.effect.CameraPos);
                    }

                    while (UiPosIndex >= 0 && UiPosIndex < PlayerManager.mapData.Effect.Camera.UiPosEffect.Count &&PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.Camera.UiPosEffect[UiPosIndex].Beat)
                    {
                        PlayerManager.effect.UiPos = PlayerManager.mapData.Effect.Camera.UiPosEffect[UiPosIndex].Value;
                        PlayerManager.effect.UiPosLerp = PlayerManager.mapData.Effect.Camera.UiPosEffect[UiPosIndex].Lerp;
                        UiPosIndex++;

                        if (PlayerManager.effect.UiPosLerp == 0 || PlayerManager.effect.UiPosLerp == 1)
                            MainCamera.UiPos = JVector3.JVector3ToVector3(PlayerManager.effect.UiPos);
                    }

                    while (CameraZoomIndex >= 0 && CameraZoomIndex < PlayerManager.mapData.Effect.Camera.CameraZoomEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.Camera.CameraZoomEffect[CameraZoomIndex].Beat)
                    {
                        PlayerManager.effect.CameraZoom = PlayerManager.mapData.Effect.Camera.CameraZoomEffect[CameraZoomIndex].Value;
                        PlayerManager.effect.CameraZoomLerp = PlayerManager.mapData.Effect.Camera.CameraZoomEffect[CameraZoomIndex].Lerp;
                        CameraZoomIndex++;

                        if (PlayerManager.effect.CameraZoomLerp == 0 || PlayerManager.effect.CameraZoomLerp == 1)
                            MainCamera.CameraZoom = PlayerManager.effect.CameraZoom;
                    }

                    while (UiZoomIndex >= 0 && UiZoomIndex < PlayerManager.mapData.Effect.Camera.UiZoomEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.Camera.UiZoomEffect[UiZoomIndex].Beat)
                    {
                        if (!GameManager.Ratio_9_16)
                            PlayerManager.effect.UiZoom = PlayerManager.mapData.Effect.Camera.UiZoomEffect[UiZoomIndex].Value;
                        else
                            PlayerManager.effect.UiZoom = PlayerManager.mapData.Effect.Camera.UiZoomEffect[UiZoomIndex].Value + 0.5f;
                        PlayerManager.effect.UiZoomLerp = PlayerManager.mapData.Effect.Camera.UiZoomEffect[UiZoomIndex].Lerp;
                        UiZoomIndex++;

                        if (PlayerManager.effect.UiZoomLerp == 0 || PlayerManager.effect.UiZoomLerp == 1)
                            MainCamera.UiZoom = PlayerManager.effect.UiZoom;
                    }

                    while (PitchIndex >= 0 && PitchIndex < PlayerManager.mapData.Effect.PitchEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.PitchEffect[PitchIndex].Beat)
                    {
                        Pitch = PlayerManager.mapData.Effect.PitchEffect[PitchIndex].Value;
                        PlayerManager.effect.PitchLerp = PlayerManager.mapData.Effect.PitchEffect[PitchIndex].Lerp;
                        PitchIndex++;

                        if (PlayerManager.effect.PitchLerp == 0 || PlayerManager.effect.PitchLerp == 1)
                            PlayerManager.effect.Pitch = Pitch;
                    }

                    while (BeatYPosIndex >= 0 && BeatYPosIndex < PlayerManager.mapData.Effect.BeatYPosEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.BeatYPosEffect[BeatYPosIndex].Beat)
                    {
                        BeatYPos = PlayerManager.mapData.Effect.BeatYPosEffect[BeatYPosIndex].Value;
                        PlayerManager.effect.BeatYPosLerp = PlayerManager.mapData.Effect.BeatYPosEffect[BeatYPosIndex].Lerp;
                        BeatYPosIndex++;

                        if (PlayerManager.effect.BeatYPosLerp == 0 || PlayerManager.effect.BeatYPosLerp == 1)
                            PlayerManager.effect.BeatYPos = BeatYPos;
                    }

                    while (VolumeIndex >= 0 && VolumeIndex < PlayerManager.mapData.Effect.VolumeEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.VolumeEffect[VolumeIndex].Beat)
                    {
                        PlayerManager.effect.Volume = PlayerManager.mapData.Effect.VolumeEffect[VolumeIndex].Value * GameManager.MainVolume;
                        PlayerManager.effect.VolumeLerp = PlayerManager.mapData.Effect.VolumeEffect[VolumeIndex].Lerp;
                        VolumeIndex++;

                        if (PlayerManager.effect.PitchLerp == 0 || PlayerManager.effect.PitchLerp == 1)
                            PlayerManager.playerManager.audioSource.volume = (float)PlayerManager.effect.Volume;
                    }

                    while (HPAddValueIndex >= 0 && HPAddValueIndex < PlayerManager.mapData.Effect.HPAddValueEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.HPAddValueEffect[HPAddValueIndex].Beat)
                    {
                        HPAddValue = PlayerManager.mapData.Effect.HPAddValueEffect[HPAddValueIndex].Value;
                        PlayerManager.effect.HPAddValueLerp = PlayerManager.mapData.Effect.HPAddValueEffect[HPAddValueIndex].Lerp;
                        HPAddValueIndex++;

                        if (PlayerManager.effect.HPAddValueLerp == 0 || PlayerManager.effect.HPAddValueLerp == 1)
                            PlayerManager.effect.HPAddValue = HPAddValue;
                    }
                    while (HPRemoveIndex >= 0 && HPRemoveIndex < PlayerManager.mapData.Effect.HPRemoveEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.HPRemoveEffect[HPRemoveIndex].Beat)
                    {
                        HPRemove = PlayerManager.mapData.Effect.HPRemoveEffect[HPRemoveIndex].Value;
                        HPRemoveIndex++;
                    }
                    while (HPRemoveValueIndex >= 0 && HPRemoveValueIndex < PlayerManager.mapData.Effect.HPRemoveValueEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.HPRemoveValueEffect[HPRemoveValueIndex].Beat)
                    {
                        HPRemoveValue = PlayerManager.mapData.Effect.HPRemoveValueEffect[HPRemoveValueIndex].Value;
                        PlayerManager.effect.HPRemoveValueLerp = PlayerManager.mapData.Effect.HPRemoveValueEffect[HPRemoveValueIndex].Lerp;
                        HPRemoveValueIndex++;

                        if (PlayerManager.effect.HPRemoveValueLerp == 0 || PlayerManager.effect.HPRemoveValueLerp == 1)
                            PlayerManager.effect.HPRemoveValue = HPRemoveValue;
                    }
                    while (MaxHPValueIndex >= 0 && MaxHPValueIndex < PlayerManager.mapData.Effect.MaxHPValueEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.MaxHPValueEffect[MaxHPValueIndex].Beat)
                    {
                        MaxHPValue = PlayerManager.mapData.Effect.MaxHPValueEffect[MaxHPValueIndex].Value;
                        PlayerManager.effect.MaxHPValueLerp = PlayerManager.mapData.Effect.MaxHPValueEffect[MaxHPValueIndex].Lerp;
                        MaxHPValueIndex++;

                        if (PlayerManager.effect.MaxHPValueLerp == 0 || PlayerManager.effect.MaxHPValueLerp == 1)
                            PlayerManager.effect.MaxHPValue = MaxHPValue;
                    }

                    while (JudgmentSizeIndex >= 0 && JudgmentSizeIndex < PlayerManager.mapData.Effect.JudgmentSizeEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.JudgmentSizeEffect[JudgmentSizeIndex].Beat)
                    {
                        JudgmentSize = PlayerManager.mapData.Effect.JudgmentSizeEffect[JudgmentSizeIndex].Value;
                        JudgmentSizeIndex++;
                    }

                    while (WindowSizeIndex >= 0 && WindowSizeIndex < PlayerManager.mapData.Effect.WindowSizeEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.WindowSizeEffect[WindowSizeIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.WindowSizeEffect.Count != 0)
                        {
                            PlayerManager.effect.WindowSize = PlayerManager.mapData.Effect.WindowSizeEffect[WindowSizeIndex].Value;
                            PlayerManager.effect.WindowSizeLerp = PlayerManager.mapData.Effect.WindowSizeEffect[WindowSizeIndex].Lerp;
                        }

                        WindowSizeIndex++;

                        if (PlayerManager.effect.WindowSizeLerp == 0 || PlayerManager.effect.WindowSizeLerp == 1)
                            SdjkSystem.WindowSize = PlayerManager.effect.WindowSize;
                    }

                    while (WindowPosIndex >= 0 && WindowPosIndex < PlayerManager.mapData.Effect.WindowPosEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.WindowPosEffect[WindowPosIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.WindowPosEffect.Count != 0)
                        {
                            PlayerManager.effect.WindowPos = PlayerManager.mapData.Effect.WindowPosEffect[WindowPosIndex].Pos;
                            PlayerManager.effect.WindowDatumPoint = PlayerManager.mapData.Effect.WindowPosEffect[WindowPosIndex].WindowDatumPoint;
                            PlayerManager.effect.ScreenDatumPoint = PlayerManager.mapData.Effect.WindowPosEffect[WindowPosIndex].ScreenDatumPoint;
                            PlayerManager.effect.WindowPosLerp = PlayerManager.mapData.Effect.WindowPosEffect[WindowPosIndex].Lerp;
                        }
                        WindowPosIndex++;

                        if (PlayerManager.effect.WindowPosLerp == 0 || PlayerManager.effect.WindowPosLerp == 1)
                            SdjkSystem.WindowPos = JVector2.JVector2ToVector2(PlayerManager.effect.WindowPos);
                    }

                    while (ABarPosIndex >= 0 && ABarPosIndex < PlayerManager.mapData.Effect.ABarPosEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.ABarPosEffect[ABarPosIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.ABarPosEffect.Count != 0)
                        {
                            ABarPos = PlayerManager.mapData.Effect.ABarPosEffect[ABarPosIndex].Value;
                            PlayerManager.effect.ABarPosLerp = PlayerManager.mapData.Effect.ABarPosEffect[ABarPosIndex].Lerp;
                        }

                        ABarPosIndex++;

                        if (PlayerManager.effect.ABarPosLerp == 0 || PlayerManager.effect.ABarPosLerp == 1)
                            PlayerManager.effect.ABarPos = ABarPos;
                    }

                    while (SBarPosIndex >= 0 && SBarPosIndex < PlayerManager.mapData.Effect.SBarPosEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.SBarPosEffect[SBarPosIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.SBarPosEffect.Count != 0)
                        {
                            SBarPos = PlayerManager.mapData.Effect.SBarPosEffect[SBarPosIndex].Value;
                            PlayerManager.effect.SBarPosLerp = PlayerManager.mapData.Effect.SBarPosEffect[SBarPosIndex].Lerp;
                        }

                        SBarPosIndex++;

                        if (PlayerManager.effect.SBarPosLerp == 0 || PlayerManager.effect.SBarPosLerp == 1)
                            PlayerManager.effect.SBarPos = SBarPos;
                    }

                    while (DBarPosIndex >= 0 && DBarPosIndex < PlayerManager.mapData.Effect.DBarPosEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.DBarPosEffect[DBarPosIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.DBarPosEffect.Count != 0)
                        {
                            DBarPos = PlayerManager.mapData.Effect.DBarPosEffect[DBarPosIndex].Value;
                            PlayerManager.effect.DBarPosLerp = PlayerManager.mapData.Effect.DBarPosEffect[DBarPosIndex].Lerp;
                        }

                        DBarPosIndex++;

                        if (PlayerManager.effect.DBarPosLerp == 0 || PlayerManager.effect.DBarPosLerp == 1)
                            PlayerManager.effect.DBarPos = DBarPos;
                    }

                    while (JBarPosIndex >= 0 && JBarPosIndex < PlayerManager.mapData.Effect.JBarPosEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.JBarPosEffect[JBarPosIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.JBarPosEffect.Count != 0)
                        {
                            JBarPos = PlayerManager.mapData.Effect.JBarPosEffect[JBarPosIndex].Value;
                            PlayerManager.effect.JBarPosLerp = PlayerManager.mapData.Effect.JBarPosEffect[JBarPosIndex].Lerp;
                        }

                        JBarPosIndex++;

                        if (PlayerManager.effect.JBarPosLerp == 0 || PlayerManager.effect.JBarPosLerp == 1)
                            PlayerManager.effect.JBarPos = JBarPos;
                    }

                    while (KBarPosIndex >= 0 && KBarPosIndex < PlayerManager.mapData.Effect.KBarPosEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.KBarPosEffect[KBarPosIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.KBarPosEffect.Count != 0)
                        {
                            KBarPos = PlayerManager.mapData.Effect.KBarPosEffect[KBarPosIndex].Value;
                            PlayerManager.effect.KBarPosLerp = PlayerManager.mapData.Effect.KBarPosEffect[KBarPosIndex].Lerp;
                        }

                        KBarPosIndex++;

                        if (PlayerManager.effect.KBarPosLerp == 0 || PlayerManager.effect.KBarPosLerp == 1)
                            PlayerManager.effect.KBarPos = KBarPos;
                    }

                    while (LBarPosIndex >= 0 && LBarPosIndex < PlayerManager.mapData.Effect.LBarPosEffect.Count && PlayerManager.VisibleCurrentBeat >= PlayerManager.mapData.Effect.LBarPosEffect[LBarPosIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.LBarPosEffect.Count != 0)
                        {
                            LBarPos = PlayerManager.mapData.Effect.LBarPosEffect[LBarPosIndex].Value;
                            PlayerManager.effect.LBarPosLerp = PlayerManager.mapData.Effect.LBarPosEffect[LBarPosIndex].Lerp;
                        }

                        LBarPosIndex++;

                        if (PlayerManager.effect.LBarPosLerp == 0 || PlayerManager.effect.LBarPosLerp == 1)
                            PlayerManager.effect.LBarPos = LBarPos;
                    }
                }
                else
                {
                    while (CameraPosIndex >= 0 && CameraPosIndex < PlayerManager.mapData.Effect.Camera.CameraPosEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.Camera.CameraPosEffect[CameraPosIndex].Beat)
                    {
                        PlayerManager.effect.CameraPos = PlayerManager.mapData.Effect.Camera.CameraPosEffect[CameraPosIndex].Value;
                        PlayerManager.effect.CameraPosLerp = PlayerManager.mapData.Effect.Camera.CameraPosEffect[CameraPosIndex].Lerp;
                        CameraPosIndex--;

                        if (PlayerManager.effect.CameraPosLerp == 0 || PlayerManager.effect.CameraPosLerp == 1)
                            MainCamera.CameraPos = JVector3.JVector3ToVector3(PlayerManager.effect.CameraPos);
                    }

                    while (UiPosIndex >= 0 && UiPosIndex < PlayerManager.mapData.Effect.Camera.UiPosEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.Camera.UiPosEffect[UiPosIndex].Beat)
                    {
                        PlayerManager.effect.UiPos = PlayerManager.mapData.Effect.Camera.UiPosEffect[UiPosIndex].Value;
                        PlayerManager.effect.UiPosLerp = PlayerManager.mapData.Effect.Camera.UiPosEffect[UiPosIndex].Lerp;
                        UiPosIndex--;

                        if (PlayerManager.effect.UiPosLerp == 0 || PlayerManager.effect.UiPosLerp == 1)
                            MainCamera.UiPos = JVector3.JVector3ToVector3(PlayerManager.effect.UiPos);
                    }

                    while (CameraZoomIndex >= 0 && CameraZoomIndex < PlayerManager.mapData.Effect.Camera.CameraZoomEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.Camera.CameraZoomEffect[CameraZoomIndex].Beat)
                    {
                        PlayerManager.effect.CameraZoom = PlayerManager.mapData.Effect.Camera.CameraZoomEffect[CameraZoomIndex].Value;
                        PlayerManager.effect.CameraZoomLerp = PlayerManager.mapData.Effect.Camera.CameraZoomEffect[CameraZoomIndex].Lerp;
                        CameraZoomIndex--;

                        if (PlayerManager.effect.CameraZoomLerp == 0 || PlayerManager.effect.CameraZoomLerp == 1)
                            MainCamera.CameraZoom = PlayerManager.effect.CameraZoom;
                    }

                    while (UiZoomIndex >= 0 && UiZoomIndex < PlayerManager.mapData.Effect.Camera.UiZoomEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.Camera.UiZoomEffect[UiZoomIndex].Beat)
                    {
                        if (!GameManager.Ratio_9_16)
                            PlayerManager.effect.UiZoom = PlayerManager.mapData.Effect.Camera.UiZoomEffect[UiZoomIndex].Value;
                        else
                            PlayerManager.effect.UiZoom = PlayerManager.mapData.Effect.Camera.UiZoomEffect[UiZoomIndex].Value + 0.5f;
                        PlayerManager.effect.UiZoomLerp = PlayerManager.mapData.Effect.Camera.UiZoomEffect[UiZoomIndex].Lerp;
                        UiZoomIndex--;

                        if (PlayerManager.effect.UiZoomLerp == 0 || PlayerManager.effect.UiZoomLerp == 1)
                            MainCamera.UiZoom = PlayerManager.effect.UiZoom;
                    }

                    while (PitchIndex >= 0 && PitchIndex < PlayerManager.mapData.Effect.PitchEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.PitchEffect[PitchIndex].Beat)
                    {
                        Pitch = PlayerManager.mapData.Effect.PitchEffect[PitchIndex].Value;
                        PlayerManager.effect.PitchLerp = PlayerManager.mapData.Effect.PitchEffect[PitchIndex].Lerp;
                        PitchIndex--;

                        if (PlayerManager.effect.PitchLerp == 0 || PlayerManager.effect.PitchLerp == 1)
                            PlayerManager.effect.Pitch = Pitch;
                    }


                    while (BeatYPosIndex >= 0 && BeatYPosIndex < PlayerManager.mapData.Effect.BeatYPosEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.BeatYPosEffect[BeatYPosIndex].Beat)
                    {
                        PlayerManager.effect.Volume = PlayerManager.mapData.Effect.VolumeEffect[VolumeIndex].Value * GameManager.MainVolume;
                        PlayerManager.effect.VolumeLerp = PlayerManager.mapData.Effect.VolumeEffect[VolumeIndex].Lerp;
                        BeatYPosIndex--;

                        if (PlayerManager.effect.BeatYPosLerp == 0 || PlayerManager.effect.BeatYPosLerp == 1)
                            PlayerManager.effect.BeatYPos = BeatYPos;
                    }

                    while (VolumeIndex >= 0 && VolumeIndex < PlayerManager.mapData.Effect.VolumeEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.VolumeEffect[VolumeIndex].Beat)
                    {
                        PlayerManager.effect.Volume = PlayerManager.mapData.Effect.VolumeEffect[VolumeIndex].Value * GameManager.MainVolume;
                        PlayerManager.effect.VolumeLerp = PlayerManager.mapData.Effect.VolumeEffect[VolumeIndex].Lerp;
                        VolumeIndex--;

                        if (PlayerManager.effect.PitchLerp == 0 || PlayerManager.effect.PitchLerp == 1)
                            PlayerManager.playerManager.audioSource.volume = (float)PlayerManager.effect.Volume;
                    }

                    while (HPAddValueIndex >= 0 && HPAddValueIndex < PlayerManager.mapData.Effect.HPAddValueEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.HPAddValueEffect[HPAddValueIndex].Beat)
                    {
                        HPAddValue = PlayerManager.mapData.Effect.HPAddValueEffect[HPAddValueIndex].Value;
                        PlayerManager.effect.HPAddValueLerp = PlayerManager.mapData.Effect.HPAddValueEffect[HPAddValueIndex].Lerp;
                        HPAddValueIndex--;

                        if (PlayerManager.effect.HPAddValueLerp == 0 || PlayerManager.effect.HPAddValueLerp == 1)
                            PlayerManager.effect.HPAddValue = HPAddValue;
                    }
                    while (HPRemoveIndex >= 0 && HPRemoveIndex < PlayerManager.mapData.Effect.HPRemoveEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.HPRemoveEffect[HPRemoveIndex].Beat)
                    {
                        HPRemove = PlayerManager.mapData.Effect.HPRemoveEffect[HPRemoveIndex].Value;
                        HPRemoveIndex--;
                    }
                    while (HPRemoveValueIndex >= 0 && HPRemoveValueIndex < PlayerManager.mapData.Effect.HPRemoveValueEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.HPRemoveValueEffect[HPRemoveValueIndex].Beat)
                    {
                        HPRemoveValue = PlayerManager.mapData.Effect.HPRemoveValueEffect[HPRemoveValueIndex].Value;
                        PlayerManager.effect.HPRemoveValueLerp = PlayerManager.mapData.Effect.HPRemoveValueEffect[HPRemoveValueIndex].Lerp;
                        HPRemoveValueIndex--;

                        if (PlayerManager.effect.HPRemoveValueLerp == 0 || PlayerManager.effect.HPRemoveValueLerp == 1)
                            PlayerManager.effect.HPRemoveValue = HPRemoveValue;
                    }
                    while (MaxHPValueIndex >= 0 && MaxHPValueIndex < PlayerManager.mapData.Effect.MaxHPValueEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.MaxHPValueEffect[MaxHPValueIndex].Beat)
                    {
                        MaxHPValue = PlayerManager.mapData.Effect.MaxHPValueEffect[MaxHPValueIndex].Value;
                        PlayerManager.effect.MaxHPValueLerp = PlayerManager.mapData.Effect.MaxHPValueEffect[MaxHPValueIndex].Lerp;
                        MaxHPValueIndex--;
                    }

                    while (JudgmentSizeIndex >= 0 && JudgmentSizeIndex < PlayerManager.mapData.Effect.JudgmentSizeEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.JudgmentSizeEffect[JudgmentSizeIndex].Beat)
                    {
                        JudgmentSize = PlayerManager.mapData.Effect.JudgmentSizeEffect[JudgmentSizeIndex].Value;
                        JudgmentSizeIndex--;
                    }

                    while (WindowSizeIndex >= 0 && WindowSizeIndex < PlayerManager.mapData.Effect.WindowSizeEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.WindowSizeEffect[WindowSizeIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.WindowSizeEffect.Count != 0)
                        {
                            PlayerManager.effect.WindowSize = PlayerManager.mapData.Effect.WindowSizeEffect[WindowSizeIndex].Value;
                            PlayerManager.effect.WindowSizeLerp = PlayerManager.mapData.Effect.WindowSizeEffect[WindowSizeIndex].Lerp;
                        }
                        WindowSizeIndex--;

                        if (PlayerManager.effect.WindowSizeLerp == 0 || PlayerManager.effect.WindowSizeLerp == 1)
                            SdjkSystem.WindowSize = PlayerManager.effect.WindowSize;
                    }

                    while (WindowPosIndex >= 0 && WindowPosIndex < PlayerManager.mapData.Effect.WindowPosEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.WindowPosEffect[WindowPosIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.WindowPosEffect.Count != 0)
                        {
                            PlayerManager.effect.WindowPos = PlayerManager.mapData.Effect.WindowPosEffect[WindowPosIndex].Pos;
                            PlayerManager.effect.WindowDatumPoint = PlayerManager.mapData.Effect.WindowPosEffect[WindowPosIndex].WindowDatumPoint;
                            PlayerManager.effect.ScreenDatumPoint = PlayerManager.mapData.Effect.WindowPosEffect[WindowPosIndex].ScreenDatumPoint;
                            PlayerManager.effect.WindowPosLerp = PlayerManager.mapData.Effect.WindowPosEffect[WindowPosIndex].Lerp;
                        }
                        WindowPosIndex--;

                        if (PlayerManager.effect.WindowPosLerp == 0 || PlayerManager.effect.WindowPosLerp == 1)
                            SdjkSystem.WindowPos = JVector2.JVector2ToVector2(PlayerManager.effect.WindowPos);
                    }

                    while (ABarPosIndex >= 0 && ABarPosIndex < PlayerManager.mapData.Effect.ABarPosEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.ABarPosEffect[ABarPosIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.ABarPosEffect.Count != 0)
                        {
                            ABarPos = PlayerManager.mapData.Effect.ABarPosEffect[ABarPosIndex].Value;
                            PlayerManager.effect.ABarPosLerp = PlayerManager.mapData.Effect.ABarPosEffect[ABarPosIndex].Lerp;
                        }

                        ABarPosIndex--;

                        if (PlayerManager.effect.ABarPosLerp == 0 || PlayerManager.effect.ABarPosLerp == 1)
                            PlayerManager.effect.ABarPos = ABarPos;
                    }

                    while (SBarPosIndex >= 0 && SBarPosIndex < PlayerManager.mapData.Effect.SBarPosEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.SBarPosEffect[SBarPosIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.SBarPosEffect.Count != 0)
                        {
                            SBarPos = PlayerManager.mapData.Effect.SBarPosEffect[SBarPosIndex].Value;
                            PlayerManager.effect.SBarPosLerp = PlayerManager.mapData.Effect.SBarPosEffect[SBarPosIndex].Lerp;
                        }

                        SBarPosIndex--;

                        if (PlayerManager.effect.SBarPosLerp == 0 || PlayerManager.effect.SBarPosLerp == 1)
                            PlayerManager.effect.SBarPos = SBarPos;
                    }

                    while (DBarPosIndex >= 0 && DBarPosIndex < PlayerManager.mapData.Effect.DBarPosEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.DBarPosEffect[DBarPosIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.DBarPosEffect.Count != 0)
                        {
                            DBarPos = PlayerManager.mapData.Effect.DBarPosEffect[DBarPosIndex].Value;
                            PlayerManager.effect.DBarPosLerp = PlayerManager.mapData.Effect.DBarPosEffect[DBarPosIndex].Lerp;
                        }

                        DBarPosIndex--;

                        if (PlayerManager.effect.DBarPosLerp == 0 || PlayerManager.effect.DBarPosLerp == 1)
                            PlayerManager.effect.DBarPos = DBarPos;
                    }

                    while (JBarPosIndex >= 0 && JBarPosIndex < PlayerManager.mapData.Effect.JBarPosEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.JBarPosEffect[JBarPosIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.JBarPosEffect.Count != 0)
                        {
                            JBarPos = PlayerManager.mapData.Effect.JBarPosEffect[JBarPosIndex].Value;
                            PlayerManager.effect.JBarPosLerp = PlayerManager.mapData.Effect.JBarPosEffect[JBarPosIndex].Lerp;
                        }

                        JBarPosIndex--;

                        if (PlayerManager.effect.JBarPosLerp == 0 || PlayerManager.effect.JBarPosLerp == 1)
                            PlayerManager.effect.JBarPos = JBarPos;
                    }

                    while (KBarPosIndex >= 0 && KBarPosIndex < PlayerManager.mapData.Effect.KBarPosEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.KBarPosEffect[KBarPosIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.KBarPosEffect.Count != 0)
                        {
                            KBarPos = PlayerManager.mapData.Effect.KBarPosEffect[KBarPosIndex].Value;
                            PlayerManager.effect.KBarPosLerp = PlayerManager.mapData.Effect.KBarPosEffect[KBarPosIndex].Lerp;
                        }

                        KBarPosIndex--;

                        if (PlayerManager.effect.KBarPosLerp == 0 || PlayerManager.effect.KBarPosLerp == 1)
                            PlayerManager.effect.KBarPos = KBarPos;
                    }

                    while (LBarPosIndex >= 0 && LBarPosIndex < PlayerManager.mapData.Effect.LBarPosEffect.Count && PlayerManager.VisibleCurrentBeat <= PlayerManager.mapData.Effect.LBarPosEffect[LBarPosIndex].Beat)
                    {
                        if (PlayerManager.mapData.Effect.LBarPosEffect.Count != 0)
                        {
                            LBarPos = PlayerManager.mapData.Effect.LBarPosEffect[LBarPosIndex].Value;
                            PlayerManager.effect.LBarPosLerp = PlayerManager.mapData.Effect.LBarPosEffect[LBarPosIndex].Lerp;
                        }

                        LBarPosIndex--;

                        if (PlayerManager.effect.LBarPosLerp == 0 || PlayerManager.effect.LBarPosLerp == 1)
                            PlayerManager.effect.LBarPos = LBarPos;
                    }
                }

                EffectPlay();
            }

            if (PlayerManager.playerManager.audioSource.pitch >= 0)
            {
                if (BPMIndex < 0)
                    BPMIndex = 0;
                while (BPMIndex < PlayerManager.mapData.Effect.BPMEffect.Count && PlayerManager.HitSoundCurrentBeat >= PlayerManager.mapData.Effect.BPMEffect[BPMIndex].Beat)
                {
                    if (BPMIndex >= 0)
                        PlayerManager.BPMChange(PlayerManager.mapData.Effect.BPMEffect[BPMIndex].Value, PlayerManager.mapData.Effect.BPMEffect[BPMIndex].Beat);

                    BPMIndex++;
                }
            }
            else
            {
                if (BPMIndex >= PlayerManager.mapData.Effect.BPMEffect.Count)
                    BPMIndex = PlayerManager.mapData.Effect.BPMEffect.Count - 1;
                while (BPMIndex >= 0 && PlayerManager.HitSoundCurrentBeat <= PlayerManager.mapData.Effect.BPMEffect[BPMIndex].Beat)
                {
                    BPMIndex--;

                    if (BPMIndex >= 0 && BPMIndex < PlayerManager.mapData.Effect.BPMEffect.Count)
                        PlayerManager.BPMChange(PlayerManager.mapData.Effect.BPMEffect[BPMIndex].Value, PlayerManager.mapData.Effect.BPMEffect[BPMIndex].Beat);
                    else
                        PlayerManager.BPMChange(PlayerManager.mapData.Effect.BPM, 0);
                }
            }
        }

        public static void EffectReset()
        {
            PlayerManager.effect.CameraPos = PlayerManager.mapData.Effect.Camera.CameraPos;
            PlayerManager.effect.CameraPosLerp = 1;
            PlayerManager.effect.UiPos = PlayerManager.mapData.Effect.Camera.UiPos;
            PlayerManager.effect.UiPosLerp = 1;
            PlayerManager.effect.CameraZoom = PlayerManager.mapData.Effect.Camera.CameraZoom;
            PlayerManager.effect.CameraZoomLerp = 1;
            if (!GameManager.Ratio_9_16)
                PlayerManager.effect.UiZoom = PlayerManager.mapData.Effect.Camera.UiZoom;
            else
                PlayerManager.effect.UiZoom = PlayerManager.mapData.Effect.Camera.UiZoom + 0.5f;
            PlayerManager.effect.UiZoomLerp = 1;
            Pitch = PlayerManager.mapData.Effect.Pitch;
            PlayerManager.effect.PitchLerp = 1;
            BeatYPos = PlayerManager.mapData.Effect.BeatYPos;
            PlayerManager.effect.BeatYPosLerp = 1;
            PlayerManager.effect.Volume = PlayerManager.mapData.Effect.Volume * GameManager.MainVolume;
            PlayerManager.effect.VolumeLerp = 1;
            HPAddValue = PlayerManager.mapData.Effect.HPAddValue;
            HPRemove = PlayerManager.mapData.Effect.HPRemove;
            HPRemoveValue = PlayerManager.mapData.Effect.HPRemoveValue;
            MaxHPValue = PlayerManager.mapData.Effect.MaxHPValue;
            JudgmentSize = PlayerManager.mapData.Effect.JudgmentSize;
            PlayerManager.effect.WindowSize = PlayerManager.mapData.Effect.WindowSize;
            PlayerManager.effect.WindowSizeLerp = 1;
            PlayerManager.effect.WindowPos = PlayerManager.mapData.Effect.WindowPos;
            PlayerManager.effect.WindowPosLerp = 1;
            PlayerManager.effect.WindowDatumPoint = PlayerManager.mapData.Effect.WindowDatumPoint;
            PlayerManager.effect.ScreenDatumPoint = PlayerManager.mapData.Effect.ScreenDatumPoint;
            SdjkSystem.tempWindowSize = 0;
            SdjkSystem.tempWindowPos = Vector2.zero;
            SdjkSystem.tempScreenDatumPoint = 0;
            SdjkSystem.tempWindowDatumPoint = 0;
            ABarPos = PlayerManager.mapData.Effect.ABarPos;
            SBarPos = PlayerManager.mapData.Effect.SBarPos;
            DBarPos = PlayerManager.mapData.Effect.DBarPos;
            JBarPos = PlayerManager.mapData.Effect.JBarPos;
            KBarPos = PlayerManager.mapData.Effect.KBarPos;
            LBarPos = PlayerManager.mapData.Effect.LBarPos;
            PlayerManager.effect.ABarPosLerp = 1;
            PlayerManager.effect.SBarPosLerp = 1;
            PlayerManager.effect.DBarPosLerp = 1;
            PlayerManager.effect.JBarPosLerp = 1;
            PlayerManager.effect.KBarPosLerp = 1;
            PlayerManager.effect.LBarPosLerp = 1;



            EffectAllIndexSet(0);
            EffectPlay();
        }

        public static void EffectAllIndexSet(int value)
        {
            CameraPosIndex = value;
            UiPosIndex = value;
            CameraZoomIndex = value;
            UiZoomIndex = value;
            PitchIndex = value;
            BeatYPosIndex = value;
            VolumeIndex = value;
            HPAddValueIndex = value;
            HPRemoveIndex = value;
            HPRemoveValueIndex = value;
            MaxHPValueIndex = value;
            BPMIndex = value;
            JudgmentSizeIndex = value;
            WindowSizeIndex = value;
            WindowPosIndex = value;
            ABarPosIndex = value;
            SBarPosIndex = value;
            DBarPosIndex = value;
            JBarPosIndex = value;
            KBarPosIndex = value;
            LBarPosIndex = value;
        }

        public static void EffectIndexReset()
        {
            if (PlayerManager.mapData.Effect.AllBeat.Count == 0 || PlayerManager.VisibleCurrentBeat < PlayerManager.mapData.Effect.AllBeat[0])
            {
                EffectReset();
                return;
            }

            if (CameraPosIndex < 0)
            {
                CameraPosIndex = 0;
                PlayerManager.effect.CameraPos = PlayerManager.mapData.Effect.Camera.CameraPos;
                PlayerManager.effect.CameraPosLerp = 1;
            }
            if (UiPosIndex < 0)
            {
                UiPosIndex = 0;
                PlayerManager.effect.UiPos = PlayerManager.mapData.Effect.Camera.UiPos;
                PlayerManager.effect.UiPosLerp = 1;
            }
            if (CameraZoomIndex < 0)
            {
                CameraZoomIndex = 0;
                PlayerManager.effect.CameraZoom = PlayerManager.mapData.Effect.Camera.CameraZoom;
                PlayerManager.effect.CameraZoomLerp = 1;
            }
            if (UiZoomIndex < 0)
            {
                UiZoomIndex = 0;
                if (!GameManager.Ratio_9_16)
                    PlayerManager.effect.UiZoom = PlayerManager.mapData.Effect.Camera.UiZoom;
                else
                    PlayerManager.effect.UiZoom = PlayerManager.mapData.Effect.Camera.UiZoom + 0.5f;
                PlayerManager.effect.UiZoomLerp = 1;
            }
            if (PitchIndex < 0)
            {
                PitchIndex = 0;
                Pitch = PlayerManager.mapData.Effect.Pitch;
                PlayerManager.effect.PitchLerp = 1;
            }
            if (BeatYPosIndex < 0)
            {
                BeatYPosIndex = 0;
                BeatYPos = PlayerManager.mapData.Effect.BeatYPos;
                PlayerManager.effect.BeatYPosLerp = 1;
            }
            if (VolumeIndex < 0)
            {
                VolumeIndex = 0;
                PlayerManager.effect.Volume = PlayerManager.mapData.Effect.Volume;
                PlayerManager.effect.VolumeLerp = 1;
            }
            if (HPAddValueIndex < 0)
            {
                HPAddValueIndex = 0;
                PlayerManager.effect.HPAddValue = PlayerManager.mapData.Effect.HPAddValue;
            }
            if (HPRemoveValueIndex < 0)
            {
                HPRemoveIndex = 0;
                PlayerManager.effect.HPRemove = PlayerManager.mapData.Effect.HPRemove;
            }
            if (HPRemoveValueIndex < 0)
            {
                HPRemoveValueIndex = 0;
                PlayerManager.effect.HPRemoveValue = PlayerManager.mapData.Effect.HPRemoveValue;
            }
            if (MaxHPValueIndex < 0)
            {
                MaxHPValue = 0;
                PlayerManager.effect.MaxHPValue = PlayerManager.mapData.Effect.MaxHPValue;
            }
            if (JudgmentSizeIndex < 0)
            {
                JudgmentSizeIndex = 0;
                PlayerManager.effect.JudgmentSize = PlayerManager.mapData.Effect.JudgmentSize;
            }
            if (WindowSizeIndex < 0)
            {
                WindowSizeIndex = 0;
                PlayerManager.effect.WindowSize = PlayerManager.mapData.Effect.WindowSize;
            }
            if (WindowPosIndex < 0)
            {
                WindowPosIndex = 0;
                PlayerManager.effect.WindowPos = PlayerManager.mapData.Effect.WindowPos;
                PlayerManager.effect.WindowDatumPoint = PlayerManager.mapData.Effect.WindowDatumPoint;
                PlayerManager.effect.ScreenDatumPoint = PlayerManager.mapData.Effect.ScreenDatumPoint;
            }
            if (ABarPosIndex < 0)
            {
                ABarPosIndex = 0;
                PlayerManager.effect.ABarPos = PlayerManager.mapData.Effect.ABarPos;
            }
            if (SBarPosIndex < 0)
            {
                SBarPosIndex = 0;
                PlayerManager.effect.SBarPos = PlayerManager.mapData.Effect.SBarPos;
            }
            if (DBarPosIndex < 0)
            {
                DBarPosIndex = 0;
                PlayerManager.effect.DBarPos = PlayerManager.mapData.Effect.DBarPos;
            }
            if (JBarPosIndex < 0)
            {
                JBarPosIndex = 0;
                PlayerManager.effect.JBarPos = PlayerManager.mapData.Effect.JBarPos;
            }
            if (KBarPosIndex < 0)
            {
                KBarPosIndex = 0;
                PlayerManager.effect.KBarPos = PlayerManager.mapData.Effect.KBarPos;
            }
            if (LBarPosIndex < 0)
            {
                LBarPosIndex = 0;
                PlayerManager.effect.LBarPos = PlayerManager.mapData.Effect.LBarPos;
            }

            if (CameraPosIndex >= PlayerManager.mapData.Effect.Camera.CameraPosEffect.Count)
                CameraPosIndex = PlayerManager.mapData.Effect.Camera.CameraPosEffect.Count - 1;
            if (UiPosIndex >= PlayerManager.mapData.Effect.Camera.UiPosEffect.Count)
                UiPosIndex = PlayerManager.mapData.Effect.Camera.UiPosEffect.Count - 1;
            if (CameraZoomIndex >= PlayerManager.mapData.Effect.Camera.CameraZoomEffect.Count)
                CameraZoomIndex = PlayerManager.mapData.Effect.Camera.CameraZoomEffect.Count - 1;
            if (UiZoomIndex >= PlayerManager.mapData.Effect.Camera.UiZoomEffect.Count)
                UiZoomIndex = PlayerManager.mapData.Effect.Camera.UiZoomEffect.Count - 1;
            if (PitchIndex >= PlayerManager.mapData.Effect.PitchEffect.Count)
                PitchIndex = PlayerManager.mapData.Effect.PitchEffect.Count - 1;
            if (BeatYPosIndex >= PlayerManager.mapData.Effect.BeatYPosEffect.Count)
                BeatYPosIndex = PlayerManager.mapData.Effect.BeatYPosEffect.Count - 1;
            if (VolumeIndex >= PlayerManager.mapData.Effect.VolumeEffect.Count)
                VolumeIndex = PlayerManager.mapData.Effect.VolumeEffect.Count - 1;
            if (HPAddValueIndex >= PlayerManager.mapData.Effect.HPAddValueEffect.Count)
                HPAddValueIndex = PlayerManager.mapData.Effect.HPAddValueEffect.Count - 1;
            if (HPRemoveIndex >= PlayerManager.mapData.Effect.HPRemoveEffect.Count)
                HPRemoveIndex = PlayerManager.mapData.Effect.HPRemoveEffect.Count - 1;
            if (HPRemoveValueIndex >= PlayerManager.mapData.Effect.HPRemoveValueEffect.Count)
                HPRemoveValueIndex = PlayerManager.mapData.Effect.HPRemoveValueEffect.Count - 1;
            if (MaxHPValueIndex >= PlayerManager.mapData.Effect.MaxHPValueEffect.Count)
                MaxHPValueIndex = PlayerManager.mapData.Effect.MaxHPValueEffect.Count - 1;
            if (JudgmentSizeIndex >= PlayerManager.mapData.Effect.JudgmentSizeEffect.Count)
                JudgmentSizeIndex = PlayerManager.mapData.Effect.JudgmentSizeEffect.Count - 1;
            if (WindowSizeIndex >= PlayerManager.mapData.Effect.WindowSizeEffect.Count)
                WindowSizeIndex = PlayerManager.mapData.Effect.WindowSizeEffect.Count - 1;
            if (WindowPosIndex >= PlayerManager.mapData.Effect.WindowPosEffect.Count)
                WindowPosIndex = PlayerManager.mapData.Effect.WindowPosEffect.Count - 1;
            if (ABarPosIndex >= PlayerManager.mapData.Effect.ABarPosEffect.Count)
                ABarPosIndex = PlayerManager.mapData.Effect.ABarPosEffect.Count - 1;
            if (SBarPosIndex >= PlayerManager.mapData.Effect.SBarPosEffect.Count)
                SBarPosIndex = PlayerManager.mapData.Effect.SBarPosEffect.Count - 1;
            if (DBarPosIndex >= PlayerManager.mapData.Effect.DBarPosEffect.Count)
                DBarPosIndex = PlayerManager.mapData.Effect.DBarPosEffect.Count - 1;
            if (JBarPosIndex >= PlayerManager.mapData.Effect.JBarPosEffect.Count)
                JBarPosIndex = PlayerManager.mapData.Effect.JBarPosEffect.Count - 1;
            if (KBarPosIndex >= PlayerManager.mapData.Effect.KBarPosEffect.Count)
                KBarPosIndex = PlayerManager.mapData.Effect.KBarPosEffect.Count - 1;
            if (LBarPosIndex >= PlayerManager.mapData.Effect.LBarPosEffect.Count)
                LBarPosIndex = PlayerManager.mapData.Effect.LBarPosEffect.Count - 1;
        }

        static void EffectPlay()
        {
            if (PlayerManager.effect.CameraPosLerp != 0 && PlayerManager.effect.CameraPosLerp != 1)
                MainCamera.CameraPos = Vector3.Lerp(MainCamera.CameraPos, JVector3.JVector3ToVector3(PlayerManager.effect.CameraPos), (float)(PlayerManager.effect.CameraPosLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch)));
            else
                MainCamera.CameraPos = JVector3.JVector3ToVector3(PlayerManager.effect.CameraPos);

            if (PlayerManager.effect.UiPosLerp != 0 && PlayerManager.effect.CameraZoomLerp != 1)
                MainCamera.UiPos = Vector3.Lerp(MainCamera.UiPos, JVector3.JVector3ToVector3(PlayerManager.effect.UiPos), (float)(PlayerManager.effect.UiPosLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch)));
            else
                MainCamera.UiPos = JVector3.JVector3ToVector3(PlayerManager.effect.UiPos);

            if (PlayerManager.effect.CameraZoomLerp != 0 && PlayerManager.effect.CameraZoomLerp != 1)
                MainCamera.CameraZoom = GameManager.Lerp(MainCamera.CameraZoom, PlayerManager.effect.CameraZoom, PlayerManager.effect.CameraZoomLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch));
            else
                MainCamera.CameraZoom =  PlayerManager.effect.CameraZoom;

            if (PlayerManager.effect.UiZoomLerp != 0 && PlayerManager.effect.UiZoomLerp != 1) 
                MainCamera.UiZoom = GameManager.Lerp(MainCamera.UiZoom, PlayerManager.effect.UiZoom, PlayerManager.effect.UiZoomLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch));
            else
                MainCamera.UiZoom =  PlayerManager.effect.UiZoom;

            if (PlayerManager.effect.PitchLerp != 0 && PlayerManager.effect.PitchLerp != 1)
                PlayerManager.effect.Pitch = GameManager.Lerp(PlayerManager.effect.Pitch, Pitch, PlayerManager.effect.PitchLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch));
            else
                PlayerManager.effect.Pitch = Pitch;

            if (PlayerManager.effect.BeatYPosLerp != 0 && PlayerManager.effect.BeatYPosLerp != 1)
                PlayerManager.effect.BeatYPos = GameManager.Lerp(PlayerManager.effect.BeatYPos,  BeatYPos,  PlayerManager.effect.BeatYPosLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch));
            else
                PlayerManager.effect.BeatYPos = BeatYPos;

            if (GameManager.UpScroll)
                PlayerManager.effect.BeatYPos = -PlayerManager.effect.BeatYPos;

            if (PlayerManager.effect.VolumeLerp != 0 && PlayerManager.effect.VolumeLerp != 1)
                PlayerManager.playerManager.audioSource.volume = (float)GameManager.Lerp(PlayerManager.playerManager.audioSource.volume, PlayerManager.effect.Volume,  (PlayerManager.effect.VolumeLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch)));
            else
                PlayerManager.playerManager.audioSource.volume = (float)PlayerManager.effect.Volume;
            
            if (PlayerManager.effect.HPAddValueLerp != 0 && PlayerManager.effect.HPAddValueLerp != 1)
                PlayerManager.effect.HPAddValue = GameManager.Lerp(PlayerManager.effect.HPAddValue, HPAddValue, PlayerManager.effect.HPAddValueLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch));
            else
                PlayerManager.effect.HPAddValue = HPAddValue;

            PlayerManager.effect.HPRemove = HPRemove;

            if (PlayerManager.effect.HPRemoveValueLerp != 0 && PlayerManager.effect.HPRemoveValueLerp != 1)
                PlayerManager.effect.HPRemoveValue = GameManager.Lerp(PlayerManager.effect.HPRemoveValue, HPRemoveValue, PlayerManager.effect.HPRemoveValueLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch));
            else
                PlayerManager.effect.HPRemoveValue = HPRemoveValue;

            if (PlayerManager.effect.MaxHPValueLerp != 0 && PlayerManager.effect.MaxHPValueLerp != 1)
                PlayerManager.effect.MaxHPValue = GameManager.Lerp(PlayerManager.effect.MaxHPValue, MaxHPValue, PlayerManager.effect.MaxHPValueLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch));
            else
                PlayerManager.effect.MaxHPValue = MaxHPValue;

            PlayerManager.effect.JudgmentSize = JudgmentSize;

            if (PlayerManager.effect.WindowSizeLerp != 0 && PlayerManager.effect.WindowSizeLerp != 1)
                SdjkSystem.WindowSize = GameManager.Lerp(SdjkSystem.WindowSize, PlayerManager.effect.WindowSize, PlayerManager.effect.WindowSizeLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch));
            else
                SdjkSystem.WindowSize = PlayerManager.effect.WindowSize;

            SdjkSystem.WindowPos = JVector2.JVector2ToVector2(PlayerManager.effect.WindowPos);
            SdjkSystem.WindowDatumPoint = PlayerManager.effect.WindowDatumPoint;
            SdjkSystem.ScreenDatumPoint = PlayerManager.effect.ScreenDatumPoint;

            if (PlayerManager.effect.ABarPosLerp != 0 && PlayerManager.effect.ABarPosLerp != 1)
                PlayerManager.effect.ABarPos = JVector3.Lerp(PlayerManager.effect.ABarPos, ABarPos, (float)(PlayerManager.effect.ABarPosLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch)));
            else
                PlayerManager.effect.ABarPos = ABarPos;

            if (PlayerManager.effect.SBarPosLerp != 0 && PlayerManager.effect.SBarPosLerp != 1)
                PlayerManager.effect.SBarPos = JVector3.Lerp(PlayerManager.effect.SBarPos, SBarPos, (float)(PlayerManager.effect.SBarPosLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch)));
            else
                PlayerManager.effect.SBarPos = SBarPos;

            if (PlayerManager.effect.DBarPosLerp != 0 && PlayerManager.effect.DBarPosLerp != 1)
                PlayerManager.effect.DBarPos = JVector3.Lerp(PlayerManager.effect.DBarPos, DBarPos, (float)(PlayerManager.effect.DBarPosLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch)));
            else
                PlayerManager.effect.DBarPos = DBarPos;

            if (PlayerManager.effect.JBarPosLerp != 0 && PlayerManager.effect.JBarPosLerp != 1)
                PlayerManager.effect.JBarPos = JVector3.Lerp(PlayerManager.effect.JBarPos, JBarPos, (float)(PlayerManager.effect.JBarPosLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch)));
            else
                PlayerManager.effect.JBarPos = JBarPos;

            if (PlayerManager.effect.KBarPosLerp != 0 && PlayerManager.effect.KBarPosLerp != 1)
                PlayerManager.effect.KBarPos = JVector3.Lerp(PlayerManager.effect.KBarPos, KBarPos, (float)(PlayerManager.effect.KBarPosLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch)));
            else
                PlayerManager.effect.KBarPos = KBarPos;

            if (PlayerManager.effect.LBarPosLerp != 0 && PlayerManager.effect.LBarPosLerp != 1)
                PlayerManager.effect.LBarPos = JVector3.Lerp(PlayerManager.effect.LBarPos, LBarPos, (float)(PlayerManager.effect.LBarPosLerp * GameManager.FpsDeltaTime * GameManager.Abs(PlayerManager.playerManager.audioSource.pitch)));
            else
                PlayerManager.effect.LBarPos = LBarPos;
        }
    }
}