using System.Collections;
using SDJK;
using SDJK.Camera;
using SDJK.MainMenu;
using SDJK.Scene;
using SDJK.Sound;
using UnityEngine;

public class DontOpenMe : MonoBehaviour
{
    public bool Text = false;

    void Update()
    {
        if (!Text)
        {
            //Don't run this code
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.V) && Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.M) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.P) && Input.GetKeyDown(KeyCode.Return))
            {
                SoundManager.StopAll(SoundType.All, false);
                MainCamera.Camera.enabled = false;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Don't open me and play");
                MainMenu.mainMenu.enabled = false;
                return;
            }
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.V) && Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.P) && Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.SceneLoading("Play Mode");
                GameManager.Level = "extra/School Live! OP";
                MainMenu.mainMenu.enabled = false;
                return;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.SceneLoading("Main Menu");
                MainCamera.Camera.enabled = true;
                SdjkSystem.sdjkSystem.gameObject.SetActive(true);
            }
        }
    }

    void Start()
    {
        if (Text)
            SdjkSystem.sdjkSystem.gameObject.SetActive(false);
    }
}
