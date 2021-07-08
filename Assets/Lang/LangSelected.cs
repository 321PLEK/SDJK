using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.Language
{
    [ExecuteInEditMode]
    public class LangSelected : MonoBehaviour
    {
        public RectTransform rectTransform;
        public Button button;
        public Text text;

        public string Lang = "";
        string tempLang;

        void Start() => Rerender();

        void Update()
        {
            if (!Application.isPlaying && tempLang != Lang)
            {
                Rerender();
                tempLang = Lang;
            }

            if (LangManager.Lang == Lang)
                button.image.color = Color.black;
            else
                button.image.color = Color.clear;

            tempLang = Lang;
        }

        public void Rerender()
        {
            if (button == null)
                button = GetComponent<Button>();
            if (text == null)
                text = transform.GetChild(0).GetComponent<Text>();
            
            string temp = LangManager.LangLoad(Lang, "language.name") + " (" + LangManager.LangLoad(Lang, "language.region") + ")";
            button.interactable = true;
            if (temp == "null (null)")
            {
                button.interactable = false;
                temp = LangManager.LangLoad(LangManager.Lang, "resourcePack.customLang");
                if (LangManager.Lang == "custom_lang")
                {
                    LangManager.Lang = "en_us";
                    Invoke("Rerender2", 0);
                }
            }
            text.text = temp;
            gameObject.name = temp;
        }

        void Rerender2() => MainMenu.MainMenu.AllRerender();

        public void LangSelect()
        {
            if (Lang != LangManager.Lang)
            {
                LangManager.Lang = Lang;
                MainMenu.MainMenu.AllRerender();
            }
        }
    }
}