using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using SDJK.MainMenu;

namespace SDJK.Sound
{
    public class SoundPrefab : MonoBehaviour
    {
        public string NameSpace;
        public string AudioName;
        public bool BGM = false;
        public bool RhythmPitchUse = false;
        public float Vol = 1;
        public float Pitch = 1;
        public bool Loop = false;

        public bool PadeIn = false;
        public bool PadeOut = false;

        public bool DoNotPlayAudio = false;

        public AudioSource audioSource;

        AudioClip audioClip;

        IEnumerator restart;

        void Start() => Reload();

        public void Reload()
        {
            if (restart != null)
                StopCoroutine(restart);

            if (RhythmPitchUse)
            {
                GameManager.BeatTimer = 0;
                GameManager.CurrentBeat = -1;

                MainMenu.MainMenu.NextBeat = 0;
            }

            restart = AudioPlay();
            StartCoroutine(restart);
        }

        public IEnumerator AudioPlay()
        {
            audioSource = GetComponent<AudioSource>();
            bool temp = false;

            if (!DoNotPlayAudio)
            {
                foreach (AudioClip item in ResourcesManager.ResourcesPackBGMList)
                {
                    if (item.name == AudioName.Substring(AudioName.LastIndexOf("/") + 1))
                    {
                        temp = true;
                        audioClip = item;
                        audioSource.clip = audioClip;
                        audioSource.Play();
                        break;
                    }
                }
            }

            if (!temp)
            {
                for (int i = 0; i < ResourcesManager.ResourcePackPath.Count; i++)
                {
                    if (File.Exists(ResourcesManager.ResourcePackPath[i] + AudioName.Replace("%NameSpace%", NameSpace) + ".mp3"))
                    {
                        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(ResourcesManager.ResourcePackPath[i] + AudioName.Replace("%NameSpace%", NameSpace) + ".mp3", AudioType.MPEG))
                        {
                            yield return www.SendWebRequest();

                            if (www.result == UnityWebRequest.Result.ConnectionError)
                                Debug.Log(www.error);
                            else
                            {
                                temp = true;
                                audioClip = DownloadHandlerAudioClip.GetContent(www);
                                audioSource.clip = audioClip;
                                audioSource.clip.name = AudioName.Substring(AudioName.LastIndexOf("/") + 1);
                                ResourcesManager.ResourcesPackBGMList.Add(audioSource.clip);
                                if (!DoNotPlayAudio)
                                    audioSource.Play();
                            }
                        }
                    }
                    else if (File.Exists(ResourcesManager.ResourcePackPath[i] + AudioName.Replace("%NameSpace%", NameSpace) + ".ogg"))
                    {
                        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(ResourcesManager.ResourcePackPath[i] + AudioName.Replace("%NameSpace%", NameSpace) + ".ogg", AudioType.OGGVORBIS))
                        {
                            yield return www.SendWebRequest();

                            if (www.result == UnityWebRequest.Result.ConnectionError)
                                Debug.Log(www.error);
                            else
                            {
                                temp = true;
                                audioClip = DownloadHandlerAudioClip.GetContent(www);
                                audioSource.clip = audioClip;
                                audioSource.clip.name = AudioName.Substring(AudioName.LastIndexOf("/") + 1);
                                ResourcesManager.ResourcesPackBGMList.Add(audioSource.clip);
                                if (!DoNotPlayAudio)
                                    audioSource.Play();
                            }
                        }
                    }
                }
            }

            if (!DoNotPlayAudio)
            {
                if (!temp)
                {
                    foreach (AudioClip[] item in ResourcesManager.BGMList)
                    {
                        foreach (AudioClip item2 in item)
                        {
                            if (item2.name == AudioName.Substring(AudioName.LastIndexOf("/") + 1))
                            {
                                temp = true;
                                audioClip = item2;
                                audioSource.clip = audioClip;
                                audioSource.Play();
                                break;
                            }
                        }
                    }
                }

                if (!temp)
                {
                    audioClip = Resources.Load<AudioClip>(AudioName.Replace("%NameSpace%", NameSpace));
                    audioSource.clip = audioClip;
                    audioSource.Play();
                }
            }

            if (DoNotPlayAudio)
                Destroy(gameObject);

            audioClip = null;
            Resources.UnloadUnusedAssets();

            yield return null;

            while (true)
            {
                if (!PadeIn && !PadeOut)
                    audioSource.volume = GameManager.MainVolume * Vol;
                else if (audioSource.volume < GameManager.MainVolume * Vol && PadeIn && !PadeOut)
                    audioSource.volume += 0.0075f * GameManager.MainVolume * Vol * 60 * Time.deltaTime;
                else if (audioSource.volume > GameManager.MainVolume * Vol && PadeIn && !PadeOut)
                    audioSource.volume = GameManager.MainVolume * Vol;
                else if (audioSource.volume > 0 && PadeOut)
                    audioSource.volume -= 0.0075f * GameManager.MainVolume * Vol * 60 * Time.deltaTime;
                else if (audioSource.volume <= 0)
                    Destroy(gameObject);

                if (RhythmPitchUse)
                    audioSource.pitch = GameManager.Pitch * Pitch * GameManager.GameSpeed;
                else
                    audioSource.pitch = Pitch * GameManager.GameSpeed;

                if (!GameManager.Pause)
                    audioSource.UnPause();
                else if (BGM)
                    audioSource.Pause();

                if (BGM && !audioSource.isPlaying && Loop && !GameManager.Pause)
                {
                    audioSource.enabled = true;
                    audioSource.Play();
                    GameManager.BeatTimer = 0;
                    GameManager.CurrentBeat = 0;
                    MainMenu.MainMenu.NextBeat = 0;
                }

                if (!audioSource.isPlaying && !GameManager.Pause)
                    Destroy(gameObject);

                yield return null;
            }
        }

        void OnDestroy()
        {
            if (BGM)
                SoundManager.BGMCount -= 1;
            else
                SoundManager.SoundCount -= 1;
        }
    }
}