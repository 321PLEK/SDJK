using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.Language
{
    public class Logo : MonoBehaviour
    {
        public Text Info;

        void Start() => Rerender();

        public void Rerender() => Info.text = LangManager.LangLoad(LangManager.Lang, "main_menu.version") + Application.version;
    }
}