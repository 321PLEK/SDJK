using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine.UI;
using SDJK.Language;
using System;

namespace SDJK.MainMenu
{
    public class ResourcePackSetting : MonoBehaviour
    {
        public Transform content;
        public GameObject prefab;
        public GameObject MainMenu;

        public static List<string> ResourcePackName = new List<string>();

        void OnEnable ()
        {
            for (int i = 0; i < ResourcesManager.ResourcePackPath.Count; i++)
            {
                string item3 = ResourcesManager.ResourcePackPath[i];

                if (!Directory.Exists("/" + item3))
                    ResourcesManager.ResourcePackPath.Remove("/" + item3);
            }

            Transform[] item = content.GetComponentsInChildren<Transform>();
            for (int i = 0; i < item.Length; i++)
            {
                Transform item3 = item[i];
                if (item3 != content)
                    Destroy(item3.gameObject);
            }

            if (!Directory.Exists(Application.persistentDataPath + "/Resource Pack"))
                Directory.CreateDirectory(Application.persistentDataPath + "/Resource Pack");

            int y = -5;
            GameObject gameObject = Instantiate(prefab, content);
            gameObject.name = "Basic Resource";
            gameObject.transform.GetComponent<Button>().interactable = false;
            gameObject.transform.GetChild(0).GetComponent<Text>().text = "Basic Resource";
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(4, y);
            y -= 52;

            string[] item2 = Directory.GetDirectories(Application.persistentDataPath + "/Resource Pack");
            for (int i = 0; i < item2.Length; i++)
            {
                string item3 = item2[i];
                string temp = item3.Replace("\\", "/");

                if (File.Exists(temp + "/info.json"))
                {
                    StreamReader sr = new StreamReader(temp + "/info.json");
                    string json = sr.ReadToEnd();
                    sr.Close();

                    Dictionary<string, object> jsonDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    if (jsonDic == null)
                        return;

                    Dictionary<string, string> jsonDic2 = jsonDic.ToDictionary(
                        x => x.Key,
                        x => x.Value.ToString()
                    );

                    if (!jsonDic2.ContainsKey("name"))
                        return;
                    else if (!jsonDic2.ContainsKey("version"))
                        return;
                    
                    gameObject = Instantiate(prefab, content);
                    gameObject.name = temp + "/";
                    Text text = gameObject.transform.GetChild(0).GetComponent<Text>();
                    text.text = jsonDic2["name"];
                    gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(4, y);
                    y -= 52;

                    if (jsonDic2["version"] != Application.version)
                        text.color = Color.red;
                }
            }
        }
    }
}