using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SDJK.Scene
{
    public class SceneManager : MonoBehaviour
    {
        public static GameObject GameObject;
        public GameObject _prefab;
        public static GameObject prefab;

        void OnEnable()
        {
            GameObject = gameObject;
            prefab = _prefab;
        }

        public static void SceneLoading(string SceneName, bool Hide = false)
        {
            if (!Hide)
            {
                SceneAni sceneAni;
                sceneAni = Instantiate(prefab, GameObject.transform).GetComponent<SceneAni>();
                sceneAni.SceneName = SceneName;
            }
            else
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneName);
            
            SaveLoad.SaveLoad.SaveData();
        }
    }
}