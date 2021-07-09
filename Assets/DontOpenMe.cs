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
        //Don't run this code
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.V) && Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.M) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.P) && Input.GetKeyDown(KeyCode.Return))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Don't open me and play");
            MainCamera.Camera.enabled = false;
            SdjkSystem.sdjkSystem.gameObject.SetActive(false);
            SoundManager.StopAll(SoundType.All, false);
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.V) && Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.P) && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.SceneLoading("Play Mode");
            GameManager.Level = "extra/School Live! OP";
            MainMenu.mainMenu.enabled = false;
        }
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        
        if (Text)
            gameObject.SetActive(false);
    }
}
