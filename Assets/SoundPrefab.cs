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

        public bool Stop = false;

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
                        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(ResourcesManager.ResourcePackPath[i] + AudioName.Replace("%NameSpace%", NameSpace) + ".mp3", AudioType.MPEG);
                        ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;
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

                        www.Dispose();
                    }
                    else if (File.Exists(ResourcesManager.ResourcePackPath[i] + AudioName.Replace("%NameSpace%", NameSpace) + ".ogg"))
                    {
                        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(ResourcesManager.ResourcePackPath[i] + AudioName.Replace("%NameSpace%", NameSpace) + ".ogg", AudioType.OGGVORBIS);
                        ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;
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

                        www.Dispose();
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

                if (BGM && !audioSource.isPlaying && Loop && !GameManager.Pause && !Stop)
                {
                    audioSource.enabled = true;
                    audioSource.Play();
                    GameManager.BeatTimer = 0;
                    GameManager.CurrentBeat = 0;
                    MainMenu.MainMenu.NextBeat = 0;

                    if (MainMenu.MainMenu.Esc)
                    {
                        GameManager.AllLevelIndex = Random.Range(0, MainMenu.MainMenu.mainMenu.AllLevelList.Count);
                        GameManager.Level = MainMenu.MainMenu.mainMenu.AllLevelList[GameManager.AllLevelIndex];
                        MainMenu.MainMenu.LevelRefresh();
                    }
                }

                if (!audioSource.isPlaying && !GameManager.Pause)
                    Destroy(gameObject);

                if (MainMenu.MainMenu.Esc)
                {
                    if (!GameManager.Ratio_9_16)
                    {
                        if (Input.GetKey(KeyCode.UpArrow))
                            GameManager.Pitch = 0.666f;
                        else if (Input.GetKey(KeyCode.DownArrow))
                            GameManager.Pitch = 1.5f;
                        else
                            GameManager.Pitch = 1;
                    }
                    else
                    {
                        if (Input.GetKey(KeyCode.LeftArrow))
                            GameManager.Pitch = 0.666f;
                        else if (Input.GetKey(KeyCode.RightArrow))
                            GameManager.Pitch = 1.5f;
                        else
                            GameManager.Pitch = 1;
                    }
                }

                yield return null;
            }
        }

        void OnDestroy()
        {
            audioSource.clip = null;
            audioClip = null;
            Resources.UnloadUnusedAssets();

            if (BGM)
                SoundManager.BGMCount -= 1;
            else
                SoundManager.SoundCount -= 1;
        }
    }
}