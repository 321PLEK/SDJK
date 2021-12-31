using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SDJK.Camera;
using SDJK.EditMode;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.Video;

namespace SDJK.PlayMode.UI.Background
{
    public class BackgroundVideo : MonoBehaviour
    {
        public VideoPlayer videoPlayer;
        public bool MainMenu = false;

        void Start()
        {
            if (GameManager.EditorOptimization && PlayerManager.Editor && !MainMenu)
                videoPlayer.gameObject.SetActive(false);
                
            if (!MainMenu && gameObject.activeSelf)
                StartCoroutine(Rerender());
        }

        public IEnumerator Rerender()
        {
            videoPlayer.Stop();
            videoPlayer.clip = null;

            videoPlayer.enabled = true;
            
            if (!MainMenu)
                videoPlayer.targetTexture.Release();

            string NameSpace = ResourcesManager.GetStringNameSpace(PlayerManager.mapData.VideoBackground, out string ResourceName);
            string NameSpaceNight = ResourcesManager.GetStringNameSpace(PlayerManager.mapData.VideoBackgroundNight, out string ResourceNameNight);
            string MapPathResourceName = ResourceName;
            string MapPathResourceNameNight = ResourceNameNight;

            ResourceName = ResourcesManager.BackgroundPath + "video/" + ResourceName;
            ResourceNameNight = ResourcesManager.BackgroundPath + "video/" + ResourceNameNight;

            bool temp = false;

            if (NameSpace == "")
                NameSpace = "sdjk";
            if (NameSpaceNight == "")
                NameSpaceNight = "sdjk";

            //리소스팩에서 리소스를 가져오기
            for (int i = 0; i < ResourcesManager.ResourcePackPath.Count; i++)
            {
                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 4)
                {
                    if (File.Exists(ResourcesManager.ResourcePackPath[i] + ResourceNameNight.Replace("%NameSpace%", NameSpaceNight) + ".mp4"))
                    {
                        videoPlayer.url = ResourcesManager.ResourcePackPath[i] + ResourceNameNight.Replace("%NameSpace%", NameSpaceNight) + ".mp4";
                        temp = true;
                    }
                    else if (File.Exists(ResourcesManager.ResourcePackPath[i] + ResourceName.Replace("%NameSpace%", NameSpace) + ".mp4"))
                    {
                        videoPlayer.url = ResourcesManager.ResourcePackPath[i] + ResourceName.Replace("%NameSpace%", NameSpace) + ".mp4";
                        temp = true;
                    }
                }
                else
                {
                    if (File.Exists(ResourcesManager.ResourcePackPath[i] + ResourceName.Replace("%NameSpace%", NameSpace) + ".mp4"))
                    {
                        videoPlayer.url = ResourcesManager.ResourcePackPath[i] + ResourceName.Replace("%NameSpace%", NameSpace) + ".mp4";
                        temp = true;
                    }
                }
            }

            if (!temp)
            {
                if (PlayerManager.MapPath != "" && (PlayerManager.Editor || PlayerManager.isEditorMapPlay))
                {
                    if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 4)
                    {
                        //맵 파일에서 리소스를 가져오기
                        string ResourceNameMapPath = PlayerManager.MapPath + MapPathResourceNameNight;

                        if (File.Exists(ResourceNameMapPath + ".mp4"))
                        {
                            videoPlayer.url = ResourceNameMapPath + ".mp4";
                            temp = true;
                        }
                        else
                        {
                            ResourceNameMapPath = PlayerManager.MapPath + MapPathResourceName;

                            if (File.Exists(ResourceNameMapPath + ".mp4"))
                            {
                                videoPlayer.url = ResourceNameMapPath + ".mp4";
                                temp = true;
                            }
                        }
                    }
                    else
                    {
                        string ResourceNameMapPath = PlayerManager.MapPath + MapPathResourceName;

                        if (File.Exists(ResourceNameMapPath + ".mp4"))
                        {
                            videoPlayer.url = ResourceNameMapPath + ".mp4";
                            temp = true;
                        }
                    }
                }
            }

