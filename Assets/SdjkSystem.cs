using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDJK
{
    public class SdjkSystem : MonoBehaviour
    {
        public static SdjkSystem sdjkSystem;
        public SdjkSystem _sdjkSystem;
        
        void Awake()
        {
            SdjkSystem[] obj = FindObjectsOfType<SdjkSystem>();
            if (obj.Length == 1)
                DontDestroyOnLoad(gameObject);
            else
                Destroy(gameObject);

            sdjkSystem = _sdjkSystem;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                if (Screen.fullScreen)
                    Screen.SetResolution(1280, 720, false);
                else
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            }
        }
    }
}