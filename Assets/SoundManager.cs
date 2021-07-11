using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDJK.Sound
{
    public class SoundManager : MonoBehaviour
    {
        public GameObject SoundPrefab;
        public GameObject _BGM;
        public GameObject _Sound;

        static GameObject soundPrefab;

        public static GameObject soundManager;
        static GameObject BGM;
        static GameObject Sound;

        public static int BGMCount = 0;
        public static int SoundCount = 0;

        [HideInInspector]
        public AudioClip[] BGMLoadAll;

        void OnEnable()
        {
            soundPrefab = SoundPrefab;

            soundManager = gameObject;
            BGM = _BGM;
            Sound = _Sound;

            BGMLoadAll = Resources.LoadAll<AudioClip>("BGM");
        }

        void Update()
        {
            soundManager = gameObject;
            _BGM = BGM;
            _Sound = Sound;
        }

        /// <summary>
        /// Play BGM
        /// </summary>
        public static void PlayBGM(string BGMName, bool Loop, float Vol, float Pitch, bool RhythmPitchUse, bool Pade)
        {
            if (BGMCount < 10)
            {
                BGMCount += 1;

                GameObject BGMObject = Instantiate(soundPrefab, SoundManager.BGM.transform);
                BGMObject.name = "BGM." + BGMName.Replace(".", "/");
                SoundPrefab SoundPrefab = BGMObject.GetComponent<SoundPrefab>();
                SoundPrefab.BGM = true;
                SoundPrefab.RhythmPitchUse = RhythmPitchUse;
                SoundPrefab.Vol = Vol;
                SoundPrefab.Pitch = Pitch;
                SoundPrefab.Loop = Loop;
                string NameSpace = ResourcesManager.GetStringNameSpace(BGMName, out string ResourceName);
                if (NameSpace == "")
                    NameSpace = "sdjk";
                SoundPrefab.NameSpace = NameSpace;
                SoundPrefab.AudioName = ResourcesManager.BGMPath + ResourceName;

                AudioSource BGM = SoundPrefab.audioSource;
                if (Pade)
                    SoundPrefab.PadeIn = true;
                else
                    BGM.volume = GameManager.MainVolume * Vol;
                if (RhythmPitchUse)
                    BGM.pitch = GameManager.Pitch * Pitch * GameManager.GameSpeed;
                else
                    BGM.pitch = Pitch * GameManager.GameSpeed;

                if (Vol <= 0)
                    SoundPrefab.DoNotPlayAudio = true;
            }
        }

        /// <summary>
        /// Stop BGM
        /// </summary>
        public static void StopBGM(string SoundBGM, bool Pade)
        {
            BGMCount -= 1;

            SoundPrefab[] allChildren = BGM.GetComponentsInChildren<SoundPrefab>();

            for (int i = 0; i < allChildren.Length; i++)
                if (allChildren[i].name == "BGM." + SoundBGM.Replace(".", "/"))
                {
                    if (!Pade)
                    {
                        allChildren[i].Stop = true;
                        Destroy(allChildren[i].gameObject);
                    }
                    else
                    {
                        allChildren[i].Stop = true;
                        allChildren[i].PadeOut = true;
                    }
                }
        }

        /// <summary>
        /// Play Sound
        /// </summary>
        public static void PlaySound(string SoundName, float Vol, float Pitch)
        {
            if (SoundCount < 256)
            {
                SoundCount += 1;
                
                GameObject SoundObject = Instantiate(soundPrefab, SoundManager.Sound.transform);
                SoundObject.name = "Sound." + SoundName.Replace(".", "/");
                SoundPrefab SoundPrefab = SoundObject.GetComponent<SoundPrefab>();
                SoundPrefab.Vol = Vol;
                SoundPrefab.Pitch = Pitch;
                string NameSpace = ResourcesManager.GetStringNameSpace(SoundName, out string ResourceName);
                if (NameSpace == "")
                    NameSpace = "sdjk";
                SoundPrefab.NameSpace = NameSpace;
                SoundPrefab.AudioName = ResourcesManager.SoundPath + ResourceName.Replace(".", "/");

                AudioSource Sound = SoundPrefab.audioSource;
                Sound.volume = GameManager.MainVolume * Vol;
                Sound.pitch = Pitch * GameManager.GameSpeed;

                if (Vol <= 0)
                    SoundPrefab.DoNotPlayAudio = true;
            }
        }

        /// <summary>
        /// Stop Sound
        /// </summary>
        public static void StopSound(string SoundName)
        {
            SoundCount -= 1;

            Transform[] allChildren = Sound.GetComponentsInChildren<Transform>();

            for (int i = 1; i < allChildren.Length; i++)
                if (allChildren[i].name == "Sound." + SoundName.Replace(".", "/"))
                    Destroy(allChildren[i].gameObject);
        }

        public static void StopAll(SoundType soundType, bool BGMPade)
        {
            if (soundType == SoundType.All)
            {
                SoundPrefab[] allChildren = soundManager.GetComponentsInChildren<SoundPrefab>();

                for (int i = 0; i < allChildren.Length; i++)
                    if (soundManager != allChildren[i] && allChildren[i] != null)
                    {
                        if (!BGMPade || !allChildren[i].BGM)
                        {
                            Destroy(allChildren[i].gameObject);
                            allChildren[i].Stop = true;
                        }
                        else
                        {
                            allChildren[i].PadeOut = true;
                            allChildren[i].Stop = true;
                        }
                    }
            }
            else if (soundType == SoundType.BGM)
            {
                SoundPrefab[] allChildren = BGM.GetComponentsInChildren<SoundPrefab>();

                for (int i = 0; i < allChildren.Length; i++)
                    if (soundManager != allChildren[i])
                    {
                        if (!BGMPade)
                        {
                            Destroy(allChildren[i].gameObject);
                            allChildren[i].Stop = true;
                        }
                        else
                        {
                            allChildren[i].PadeOut = true;
                            allChildren[i].Stop = true;
                        }
                    }
            }
            else if (soundType == SoundType.Sound)
            {
                Transform[] allChildren = Sound.GetComponentsInChildren<Transform>();

                for (int i = 0; i < allChildren.Length; i++)
                    if (soundManager != allChildren[i])
                        Destroy(allChildren[i].gameObject);
            }
        }
    }

    public enum SoundType
    {
        All,
        BGM,
        Sound,
    }
}