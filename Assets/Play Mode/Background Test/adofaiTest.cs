using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDJK.Camera;
using SDJK.PlayMode;
using SDJK;

public class adofaiTest : MonoBehaviour
{
    public Transform Red;
    public Transform Blue;
    //int i = 0;
    int i2 = 0;
    bool blue = false;

    public bool _blue = false;

    float CameraPosTemp;

    void Update()
    {
        /*if (PlayerManager.AllBeat.Count > i && i >= 0)
        {
            if (PlayerManager.Pitch >= 0)
            {
                if (PlayerManager.CurrentBeat >= PlayerManager.AllBeat[i])
                {
                    i++;
                    i2++;
                    blue = !blue;
                    if (!blue)
                        CameraPosTemp = Vector2.Distance(MainCamera.CameraPos, Red.position);
                    else
                        CameraPosTemp = Vector2.Distance(MainCamera.CameraPos, Blue.position);
                }
            }
            else
            {
                if (PlayerManager.CurrentBeat <= PlayerManager.AllBeat[i])
                {
                    i--;
                    i2++;
                    blue = !blue;
                    if (!blue)
                        CameraPosTemp = Vector2.Distance(MainCamera.CameraPos, Red.position);
                    else
                        CameraPosTemp = Vector2.Distance(MainCamera.CameraPos, Blue.position);
                }
            }
        }*/

        if (Input.anyKeyDown)
        {
            i2++;
            blue = !blue;
            if (!blue)
                CameraPosTemp = Vector2.Distance(MainCamera.CameraPos, Red.position);
            else
                CameraPosTemp = Vector2.Distance(MainCamera.CameraPos, Blue.position);
        }

        if (!blue && !_blue)
        {
            transform.localEulerAngles = new Vector3(0, 0, (float)-((PlayerManager.HitSoundCurrentBeat * 180) - (180 * i2)));
            Blue.localPosition = -transform.right * 1.5f + Red.localPosition;
            transform.localEulerAngles = Vector2.zero;
        }

        if (blue && _blue)
        {
            transform.localEulerAngles = new Vector3(0, 0, (float)-((PlayerManager.HitSoundCurrentBeat * 180) - (180 * i2)));
            Red.localPosition = -transform.right * 1.5f + Blue.localPosition;
            transform.localEulerAngles = Vector2.zero;
        }

        if (!_blue)
        {
            MainCamera.posMode = PosMode.World;

            if (!blue)
                MainCamera.CameraPos = Vector3.MoveTowards(MainCamera.CameraPos, Red.position + Vector3.back * 14, (float) (PlayerManager.mapData.Effect.BPM / 60 * CameraPosTemp * GameManager.DeltaTime * PlayerManager.mapData.Effect.Pitch));
            else
                MainCamera.CameraPos = Vector3.MoveTowards(MainCamera.CameraPos, Blue.position + Vector3.back * 14, (float) (PlayerManager.mapData.Effect.BPM / 60 * CameraPosTemp * GameManager.DeltaTime * PlayerManager.mapData.Effect.Pitch));
        }
    }
}
