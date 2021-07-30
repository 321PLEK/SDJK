using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using SDJK.MainMenu;
using SDJK.Sound;

namespace SDJK
{
    public class ResourcesManager : MonoBehaviour
    {
        //Texture
        readonly public static string GuiTexturePath = "assets/%NameSpace%/textures/gui/";

        readonly public static string SoundPath = "assets/%NameSpace%/sound/";
        readonly public static string BGMPath = "assets/%NameSpace%/bgm/";

        readonly public static string LangPath = "assets/%NameSpace%/lang/";

        readonly public static string MapPath = "assets/%NameSpace%/map/";
        readonly public static string ExtraMapPath = "assets/%NameSpace%/map/extra/";

        readonly public static string BackgroundPath = "assets/%NameSpace%/textures/background/";

        public static List<AudioClip[]> BGMList = new List<AudioClip[]>();
        public static List<AudioClip> ResourcesPackBGMList = new List<AudioClip>();

        public static List<string> NameSpaceList = new List<string>();
        public static List<string> LangList = new List<string>();

        public static List<string> ResourcePackPath = new List<string>();

        public static ResourcesManager resourcesManager;

        public MainMenu.MainMenu mainMenu;

        public static List<string> AllLevelList = new List<string>();
        public static List<string> LevelList = new List<string>();
        public static List<string> ExtraLevelList = new List<string>();

        /// <summary>
        /// 리소스팩에서 리소스를 찾고, 반환 합니다 네임스페이스는 sdjk 입니다
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static T Search<T>(string Path) => search<T>("sdjk", Path);

        /// <summary>
        /// 리소스팩에서 리소스를 찾고, 반환 합니다
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="NameSpace">리소스팩 폴더에서 찾을 네임스페이스 폴더 값입니다 비어있을경우 sdjk로 간주합니다</param>
        /// <param name="Path">리소스의 경로입니다</param>
        /// <returns></returns>
        public static T Search<T>(string NameSpace, string Path)
        {
            if (NameSpace == "")
                NameSpace = "sdjk";
            return search<T>(NameSpace, Path);
        }

