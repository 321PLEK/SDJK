using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDJK.Renderer;

namespace SDJK.Renderer
{
    [RequireComponent(typeof(Image)),RequireComponent(typeof(Button)), ExecuteInEditMode]
    public class ButtonResource : MonoBehaviour
    {
        public string TextureName = "minecraft:widgets";
        public string SpriteName = "button";
        public string SelectedTextureName = "minecraft:widgets";
        public string SelectedSpriteName = "button_selected";
        public string DisabledTextureName = "minecraft:widgets";
        public string DisabledSpriteName = "button_disabled";
        public Image.Type type;
        public bool CustomPath = false;

        Button button;

        Sprite[] sprite;
        Sprite[] sprite2;
        Sprite[] sprite3;

        void Awake()
        {
            button = GetComponent<Button>();
            Rerender();
        }

        public void Rerender()
        {
            if (button == null)
                button = GetComponent<Button>();

            button.transition = Selectable.Transition.SpriteSwap;

            //스프라이트 찾기
            if (!CustomPath)
            {
                sprite = ResourcesManager.SearchAll<Sprite>(ResourcesManager.GetStringNameSpace(TextureName, out string temp), ResourcesManager.GuiTexturePath + temp);
                sprite2 = ResourcesManager.SearchAll<Sprite>(ResourcesManager.GetStringNameSpace(SelectedTextureName, out temp), ResourcesManager.GuiTexturePath + temp);
                sprite3 = ResourcesManager.SearchAll<Sprite>(ResourcesManager.GetStringNameSpace(DisabledTextureName, out temp), ResourcesManager.GuiTexturePath + temp);
            }
            else
            {
                sprite = ResourcesManager.SearchAll<Sprite>(ResourcesManager.GetStringNameSpace(TextureName, out string temp), temp);
                sprite2 = ResourcesManager.SearchAll<Sprite>(ResourcesManager.GetStringNameSpace(SelectedTextureName, out temp), temp);
                sprite3 = ResourcesManager.SearchAll<Sprite>(ResourcesManager.GetStringNameSpace(DisabledTextureName, out temp), temp);
            }

            //스프라이트 세팅
            SpriteState spriteState;

            for (int i = 0; i < sprite.Length; i++)
            {
                if (sprite[i].name == SpriteName)
                    button.image.sprite = sprite[i];
            }

            for (int i = 0; i < sprite2.Length; i++)
            {
                if (sprite2[i].name == SelectedSpriteName)
                {
                    spriteState.highlightedSprite = sprite2[i];
                    spriteState.pressedSprite = sprite2[i];
                    spriteState.selectedSprite = sprite2[i];
                }
            }

            for (int i = 0; i < sprite3.Length; i++)
            {
                if (sprite3[i].name == DisabledSpriteName)
                    spriteState.disabledSprite = sprite3[i];
            }

            button.spriteState = spriteState;
            button.image.type = type;

            sprite = null;
            sprite2 = null;
            sprite3 = null;
        }
    }
}