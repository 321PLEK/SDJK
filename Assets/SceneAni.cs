using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDJK.Sound;

namespace SDJK.Scene
{
    public class SceneAni : MonoBehaviour
    {
        public string SceneName = "";
        public RectTransform image;

        IEnumerator Start()
        {
            AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneName);
            asyncOperation.allowSceneActivation = false;
            SoundManager.StopAll(SoundType.All, false);

            while (true)
            {
                if (image.offsetMax.x < -0.01f)
                {
                    image.offsetMin = Vector2.zero;
                    image.offsetMax = Vector2.Lerp(image.offsetMax, Vector2.zero, 0.2f * GameManager.FpsDeltaTime);
                }
                else if (asyncOperation.progress >= 0.9f || asyncOperation.isDone)
                {
                    asyncOperation.allowSceneActivation = true;
                    
                    yield return null;
                    
                    image.offsetMin = Vector2.Lerp(image.offsetMin, new Vector2(Screen.width, 0), 0.2f * GameManager.FpsDeltaTime);
                    image.offsetMax = Vector2.zero;

                    if (image.offsetMin.x >= Screen.width - 0.01f)
                        Destroy(gameObject);
                }

                yield return null;
            }
        }
    }
}