        static T search<T>(string NameSpace, string Path)
        {
            T ret;
            {
                if (typeof(T) == typeof(Sprite))
                {
                    Texture2D originalTexture2D = Resources.Load<Texture2D>(Path.Replace("%NameSpace%", NameSpace));
                    Sprite originalSprite = Resources.Load<Sprite>(Path.Replace("%NameSpace%", NameSpace));
                    Texture2D TextureLoadTemp = new Texture2D(0, 0);
                    byte[] byteTexture;
                    Sprite sprite;

                    for (int i = 0; i < ResourcePackPath.Count; i++)
                    {
                        if (File.Exists(ResourcePackPath[i] + Path.Replace("%NameSpace%", NameSpace) + ".png"))
                        {
                            byteTexture = File.ReadAllBytes(ResourcePackPath[i] + Path.Replace("%NameSpace%", NameSpace) + ".png");
                            if (byteTexture.Length > 0)
                            {
                                TextureLoadTemp.name = "temp";
                                TextureLoadTemp.LoadImage(byteTexture);
                                if (originalSprite != null)
                                    TextureLoadTemp.filterMode = originalTexture2D.filterMode;
                                else
                                    TextureLoadTemp.filterMode = FilterMode.Point;

                                Rect rect = new Rect(0, 0, TextureLoadTemp.width, TextureLoadTemp.height);
                                Vector2 pivot = Vector2.zero;
                                if (originalSprite != null)
                                    pivot = originalSprite.pivot;

                                sprite = Sprite.Create(TextureLoadTemp, rect, pivot);
                                return (T)Convert.ChangeType(sprite, typeof(T));
                            }
                        }
                    }

                    sprite = originalSprite;
                    ret = (T)Convert.ChangeType(sprite, typeof(T));
                }
                else if (typeof(T) == typeof(Texture2D))
                {
                    Texture filterMode = Resources.Load<Texture2D>(Path.Replace("%NameSpace%", NameSpace));
                    Texture2D TextureLoadTemp = new Texture2D(0, 0);
                    byte[] byteTexture;

                    for (int i = 0; i < ResourcePackPath.Count; i++)
                    {
                        if (File.Exists(ResourcePackPath[i] + Path.Replace("%NameSpace%", NameSpace) + ".png"))
                        {
                            byteTexture = File.ReadAllBytes(ResourcePackPath[i] + Path.Replace("%NameSpace%", NameSpace) + ".png");
                            if (byteTexture.Length > 0)
                            {
                                TextureLoadTemp.name = "temp";
                                TextureLoadTemp.LoadImage(byteTexture);
                                if (filterMode != null)
                                    TextureLoadTemp.filterMode = filterMode.filterMode;
                                else
                                    TextureLoadTemp.filterMode = FilterMode.Point;

                                return (T)Convert.ChangeType(TextureLoadTemp, typeof(T));
                            }
                        }
                    }

                    ret = (T)Convert.ChangeType(filterMode, typeof(T));
                }
                else if (typeof(T) == typeof(AudioClip))
                    ret = (T)Convert.ChangeType(Resources.Load<AudioClip>(Path.Replace("%NameSpace%", NameSpace)), typeof(T));
                else if (typeof(T) == typeof(string))
                {
                    ret = (T)Convert.ChangeType(Resources.Load<TextAsset>(Path.Replace("%NameSpace%", NameSpace)) + "", typeof(T));

                    for (int i = 0; i < ResourcePackPath.Count; i++)
                    {
                        if (File.Exists(ResourcePackPath[i] + Path.Replace("%NameSpace%", NameSpace) + ".json"))
                        {
                            StreamReader sr = new StreamReader(ResourcePackPath[i] + Path.Replace("%NameSpace%", NameSpace) + ".json");
                            string text = sr.ReadToEnd();
                            sr.Close();

                            return (T)Convert.ChangeType(text, typeof(string));
                        }
                        else if (File.Exists(ResourcePackPath[i] + Path.Replace("%NameSpace%", NameSpace) + ".sdjk"))
                        {
                            StreamReader sr = new StreamReader(ResourcePackPath[i] + Path.Replace("%NameSpace%", NameSpace) + ".sdjk");
                            string text = sr.ReadToEnd();
                            sr.Close();
                            
                            return (T)Convert.ChangeType(text, typeof(string));
                        }
                        else if (File.Exists(ResourcePackPath[i] + Path.Replace("%NameSpace%", NameSpace) + ".txt"))
                        {
                            StreamReader sr = new StreamReader(ResourcePackPath[i] + Path.Replace("%NameSpace%", NameSpace) + ".txt");
                            string text = sr.ReadToEnd();
                            sr.Close();

                            return (T)Convert.ChangeType(text, typeof(string));
                        }
                        else if (File.Exists(ResourcePackPath[i] + Path.Replace("%NameSpace%", NameSpace) + ".text"))
                        {
                            StreamReader sr = new StreamReader(ResourcePackPath[i] + Path.Replace("%NameSpace%", NameSpace) + ".text");
                            string text = sr.ReadToEnd();
                            sr.Close();

                            return (T)Convert.ChangeType(text, typeof(string));
                        }
                    }
                }
                else
                    ret = (T)Convert.ChangeType(Resources.Load(Path.Replace("%NameSpace%", NameSpace)), typeof(T));
            }
            return ret;
        }

        /// <summary>
        /// 리소스팩에서 폴더에 있는 모든 리소스를 찾고, 반환 합니다 네임스페이스는 sdjk 입니다
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static T[] SearchAll<T>(string Path) => searchAll<T>("sdjk", Path);

        /// <summary>
        /// 리소스팩에서 폴더에 있는 모든 리소스를 찾고, 반환 합니다
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="NameSpace">리소스팩 폴더에서 찾을 네임스페이스 폴더 값입니다 비어있을경우 sdjk로 간주합니다</param>
        /// <param name="Path">폴더의 경로입니다</param>
        /// <returns></returns>
        public static T[] SearchAll<T>(string NameSpace, string Path)
        {
            if (NameSpace == "")
                NameSpace = "sdjk";

            return searchAll<T>(NameSpace, Path);
        }

