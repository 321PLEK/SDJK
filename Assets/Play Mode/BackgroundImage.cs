using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace SDJK.PlayMode.UI.Background
{
    public class BackgroundImage : MonoBehaviour
    {
        public Image image;
        public VideoPlayer videoPlayer;
        public bool MainManu = false;

        void Start() => Rerender();

        public void Rerender()
        {
            if (!videoPlayer.isPlaying && GameManager.BackgroundEnable)
            {
                image.enabled = true;
                image.color = Color.white;

                string NameSpace = ResourcesManager.GetStringNameSpace(PlayerManager.mapData.Background, out string ResourceName);
                string NameSpaceNight = ResourcesManager.GetStringNameSpace(PlayerManager.mapData.BackgroundNight, out string ResourceNameNight);
                string MapPathResourceName = ResourceName;
                string MapPathResourceNameNight = ResourceNameNight;
                ResourceName = ResourcesManager.BackgroundPath + ResourceName;
                ResourceNameNight = ResourcesManager.BackgroundPath + ResourceNameNight;

                if (NameSpace == "")
                    NameSpace = "sdjk";
                if (NameSpaceNight == "")
                    NameSpaceNight = "sdjk";

                //리소스팩에서 리소스를 가져오기
                Texture2D filterMode = Resources.Load<Texture2D>(ResourceName.Replace("%NameSpace%", NameSpace));
                Texture2D TextureLoadTemp = new Texture2D(0, 0);
                byte[] byteTexture;

                for (int i = 0; i < ResourcesManager.ResourcePackPath.Count; i++)
                {
                    if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 4)
                    {
                        if (File.Exists(ResourcesManager.ResourcePackPath[i] + ResourceNameNight.Replace("%NameSpace%", NameSpaceNight) + ".png"))
                        {
                            byteTexture = File.ReadAllBytes(ResourcesManager.ResourcePackPath[i] + ResourceNameNight.Replace("%NameSpace%", NameSpaceNight) + ".png");
                            if (byteTexture.Length > 0)
                            {
                                TextureLoadTemp.name = "temp";
                                TextureLoadTemp.LoadImage(byteTexture);
                                TextureLoadTemp.filterMode = filterMode.filterMode;

                                image.sprite = Sprite.Create(TextureLoadTemp, new Rect(0, 0, TextureLoadTemp.width, TextureLoadTemp.height), new Vector2(0.5f, 0.5f));
                                return;
                            }
                        }
                        else if (File.Exists(ResourcesManager.ResourcePackPath[i] + ResourceName.Replace("%NameSpace%", NameSpace) + ".png"))
                        {
                            byteTexture = File.ReadAllBytes(ResourcesManager.ResourcePackPath[i] + ResourceName.Replace("%NameSpace%", NameSpace) + ".png");
                            if (byteTexture.Length > 0)
                            {
                                TextureLoadTemp.name = "temp";
                                TextureLoadTemp.LoadImage(byteTexture);
                                TextureLoadTemp.filterMode = filterMode.filterMode;

                                image.sprite = Sprite.Create(TextureLoadTemp, new Rect(0, 0, TextureLoadTemp.width, TextureLoadTemp.height), new Vector2(0.5f, 0.5f));
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (File.Exists(ResourcesManager.ResourcePackPath[i] + ResourceName.Replace("%NameSpace%", NameSpace) + ".png"))
                        {
                            byteTexture = File.ReadAllBytes(ResourcesManager.ResourcePackPath[i] + ResourceName.Replace("%NameSpace%", NameSpace) + ".png");
                            if (byteTexture.Length > 0)
                            {
                                TextureLoadTemp.name = "temp";
                                TextureLoadTemp.LoadImage(byteTexture);
                                TextureLoadTemp.filterMode = filterMode.filterMode;

                                image.sprite = Sprite.Create(TextureLoadTemp, new Rect(0, 0, TextureLoadTemp.width, TextureLoadTemp.height), new Vector2(0.5f, 0.5f));
                                return;
                            }
                        }
                    }
                }

                if (PlayerManager.MapPath != "" && (PlayerManager.Editor || PlayerManager.isEditorMapPlay))
                {
                    if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 4)
                    {
                        //맵 파일에서 리소스를 가져오기
                        string ResourceNameMapPath = PlayerManager.MapPath + MapPathResourceNameNight;

                        TextureLoadTemp = new Texture2D(0, 0);

                        if (File.Exists(ResourceNameMapPath + ".png"))
                        {
                            byteTexture = File.ReadAllBytes(ResourceNameMapPath + ".png");
                            if (byteTexture.Length > 0)
                            {
                                TextureLoadTemp.name = "temp";
                                TextureLoadTemp.LoadImage(byteTexture);
                                TextureLoadTemp.filterMode = FilterMode.Point;

                                image.sprite = Sprite.Create(TextureLoadTemp, new Rect(0, 0, TextureLoadTemp.width, TextureLoadTemp.height), new Vector2(0.5f, 0.5f));
                                return;
                            }
                        }
                        else
                        {
                            ResourceNameMapPath = PlayerManager.MapPath + MapPathResourceName;

                            if (File.Exists(ResourceNameMapPath + ".png"))
                            {
                                byteTexture = File.ReadAllBytes(ResourceNameMapPath + ".png");
                                if (byteTexture.Length > 0)
                                {
                                    TextureLoadTemp.name = "temp";
                                    TextureLoadTemp.LoadImage(byteTexture);
                                    TextureLoadTemp.filterMode = FilterMode.Point;

                                    image.sprite = Sprite.Create(TextureLoadTemp, new Rect(0, 0, TextureLoadTemp.width, TextureLoadTemp.height), new Vector2(0.5f, 0.5f));
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        //맵 파일에서 리소스를 가져오기
                        string ResourceNameMapPath = PlayerManager.MapPath + MapPathResourceName;

                        TextureLoadTemp = new Texture2D(0, 0);

                        if (File.Exists(ResourceNameMapPath + ".png"))
                        {
                            byteTexture = File.ReadAllBytes(ResourceNameMapPath + ".png");
                            if (byteTexture.Length > 0)
                            {
                                TextureLoadTemp.name = "temp";
                                TextureLoadTemp.LoadImage(byteTexture);
                                TextureLoadTemp.filterMode = FilterMode.Point;

                                image.sprite = Sprite.Create(TextureLoadTemp, new Rect(0, 0, TextureLoadTemp.width, TextureLoadTemp.height), new Vector2(0.5f, 0.5f));
                                return;
                            }
                        }
                    }
                }

                //원본 리소스를 가져오기
                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 4)
                {
                    image.sprite = Resources.Load<Sprite>(ResourceNameNight.Replace("%NameSpace%", NameSpace));

                    if (image.sprite == null)
                        image.sprite = Resources.Load<Sprite>(ResourceName.Replace("%NameSpace%", NameSpace));
                }
                else
                    image.sprite = Resources.Load<Sprite>(ResourceName.Replace("%NameSpace%", NameSpace));

                if (image.sprite == null)
                    image.color = Color.clear;
            }
            else
            {
                image.color = Color.clear;
                image.enabled = false;
            }
        }
    }
}