using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDJK.PlayMode;

namespace SDJK.PlayMode.Sound
{
    public class HitSound : MonoBehaviour
    {
        public AudioSource _audioSource;
        public static AudioSource audioSource;
        public static AudioClip startHitSound;
        public static AudioClip hitSound;
        public static AudioClip endHitSound;

        public bool Editor = false;
        public PlayerManager playerManager;

        public static int BeatIndex = 0;

        void Awake() => audioSource = _audioSource;

        IEnumerator Start()
        {
            //리소스팩 히트 사운드
            startHitSound = ResourcesManager.Search<AudioClip>("sdjk", ResourcesManager.SoundPath + "play mode/start hit sound");
            hitSound = ResourcesManager.Search<AudioClip>("sdjk", ResourcesManager.SoundPath + "play mode/hit sound");
            endHitSound = ResourcesManager.Search<AudioClip>("sdjk", ResourcesManager.SoundPath + "play mode/end hit sound");
            BeatIndex = 0;

            bool temp = false;
            string NameSpace = ResourcesManager.GetStringNameSpace("sdjk:play mode/start hit sound", out string ResourceName);
            ResourceName = ResourcesManager.SoundPath + ResourceName;

            foreach (AudioClip item in ResourcesManager.ResourcesPackBGMList)
            {
                if (item.name == ResourceName.Substring(ResourceName.LastIndexOf("/") + 1))
                {
                    temp = true;
                    startHitSound = item;
                    break;
                }
            }

            if (!temp)
            {
                foreach (AudioClip[] item in ResourcesManager.BGMList)
                {
                    foreach (AudioClip item2 in item)
                    {
                        if (item2.name == ResourceName.Substring(ResourceName.LastIndexOf("/") + 1))
                        {
                            temp = true;
                            startHitSound = item2;
                            break;
                        }
                    }
                }
            }

            if (!temp)
                startHitSound = Resources.Load<AudioClip>(ResourceName.Replace("%NameSpace%", NameSpace));

            temp = false;
            NameSpace = ResourcesManager.GetStringNameSpace("sdjk:play mode/hit sound", out ResourceName);
            ResourceName = ResourcesManager.SoundPath + ResourceName;

            foreach (AudioClip item in ResourcesManager.ResourcesPackBGMList)
            {
                if (item.name == ResourceName.Substring(ResourceName.LastIndexOf("/") + 1))
                {
                    temp = true;
                    hitSound = item;
                    break;
                }
            }

            if (!temp)
            {
                foreach (AudioClip[] item in ResourcesManager.BGMList)
                {
                    foreach (AudioClip item2 in item)
                    {
                        if (item2.name == ResourceName.Substring(ResourceName.LastIndexOf("/") + 1))
                        {
                            temp = true;
                            hitSound = item2;
                            break;
                        }
                    }
                }
            }

            if (!temp)
                hitSound = Resources.Load<AudioClip>(ResourceName.Replace("%NameSpace%", NameSpace));

            temp = false;
            NameSpace = ResourcesManager.GetStringNameSpace("sdjk:play mode/end hit sound", out ResourceName);
            ResourceName = ResourcesManager.SoundPath + ResourceName;

            foreach (AudioClip item in ResourcesManager.ResourcesPackBGMList)
            {
                if (item.name == ResourceName.Substring(ResourceName.LastIndexOf("/") + 1))
                {
                    temp = true;
                    endHitSound = item;
                    break;
                }
            }

            if (!temp)
            {
                foreach (AudioClip[] item in ResourcesManager.BGMList)
                {
                    foreach (AudioClip item2 in item)
                    {
                        if (item2.name == ResourceName.Substring(ResourceName.LastIndexOf("/") + 1))
                        {
                            temp = true;
                            endHitSound = item2;
                            break;
                        }
                    }
                }
            }

            if (!temp)
                endHitSound = Resources.Load<AudioClip>(ResourceName.Replace("%NameSpace%", NameSpace));

            if (!Editor)
            {
                PlayerManager.BPMChange(PlayerManager.mapData.Effect.BPM, 0);
                PlayerManager.HitSoundCurrentBeat = -10;
                PlayerManager.BeatTimer = 0;

                bool end = false;

                if (!GameManager.OsuHitSound)
                {
                    while (!end)
                    {
                        //히트 사운드 재생
                        if (BeatIndex >= PlayerManager.mapData.AllBeat.Count - 1)
                        {
                            end = true;
                            audioSource.PlayOneShot(endHitSound, audioSource.volume * GameManager.MainVolume);
                            break;
                        }

                        if (BeatIndex <= 0 && PlayerManager.effect.Pitch * GameManager.GameSpeed < 0)
                        {
                            end = true;
                            audioSource.PlayOneShot(endHitSound, audioSource.volume * GameManager.MainVolume);
                            break;
                        }

                        if (!end)
                        {
                            if (PlayerManager.effect.Pitch * GameManager.GameSpeed >= 0)
                            {
                                int hitSoundPlay = 0;
                                while (PlayerManager.HitSoundCurrentBeat >= PlayerManager.mapData.AllBeat[BeatIndex])
                                {
                                    if (BeatIndex > 0 && BeatIndex < PlayerManager.mapData.AllBeat.Count - 2 && (PlayerManager.mapData.AllBeat[BeatIndex] != PlayerManager.mapData.AllBeat[BeatIndex - 1] || PlayerManager.mapData.HitSoundSimultaneousPlayAllow))
                                        hitSoundPlay++;
                                    else if (BeatIndex <= 0)
                                        hitSoundPlay++;
                                    BeatIndex++;
                                }

                                for (int i = 0; i < hitSoundPlay; i++)
                                    audioSource.PlayOneShot(hitSound, audioSource.volume * GameManager.MainVolume);
                            }
                            else
                            {
                                int hitSoundPlay = 0;
                                while (PlayerManager.HitSoundCurrentBeat <= PlayerManager.mapData.AllBeat[BeatIndex])
                                {
                                    if (BeatIndex > 1 && (BeatIndex < PlayerManager.mapData.AllBeat.Count - 2 && PlayerManager.mapData.AllBeat[BeatIndex] != PlayerManager.mapData.AllBeat[BeatIndex + 1] || PlayerManager.mapData.HitSoundSimultaneousPlayAllow))
                                        hitSoundPlay++;
                                    else if (BeatIndex >= PlayerManager.mapData.AllBeat.Count - 2)
                                        hitSoundPlay++;

                                    BeatIndex--;
                                }

                                for (int i = 0; i < hitSoundPlay; i++)
                                    audioSource.PlayOneShot(hitSound, audioSource.volume * GameManager.MainVolume);
                            }
                        }

                        yield return null;
                    }
                }
            }
            else
            {
                //히트 사운드 재생
                if (!GameManager.OsuHitSound)
                {
                    float tempTime = 0;
                    while (true)
                    {
                        if (PlayerManager.playerManager.audioSource.pitch >= 0)
                        {
                            if (BeatIndex < 0)
                                BeatIndex = 0;

                            int hitSoundPlay = 0;
                            while (BeatIndex < PlayerManager.mapData.AllBeat.Count - 1 && PlayerManager.HitSoundCurrentBeat >= PlayerManager.mapData.AllBeat[BeatIndex])
                            {
                                if (!Input.GetKey(KeyCode.T))
                                {
                                    if (BeatIndex > 0 && (PlayerManager.mapData.AllBeat[BeatIndex] != PlayerManager.mapData.AllBeat[BeatIndex - 1] || PlayerManager.mapData.HitSoundSimultaneousPlayAllow))
                                        hitSoundPlay++;
                                    else if (BeatIndex <= 0)
                                        hitSoundPlay++;
                                }

                                BeatIndex++;
                            }

                            for (int i = 0; i < hitSoundPlay; i++)
                                audioSource.PlayOneShot(hitSound, audioSource.volume * GameManager.MainVolume);
                        }
                        else
                        {
                            if (BeatIndex >= PlayerManager.mapData.AllBeat.Count - 1)
                                BeatIndex = PlayerManager.mapData.AllBeat.Count - 2;

                            int hitSoundPlay = 0;
                            while (BeatIndex >= 0 && PlayerManager.HitSoundCurrentBeat <= PlayerManager.mapData.AllBeat[BeatIndex])
                            {
                                if (!Input.GetKey(KeyCode.T))
                                {
                                    if (BeatIndex < PlayerManager.mapData.AllBeat.Count - 2 && (PlayerManager.mapData.AllBeat[BeatIndex] != PlayerManager.mapData.AllBeat[BeatIndex + 1] || PlayerManager.mapData.HitSoundSimultaneousPlayAllow))
                                        hitSoundPlay++;
                                    else if (BeatIndex >= PlayerManager.mapData.AllBeat.Count - 2)
                                        hitSoundPlay++;
                                }

                                BeatIndex--;
                            }

                            for (int i = 0; i < hitSoundPlay; i++)
                                audioSource.PlayOneShot(hitSound, audioSource.volume * GameManager.MainVolume);
                        }

                        tempTime = PlayerManager.playerManager.audioSource.time;
                        yield return null;
                    }
                }
            }
        }
    }
}