using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.Language
{
    [ExecuteInEditMode]
    public class LangText : MonoBehaviour
    {
        public string translation = "";
        public Text text;

        string tempLang;
        string tempTranslation;

        void Start() => Rerender();

        void Update()
        {
            if (LangManager.Lang != tempLang || translation != tempTranslation)
            {
                Rerender();
                tempLang = LangManager.Lang;
                tempTranslation = translation;
            }
        }

        public void Rerender()
        {
            if (text == null)
                text = GetComponent<Text>();
            text.text = LangManager.LangLoad(LangManager.Lang, translation);
        }
    }
}