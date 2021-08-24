using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.Renderer
{
    [RequireComponent(typeof(SpriteRenderer)), ExecuteInEditMode]
    public class SpriteResource : MonoBehaviour
    {
        public string TextureName = "";
        public string SpriteName = "";
        public Color Color = Color.white;
        public bool CustomPath = false;

        Sprite sprite = null;

        SpriteRenderer spriteRenderer;

        public void Rerender()
        {
            //주석 적기 귀찮다...
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            if (!CustomPath)
            {
                sprite = ResourcesManager.Search<Sprite>(ResourcesManager.GetStringNameSpace(TextureName, out string temp), ResourcesManager.GuiTexturePath + temp);
                spriteRenderer.sprite = sprite;
            }
            else
            {
                sprite = ResourcesManager.Search<Sprite>(ResourcesManager.GetStringNameSpace(TextureName, out string temp), temp);
                spriteRenderer.sprite = sprite;
            }

            spriteRenderer.color = Color;
            sprite = null;
        }

        void Awake() => Rerender();

        void OnDestroy()
        {
            sprite = null;
            if (spriteRenderer != null)
                spriteRenderer.sprite = null;
        }
    }
}