using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.Language
{
    public class Logo : MonoBehaviour
    {
        public Text LogoText;
        public Text Info;

        void Start() => Rerender();

        public void Rerender()
        {
            Info.text = LangManager.LangLoad(LangManager.Lang, "main_menu.version") + Application.version;
            LogoText.text = "";
            LogoText.text += GameManager.S.ToString()[0];
            LogoText.text += GameManager.D.ToString()[0];
            LogoText.text += GameManager.J.ToString()[0];
            LogoText.text += GameManager.K.ToString()[0];
        }
    }
}