            if (!temp)
            {
                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 4)
                {
                    //원본 리소스를 가져오기
                    if (File.Exists(Application.streamingAssetsPath + "/" + ResourceNameNight.Replace("%NameSpace%", NameSpaceNight) + ".mp4"))
                    {
                        videoPlayer.url = Application.streamingAssetsPath + "/" + ResourceNameNight.Replace("%NameSpace%", NameSpaceNight) + ".mp4";
                        temp = true;
                    }
                    else if (File.Exists(Application.streamingAssetsPath + "/" + ResourceName.Replace("%NameSpace%", NameSpace) + ".mp4"))
                    {
                        videoPlayer.url = Application.streamingAssetsPath + "/" + ResourceName.Replace("%NameSpace%", NameSpace) + ".mp4";
                        temp = true;
                    }
                }
                else
                {
                    //원본 리소스를 가져오기
                    if (File.Exists(Application.streamingAssetsPath + "/" + ResourceName.Replace("%NameSpace%", NameSpace) + ".mp4"))
                    {
                        videoPlayer.url = Application.streamingAssetsPath + "/" + ResourceName.Replace("%NameSpace%", NameSpace) + ".mp4";
                        temp = true;
                    }
                }
            }

            if (!temp)
                videoPlayer.enabled = false;
            else if (!MainMenu)
            {
                videoPlayer.Stop();

                if (!PlayerManager.Editor)
                    yield return new WaitUntil(() => PlayerManager.playerManager.audioSource.isPlaying);

                videoPlayer.Play();
                if (!PlayerManager.Editor)
                    videoPlayer.time = PlayerManager.playerManager.audioSource.time + PlayerManager.mapData.VideoOffset - GameManager.InputOffset;
            }

            yield return null;

            if (MainMenu && videoPlayer.targetCamera == null)
                videoPlayer.targetCamera = MainCamera.Camera;
        }

        void Update()
        {
            float time;

            if (!MainMenu)
                time = PlayerManager.playerManager.audioSource.time;
            else
                time = GameManager.BeatTimer;

            if (videoPlayer.enabled)
            {
                float Pitch;
                if (!MainMenu)
                    Pitch = PlayerManager.playerManager.audioSource.pitch;
                else
                    Pitch = GameManager.Pitch;

                videoPlayer.playbackSpeed = Pitch;

                if (GameManager.FPS >= 100)
                {
                    if (videoPlayer.time < time + PlayerManager.mapData.VideoOffset - GameManager.InputOffset - 0.041f)
                        videoPlayer.playbackSpeed = Pitch * 2;

                    if (videoPlayer.time > time + PlayerManager.mapData.VideoOffset - GameManager.InputOffset - 0.039f)
                        videoPlayer.playbackSpeed = Pitch * 0;
                }
                else
                {
                    if (videoPlayer.time < time + PlayerManager.mapData.VideoOffset - GameManager.InputOffset - 0.041f)
                        videoPlayer.playbackSpeed = Pitch * 2;

                    if (videoPlayer.time > time + PlayerManager.mapData.VideoOffset - GameManager.InputOffset - 0.019f)
                        videoPlayer.playbackSpeed = Pitch * 0;
                }

                if (PlayerManager.Editor || MainMenu)
                {
                    if (!MainMenu)
                    {
                        if (!Input.GetKey(KeyCode.UpArrow) && !EditorManager.AutoScroll || Input.GetKey(KeyCode.DownArrow))
                        {
                            videoPlayer.playbackSpeed = 0;
                            videoPlayer.time = time + PlayerManager.mapData.VideoOffset - GameManager.InputOffset;
                        }
                    }

                    if (!videoPlayer.isPlaying)
                    {
                        videoPlayer.Play();
                        videoPlayer.time = time + PlayerManager.mapData.VideoOffset - GameManager.InputOffset;
                    }

                    if (time >= videoPlayer.length)
                        videoPlayer.Pause();
                }
            }
        }
    }
}