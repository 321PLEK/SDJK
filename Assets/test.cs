using UnityEngine;

public class test : MonoBehaviour
{
#if UNITY_STANDALONE_WIN
    int i = 6;
    bool b = false;
    float x = 0;
    float y = 0;
    float rotation = 0;

    void Start()
    {
        x = Screen.currentResolution.width * 0.5f;
        y = 0;
    }

    void Update()
    {
        /*if (RhythmManager.CurrentBeat > NextBeat && b)
        {
            NextBeat += 1;
            x += x;
            b = false;
        }
        else if (RhythmManager.CurrentBeat > NextBeat && !b)
        {
            NextBeat += 1;
            x += x;
            b = true;
        }
        
        x = Mathf.Lerp(x, 100, 0.15f * 60 * Time.deltaTime);

        if (!Input.GetKey(KeyCode.Space))
        {
            if (b)
                WindowManager.SetWindowPosition(x, y, WindowManager.datumPoint.LeftCenter, WindowManager.datumPoint.LeftCenter);
            else
                WindowManager.SetWindowPosition(-x, y, WindowManager.datumPoint.RightCenter, WindowManager.datumPoint.RightCenter);
        }*/

        if (Input.GetKeyDown(KeyCode.Space))
        {
            x = 0;
            y = 0;
            i++;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            b = !b;

        if (i == 0)
        {
            rotation -= 0.025f * 60 * Time.deltaTime;
            x = Mathf.Sin(rotation) * 150;
            y = Mathf.Cos(rotation) * 150;

            WindowManager.SetWindowPosition(x, y, 1280, 720, WindowManager.datumPoint.Center, WindowManager.datumPoint.Center, b);
        }
        else if (i == 1)
        {
            x += 20 * 60 * Time.deltaTime;
            y = 0;
            WindowManager.GetWindowResolution(out int width, out _);

            if (x > Screen.currentResolution.width + width)
                x = 0;

            WindowManager.SetWindowPosition(x, y, 1280, 720, WindowManager.datumPoint.RightCenter, WindowManager.datumPoint.LeftCenter, b);
        }
        else if (i == 2)
        {
            x = 0;
            y += 20 * 60 * Time.deltaTime;
            WindowManager.GetWindowResolution(out _, out int height);

            if (y > Screen.currentResolution.height + height)
                y = 0;

            WindowManager.SetWindowPosition(x, y, 1280, 720, WindowManager.datumPoint.CenterBottom, WindowManager.datumPoint.CenterTop, b);
        }
        else if (i == 3)
        {
            x -= 20 * 60 * Time.deltaTime;
            y = 0;
            WindowManager.GetWindowResolution(out int width, out _);

            if (x < -(Screen.currentResolution.width + width))
                x = 0;

            WindowManager.SetWindowPosition(x, y, 1280, 720, WindowManager.datumPoint.LeftCenter, WindowManager.datumPoint.RightCenter, b);
        }
        else if (i == 4)
        {
            x = 0;
            y -= 20 * 60 * Time.deltaTime;
            WindowManager.GetWindowResolution(out _, out int height);

            if (y < -(Screen.currentResolution.height + height))
                y = 0;

            WindowManager.SetWindowPosition(x, y, 1280, 720, WindowManager.datumPoint.CenterTop, WindowManager.datumPoint.CenterBottom, b);
        }
        /*else if (i == 5)
            WindowManager.SetWindowPosition(System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, WindowManager.datumPoint.Center, WindowManager.datumPoint.LeftTop, b);*/
        else if (i == 5)
            WindowManager.SetWindowPosition(0, 0, 1280, 720, WindowManager.datumPoint.Center, WindowManager.datumPoint.Center, b);
        else
            i = 0;
    }
#endif
}