        static T[] searchAll<T>(string NameSpace, string Path)
        {
            T[] ret;
            {
                if (typeof(T) == typeof(Sprite))
                    ret = (T[])Convert.ChangeType(Resources.LoadAll<Sprite>(Path.Replace("%NameSpace%", NameSpace)), typeof(T[]));
                else if (typeof(T) == typeof(AudioClip))
                    ret = (T[])Convert.ChangeType(Resources.LoadAll<AudioClip>(Path.Replace("%NameSpace%", NameSpace)), typeof(T[]));
                else if (typeof(T) == typeof(string))
                    ret = (T[])Convert.ChangeType(Resources.LoadAll<TextAsset>(Path.Replace("%NameSpace%", NameSpace)), typeof(T[]));
                else
                    ret = (T[])Convert.ChangeType(Resources.LoadAll(Path.Replace("%NameSpace%", NameSpace)), typeof(T[]));
            }
            return ret;
        }

        /// <summary>
        /// 네임스페이스와 리소스 이름을 쪼개서 네임스페이스랑 리소스 이름을 반환합니다 (예: sdjk:asdf = 네임스페이스: sdjk, 리소스 이름: asdf) (예: asdf = 네임스페이스: "", 에셋 이름: asdf)
        /// </summary>
        /// <param name="String">쪼갤 원본 문자열</param>
        /// <param name="ResourceName">반환할 리소스 이름</param>
        /// <returns></returns>
        public static string GetStringNameSpace(string String, out string ResourceName)
        {
            if (String.Contains(":"))
            {
                ResourceName = String.Substring(String.IndexOf(":") + 1);
                return String.Substring(0, String.IndexOf(":"));
            }
            else
            {
                ResourceName = String;
                return "";
            }
        }

        public static void NameSpaceRefresh()
        {
            NameSpaceList.Clear();
            NameSpaceList.Add("sdjk");

            for (int i = 0; i < ResourcePackPath.Count; i++)
            {
                string item = ResourcePackPath[i];
                if (!Directory.Exists(item))
                {
                    ResourcePackPath.RemoveAt(i);
                    ResourcePackSetting.ResourcePackName.RemoveAt(i);
                    NameSpaceRefresh();
                    return;
                }

                if (Directory.Exists(item + "assets"))
                {
                    foreach (string item2 in Directory.GetDirectories(item + "assets"))
                    {
                        string temp = item2.Replace("\\", "/");
                        temp = temp.Substring(temp.LastIndexOf("/") + 1);

                        if (!NameSpaceList.Contains(temp))
                            NameSpaceList.Add(temp);
                    }
                }
            }
        }

        public static void LangRefresh()
        {
            LangList.Clear();
            LangList.Add("en_us");
            LangList.Add("ko_kr");
            LangList.Add("TS_FG");

            for (int i = 0; i < ResourcePackPath.Count; i++)
            {
                for (int ii = 0; i < NameSpaceList.Count; i++)
                {
                    string item = ResourcePackPath[i];
                    if (Directory.Exists(item + LangPath.Replace("%NameSpace%", NameSpaceList[ii])))
                    {
                        string[] item2 = Directory.GetFiles(item + LangPath.Replace("%NameSpace%", NameSpaceList[ii]));
                        for (int iii = 0; iii < item2.Length; iii++)
                        {
                            string item3 = item2[iii];
                            string temp = item3.Replace("\\", "/");
                            int iiii = temp.LastIndexOf("/") + 1;
                            temp = temp.Substring(iiii, temp.LastIndexOf(".") - iiii);

                            if (!LangList.Contains(temp))
                                LangList.Add(temp);
                        }
                    }
                }
            }
        }

        public static void LevelRefresh()
        {
            resourcesManager.mainMenu.LevelList.Clear();
            for (int i = 0; i < LevelList.Count; i++)
                resourcesManager.mainMenu.LevelList.Add(LevelList[i]);

            resourcesManager.mainMenu.AllLevelList.Clear();
            for (int i = 0; i < AllLevelList.Count; i++)
                resourcesManager.mainMenu.AllLevelList.Add(AllLevelList[i]);

            for (int i = 0; i < ResourcePackPath.Count; i++)
            {
                for (int ii = 0; i < NameSpaceList.Count; i++)
                {
                    string item = ResourcePackPath[i];
                    if (Directory.Exists(item + MapPath.Replace("%NameSpace%", NameSpaceList[ii])))
                    {
                        string[] item2 = Directory.GetFiles(item + MapPath.Replace("%NameSpace%", NameSpaceList[ii]));
                        for (int iii = 0; iii < item2.Length; iii++)
                        {
                            string item3 = item2[iii];
                            string temp = item3.Replace("\\", "/");
                            int iiii = temp.LastIndexOf("/") + 1;
                            temp = temp.Substring(iiii, temp.LastIndexOf(".") - iiii);

                            if (!resourcesManager.mainMenu.LevelList.Contains(temp))
                                resourcesManager.mainMenu.LevelList.Add(temp);

                            if (!resourcesManager.mainMenu.AllLevelList.Contains(temp))
                                resourcesManager.mainMenu.AllLevelList.Add(temp);
                        }
                    }
                }
            }
        }

