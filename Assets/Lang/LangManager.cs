using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

namespace SDJK.Language
{
    public class LangManager : MonoBehaviour
    {
        public GameObject prefab;
        public static string Lang = "ko_kr";

        void OnEnable()
        {
            Transform[] item = GetComponentsInChildren<Transform>();
            for (int i = 1; i < item.Length; i++)
            {
                Transform item2 = item[i];
                Destroy(item2.gameObject);
            }

            for (int i = 0; i < ResourcesManager.LangList.Count; i++)
            {
                GameObject gameObject = Instantiate(prefab, transform);
                LangSelected langSelected = gameObject.GetComponent<LangSelected>();
                langSelected.rectTransform.anchoredPosition = new Vector2(4, -50 * i - 5);
                langSelected.Lang = ResourcesManager.LangList[i];
                langSelected.Rerender();
            }
        }

        public static string LangLoad(string LangName, string Translation)
        {
            string json = ResourcesManager.Search<string>(ResourcesManager.GetStringNameSpace(LangName, out string Name), ResourcesManager.LangPath + Name);
            
            Dictionary<string, object> jsonDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (jsonDic == null)
                return "null";

            Dictionary<string, string> jsonDic2 = jsonDic.ToDictionary(
                x => x.Key,
                x => x.Value.ToString()
            );

            if (!jsonDic2.ContainsKey(Translation))
            {
                string NameSpace = ResourcesManager.GetStringNameSpace(LangName, out Name);
                if (NameSpace == "")
                    NameSpace = "sdjk";

                TextAsset json2 = Resources.Load<TextAsset>((ResourcesManager.LangPath + Name).Replace("%NameSpace%", NameSpace));
                
                if (json2 == null)
                    return "null";
                string json3 = json2.ToString();

                jsonDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(json3);
                if (jsonDic == null)
                    return "null";

                jsonDic2 = jsonDic.ToDictionary(
                    x => x.Key,
                    x => x.Value.ToString()
                );

                if (!jsonDic2.ContainsKey(Translation))
                    return "null";
            }
            return jsonDic2[Translation];
        }
    }
}