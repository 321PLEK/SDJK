using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        void Start()
        {
            if (GameManager.EditorOptimization && PlayerManager.Editor)
                videoPlayer.gameObject.SetActive(false);
                
            Rerender();
        }

        public IEnumerator Rerender()
        {
            videoPlayer.enabled = true;
            
            videoPlayer.Stop();
            videoPlayer.clip = null;
            
            videoPlayer.targetTexture.Release();

            string NameSpace = ResourcesManager.GetStringNameSpace(PlayerManager.mapData.VideoBackground, out string ResourceName);
            string MapPathResourceName = ResourceName;
            ResourceName = ResourcesManager.BackgroundPath + "video/" + ResourceName;

            bool temp = false;

            if (NameSpace == "")
                NameSpace = "sdjk";
            
            //리소스팩에서 리소스를 가져오기
            for (int i = 0; i < ResourcesManager.ResourcePackPath.Count; i++)
            {
                if (File.Exists(ResourcesManager.ResourcePackPath[i] + ResourceName.Replace("%NameSpace%", NameSpace) + ".mp4"))
                {
                    videoPlayer.url = ResourcesManager.ResourcePackPath[i] + ResourceName.Replace("%NameSpace%", NameSpace) + ".mp4";
                    temp = true;
                }
            }

            if (!temp)
            {
                if (PlayerManager.MapPath != "" && (PlayerManager.Editor || PlayerManager.isEditorMapPlay))
                {
                    //맵 파일에서 리소스를 가져오기
                    string ResourceNameMapPath = PlayerManager.MapPath + MapPathResourceName;

                    if (File.Exists(ResourceNameMapPath + ".mp4"))
                    {
                        videoPlayer.url = ResourceNameMapPath + ".mp4";
                        temp = true;
                    }
                }
            }

            if (!temp)
            {
                //원본 리소스를 가져오기
                if (File.Exists(Application.streamingAssetsPath + "/" + ResourceName.Replace("%NameSpace%", NameSpace) + ".mp4"))
                {
                    videoPlayer.url = Application.streamingAssetsPath + "/" + ResourceName.Replace("%NameSpace%", NameSpace) + ".mp4";
                    temp = true;
                }
            }

            if (!temp)
                videoPlayer.enabled = false;
            else
            {
                videoPlayer.Stop();

                if (!PlayerManager.Editor)
                    yield return new WaitUntil(() => PlayerManager.playerManager.audioSource.isPlaying);

                videoPlayer.Play();
                if (!PlayerManager.Editor)
                    videoPlayer.time = PlayerManager.playerManager.audioSource.time + PlayerManager.mapData.VideoOffset - GameManager.InputOffset;
            }
        }

        void Update()
        {
            if (videoPlayer.enabled)
            {
                videoPlayer.playbackSpeed = PlayerManager.playerManager.audioSource.pitch;

                if (videoPlayer.time < PlayerManager.playerManager.audioSource.time + PlayerManager.mapData.VideoOffset - GameManager.InputOffset - 0.041f)
                        videoPlayer.playbackSpeed = PlayerManager.playerManager.audioSource.pitch * 2;
                
                if (videoPlayer.time > PlayerManager.playerManager.audioSource.time + PlayerManager.mapData.VideoOffset - GameManager.InputOffset - 0.039f)
                    videoPlayer.playbackSpeed = PlayerManager.playerManager.audioSource.pitch * 0;

                if (PlayerManager.Editor)
                {
                    if (!Input.GetKey(KeyCode.UpArrow) && !EditorManager.AutoScroll || Input.GetKey(KeyCode.DownArrow))
                    {
                        videoPlayer.playbackSpeed = 0;
                        videoPlayer.time = PlayerManager.playerManager.audioSource.time + PlayerManager.mapData.VideoOffset - GameManager.InputOffset;
                    }

                    if (!videoPlayer.isPlaying)
                    {
                        videoPlayer.Play();
                        videoPlayer.time = PlayerManager.playerManager.audioSource.time + PlayerManager.mapData.VideoOffset - GameManager.InputOffset;
                    }

                    if (PlayerManager.playerManager.audioSource.time >= videoPlayer.length)
                        videoPlayer.Pause();
                }
            }
        }
    }
}