        public static void ExtraLevelRefresh()
        {
            resourcesManager.mainMenu.ExtraLevelList.Clear();
            for (int i = 0; i < ExtraLevelList.Count; i++)
                resourcesManager.mainMenu.ExtraLevelList.Add(ExtraLevelList[i]);

            for (int i = 0; i < ResourcePackPath.Count; i++)
            {
                for (int ii = 0; i < NameSpaceList.Count; i++)
                {
                    string item = ResourcePackPath[i];
                    if (Directory.Exists(item + ExtraMapPath.Replace("%NameSpace%", NameSpaceList[ii])))
                    {
                        string[] item2 = Directory.GetFiles(item + ExtraMapPath.Replace("%NameSpace%", NameSpaceList[ii]));
                        for (int iii = 0; iii < item2.Length; iii++)
                        {
                            string item3 = item2[iii];
                            string temp = item3.Replace("\\", "/");
                            int iiii = temp.LastIndexOf("/") + 1;
                            temp = temp.Substring(iiii, temp.LastIndexOf(".") - iiii);

                            if (!resourcesManager.mainMenu.ExtraLevelList.Contains("extra/" + temp))
                                resourcesManager.mainMenu.ExtraLevelList.Add("extra/" + temp);
                            if (!resourcesManager.mainMenu.AllLevelList.Contains("extra/" + temp))
                                resourcesManager.mainMenu.AllLevelList.Add("extra/" + temp);
                        }
                    }
                }
            }
        }

        public static void BGMRefresh()
        {
            BGMList.Clear();
            ResourcesPackBGMList.Clear();

            for (int i = 0; i < NameSpaceList.Count; i++)
                BGMList.Add(SearchAll<AudioClip>(NameSpaceList[i], BGMPath));

            for (int i = 0; i < ResourcePackPath.Count; i++)
            {
                for (int ii = 0; i < NameSpaceList.Count; i++)
                {
                    string item = ResourcePackPath[i];
                    if (Directory.Exists(item + BGMPath.Replace("%NameSpace%", NameSpaceList[ii])))
                    {
                        string[] item2 = Directory.GetFiles(item + BGMPath.Replace("%NameSpace%", NameSpaceList[ii]));
                        for (int iii = 0; iii < item2.Length; iii++)
                        {
                            string item3 = item2[iii];
                            string temp = item3.Replace("\\", "/");
                            int iiii = temp.LastIndexOf("/") + 1;
                            temp = temp.Substring(iiii, temp.LastIndexOf(".") - iiii);

                            SoundManager.PlaySound(temp, 0, 1);
                        }
                    }
                }
            }

            SoundManager.PlaySound("play mode.start hit sound", 0, 1);
            SoundManager.PlaySound("play mode.hit sound", 0, 1);
            SoundManager.PlaySound("play mode.end hit sound", 0, 1);
        }

        static bool Start = false;

        void Awake()
        {
            resourcesManager = this;

            if (!Start)
            {
                AllLevelList.Clear();
                for (int i = 0; i < resourcesManager.mainMenu.AllLevelList.Count; i++)
                    AllLevelList.Add(resourcesManager.mainMenu.AllLevelList[i]);

                LevelList.Clear();
                for (int i = 0; i < resourcesManager.mainMenu.LevelList.Count; i++)
                    LevelList.Add(resourcesManager.mainMenu.LevelList[i]);

                ExtraLevelList.Clear();
                for (int i = 0; i < resourcesManager.mainMenu.ExtraLevelList.Count; i++)
                    ExtraLevelList.Add("extra/" + resourcesManager.mainMenu.ExtraLevelList[i]);

                NameSpaceRefresh();
                LangRefresh();
                LevelRefresh();
                ExtraLevelRefresh();
                BGMRefresh();
            }

            Start = true;
        }
    }
}