using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDJK.Renderer
{
    [RequireComponent(typeof(Image)), ExecuteInEditMode]
    public class ImageResource : MonoBehaviour
    {
        public string TextureName = "";
        public string SpriteName = "";
        public Color Color = Color.white;
        public Image.Type type;
        public bool CustomPath = false;
        public Vector4 Border;
        static Vector2 Pivot = new Vector2(0.5f, 0.5f);

        Sprite sprite = null;

        Image image;

        public void Rerender()
        {
            if (image == null)
                image = GetComponent<Image>();

            //스프라이트 찾기
            if (Border != Vector4.zero)
            {
                if (!CustomPath)
                {
                    Texture2D texture2D = ResourcesManager.Search<Texture2D>(ResourcesManager.GetStringNameSpace(TextureName, out string temp), ResourcesManager.GuiTexturePath + temp);
                    sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Pivot, 90000, 0, SpriteMeshType.Tight, Border);
                    image.sprite = sprite;
                }
                else
                {
                    Texture2D texture2D = ResourcesManager.Search<Texture2D>(ResourcesManager.GetStringNameSpace(TextureName, out string temp), temp);
                    sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Pivot, 90000, 0, SpriteMeshType.Tight, Border);
                    image.sprite = sprite;
                }
            }
            else
            {
                if (!CustomPath)
                    image.sprite = ResourcesManager.Search<Sprite>(ResourcesManager.GetStringNameSpace(TextureName, out string temp), ResourcesManager.GuiTexturePath + temp);
                else
                    image.sprite = ResourcesManager.Search<Sprite>(ResourcesManager.GetStringNameSpace(TextureName, out string temp), temp);
            }

            //스프라이트 설정
            image.color = Color;
            image.type = type;

            sprite = null;
        }

        void Awake() => Rerender();

        void OnDestroy()
        {
            sprite = null;
            if (image != null)
                image.sprite = null;
        }
    }
}