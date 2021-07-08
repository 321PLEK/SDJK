using System.Collections;
using SDJK;
using SDJK.Camera;
using SDJK.Sound;
using UnityEngine;

public class DontOpenMe : MonoBehaviour
{
    public bool Text = false;

    void Update()
    {
        //Don't run this code
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.M) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.P) && Input.GetKeyDown(KeyCode.Return))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Don't open me and play");
            MainCamera.Camera.enabled = false;
            SdjkSystem.sdjkSystem.gameObject.SetActive(false);
            SoundManager.StopAll(SoundType.All, false);
        }
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        
        if (Text)
            gameObject.SetActive(false);
    }
}
