using SDJK.Language;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.MainMenu
{
    public class ResourcePackSelect : MonoBehaviour
    {
        public Button button;
        public Text text;

        void Start() => Rerender();

        void Update()
        {
            if (ResourcesManager.ResourcePackPath.Contains(gameObject.name) || gameObject.name == "Basic Resource")
                button.image.color = Color.black;
            else
                button.image.color = Color.clear;
        }

        public void Rerender()
        {
            if (gameObject.name == "Basic Resource")
            {
                text.text = "";

                foreach (string item in ResourcePackSetting.ResourcePackName)
                    text.text += item + " < ";

                text.text += LangManager.LangLoad(LangManager.Lang, "resourcePack.basicResource");
            }
        }

        public void ResourceToggle()
        {
            if (!ResourcesManager.ResourcePackPath.Contains(gameObject.name))
            {
                ResourcePackSetting.ResourcePackName.Insert(0, text.text);
                ResourcesManager.ResourcePackPath.Insert(0, gameObject.name);
                MainMenu.AllRerender();
            }
            else
            {
                ResourcePackSetting.ResourcePackName.Remove(text.text);
                ResourcesManager.ResourcePackPath.Remove(gameObject.name);
                MainMenu.AllRerender();
            }
        }
    }
}